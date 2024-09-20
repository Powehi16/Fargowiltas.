using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Fargowiltas;

internal class DevianttDialogueTracker
{
	public static class HelpDialogueType
	{
		public static readonly byte BossOrEvent = 0;

		public static readonly byte Environment = 1;

		public static readonly byte Misc = 2;
	}

	public struct HelpDialogue
	{
		public readonly LocalizedText Message;

		public readonly byte Type;

		public readonly Predicate<string> Predicate;

		public HelpDialogue(LocalizedText message, byte type, Predicate<string> predicate)
		{
			Message = message;
			Type = type;
			Predicate = predicate;
		}

		public bool CanDisplay(string deviName)
		{
			return Predicate(deviName);
		}
	}

	public List<HelpDialogue> PossibleDialogue;

	private int lastDialogueType;

	public DevianttDialogueTracker()
	{
		PossibleDialogue = new List<HelpDialogue>();
	}

	public void AddDialogue(string messageKey, byte type, Predicate<string> predicate, string keyPath = "Fargowiltas.NPCs.Deviantt.HelpDialogue")
	{
		LocalizedText message = Language.GetText("Mods." + keyPath + "." + messageKey);
		PossibleDialogue.Add(new HelpDialogue(message, type, predicate));
	}

	public string GetDialogue(string deviName)
	{
		WeightedRandom<string> dialogueChooser = new WeightedRandom<string>();
		var (sortedDialogue, type) = SortDialogue(deviName);
		foreach (HelpDialogue item in sortedDialogue)
		{
			dialogueChooser.Add(item.Message.Value);
		}
		lastDialogueType = type;
		return dialogueChooser;
	}

	private (List<HelpDialogue> sortedDialogue, int type) SortDialogue(string deviName)
	{
		List<HelpDialogue> sortedDialogue = new List<HelpDialogue>();
		int typeChoice = 0;
		int attempts = 0;
		while (true)
		{
			attempts++;
			typeChoice = Main.rand.Next(3);
			if (typeChoice != lastDialogueType || typeChoice == HelpDialogueType.Misc)
			{
				sortedDialogue = PossibleDialogue.Where((HelpDialogue dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();
				if (sortedDialogue.Count != 0)
				{
					break;
				}
			}
			if (attempts == 100)
			{
				typeChoice = HelpDialogueType.BossOrEvent;
				sortedDialogue = PossibleDialogue.Where((HelpDialogue dialogue) => dialogue.Type == typeChoice && dialogue.CanDisplay(deviName)).ToList();
				break;
			}
		}
		return (sortedDialogue: sortedDialogue, type: typeChoice);
	}

	public void AddVanillaDialogue()
	{
		AddDialogue("DownedMutant", HelpDialogueType.BossOrEvent, (string name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? ((object)false)));
		AddDialogue("DownedAbom", HelpDialogueType.BossOrEvent, (string name) => (bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom") ?? ((object)false)) && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("DownedMutant") ?? ((object)false)));
		AddDialogue("DownedEridanus", HelpDialogueType.Misc, (string name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus") && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedAbom"));
		AddDialogue("PostML", HelpDialogueType.BossOrEvent, (string name) => NPC.downedMoonlord && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedEridanus"));
		AddDialogue("MoonLord", HelpDialogueType.BossOrEvent, (string name) => NPC.downedAncientCultist && !NPC.downedMoonlord);
		AddDialogue("Cultist", HelpDialogueType.BossOrEvent, (string name) => NPC.downedFishron && !NPC.downedAncientCultist);
		AddDialogue("Fishron", HelpDialogueType.BossOrEvent, (string name) => FargoWorld.DownedBools["betsy"] && !NPC.downedFishron);
		AddDialogue("Betsy", HelpDialogueType.BossOrEvent, (string name) => NPC.downedGolemBoss && !FargoWorld.DownedBools["betsy"]);
		AddDialogue("Golem", HelpDialogueType.BossOrEvent, (string name) => NPC.downedPlantBoss && !NPC.downedGolemBoss);
		AddDialogue("Plantera", HelpDialogueType.BossOrEvent, (string name) => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss);
		AddDialogue("Destroyer", HelpDialogueType.BossOrEvent, (string name) => Main.hardMode && !NPC.downedMechBoss1);
		AddDialogue("Twins", HelpDialogueType.BossOrEvent, (string name) => Main.hardMode && !NPC.downedMechBoss2);
		AddDialogue("Prime", HelpDialogueType.BossOrEvent, (string name) => Main.hardMode && !NPC.downedMechBoss3);
		AddDialogue("WOF", HelpDialogueType.BossOrEvent, (string name) => (bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi") && !Main.hardMode);
		AddDialogue("Deviantt", HelpDialogueType.BossOrEvent, (string name) => NPC.downedBoss3 && !(bool)ModLoader.GetMod("FargowiltasSouls").Call("DownedDevi"));
		AddDialogue("Skeletron", HelpDialogueType.BossOrEvent, (string name) => NPC.downedQueenBee && !NPC.downedBoss3);
		AddDialogue("QueenBee", HelpDialogueType.BossOrEvent, (string name) => NPC.downedBoss2 && !NPC.downedQueenBee);
		AddDialogue("Brain", HelpDialogueType.BossOrEvent, (string name) => NPC.downedBoss1 && !NPC.downedBoss2 && WorldGen.crimson);
		AddDialogue("Eater", HelpDialogueType.BossOrEvent, (string name) => NPC.downedBoss1 && !NPC.downedBoss2 && !WorldGen.crimson);
		AddDialogue("GoblinsCrimson", HelpDialogueType.BossOrEvent, (string name) => !NPC.downedGoblins && WorldGen.crimson);
		AddDialogue("GoblinsCorruption", HelpDialogueType.BossOrEvent, (string name) => !NPC.downedGoblins && !WorldGen.crimson);
		AddDialogue("EOC", HelpDialogueType.BossOrEvent, (string name) => NPC.downedSlimeKing && !NPC.downedBoss1);
		AddDialogue("Slime", HelpDialogueType.BossOrEvent, (string name) => !NPC.downedSlimeKing);
		AddDialogue("Auras", HelpDialogueType.Misc, (string name) => true);
		AddDialogue("Debuffs", HelpDialogueType.Misc, (string name) => true);
		AddDialogue("SoulToggle", HelpDialogueType.Misc, (string name) => true);
		AddDialogue("CactusDamage", HelpDialogueType.Environment, (string name) => true);
		AddDialogue("RainLightning", HelpDialogueType.Environment, (string name) => true);
		AddDialogue("LifeCrystals", HelpDialogueType.Misc, (string name) => Main.LocalPlayer.statLifeMax < 400);
		AddDialogue("Fish", HelpDialogueType.Misc, (string name) => !Main.hardMode);
		AddDialogue("Underwater", HelpDialogueType.Environment, (string name) => !Main.LocalPlayer.GetJumpState(ExtraJump.Flipper).Enabled && !Main.LocalPlayer.gills && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("MutantAntibodies") ?? ((object)false)));
		AddDialogue("Underworld", HelpDialogueType.Environment, (string name) => !Main.LocalPlayer.fireWalk && Main.LocalPlayer.lavaMax <= 0 && !Main.LocalPlayer.buffImmune[24] && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? ((object)false)));
		AddDialogue("SpaceSuffocation", HelpDialogueType.Environment, (string name) => !Main.LocalPlayer.buffImmune[68] && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? ((object)false)));
		AddDialogue("UndergroundHallow", HelpDialogueType.Environment, (string name) => Main.hardMode && !(bool)(ModLoader.GetMod("FargowiltasSouls").Call("PureHeart") ?? ((object)false)));
		AddDialogue("LifeFruit", HelpDialogueType.Misc, (string name) => Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.LocalPlayer.statLifeMax2 < 500);
	}
}
