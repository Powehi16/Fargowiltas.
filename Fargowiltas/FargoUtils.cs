using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas;

internal static class FargoUtils
{
	public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	public static bool EternityMode => Fargowiltas.ModLoaded["FargowiltasSouls"] && (bool)ModLoader.GetMod("FargowiltasSouls").Call("EternityMode");

	public static bool HasAnyItem(this Player player, params int[] itemIDs)
	{
		return itemIDs.Any((int itemID) => player.HasItem(itemID));
	}

	public static FargoPlayer GetFargoPlayer(this Player player)
	{
		return player.GetModPlayer<FargoPlayer>();
	}

	public static void AddWithCondition<T>(this List<T> list, T type, bool condition)
	{
		if (condition)
		{
			list.Add(type);
		}
	}

	public static void AddDebuffImmunities(this NPC npc, List<int> debuffs)
	{
		foreach (int buffType in debuffs)
		{
			NPCID.Sets.SpecificDebuffImmunity[npc.type][buffType] = true;
		}
	}

	public static void TryDowned(string seller, Color color, params string[] names)
	{
		TryDowned(seller, color, conditions: true, names);
	}

	public static void TryDowned(string seller, Color color, bool conditions, params string[] names)
	{
		bool update = false;
		foreach (string name in names)
		{
			if (!FargoWorld.DownedBools[name])
			{
				FargoWorld.DownedBools[name] = true;
				update = true;
			}
		}
		if (!update)
		{
			return;
		}
		seller = Language.GetTextValue("Mods.Fargowiltas.NPCs." + seller + ".DisplayName");
		string text = Language.GetTextValue("Mods.Fargowiltas.MessageInfo.NewItemUnlocked", seller);
		if (Main.netMode == 0)
		{
			if (conditions)
			{
				Main.NewText(text, color);
			}
		}
		else if (Main.netMode == 2)
		{
			if (conditions)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
			}
			NetMessage.SendData(7);
		}
	}

	public static void PrintText(string text)
	{
		PrintText(text, Color.White);
	}

	public static void PrintText(string text, Color color)
	{
		if (Main.netMode == 0)
		{
			Main.NewText(text, color);
		}
		else if (Main.netMode == 2)
		{
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
		}
	}

	public static void PrintText(string text, int r, int g, int b)
	{
		PrintText(text, new Color(r, g, b));
	}

	public static void PrintLocalization(string fargoKey, params object[] args)
	{
		PrintText(Language.GetTextValue("Mods.Fargowiltas." + fargoKey, args));
	}

	public static void PrintLocalization(string fargoKey, Color color, params object[] args)
	{
		PrintText(Language.GetTextValue("Mods.Fargowiltas." + fargoKey, args), color);
	}

	public static void SpawnBossNetcoded(Player player, int bossType)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			SoundEngine.PlaySound(in SoundID.Roar, player.position);
			if (Main.netMode != 1)
			{
				NPC.SpawnOnPlayer(player.whoAmI, bossType);
			}
			else
			{
				NetMessage.SendData(61, -1, -1, null, player.whoAmI, bossType);
			}
		}
	}
}
