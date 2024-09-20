using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fargowilta;
using Fargowiltas.Common.Configs;
using Fargowiltas.Items.CaughtNPCs;
using Fargowiltas.Items.Misc;
using Fargowiltas.Items.Tiles;
using Fargowiltas.NPCs;
using Fargowiltas.Projectiles;
using Fargowiltas.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas;

public class Fargowiltas : Mod
{
	internal static MutantSummonTracker summonTracker;

	internal static DevianttDialogueTracker dialogueTracker;

	public static ModKeybind HomeKey;

	public static ModKeybind StatKey;

	public static ModKeybind DashKey;

	public static ModKeybind SetBonusKey;

	private UIManager _userInterfaceManager;

	internal static bool SwarmActive;

	internal static int SwarmKills;

	internal static int SwarmTotal;

	internal static int SwarmSpawned;

	internal static Dictionary<string, bool> ModLoaded;

	internal static Dictionary<int, string> ModRareEnemies = new Dictionary<int, string>();

	public List<StatSheetUI.Stat> ModStats;

	public List<StatSheetUI.PermaUpgrade> PermaUpgrades;

	private string[] mods;

	internal static Fargowiltas Instance;

	public static UIManager UserInterfaceManager => Instance._userInterfaceManager;

	public override uint ExtraPlayerBuffSlots => FargoServerConfig.Instance.ExtraBuffSlots;

	internal static bool IsEventOccurring => Main.invasionType != 0 || Main.pumpkinMoon || Main.snowMoon || Main.eclipse || Main.bloodMoon || Main.WindyEnoughForKiteDrops || Main.IsItRaining || Main.IsItStorming || Main.slimeRain || BirthdayParty.PartyIsUp || DD2Event.Ongoing || Sandstorm.Happening || (NPC.downedTowers && (NPC.LunarApocalypseIsUp || NPC.ShieldStrengthTowerNebula > 0 || NPC.ShieldStrengthTowerSolar > 0 || NPC.ShieldStrengthTowerStardust > 0 || NPC.ShieldStrengthTowerVortex > 0));

	public override void Load()
	{
		Instance = this;
		ModStats = new List<StatSheetUI.Stat>();
		PermaUpgrades = new List<StatSheetUI.PermaUpgrade>
		{
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5337], () => Main.LocalPlayer.usedAegisCrystal),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5338], () => Main.LocalPlayer.usedAegisFruit),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5339], () => Main.LocalPlayer.usedArcaneCrystal),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5342], () => Main.LocalPlayer.usedAmbrosia),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5341], () => Main.LocalPlayer.usedGummyWorm),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5340], () => Main.LocalPlayer.usedGalaxyPearl),
			new StatSheetUI.PermaUpgrade(ContentSamples.ItemsByType[5326], () => Main.LocalPlayer.ateArtisanBread)
		};
		summonTracker = new MutantSummonTracker();
		dialogueTracker = new DevianttDialogueTracker();
		dialogueTracker.AddVanillaDialogue();
		HomeKey = KeybindLoader.RegisterKeybind(this, "Home", "Home");
		StatKey = KeybindLoader.RegisterKeybind(this, "Stat", "RightShift");
		DashKey = KeybindLoader.RegisterKeybind(this, "Dash", "C");
		SetBonusKey = KeybindLoader.RegisterKeybind(this, "SetBonus", "V");
		_userInterfaceManager = new UIManager();
		_userInterfaceManager.LoadUI();
		mods = new string[6] { "FargowiltasSouls", "FargowiltasSoulsDLC", "ThoriumMod", "CalamityMod", "MagicStorage", "WikiThis" };
		ModLoaded = new Dictionary<string, bool>();
		string[] array = mods;
		foreach (string mod in array)
		{
			ModLoaded.Add(mod, value: false);
		}
		CaughtNPCItem.RegisterItems();
		ItemID.Sets.BannerStrength = ItemID.Sets.Factory.CreateCustomSet(new ItemID.BannerEffect(1f));
		On_Player.DoCommonDashHandle += OnVanillaDash;
		On_Player.KeyDoubleTap += OnVanillaDoubleTapSetBonus;
		On_Player.KeyHoldDown += OnVanillaHoldSetBonus;
		On_Recipe.FindRecipes += FindRecipes_ElementalAssemblerGraveyardHack;
		On_WorldGen.CountTileTypesInArea += CountTileTypesInArea_PurityTotemHack;
		On_SceneMetrics.ExportTileCountsToMain += ExportTileCountsToMain_PurityTotemHack;
		On_Player.HasUnityPotion += OnHasUnityPotion;
		On_Player.TakeUnityPotion += OnTakeUnityPotion;
		On_Player.DropTombstone += DisableTombstones;
	}

	private static IEnumerable<Item> GetWormholes(Player self)
	{
		return from x in self.inventory.Concat(self.bank.item).Concat(self.bank2.item)
			where x.type == 2997
			select x;
	}

	private static void OnTakeUnityPotion(On_Player.orig_TakeUnityPotion orig, Player self)
	{
		List<Item> wormholes = GetWormholes(self).ToList();
		if (!FargoServerConfig.Instance.UnlimitedPotionBuffsOn120 || wormholes.Select((Item x) => x.stack).Sum() < 30)
		{
			Item pot = wormholes.First();
			pot.stack--;
			if (pot.stack <= 0)
			{
				pot.SetDefaults(0, noMatCheck: false, null);
			}
		}
	}

	private static void DisableTombstones(On_Player.orig_DropTombstone orig, Player self, long coinsOwned, NetworkText deathText, int hitDirection)
	{
		if (!FargoServerConfig.Instance.DisableTombstones)
		{
			orig(self, coinsOwned, deathText, hitDirection);
		}
	}

	private static bool OnHasUnityPotion(On_Player.orig_HasUnityPotion orig, Player self)
	{
		return (from x in GetWormholes(self)
			select x.stack).Sum() > 0;
	}

	private static void FindRecipes_ElementalAssemblerGraveyardHack(On_Recipe.orig_FindRecipes orig, bool canDelayCheck)
	{
		bool oldZoneGraveyard = Main.LocalPlayer.ZoneGraveyard;
		if (!Main.gameMenu && Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<FargoPlayer>().ElementalAssemblerNearby > 0f)
		{
			Main.LocalPlayer.ZoneGraveyard = true;
		}
		orig(canDelayCheck);
		Main.LocalPlayer.ZoneGraveyard = oldZoneGraveyard;
	}

	private static void CountTileTypesInArea_PurityTotemHack(On_WorldGen.orig_CountTileTypesInArea orig, int[] tileTypeCounts, int startX, int endX, int startY, int endY)
	{
		orig(tileTypeCounts, startX, endX, startY, endY);
		if (tileTypeCounts[ModContent.TileType<PurityTotemSheet>()] > 0)
		{
			tileTypeCounts[27] += 1800;
		}
	}

	private void ExportTileCountsToMain_PurityTotemHack(On_SceneMetrics.orig_ExportTileCountsToMain orig, SceneMetrics self)
	{
		orig(self);
		if (self.GetTileCount((ushort)ModContent.TileType<PurityTotemSheet>()) > 0)
		{
			self.BloodTileCount = Math.Max(self.BloodTileCount - 9000, 0);
			self.EvilTileCount = Math.Max(self.EvilTileCount - 9000, 0);
			self.GraveyardTileCount = Math.Max(self.GraveyardTileCount - 9000, 0);
			if (self.GetTileCount(27) > 0)
			{
				self.HasSunflower = true;
			}
		}
	}

	public override void Unload()
	{
		On_Player.DoCommonDashHandle -= OnVanillaDash;
		On_Player.KeyDoubleTap -= OnVanillaDoubleTapSetBonus;
		On_Player.KeyHoldDown -= OnVanillaHoldSetBonus;
		On_Recipe.FindRecipes -= FindRecipes_ElementalAssemblerGraveyardHack;
		On_WorldGen.CountTileTypesInArea -= CountTileTypesInArea_PurityTotemHack;
		On_SceneMetrics.ExportTileCountsToMain -= ExportTileCountsToMain_PurityTotemHack;
		On_Player.HasUnityPotion -= OnHasUnityPotion;
		On_Player.TakeUnityPotion -= OnTakeUnityPotion;
		On_Player.DropTombstone -= DisableTombstones;
		summonTracker = null;
		dialogueTracker = null;
		HomeKey = null;
		StatKey = null;
		mods = null;
		ModLoaded = null;
		Instance = null;
	}

	public override void PostSetupContent()
	{
		try
		{
			string[] array = mods;
			foreach (string mod in array)
			{
				ModLoaded[mod] = ModLoader.TryGetMod(mod, out var _);
			}
		}
		catch (Exception e)
		{
			base.Logger.Error("Fargowiltas PostSetupContent Error: " + e.StackTrace + e.Message);
		}
		if (ModLoader.TryGetMod("Wikithis", out var wikithis) && !Main.dedServ)
		{
			wikithis.Call("AddModURL", this, "https://fargosmods.wiki.gg/wiki/{}");
		}
	}

	public override object Call(params object[] args)
	{
		try
		{
			switch (args[0].ToString())
			{
			case "AddIndestructibleRectangle":
				if (args[1].GetType() == typeof(Rectangle))
				{
					Rectangle rectangle = (Rectangle)args[1];
					FargoGlobalProjectile.CannotDestroyRectangle.Add(rectangle);
				}
				break;
			case "AddIndestructibleTileType":
				if (args[1].GetType() == typeof(int))
				{
					int tile = (int)args[1];
					FargoSets.Tiles.InstaCannotDestroy[tile] = true;
				}
				break;
			case "AddIndestructibleWallType":
				if (args[1].GetType() == typeof(int))
				{
					int wall = (int)args[1];
					FargoSets.Walls.InstaCannotDestroy[wall] = true;
				}
				break;
			case "AddEvilAltar":
				if (args[1].GetType() == typeof(int))
				{
					int tile = (int)args[1];
					FargoSets.Tiles.EvilAltars[tile] = true;
				}
				break;
			case "AddStat":
			{
				if (args[1].GetType() != typeof(int))
				{
					throw new Exception("Call Error (Fargo Mutant Mod AddStat): args[1] must be of type int");
				}
				if (args[2].GetType() != typeof(Func<string>))
				{
					throw new Exception("Call Error (Fargo Mutant Mod AddStat): args[2] must be of type Func<string>");
				}
				int itemID = (int)args[1];
				Func<string> TextFunction = (Func<string>)args[2];
				ModStats.Add(new StatSheetUI.Stat(itemID, TextFunction));
				break;
			}
			case "AddPermaUpgrade":
			{
				if (args[1].GetType() != typeof(Item))
				{
					throw new Exception("Call Error (Fargo Mutant Mod AddStat): args[1] must be of type Item");
				}
				if (args[2].GetType() != typeof(Func<bool>))
				{
					throw new Exception("Call Error (Fargo Mutant Mod AddStat): args[2] must be of type Func<bool>");
				}
				Item item = (Item)args[1];
				Func<bool> ConsumedFunction = (Func<bool>)args[2];
				PermaUpgrades.Add(new StatSheetUI.PermaUpgrade(item, ConsumedFunction));
				break;
			}
			case "SwarmActive":
				return SwarmActive;
			case "AddSummon":
			{
				if (summonTracker.SummonsFinalized)
				{
					throw new Exception("Call Error: Summons must be added before AddRecipes");
				}
				int itemId;
				int funcIndex;
				if (args[2].GetType() == typeof(string))
				{
					itemId = ModContent.Find<ModItem>(Convert.ToString(args[2]), Convert.ToString(args[3])).Type;
					funcIndex = 4;
				}
				else
				{
					itemId = Convert.ToInt32(args[2]);
					funcIndex = 3;
				}
				summonTracker.AddSummon(Convert.ToSingle(args[1]), itemId, args[funcIndex] as Func<bool>, Convert.ToInt32(args[funcIndex + 1]));
				break;
			}
			case "AddDevianttHelpDialogue":
				if (args[4].GetType() == typeof(string) && args[4].ToString().Length > 0)
				{
					dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>, args[4] as string);
				}
				else
				{
					dialogueTracker.AddDialogue(args[1] as string, (byte)args[2], args[3] as Predicate<string>);
				}
				break;
			case "LowRenderProj":
				((Projectile)args[1]).GetGlobalProjectile<FargoGlobalProjectile>().lowRender = true;
				break;
			case "DoubleTapDashDisabled":
				return FargoClientConfig.Instance.DoubleTapDashDisabled;
			}
		}
		catch (Exception e)
		{
			base.Logger.Error("Call Error: " + e.StackTrace + e.Message);
		}
		return base.Call(args);
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		byte messageType = reader.ReadByte();
		switch (messageType)
		{
		case 0:
			FargoNet.HandlePacket(reader, messageType);
			break;
		case 1:
			if (whoAmI >= 0 && whoAmI < FargoWorld.CurrentSpawnRateTile.Length)
			{
				FargoWorld.CurrentSpawnRateTile[whoAmI] = reader.ReadBoolean();
			}
			break;
		case 2:
			if (Main.netMode == 2 && IsEventOccurring)
			{
				TryClearEvents();
				NetMessage.SendData(7);
			}
			break;
		case 3:
			if (Main.netMode == 2)
			{
				Main.AnglerQuestSwap();
			}
			break;
		case 4:
		{
			int n = reader.ReadInt32();
			int lifeMax = reader.ReadInt32();
			if (Main.netMode == 1 && n >= 0 && n < Main.maxNPCs)
			{
				Main.npc[n].lifeMax = lifeMax;
			}
			break;
		}
		case 5:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] != null && Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<global::Fargowiltas.NPCs.SuperDummy>())
				{
					NPC npc = Main.npc[i];
					npc.life = 0;
					npc.HitEffect();
					Main.npc[i].SimpleStrikeNPC(int.MaxValue, 0, crit: false, 0f, null, damageVariation: false, 0f, noPlayerInteraction: true);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(23, -1, -1, null, i);
					}
				}
			}
			break;
		}
		case 6:
			if (Main.netMode == 2)
			{
				NetMessage.SendData(7);
			}
			break;
		case 7:
		{
			bool isBattle = reader.ReadBoolean();
			int p = reader.ReadInt32();
			bool cry = reader.ReadBoolean();
			BattleCry.GenerateText(isBattle, Main.player[p], cry);
			break;
		}
		case 8:
		{
			int p = reader.ReadInt32();
			Main.player[p].GetModPlayer<FargoPlayer>().BattleCry = reader.ReadBoolean();
			Main.player[p].GetModPlayer<FargoPlayer>().CalmingCry = reader.ReadBoolean();
			break;
		}
		case 9:
		{
			int p = reader.ReadByte();
			int deathFruitHealth = reader.ReadByte();
			if (p >= 0 && p < 255 && Main.player[p].active)
			{
				Main.player[p].GetModPlayer<FargoPlayer>().DeathFruitHealth = deathFruitHealth;
			}
			break;
		}
		}
	}

	internal static bool TryClearEvents()
	{
		bool canClearEvent = FargoWorld.AbomClearCD <= 0;
		if (canClearEvent)
		{
			if (Main.invasionType != 0)
			{
				Main.invasionType = 0;
				FargoUtils.PrintLocalization("MessageInfo.CancelEvent", new Color(175, 75, 255));
			}
			if (Main.pumpkinMoon)
			{
				Main.pumpkinMoon = false;
				FargoUtils.PrintLocalization("MessageInfo.CancelPumpkinMoon", new Color(175, 75, 255));
			}
			if (Main.snowMoon)
			{
				Main.snowMoon = false;
				FargoUtils.PrintLocalization("MessageInfo.CancelFrostMoon", new Color(175, 75, 255));
			}
			if (Main.eclipse)
			{
				Main.eclipse = false;
				FargoUtils.PrintLocalization("MessageInfo.CancelEclipse", new Color(175, 75, 255));
			}
			if (Main.bloodMoon)
			{
				Main.bloodMoon = false;
				FargoUtils.PrintLocalization("MessageInfo.CancelBloodMoon", new Color(175, 75, 255));
			}
			if (Main.WindyEnoughForKiteDrops)
			{
				Main.windSpeedTarget = 0f;
				Main.windSpeedCurrent = 0f;
				FargoUtils.PrintLocalization("MessageInfo.CancelWindyDay", new Color(175, 75, 255));
			}
			if (Main.slimeRain)
			{
				Main.StopSlimeRain();
				Main.slimeWarningDelay = 1;
				Main.slimeWarningTime = 1;
			}
			if (BirthdayParty.PartyIsUp)
			{
				BirthdayParty.CheckNight();
			}
			if (DD2Event.Ongoing && Main.netMode != 1)
			{
				DD2Event.StopInvasion();
				FargoUtils.PrintLocalization("MessageInfo.CancelOOA", new Color(175, 75, 255));
			}
			if (Sandstorm.Happening)
			{
				Sandstorm.Happening = false;
				Sandstorm.TimeLeft = 0.0;
				Sandstorm.IntendedSeverity = 0f;
				FargoUtils.PrintLocalization("MessageInfo.CancelSandstorm", new Color(175, 75, 255));
			}
			if (NPC.downedTowers && (NPC.LunarApocalypseIsUp || NPC.ShieldStrengthTowerNebula > 0 || NPC.ShieldStrengthTowerSolar > 0 || NPC.ShieldStrengthTowerStardust > 0 || NPC.ShieldStrengthTowerVortex > 0))
			{
				NPC.LunarApocalypseIsUp = false;
				NPC.ShieldStrengthTowerNebula = 0;
				NPC.ShieldStrengthTowerSolar = 0;
				NPC.ShieldStrengthTowerStardust = 0;
				NPC.ShieldStrengthTowerVortex = 0;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].active && (Main.npc[i].type == 507 || Main.npc[i].type == 517 || Main.npc[i].type == 493 || Main.npc[i].type == 422))
					{
						Main.npc[i].dontTakeDamage = false;
						Main.npc[i].GetGlobalNPC<FargoGlobalNPC>().NoLoot = true;
						Main.npc[i].StrikeInstantKill();
					}
				}
				FargoUtils.PrintLocalization("MessageInfo.CancelLunarEvent", new Color(175, 75, 255));
			}
			if (Main.IsItRaining || Main.IsItStorming)
			{
				Main.StopRain();
				Main.cloudAlpha = 0f;
				if (Main.netMode == 2)
				{
					Main.SyncRain();
				}
				FargoUtils.PrintLocalization("MessageInfo.CancelRain", new Color(175, 75, 255));
			}
			FargoWorld.AbomClearCD = 7200;
		}
		return canClearEvent;
	}

	internal static void SpawnBoss(Player player, int bossType, bool spawnMessage = true, int overrideDirection = 0, int overrideDirectionY = 0, string overrideDisplayName = "", bool namePlural = false)
	{
		if (overrideDirection == 0)
		{
			overrideDirection = ((!Main.rand.NextBool(2)) ? 1 : (-1));
		}
		if (overrideDirectionY == 0)
		{
			overrideDirectionY = -1;
		}
		Vector2 npcCenter = player.Center + new Vector2(MathHelper.Lerp(500f, 800f, (float)Main.rand.NextDouble()) * (float)overrideDirection, 800f * (float)overrideDirectionY);
		SpawnBoss(player, bossType, spawnMessage, npcCenter, overrideDisplayName, namePlural);
	}

	internal static int SpawnBoss(Player player, int bossType, bool spawnMessage = true, Vector2 npcCenter = default(Vector2), string overrideDisplayName = "", bool namePlural = false)
	{
		if (npcCenter == default(Vector2))
		{
			npcCenter = player.Center;
		}
		if (Main.netMode != 1)
		{
			int npcID = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)npcCenter.X, (int)npcCenter.Y, bossType);
			Main.npc[npcID].Center = npcCenter;
			Main.npc[npcID].netUpdate2 = true;
			if (spawnMessage)
			{
				string npcName = ((!string.IsNullOrEmpty(Main.npc[npcID].GivenName)) ? Main.npc[npcID].GivenName : overrideDisplayName);
				if (namePlural)
				{
					if (Main.netMode == 0)
					{
						Main.NewText(Language.GetTextValue("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), 175, 75);
					}
					else if (Main.netMode == 2)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Fargowiltas.MessageInfo.HaveAwoken", npcName), new Color(175, 75, 255));
					}
				}
				else if (Main.netMode == 0)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), 175, 75);
				}
				else if (Main.netMode == 2)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", npcName), new Color(175, 75, 255));
				}
			}
		}
		else
		{
			FargoNet.SendNetMessage(0, (byte)player.whoAmI, (short)bossType, spawnMessage, (int)npcCenter.X, (int)npcCenter.Y, overrideDisplayName, namePlural);
		}
		return 200;
	}

	private static void OnVanillaDash(On_Player.orig_DoCommonDashHandle orig, Player player, out int dir, out bool dashing, Player.DashStartAction dashStartAction)
	{
		if (FargoClientConfig.Instance.DoubleTapDashDisabled)
		{
			player.dashTime = 0;
		}
		orig(player, out dir, out dashing, dashStartAction);
		if (player.whoAmI != Main.myPlayer || !DashKey.JustPressed || player.CCed)
		{
			return;
		}
		InputManager modPlayer = player.GetModPlayer<InputManager>();
		if (player.controlRight && player.controlLeft)
		{
			dir = modPlayer.latestXDirPressed;
		}
		else if (player.controlRight)
		{
			dir = 1;
		}
		else if (player.controlLeft)
		{
			dir = -1;
		}
		if (dir == 0)
		{
			return;
		}
		player.direction = dir;
		dashing = true;
		if (player.dashTime > 0)
		{
			player.dashTime--;
		}
		if (player.dashTime < 0)
		{
			player.dashTime++;
		}
		if ((player.dashTime <= 0 && player.direction == -1) || (player.dashTime >= 0 && player.direction == 1))
		{
			player.dashTime = 15;
			return;
		}
		dashing = true;
		player.dashTime = 0;
		player.timeSinceLastDashStarted = 0;
		if (dashStartAction != null)
		{
			dashStartAction?.Invoke(dir);
		}
	}

	private static void OnVanillaDoubleTapSetBonus(On_Player.orig_KeyDoubleTap orig, Player player, int keyDir)
	{
		if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || SetBonusKey.JustPressed)
		{
			orig(player, keyDir);
		}
	}

	private static void OnVanillaHoldSetBonus(On_Player.orig_KeyHoldDown orig, Player player, int keyDir, int holdTime)
	{
		if (!FargoClientConfig.Instance.DoubleTapSetBonusDisabled || SetBonusKey.Current)
		{
			orig(player, keyDir, holdTime);
		}
	}
}
