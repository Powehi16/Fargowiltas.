using Fargowiltas.Items.Ammos.Bullets;
using Fargowiltas.Items.Tiles;
using Fargowiltas.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes;

public class RecipeGroups : ModSystem
{
	internal static int AnyGoldBar;

	internal static int AnyDemonAltar;

	internal static int AnyAnvil;

	internal static int AnyHMAnvil;

	internal static int AnyForge;

	internal static int AnyBookcase;

	internal static int AnyCookingPot;

	internal static int AnyTombstone;

	internal static int AnyWoodenTable;

	internal static int AnyWoodenChair;

	internal static int AnyWoodenSink;

	internal static int AnyButterfly;

	internal static int AnySquirrel;

	internal static int AnyCommonFish;

	internal static int AnyDragonfly;

	internal static int AnyBird;

	internal static int AnyDuck;

	internal static int AnyFoodT2;

	internal static int AnyFoodT3;

	internal static int AnyGemRobe;

	public override void AddRecipeGroups()
	{
		RecipeGroup group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ModContent.ItemType<SilverPouch>()), ModContent.ItemType<SilverPouch>(), ModContent.ItemType<TungstenPouch>());
		RecipeGroup.RegisterGroup("Fargowiltas:AnySilverPouch", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(19), 19, 706);
		AnyGoldBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyGoldBar", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(57), 57, 1257);
		AnyGoldBar = RecipeGroup.RegisterGroup("Fargowiltas:AnyDemoniteBar", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(ModContent.ItemType<DemonAltar>()), ModContent.ItemType<DemonAltar>(), ModContent.ItemType<CrimsonAltar>());
		AnyDemonAltar = RecipeGroup.RegisterGroup("Fargowiltas:AnyDemonAltar", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(35), 35, 716);
		AnyAnvil = RecipeGroup.RegisterGroup("Fargowiltas:AnyAnvil", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(525), 525, 1220);
		AnyHMAnvil = RecipeGroup.RegisterGroup("Fargowiltas:AnyHMAnvil", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(524), 524, 1221);
		AnyForge = RecipeGroup.RegisterGroup("Fargowiltas:AnyForge", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(354), 354, 1414, 2138, 2554, 2020, 3917, 2233, 2021, 2022, 2031, 2025, 2137, 1512, 3167, 1415, 2023, 2135, 3166, 3165, 2540, 1463, 2536, 2027, 1416, 2670, 2026, 2136, 2029, 2569, 2028, 2024, 5192);
		AnyBookcase = RecipeGroup.RegisterGroup("Fargowiltas:AnyBookcase", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(345), 345, 1791);
		AnyCookingPot = RecipeGroup.RegisterGroup("Fargowiltas:AnyCookingPot", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.87", isVanillaKey: true), 2001, 1994, 1995, 1996, 1998, 1999, 1997, 2000, 4845);
		AnyButterfly = RecipeGroup.RegisterGroup("Fargowiltas:AnyButterfly", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(2018), 2018, 3563);
		AnySquirrel = RecipeGroup.RegisterGroup("Fargowiltas:AnySquirrel", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("CommonFish"), 2299, 2290, 2297, 2301, 2298, 2300);
		AnyCommonFish = RecipeGroup.RegisterGroup("Fargowiltas:AnyCommonFish", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("LegacyMisc.105", isVanillaKey: true), 4334, 4335, 4336, 4337, 4338, 4339);
		AnyDragonfly = RecipeGroup.RegisterGroup("Fargowiltas:AnyDragonfly", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(2015), 2015, 2016, 2017, 2123, 2122, 4374, 4359);
		AnyBird = RecipeGroup.RegisterGroup("Fargowiltas:AnyBird", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(2123), 2123, 2122, 4374);
		AnyDuck = RecipeGroup.RegisterGroup("Fargowiltas:AnyDuck", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(321), 321, 1174, 1175, 1173, 1176, 1177, 3229, 3230, 3231, 3232, 3233);
		AnyTombstone = RecipeGroup.RegisterGroup("Fargowiltas:AnyTombstone", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(32), 32, 677, 5207, 639, 829, 640, 1816, 638, 917, 2532, 2259, 4583);
		AnyWoodenTable = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenTable", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(34), 34, 2557, 5196, 629, 806, 630, 1814, 628, 915, 2524, 2228, 4572);
		AnyWoodenChair = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenChair", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText(2827), 2827, 2852, 5205, 2829, 2833, 2830, 2847, 2828, 2835, 2850, 2849, 4581);
		AnyWoodenSink = RecipeGroup.RegisterGroup("Fargowiltas:AnyWoodenSink", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT2"), 357, 2426, 1787, 2427, 4019, 5093, 3195, 4403, 5092, 4623, 4032, 4034, 4012, 4016, 4017, 4018, 4020, 4021, 4026, 4028, 4035, 5042);
		AnyFoodT2 = RecipeGroup.RegisterGroup("Fargowiltas:AnyFoodT2", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("FoodT3"), 4022, 4615, 4027, 4029, 4036, 4037, 4025, 4011, 3532, 1920, 4013, 1919, 1911);
		AnyFoodT3 = RecipeGroup.RegisterGroup("Fargowiltas:AnyFoodT3", group);
		group = new RecipeGroup(() => RecipeHelper.GenerateAnyItemRecipeGroupText("GemRobe"), 4256, 1282, 1287, 1285, 1286, 1284, 1283);
		AnyGemRobe = RecipeGroup.RegisterGroup("Fargowiltas:AnyGemRobe", group);
	}
}
