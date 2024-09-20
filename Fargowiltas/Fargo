using System;
using System.Collections.Generic;
using Fargowiltas.Items;
using Fargowiltas.Items.Misc;
using Fargowiltas.Items.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas;

public class FargoSets : ModSystem
{
	public class Items
	{
		public static bool[] MechanicalAccessory;

		public static bool[] InfoAccessory;

		public static bool[] SquirrelSellsDirectly;

		public static bool[] NonBuffPotion;

		public static bool[] BuffStation;

		public static List<FargoGlobalItem.ShopTooltip>[] RegisteredShopTooltips;
	}

	public class Tiles
	{
		public static bool[] InstaCannotDestroy;

		public static bool[] DungeonTile;

		public static bool[] HardmodeOre;

		public static bool[] EvilAltars;
	}

	public class Walls
	{
		public static bool[] InstaCannotDestroy;

		public static bool[] DungeonWall;
	}

	public override void PostSetupContent()
	{
		SetFactory itemFactory = ItemID.Sets.Factory;
		Items.MechanicalAccessory = itemFactory.CreateBoolSet(false, 3619, 3611, 486, 2799, 2216, 3061, 5126, 3624, 4346, 4767, 5323, 5309, 5095);
		Items.InfoAccessory = itemFactory.CreateBoolSet(false, 15, 707, 16, 708, 17, 709, 393, 18, 395, 3123, 3124, 5358, 5359, 5360, 5361, 3121, 3119, 3102, 3099, 3118, 3120, 3037, 3096, 3084, 3095, 3036, 3122);
		int[] obj = new int[19]
		{
			3124, 5358, 5437, 5361, 5360, 5359, 1613, 1326, 5000, 5043,
			5126, 4956, 0, 0, 0, 0, 0, 0, 0
		};
		obj[12] = ModContent.ItemType<Omnistation>();
		obj[13] = ModContent.ItemType<Omnistation2>();
		obj[14] = ModContent.ItemType<CrucibleCosmos>();
		obj[15] = ModContent.ItemType<ElementalAssembler>();
		obj[16] = ModContent.ItemType<MultitaskCenter>();
		obj[17] = ModContent.ItemType<PortableSundial>();
		obj[18] = ModContent.ItemType<BattleCry>();
		Items.SquirrelSellsDirectly = itemFactory.CreateBoolSet(defaultState: false, obj);
		int[] obj2 = new int[5] { 2350, 4870, 2997, 2351, 0 };
		obj2[4] = ModContent.ItemType<BigSuckPotion>();
		Items.NonBuffPotion = itemFactory.CreateBoolSet(defaultState: false, obj2);
		Items.BuffStation = itemFactory.CreateBoolSet(false, 3198, 2177, 487, 2999, 3814);
		Items.RegisteredShopTooltips = itemFactory.CreateCustomSet<List<FargoGlobalItem.ShopTooltip>>(null, Array.Empty<object>());
		SetFactory tileFactory = TileID.Sets.Factory;
		Tiles.InstaCannotDestroy = tileFactory.CreateBoolSet(false);
		Tiles.DungeonTile = tileFactory.CreateBoolSet(false, 41, 43, 44);
		Tiles.HardmodeOre = tileFactory.CreateBoolSet(false, 107, 221, 108, 222, 111, 223);
		Tiles.EvilAltars = tileFactory.CreateBoolSet(false, 26);
		SetFactory wallFactory = WallID.Sets.Factory;
		Walls.InstaCannotDestroy = wallFactory.CreateBoolSet(false);
		Walls.DungeonWall = wallFactory.CreateBoolSet(false, 94, 95, 7, 98, 99, 8, 96, 97, 9);
	}
}
