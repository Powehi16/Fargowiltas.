using System;
using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Common.Configs;
using Fargowiltas.Items.Ammos.Coins;
using Fargowiltas.Items.CaughtNPCs;
using Fargowiltas.NPCs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items;

public class FargoGlobalItem : GlobalItem
{
	public class ShopTooltip
	{
		public List<int> NpcItemIDs = new List<int>();

		public List<string> NpcNames = new List<string>();

		public string Condition;
	}

	private static readonly int[] Hearts = new int[3] { 58, 1734, 1867 };

	private static readonly int[] Stars = new int[3] { 184, 1735, 1868 };

	private bool firstTick = true;

	public override bool InstancePerEntity => true;

	private static string ExpandedTooltipLoc(string line)
	{
		return Language.GetTextValue("Mods.Fargowiltas.ExpandedTooltips." + line);
	}

	public override GlobalItem Clone(Item item, Item itemClone)
	{
		return base.Clone(item, itemClone);
	}

	private TooltipLine FountainTooltip(string biome)
	{
		return new TooltipLine(base.Mod, "Tooltip0", "[i:909] [c/AAAAAA:" + ExpandedTooltipLoc("Fountain" + biome) + "]");
	}

	public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
	{
		if (weapon.type == 905)
		{
			if (ammo.type == 71 || ammo.type == ModContent.ItemType<CopperCoinBag>())
			{
				type = 158;
			}
			if (ammo.type == 72 || ammo.type == ModContent.ItemType<SilverCoinBag>())
			{
				type = 159;
			}
			if (ammo.type == 73 || ammo.type == ModContent.ItemType<GoldCoinBag>())
			{
				type = 160;
			}
			if (ammo.type == 74 || ammo.type == ModContent.ItemType<PlatinumCoinBag>())
			{
				type = 161;
			}
		}
	}

	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
	{
		FargoServerConfig fargoServerConfig = FargoServerConfig.Instance;
		if (!FargoClientConfig.Instance.ExpandedTooltips)
		{
			return;
		}
		if (FargoSets.Items.RegisteredShopTooltips[item.type] == null)
		{
			List<ShopTooltip> registeredShopTooltips = new List<ShopTooltip>();
			foreach (AbstractNPCShop shop in NPCShopDatabase.AllShops)
			{
				if (shop.NpcType == ModContent.NPCType<Squirrel>())
				{
					continue;
				}
				foreach (AbstractNPCShop.Entry entry in shop.ActiveEntries.Where((AbstractNPCShop.Entry e) => !e.Item.IsAir && e.Item.type == item.type))
				{
					Item npcItem = null;
					using (IEnumerator<KeyValuePair<int, Item>> enumerator3 = ContentSamples.ItemsByType.Where((KeyValuePair<int, Item> i) => i.Value.ModItem != null && i.Value.ModItem is CaughtNPCItem caughtNPCItem && caughtNPCItem.AssociatedNpcId == shop.NpcType).GetEnumerator())
					{
						if (enumerator3.MoveNext())
						{
							npcItem = enumerator3.Current.Value;
						}
					}
					if (npcItem == null)
					{
						npcItem = item;
					}
					string conditions = "";
					int i = 0;
					foreach (Condition condition in entry.Conditions)
					{
						string grammar = ((i > 0) ? ", " : "");
						conditions = conditions + grammar + condition.Description.Value;
						i++;
					}
					string conditionLine = ((i > 0) ? (": " + conditions) : "");
					string npcName = ContentSamples.NpcsByNetId[shop.NpcType].FullName;
					if (registeredShopTooltips.Any((ShopTooltip t) => t.NpcNames.Any((string n) => n == npcName) && t.Condition == conditionLine))
					{
						continue;
					}
					bool registered = false;
					foreach (ShopTooltip regTooltip in registeredShopTooltips)
					{
						if (regTooltip.Condition == conditionLine && !regTooltip.NpcNames.Contains(npcName))
						{
							regTooltip.NpcNames.Add(npcName);
							regTooltip.NpcItemIDs.Add(npcItem.type);
							registered = true;
							break;
						}
					}
					if (!registered)
					{
						ShopTooltip tooltip = new ShopTooltip();
						tooltip.NpcItemIDs.Add(npcItem.type);
						tooltip.NpcNames.Add(npcName);
						tooltip.Condition = conditionLine;
						registeredShopTooltips.Add(tooltip);
					}
					break;
				}
			}
			FargoSets.Items.RegisteredShopTooltips[item.type] = registeredShopTooltips;
		}
		foreach (ShopTooltip tooltip in FargoSets.Items.RegisteredShopTooltips[item.type])
		{
			List<int> displayIDs = tooltip.NpcItemIDs.Where((int i) => i != item.type)?.ToList();
			int id = item.type;
			if (displayIDs.Any())
			{
				int timer = (int)(Main.GlobalTimeWrappedHourly * 60f);
				int index = timer / 60;
				index %= displayIDs.Count;
				id = displayIDs[index];
			}
			string names = "";
			int i = 0;
			foreach (string npcName in tooltip.NpcNames)
			{
				string grammar = ((i > 0) ? ", " : "");
				names = names + grammar + npcName;
				i++;
			}
			if (i > 5)
			{
				names = ExpandedTooltipLoc("SeveralVendors");
			}
			string text = $"[i:{id}] [c/AAAAAA:{ExpandedTooltipLoc("SoldBy")} {names}{tooltip.Condition}]";
			TooltipLine line = new TooltipLine(base.Mod, "TooltipNPCSold", text);
			tooltips.Add(line);
		}
		switch (item.type)
		{
		case 909:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Ocean"));
			}
			break;
		case 910:
		case 4417:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Desert"));
			}
			break;
		case 940:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Jungle"));
			}
			break;
		case 941:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Snow"));
			}
			break;
		case 942:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Corruption"));
			}
			break;
		case 943:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Crimson"));
			}
			break;
		case 944:
			if (fargoServerConfig.Fountains)
			{
				tooltips.Add(FountainTooltip("Hallow"));
			}
			break;
		case 1991:
		case 3183:
		case 4821:
			if (fargoServerConfig.CatchNPCs)
			{
				tooltips.Add(new TooltipLine(base.Mod, "Tooltip0", "[i:1991] [c/AAAAAA:" + ExpandedTooltipLoc("CatchNPCs") + "]"));
			}
			break;
		}
		if (fargoServerConfig.ExtraLures)
		{
			if (item.type == 2354)
			{
				TooltipLine line = new TooltipLine(base.Mod, "Tooltip1", "[i:2373] [c/AAAAAA:" + ExpandedTooltipLoc("ExtraLure1") + "]");
				tooltips.Insert(3, line);
			}
			if (item.type == 2292 || item.type == 2293 || item.type == 2421 || item.type == 4442 || item.type == 4325)
			{
				TooltipLine line = new TooltipLine(base.Mod, "Tooltip1", "[i:2373] [c/AAAAAA:" + ExpandedTooltipLoc("Lures2") + "]");
				tooltips.Insert(3, line);
			}
			if (item.type == 2295 || item.type == 2296)
			{
				TooltipLine line = new TooltipLine(base.Mod, "Tooltip1", "[i:2373] [c/AAAAAA:" + ExpandedTooltipLoc("Lures3") + "]");
				tooltips.Insert(3, line);
			}
			if (item.type == 2294 || item.type == 2422)
			{
				TooltipLine line = new TooltipLine(base.Mod, "Tooltip1", "[i:2373] [c/AAAAAA:" + ExpandedTooltipLoc("Lures5") + "]");
				tooltips.Insert(3, line);
			}
		}
		if (fargoServerConfig.TorchGodEX && item.type == 5043)
		{
			TooltipLine line = new TooltipLine(base.Mod, "TooltipTorchGod1", "[i:5043] [c/AAAAAA:" + ExpandedTooltipLoc("AutoTorch") + "]");
			tooltips.Add(line);
			line = new TooltipLine(base.Mod, "TooltipTorchGod2", "[i:5043] [c/AAAAAA:" + ExpandedTooltipLoc("TrueTorchLuck") + "]");
			tooltips.Add(line);
		}
		if (fargoServerConfig.UnlimitedPotionBuffsOn120 && item.maxStack > 1)
		{
			if (item.buffType != 0)
			{
				TooltipLine line = new TooltipLine(base.Mod, "TooltipUnlim", "[i:87] [c/AAAAAA:" + ExpandedTooltipLoc("UnlimitedBuff30") + "]");
				tooltips.Add(line);
			}
			else if (item.bait > 0)
			{
				TooltipLine line = new TooltipLine(base.Mod, "TooltipUnlim", "[i:5139] [c/AAAAAA:" + ExpandedTooltipLoc("UnlimitedUse30") + "]");
				tooltips.Add(line);
			}
		}
		if (fargoServerConfig.PermanentStationsNearby && FargoSets.Items.BuffStation[item.type])
		{
			TooltipLine line = new TooltipLine(base.Mod, "TooltipUnlim", $"[i:{item.type}] [c/AAAAAA:{ExpandedTooltipLoc("PermanentEffectNearby")}]");
			tooltips.Add(line);
		}
		if (fargoServerConfig.PiggyBankAcc && (FargoSets.Items.InfoAccessory[item.type] || FargoSets.Items.MechanicalAccessory[item.type]))
		{
			TooltipLine line = new TooltipLine(base.Mod, "TooltipUnlim", "[i:87] [c/AAAAAA:" + ExpandedTooltipLoc("WorksFromBanks") + "]");
			tooltips.Add(line);
		}
		if (Squirrel.SquirrelSells(item, out var sellType) != SquirrelShopGroup.End)
		{
			TooltipLine line = new TooltipLine(base.Mod, "TooltipSquirrel", $"[i:{CaughtNPCItem.CaughtTownies[ModContent.NPCType<Squirrel>()]}] [c/AAAAAA:{ExpandedTooltipLoc(sellType.ToString())}]");
			tooltips.Add(line);
		}
	}

	public override void SetDefaults(Item item)
	{
		if (FargoServerConfig.Instance.IncreaseMaxStack && item.maxStack > 10 && item.maxStack != 100 && (item.type < 71 || item.type > 74))
		{
			item.maxStack = 9999;
		}
	}

	public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
	{
		switch (item.type)
		{
		case 3318:
			itemLoot.Add(ItemDropRule.Common(1309, 25));
			break;
		case 2334:
		{
			LeadingConditionRule leadingRule = new LeadingConditionRule(new Conditions.NotRemixSeed());
			IItemDropRule dropRuleNormal = ItemDropRule.OneFromOptions(40, 280, 281, 284, 3069);
			IItemDropRule dropRuleRemix = ItemDropRule.OneFromOptions(40, 280, 281, 284);
			leadingRule.OnSuccess(dropRuleNormal);
			leadingRule.OnFailedConditions(dropRuleRemix);
			itemLoot.Add(leadingRule);
			break;
		}
		case 2336:
			itemLoot.Add(ItemDropRule.OneFromOptions(10, 49, 50, 53, 55, 975, 930, 54, 906, 857, 934));
			itemLoot.Add(ItemDropRule.Common(3064, 20));
			break;
		}
	}

	public override void PostUpdate(Item item)
	{
		if (FargoServerConfig.Instance.Halloween == SeasonSelections.AlwaysOn && FargoServerConfig.Instance.Christmas == SeasonSelections.AlwaysOn && firstTick)
		{
			if (Array.IndexOf(Hearts, item.type) >= 0)
			{
				item.type = Hearts[Main.rand.Next(Hearts.Length)];
			}
			if (Array.IndexOf(Stars, item.type) >= 0)
			{
				item.type = Stars[Main.rand.Next(Stars.Length)];
			}
			firstTick = false;
		}
	}

	public override bool CanUseItem(Item item, Player player)
	{
		if (item.type == 424 || item.type == 1103 || item.type == 3347)
		{
			if (FargoServerConfig.Instance.ExtractSpeed && player.GetModPlayer<FargoPlayer>().extractSpeed)
			{
				item.useTime = 2;
				item.useAnimation = 3;
			}
			else
			{
				item.useTime = 10;
				item.useAnimation = 15;
			}
		}
		return base.CanUseItem(item, player);
	}

	public static void TryUnlimBuff(Item item, Player player)
	{
		if (!item.IsAir && FargoServerConfig.Instance.UnlimitedPotionBuffsOn120 && item.stack >= 30 && item.buffType != 0)
		{
			player.AddBuff(item.buffType, 2);
			if (item.type == 4478)
			{
				player.GetModPlayer<FargoPlayer>().luckPotionBoost = Math.Max(player.GetModPlayer<FargoPlayer>().luckPotionBoost, 0.1f);
			}
			else if (item.type == 4479)
			{
				player.GetModPlayer<FargoPlayer>().luckPotionBoost = Math.Max(player.GetModPlayer<FargoPlayer>().luckPotionBoost, 0.2f);
			}
		}
	}

	public static void TryPiggyBankAcc(Item item, Player player)
	{
		if (!item.IsAir && item.maxStack <= 1 && FargoServerConfig.Instance.PiggyBankAcc)
		{
			player.RefreshInfoAccsFromItemType(item);
			player.RefreshMechanicalAccsFromItemType(item.type);
		}
	}

	public override void UpdateInventory(Item item, Player player)
	{
		TryUnlimBuff(item, player);
	}

	public override void UpdateAccessory(Item item, Player player, bool hideVisual)
	{
		if (item.type != 576 || Main.curMusic <= 0 || Main.curMusic > 41)
		{
			return;
		}
		int itemId = Main.curMusic switch
		{
			1 => 562, 
			2 => 563, 
			3 => 564, 
			4 => 566, 
			5 => 567, 
			6 => 565, 
			7 => 568, 
			8 => 569, 
			9 => 571, 
			10 => 570, 
			11 => 573, 
			12 => 572, 
			13 => 574, 
			28 => 1963, 
			29 => 1610, 
			30 => 1963, 
			31 => 1964, 
			32 => 1965, 
			33 => 2742, 
			34 => 3370, 
			35 => 3236, 
			36 => 3237, 
			37 => 3235, 
			38 => 3044, 
			39 => 3371, 
			40 => 3796, 
			41 => 3869, 
			_ => 1596 + Main.curMusic - 14, 
		};
		for (int i = 0; i < player.armor.Length; i++)
		{
			Item accessory = player.armor[i];
			if (accessory.accessory && accessory.type == item.type)
			{
				player.armor[i].SetDefaults(itemId, noMatCheck: false, null);
				break;
			}
		}
	}

	public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player)
	{
		if (FargoServerConfig.Instance.UnlimitedAmmo && Main.hardMode && ammo.ammo != 0 && ammo.stack >= 3996)
		{
			return false;
		}
		return true;
	}

	public override bool? CanConsumeBait(Player player, Item bait)
	{
		if (FargoServerConfig.Instance.UnlimitedPotionBuffsOn120 && bait.stack >= 30)
		{
			return false;
		}
		return base.CanConsumeBait(player, bait);
	}

	public override bool ConsumeItem(Item item, Player player)
	{
		if (FargoServerConfig.Instance.UnlimitedConsumableWeapons && Main.hardMode && item.damage > 0 && item.ammo == 0 && item.stack >= 3996)
		{
			return false;
		}
		if (FargoServerConfig.Instance.UnlimitedPotionBuffsOn120 && (item.buffType > 0 || FargoSets.Items.NonBuffPotion[item.type]) && (item.stack >= 30 || player.inventory.Any((Item i) => i.type == item.type && !i.IsAir && i.stack >= 30)))
		{
			return false;
		}
		return true;
	}

	public override bool OnPickup(Item item, Player player)
	{
		string dye = "";
		switch (item.type)
		{
		case 1115:
			dye = "RedHusk";
			break;
		case 1114:
			dye = "OrangeBloodroot";
			break;
		case 1110:
			dye = "YellowMarigold";
			break;
		case 1112:
			dye = "LimeKelp";
			break;
		case 1108:
			dye = "GreenMushroom";
			break;
		case 1107:
			dye = "TealMushroom";
			break;
		case 1116:
			dye = "CyanHusk";
			break;
		case 1109:
			dye = "SkyBlueFlower";
			break;
		case 1111:
			dye = "BlueBerries";
			break;
		case 1118:
			dye = "PurpleMucos";
			break;
		case 1117:
			dye = "VioletHusk";
			break;
		case 1113:
			dye = "PinkPricklyPear";
			break;
		case 1119:
			dye = "BlackInk";
			break;
		}
		if (dye != "")
		{
			player.GetModPlayer<FargoPlayer>().FirstDyeIngredients[dye] = true;
		}
		if (Squirrel.SquirrelSells(item, out var _) != SquirrelShopGroup.End)
		{
			player.GetModPlayer<FargoPlayer>().ItemHasBeenOwned[item.type] = true;
		}
		return base.OnPickup(item, player);
	}

	public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
	{
		if (equippedItem.wingSlot != 0 && incomingItem.wingSlot != 0)
		{
			player.GetModPlayer<FargoPlayer>().ResetStatSheetWings();
		}
		return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
	}

	public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
	{
		player.GetModPlayer<FargoPlayer>().StatSheetMaxAscentMultiplier = maxAscentMultiplier;
		player.GetModPlayer<FargoPlayer>().CanHover = ArmorIDs.Wing.Sets.Stats[item.wingSlot].HasDownHoverStats || ArmorIDs.Wing.Sets.Stats[player.wingsLogic].HasDownHoverStats;
	}

	public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
	{
		player.GetModPlayer<FargoPlayer>().StatSheetWingSpeed = speed;
	}

	public override void GrabRange(Item item, Player player, ref int grabRange)
	{
		if (player.GetFargoPlayer().bigSuck && !ItemID.Sets.IsAPickup[item.type])
		{
			grabRange += 144000;
		}
	}

	public override bool GrabStyle(Item item, Player player)
	{
		if (player.GetFargoPlayer().bigSuck && !ItemID.Sets.IsAPickup[item.type])
		{
			item.position += (player.MountedCenter - item.Center) / 15f;
			item.position += player.position - player.oldPosition;
		}
		return base.GrabStyle(item, player);
	}
}
