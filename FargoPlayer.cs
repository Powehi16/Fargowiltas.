using System.Collections.Generic;
using System.IO;
using Fargowiltas.Common.Configs;
using Fargowiltas.Items;
using Fargowiltas.Items.Misc;
using Fargowiltas.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Fargowiltas;

public class FargoPlayer : ModPlayer
{
	public bool extractSpeed;

	public bool HasDrawnDebuffLayer;

	internal bool BattleCry;

	internal bool CalmingCry;

	internal int originalSelectedItem;

	internal bool autoRevertSelectedItem;

	public float luckPotionBoost;

	public float ElementalAssemblerNearby;

	public float StatSheetMaxAscentMultiplier;

	public float StatSheetWingSpeed;

	public bool? CanHover = null;

	public int DeathFruitHealth;

	public bool bigSuck;

	public int StationSoundCooldown;

	internal Dictionary<string, bool> FirstDyeIngredients = new Dictionary<string, bool>();

	public bool[] ItemHasBeenOwned;

	public bool[] ItemHasBeenOwnedAtThirtyStack;

	private readonly string[] tags = new string[13]
	{
		"RedHusk", "OrangeBloodroot", "YellowMarigold", "LimeKelp", "GreenMushroom", "TealMushroom", "CyanHusk", "SkyBlueFlower", "BlueBerries", "PurpleMucos",
		"VioletHusk", "PinkPricklyPear", "BlackInk"
	};

	public override void Initialize()
	{
		ItemHasBeenOwned = ItemID.Sets.Factory.CreateBoolSet(false);
		ItemHasBeenOwnedAtThirtyStack = ItemID.Sets.Factory.CreateBoolSet(false);
	}

	public override void SaveData(TagCompound tag)
	{
		string name = "FargoDyes" + base.Player.name;
		List<string> dyes = new List<string>();
		string[] array = tags;
		foreach (string tagString in array)
		{
			if (FirstDyeIngredients.TryGetValue(tagString, out var _))
			{
				dyes.AddWithCondition(tagString, FirstDyeIngredients[tagString]);
			}
			else
			{
				dyes.AddWithCondition(tagString, condition: false);
			}
		}
		tag.Add(name, dyes);
		tag.Add("DeathFruitHealth", DeathFruitHealth);
		if (BattleCry)
		{
			tag.Add("FargoBattleCry" + base.Player.name, true);
		}
		if (CalmingCry)
		{
			tag.Add("FargoCalmingCry" + base.Player.name, true);
		}
		List<string> ownedItemsData = new List<string>();
		for (int i = 0; i < ItemHasBeenOwned.Length; i++)
		{
			if (!ItemHasBeenOwned[i])
			{
				continue;
			}
			if (i >= ItemID.Count)
			{
				ModItem modItem = ItemLoader.GetItem(i);
				if (modItem != null && modItem != null)
				{
					ownedItemsData.Add(modItem.FullName ?? "");
				}
			}
			else
			{
				ownedItemsData.Add($"{i}");
			}
		}
		tag.Add("OwnedItemsList", ownedItemsData);
	}

	public override void LoadData(TagCompound tag)
	{
		string name = "FargoDyes" + base.Player.name;
		IList<string> dyes = tag.GetList<string>(name);
		string[] array = tags;
		foreach (string downedTag in array)
		{
			FirstDyeIngredients[downedTag] = dyes.Contains(downedTag);
		}
		DeathFruitHealth = tag.GetInt("DeathFruitHealth");
		BattleCry = tag.ContainsKey("FargoBattleCry" + base.Player.name);
		CalmingCry = tag.ContainsKey("FargoCalmingCry" + base.Player.name);
		ItemHasBeenOwned = ItemID.Sets.Factory.CreateBoolSet(false);
		IList<string> ownedItemsData = tag.GetList<string>("OwnedItemsList");
		foreach (string entry in ownedItemsData)
		{
			ModItem item;
			if (int.TryParse(entry, out var type) && type < ItemID.Count)
			{
				ItemHasBeenOwned[type] = true;
			}
			else if (ModContent.TryFind<ModItem>(entry, out item))
			{
				ItemHasBeenOwned[item.Type] = true;
			}
		}
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = base.Mod.GetPacket();
		packet.Write((byte)9);
		packet.Write((byte)base.Player.whoAmI);
		packet.Write((byte)DeathFruitHealth);
		packet.Send(toWho, fromWho);
	}

	public void ReceivePlayerSync(BinaryReader reader)
	{
		DeathFruitHealth = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		FargoPlayer clone = (FargoPlayer)targetCopy;
		clone.DeathFruitHealth = DeathFruitHealth;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		FargoPlayer clone = (FargoPlayer)clientPlayer;
		if (DeathFruitHealth != clone.DeathFruitHealth)
		{
			SyncPlayer(-1, Main.myPlayer, newPlayer: false);
		}
	}

	public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
	{
		string[] array = tags;
		foreach (string tag in array)
		{
			FirstDyeIngredients[tag] = false;
		}
	}

	public override void OnEnterWorld()
	{
		global::Fargowiltas.Items.Misc.BattleCry.SyncCry(base.Player);
	}

	public override void ResetEffects()
	{
		extractSpeed = false;
		HasDrawnDebuffLayer = false;
		bigSuck = false;
	}

	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (Fargowiltas.HomeKey.JustPressed)
		{
			AutoUseMirror();
		}
		if (Fargowiltas.StatKey.JustPressed)
		{
			if (!Main.playerInventory)
			{
				Main.playerInventory = true;
			}
			Fargowiltas.UserInterfaceManager.ToggleStatSheet();
		}
	}

	public override void PostUpdateBuffs()
	{
		if (FargoServerConfig.Instance.UnlimitedPotionBuffsOn120)
		{
			Item[] item2 = base.Player.bank.item;
			foreach (Item item in item2)
			{
				FargoGlobalItem.TryUnlimBuff(item, base.Player);
			}
			Item[] item3 = base.Player.bank2.item;
			foreach (Item item in item3)
			{
				FargoGlobalItem.TryUnlimBuff(item, base.Player);
			}
		}
		if (FargoServerConfig.Instance.PiggyBankAcc)
		{
			Item[] item4 = base.Player.bank.item;
			foreach (Item item in item4)
			{
				FargoGlobalItem.TryPiggyBankAcc(item, base.Player);
			}
			Item[] item5 = base.Player.bank2.item;
			foreach (Item item in item5)
			{
				FargoGlobalItem.TryPiggyBankAcc(item, base.Player);
			}
		}
	}

	public override void PostUpdateEquips()
	{
		if (Fargowiltas.SwarmActive)
		{
			base.Player.buffImmune[37] = true;
		}
	}

	public override void UpdateDead()
	{
		StationSoundCooldown = 0;
	}

	public override void PostUpdateMiscEffects()
	{
		if (ElementalAssemblerNearby > 0f)
		{
			ElementalAssemblerNearby -= 1f;
			base.Player.alchemyTable = true;
		}
		if (StationSoundCooldown > 0)
		{
			StationSoundCooldown--;
		}
		if (base.Player.equippedWings == null)
		{
			ResetStatSheetWings();
		}
		ForceBiomes();
	}

	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
	{
		FargoServerConfig config = FargoServerConfig.Instance;
		if (config.EnemyDamage != 1f || config.BossDamage != 1f)
		{
			if (config.BossDamage > config.EnemyDamage && (npc.boss || npc.type == 13 || npc.type == 14 || npc.type == 15 || (config.BossApplyToAllWhenAlive && FargoGlobalNPC.AnyBossAlive())))
			{
				modifiers.FinalDamage *= config.BossDamage;
			}
			else
			{
				modifiers.FinalDamage *= config.EnemyDamage;
			}
		}
	}

	public void ResetStatSheetWings()
	{
		StatSheetMaxAscentMultiplier = 0f;
		StatSheetWingSpeed = 0f;
		CanHover = null;
	}

	private void ForceBiomes()
	{
		if (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.eaterBoss, 13) && base.Player.Distance(Main.npc[FargoGlobalNPC.eaterBoss].Center) < 3000f)
		{
			base.Player.ZoneCorrupt = true;
		}
		if (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.brainBoss, 266) && base.Player.Distance(Main.npc[FargoGlobalNPC.brainBoss].Center) < 3000f)
		{
			base.Player.ZoneCrimson = true;
		}
		if ((FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.plantBoss, 262) && base.Player.Distance(Main.npc[FargoGlobalNPC.plantBoss].Center) < 3000f) || (FargoGlobalNPC.SpecificBossIsAlive(ref FargoGlobalNPC.beeBoss, 222) && base.Player.Distance(Main.npc[FargoGlobalNPC.beeBoss].Center) < 3000f))
		{
			base.Player.ZoneJungle = true;
		}
		if (!FargoServerConfig.Instance.Fountains)
		{
			return;
		}
		switch (Main.SceneMetrics.ActiveFountainColor)
		{
		case -1:
			break;
		case 0:
			base.Player.ZoneBeach = true;
			break;
		case 2:
			base.Player.ZoneCorrupt = true;
			break;
		case 3:
			base.Player.ZoneJungle = true;
			break;
		case 4:
			if (Main.hardMode)
			{
				base.Player.ZoneHallow = true;
			}
			break;
		case 5:
			base.Player.ZoneSnow = true;
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			base.Player.ZoneCrimson = true;
			break;
		case 6:
		case 12:
			base.Player.ZoneDesert = true;
			if (base.Player.Center.Y > 3200f)
			{
				base.Player.ZoneUndergroundDesert = true;
			}
			break;
		case 1:
		case 7:
		case 11:
			break;
		}
	}

	public override void PostUpdate()
	{
		if (autoRevertSelectedItem && base.Player.itemTime == 0 && base.Player.itemAnimation == 0)
		{
			base.Player.selectedItem = originalSelectedItem;
			autoRevertSelectedItem = false;
		}
		if (FargoWorld.OverloadedSlimeRain && Main.rand.NextBool(20))
		{
			SlimeRainSpawns();
		}
	}

	public void SlimeRainSpawns()
	{
		int type = -3;
		int[] slimes = new int[12]
		{
			535, 537, 147, 184, 16, 204, 71, 225, 141, 81,
			183, 138
		};
		int rand = Main.rand.Next(50);
		if (rand == 0)
		{
			type = -4;
		}
		else if (rand < 20)
		{
			type = slimes[Main.rand.Next(slimes.Length)];
		}
		Vector2 pos = new Vector2((int)base.Player.position.X + Main.rand.Next(-800, 800), (int)base.Player.position.Y + Main.rand.Next(-800, -250));
	}

	public override bool PreModifyLuck(ref float luck)
	{
		if (FargoWorld.Matsuri && !Main.IsItRaining && !Main.IsItStorming)
		{
			LanternNight.GenuineLanterns = true;
			LanternNight.ManualLanterns = false;
		}
		return base.PreModifyLuck(ref luck);
	}

	public override void ModifyLuck(ref float luck)
	{
		luck += luckPotionBoost;
		luckPotionBoost = 0f;
	}

	public void AutoUseMirror()
	{
		int potionofReturn = -1;
		int recallPotion = -1;
		int magicMirror = -1;
		for (int i = 0; i < base.Player.inventory.Length; i++)
		{
			switch (base.Player.inventory[i].type)
			{
			case 4870:
				potionofReturn = i;
				break;
			case 2350:
				recallPotion = i;
				break;
			case 50:
			case 3124:
			case 3199:
			case 5358:
				magicMirror = i;
				break;
			}
		}
		if (potionofReturn != -1)
		{
			QuickUseItemAt(potionofReturn);
		}
		else if (recallPotion != -1)
		{
			QuickUseItemAt(recallPotion);
		}
		else if (magicMirror != -1)
		{
			QuickUseItemAt(magicMirror);
		}
	}

	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
	{
		StatModifier @default = StatModifier.Default;
		@default.Base = -DeathFruitHealth;
		health = @default;
		mana = StatModifier.Default;
	}

	public void QuickUseItemAt(int index, bool use = true)
	{
		if (!autoRevertSelectedItem && base.Player.selectedItem != index && base.Player.inventory[index].type != 0)
		{
			originalSelectedItem = base.Player.selectedItem;
			autoRevertSelectedItem = true;
			base.Player.selectedItem = index;
			base.Player.controlUseItem = true;
			if (use && CombinedHooks.CanUseItem(base.Player, base.Player.inventory[base.Player.selectedItem]) && base.Player.whoAmI == Main.myPlayer)
			{
				base.Player.ItemCheck();
			}
		}
	}
}
