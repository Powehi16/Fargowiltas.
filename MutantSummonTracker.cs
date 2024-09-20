using System;
using System.Collections.Generic;
using Fargowiltas.Items.Summons.Mutant;
using Fargowiltas.Items.Summons.VanillaCopy;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas;

internal class MutantSummonTracker
{
	public const float KingSlime = 1f;

	public const float EyeOfCthulhu = 2f;

	public const float EaterOfWorlds = 3f;

	public const float QueenBee = 4f;

	public const float Skeletron = 5f;

	public const float DeerClops = 6f;

	public const float WallOfFlesh = 7f;

	public const float QueenSlime = 8f;

	public const float TheTwins = 9f;

	public const float TheDestroyer = 10f;

	public const float SkeletronPrime = 11f;

	public const float Plantera = 12f;

	public const float Golem = 13f;

	public const float DukeFishron = 14f;

	public const float EmpressOfLight = 15f;

	public const float Betsy = 16f;

	public const float LunaticCultist = 17f;

	public const float Moonlord = 18f;

	internal List<MutantSummonInfo> SortedSummons;

	internal List<MutantSummonInfo> EventSummons;

	internal bool SummonsFinalized = false;

	public MutantSummonTracker()
	{
		Fargowiltas.summonTracker = this;
		InitializeVanillaSummons();
	}

	private void InitializeVanillaSummons()
	{
		SortedSummons = new List<MutantSummonInfo>
		{
			new MutantSummonInfo(1f, ModContent.ItemType<SlimyCrown>(), () => NPC.downedSlimeKing, Item.buyPrice(0, 5)),
			new MutantSummonInfo(2f, ModContent.ItemType<SuspiciousEye>(), () => NPC.downedBoss1, Item.buyPrice(0, 8)),
			new MutantSummonInfo(3f, ModContent.ItemType<WormyFood>(), () => NPC.downedBoss2, Item.buyPrice(0, 10)),
			new MutantSummonInfo(3f, ModContent.ItemType<GoreySpine>(), () => NPC.downedBoss2, Item.buyPrice(0, 10)),
			new MutantSummonInfo(6f, ModContent.ItemType<DeerThing2>(), () => NPC.downedDeerclops, Item.buyPrice(0, 12)),
			new MutantSummonInfo(4f, ModContent.ItemType<Abeemination2>(), () => NPC.downedQueenBee, Item.buyPrice(0, 15)),
			new MutantSummonInfo(5f, ModContent.ItemType<SuspiciousSkull>(), () => NPC.downedBoss3, Item.buyPrice(0, 15)),
			new MutantSummonInfo(7f, ModContent.ItemType<FleshyDoll>(), () => Main.hardMode, Item.buyPrice(0, 20)),
			new MutantSummonInfo(7.0001f, ModContent.ItemType<DeathBringerFairy>(), () => Main.hardMode, Item.buyPrice(0, 50)),
			new MutantSummonInfo(8f, ModContent.ItemType<JellyCrystal>(), () => NPC.downedQueenSlime, Item.buyPrice(0, 25)),
			new MutantSummonInfo(9f, ModContent.ItemType<MechEye>(), () => NPC.downedMechBoss2, Item.buyPrice(0, 40)),
			new MutantSummonInfo(10f, ModContent.ItemType<MechWorm>(), () => NPC.downedMechBoss1, Item.buyPrice(0, 40)),
			new MutantSummonInfo(11f, ModContent.ItemType<MechSkull>(), () => NPC.downedMechBoss3, Item.buyPrice(0, 40)),
			new MutantSummonInfo(11.0001f, ModContent.ItemType<MechanicalAmalgam>(), () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3, Item.buyPrice(1)),
			new MutantSummonInfo(12f, ModContent.ItemType<PlanterasFruit>(), () => NPC.downedPlantBoss, Item.buyPrice(0, 50)),
			new MutantSummonInfo(13f, ModContent.ItemType<LihzahrdPowerCell2>(), () => NPC.downedGolemBoss, Item.buyPrice(0, 60)),
			new MutantSummonInfo(15f, ModContent.ItemType<PrismaticPrimrose>(), () => NPC.downedEmpressOfLight, Item.buyPrice(0, 60)),
			new MutantSummonInfo(14f, ModContent.ItemType<TruffleWorm2>(), () => NPC.downedFishron, Item.buyPrice(0, 60)),
			new MutantSummonInfo(17f, ModContent.ItemType<CultistSummon>(), () => NPC.downedAncientCultist, Item.buyPrice(0, 75)),
			new MutantSummonInfo(18f, ModContent.ItemType<CelestialSigil2>(), () => NPC.downedMoonlord, Item.buyPrice(1)),
			new MutantSummonInfo(18.0001f, ModContent.ItemType<MutantVoodoo>(), () => NPC.downedMoonlord, Item.buyPrice(2))
		};
		EventSummons = new List<MutantSummonInfo>();
	}

	internal void FinalizeSummonData()
	{
		SortedSummons.Sort((MutantSummonInfo x, MutantSummonInfo y) => x.progression.CompareTo(y.progression));
		SummonsFinalized = true;
	}

	internal void AddSummon(float progression, int itemId, Func<bool> downed, int price)
	{
		SortedSummons.Add(new MutantSummonInfo(progression, itemId, downed, price));
	}

	internal void AddEventSummon(float progression, int itemId, Func<bool> downed, int price)
	{
		EventSummons.Add(new MutantSummonInfo(progression, itemId, downed, price));
	}
}
