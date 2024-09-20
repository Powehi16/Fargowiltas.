using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fargowiltas.Common.Configs;
using Fargowiltas.Items.Tiles;
using Fargowiltas.NPCs;
using Fargowiltas.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Fargowiltas;

public class FargoWorld : ModSystem
{
	internal static int AbomClearCD;

	internal static int WoodChopped;

	internal static bool OverloadGoblins;

	internal static bool OverloadPirates;

	internal static bool OverloadPumpkinMoon;

	internal static bool OverloadFrostMoon;

	internal static bool OverloadMartians;

	internal static bool OverloadedSlimeRain;

	internal static bool Matsuri;

	internal static bool[] CurrentSpawnRateTile;

	internal static Dictionary<string, bool> DownedBools = new Dictionary<string, bool>();

	private readonly string[] tags = new string[52]
	{
		"lumberjack", "betsy", "boss", "rareEnemy", "pinky", "undeadMiner", "tim", "doctorBones", "mimic", "wyvern",
		"runeWizard", "nymph", "moth", "rainbowSlime", "paladin", "medusa", "clown", "iceGolem", "sandElemental", "mothron",
		"mimicHallow", "mimicCorrupt", "mimicCrimson", "mimicJungle", "goblinSummoner", "flyingDutchman", "dungeonSlime", "pirateCaptain", "skeletonGun", "skeletonMage",
		"boneLee", "darkMage", "ogre", "headlessHorseman", "babyGuardian", "squirrel", "worm", "nailhead", "zombieMerman", "eyeFish",
		"bloodEel", "goblinShark", "dreadnautilus", "gnome", "redDevil", "goldenSlime", "goblinScout", "pumpking", "mourningWood", "iceQueen",
		"santank", "everscream"
	};

	public override void PreWorldGen()
	{
		SetWorldBool(FargoServerConfig.Instance.DrunkWorld, ref Main.drunkWorld);
		SetWorldBool(FargoServerConfig.Instance.BeeWorld, ref Main.notTheBeesWorld);
		SetWorldBool(FargoServerConfig.Instance.WorthyWorld, ref Main.getGoodWorld);
		SetWorldBool(FargoServerConfig.Instance.CelebrationWorld, ref Main.tenthAnniversaryWorld);
		SetWorldBool(FargoServerConfig.Instance.ConstantWorld, ref Main.dontStarveWorld);
		string[] array = tags;
		foreach (string tag in array)
		{
			DownedBools[tag] = false;
		}
		WoodChopped = 0;
	}

	private void SetWorldBool(SeasonSelections toggle, ref bool flag)
	{
		switch (toggle)
		{
		case SeasonSelections.AlwaysOn:
			flag = true;
			break;
		case SeasonSelections.AlwaysOff:
			flag = false;
			break;
		case SeasonSelections.Normal:
			break;
		}
	}

	private void ResetFlags()
	{
		AbomClearCD = 0;
		OverloadGoblins = false;
		OverloadPirates = false;
		OverloadPumpkinMoon = false;
		OverloadFrostMoon = false;
		OverloadMartians = false;
		OverloadedSlimeRain = false;
		Matsuri = false;
		string[] array = tags;
		foreach (string tag in array)
		{
			DownedBools[tag] = false;
		}
		CurrentSpawnRateTile = new bool[(Main.netMode != 2) ? 1 : 255];
	}

	public override void OnWorldLoad()
	{
		ResetFlags();
	}

	public override void OnWorldUnload()
	{
		FargoGlobalProjectile.CannotDestroyRectangle.Clear();
		ResetFlags();
	}

	public override void SaveWorldData(TagCompound tag)
	{
		List<string> downed = new List<string>();
		string[] array = tags;
		foreach (string downedTag in array)
		{
			if (DownedBools.TryGetValue(downedTag, out var down) && down)
			{
				downed.AddWithCondition(downedTag, down);
			}
		}
		tag.Add("downed", downed);
		tag.Add("matsuri", Matsuri);
	}

	public override void LoadWorldData(TagCompound tag)
	{
		IList<string> downed = tag.GetList<string>("downed");
		string[] array = tags;
		foreach (string downedTag in array)
		{
			DownedBools[downedTag] = downed.Contains(downedTag);
		}
		Matsuri = tag.Get<bool>("matsuri");
	}

	public override void NetReceive(BinaryReader reader)
	{
		string[] array = tags;
		foreach (string tag in array)
		{
			DownedBools[tag] = reader.ReadBoolean();
		}
		AbomClearCD = reader.ReadInt32();
		WoodChopped = reader.ReadInt32();
		Matsuri = reader.ReadBoolean();
		Fargowiltas.SwarmActive = reader.ReadBoolean();
	}

	public override void NetSend(BinaryWriter writer)
	{
		string[] array = tags;
		foreach (string tag in array)
		{
			writer.Write(DownedBools[tag]);
		}
		writer.Write(AbomClearCD);
		writer.Write(WoodChopped);
		writer.Write(Matsuri);
		writer.Write(Fargowiltas.SwarmActive);
	}

	public override void PostUpdateWorld()
	{
		SetWorldBool(FargoServerConfig.Instance.Halloween, ref Main.halloween);
		SetWorldBool(FargoServerConfig.Instance.Christmas, ref Main.xMas);
		SetWorldBool(FargoServerConfig.Instance.DrunkWorld, ref Main.drunkWorld);
		SetWorldBool(FargoServerConfig.Instance.BeeWorld, ref Main.notTheBeesWorld);
		SetWorldBool(FargoServerConfig.Instance.WorthyWorld, ref Main.getGoodWorld);
		SetWorldBool(FargoServerConfig.Instance.CelebrationWorld, ref Main.tenthAnniversaryWorld);
		SetWorldBool(FargoServerConfig.Instance.ConstantWorld, ref Main.dontStarveWorld);
		if (Matsuri)
		{
			LanternNight.NextNightIsLanternNight = true;
		}
		if (Main.netMode != 1 && Fargowiltas.SwarmActive && NoBosses() && !NPC.AnyNPCs(13) && !NPC.AnyNPCs(68) && !NPC.AnyNPCs(564))
		{
			Fargowiltas.SwarmActive = false;
			FargoGlobalNPC.LastWoFIndex = -1;
			FargoGlobalNPC.WoFDirection = 0;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(7);
			}
		}
		if (AbomClearCD > 0)
		{
			AbomClearCD--;
		}
		if (OverloadGoblins && Main.invasionType != 1)
		{
			OverloadGoblins = false;
		}
		if (OverloadPirates && Main.invasionType != 3)
		{
			OverloadPirates = false;
		}
		if (OverloadPumpkinMoon && !Main.pumpkinMoon)
		{
			OverloadPumpkinMoon = false;
		}
		if (OverloadFrostMoon && !Main.snowMoon)
		{
			OverloadFrostMoon = false;
		}
		if (OverloadMartians && Main.invasionType != 4)
		{
			OverloadMartians = false;
		}
		if (OverloadedSlimeRain && !Main.slimeRain)
		{
			OverloadedSlimeRain = false;
		}
	}

	public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		ref bool current = ref CurrentSpawnRateTile[0];
		bool oldSpawnRateTile = current;
		current = tileCounts[ModContent.TileType<RegalStatueSheet>()] > 0;
		if (Main.netMode == 1 && current != oldSpawnRateTile)
		{
			ModPacket packet = Fargowiltas.Instance.GetPacket();
			packet.Write((byte)1);
			packet.Write(current);
			packet.Send();
		}
	}

	public override void PreUpdateWorld()
	{
		bool rate = false;
		for (int i = 0; i < CurrentSpawnRateTile.Length; i++)
		{
			if (!CurrentSpawnRateTile[i])
			{
				continue;
			}
			Player player = Main.player[i];
			if (player.active)
			{
				if (!player.dead)
				{
					rate = true;
				}
			}
			else
			{
				CurrentSpawnRateTile[i] = false;
			}
		}
		if (rate)
		{
			Main.checkForSpawns += 81;
		}
	}

	private bool NoBosses()
	{
		return Main.npc.All((NPC i) => !i.active || !i.boss);
	}

	public override void UpdateUI(GameTime gameTime)
	{
		base.UpdateUI(gameTime);
		Fargowiltas.UserInterfaceManager.UpdateUI(gameTime);
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		base.ModifyInterfaceLayers(layers);
		Fargowiltas.UserInterfaceManager.ModifyInterfaceLayers(layers);
	}

	public override void AddRecipes()
	{
		Fargowiltas.summonTracker.FinalizeSummonData();
	}
}
