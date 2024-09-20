using Fargowiltas.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes;

public class ContainerRecipeSystem : ModSystem
{
	public override void AddRecipes()
	{
		AddPreHMTreasureBagRecipes();
		AddHMTreasureBagRecipes();
		AddEventTreasureBagRecipes();
		AddGrabBagRecipes();
		AddCrateRecipes();
		AddBiomeKeyRecipes();
		CreateTreasureGroupRecipe(5010, 274, 3019, 218, 220);
		RecipeHelper.CreateSimpleRecipe(5010, 112, 220, 1, 1, true, false, Condition.NotRemixWorld);
	}

	private static void AddPreHMTreasureBagRecipes()
	{
		CreateTreasureGroupRecipe(2489, 1309);
		CreateTreasureGroupRecipe(1360, 1299);
		CreateTreasureGroupRecipe(1361, 994);
		CreateTreasureGroupRecipe(1362, 3060);
		CreateTreasureGroupRecipe(1363, 1313);
		CreateTreasureGroupRecipe(3322, 2888, 1121, 1123);
		CreateTreasureGroupRecipe(1364, 2502, 1170);
		CreateTreasureGroupRecipe(5111, 5117, 5118, 5119, 5095);
		CreateTreasureGroupRecipe(5108, 5098, 5101, 5113);
		CreateTreasureGroupRecipe(3324, 491, 489, 2998, 490, 434, 426, 514, 4912);
	}

	private static void AddHMTreasureBagRecipes()
	{
		CreateTreasureGroupRecipe(4957, 4758, 4981, 4980);
		CreateTreasureGroupRecipe(3328, 758, 1157, 1255, 788, 1178, 3018, 1259, 1155);
		CreateTreasureGroupRecipe(1370, 1305, 1182);
		CreateTreasureGroupRecipe(3329, 1258, 1122, 899, 1248, 1294, 1295, 1296, 1297);
		CreateTreasureGroupRecipe(3330, 2611, 2624, 2622, 2621, 2623);
		CreateTreasureGroupRecipe(2589, 2609);
		CreateTreasureGroupRecipe(4782, 4923, 4914, 4952, 4953, 4778);
		CreateTreasureGroupRecipe(4783, 4715, 5075, 4823);
		CreateTreasureGroupRecipe(3332, 3063, 3389, 3065, 1553, 3930, 3541, 3570, 3571, 3569);
		CreateTreasureGroupRecipe(3595, 4469);
	}

	private static void AddEventTreasureBagRecipes()
	{
		CreateTreasureGroupRecipe(3867, 3857, 3855, 3810, 3809);
		CreateTreasureGroupRecipe(3868, 3811, 3812, 3854, 3852, 3823, 3835, 3836, 3856);
		CreateTreasureGroupRecipe(3860, 3883, 3827, 3870, 3858, 3859);
		CreateTreasureGroupRecipe(1855, 1829, 1831, 1835, 1837, 1845);
		CreateTreasureGroupRecipe(1856, 1826, 1801, 1802, 1782, 1784, 1811, 4680);
		CreateTreasureGroupRecipe(1962, 1928, 1916, 1930, 1871);
		CreateTreasureGroupRecipe(1961, 1910, 1929);
		CreateTreasureGroupRecipe(1960, 1931, 1946, 1947, 1959, 1914);
		CreateTreasureGroupRecipe(3358, 2797, 2749, 2795, 2796, 2880, 2769, 2800, 2882, 2798);
		CreateTreasureGroupRecipe(3359, 855, 854, 905, 2584, 3033, 672, 4471);
	}

	private static void AddBiomeKeyRecipes()
	{
		RecipeHelper.CreateSimpleRecipe(1535, 1569, 134, 1, 1, true, false, Condition.DownedPlantera);
		RecipeHelper.CreateSimpleRecipe(1534, 1571, 134, 1, 1, true, false, Condition.DownedPlantera);
		RecipeHelper.CreateSimpleRecipe(1533, 1156, 134, 1, 1, true, false, Condition.DownedPlantera);
		RecipeHelper.CreateSimpleRecipe(1537, 1572, 134, 1, 1, true, false, Condition.DownedPlantera);
		RecipeHelper.CreateSimpleRecipe(1536, 1260, 134, 1, 1, true, false, Condition.DownedPlantera);
		RecipeHelper.CreateSimpleRecipe(4714, 4607, 134, 1, 1, true, false, Condition.DownedPlantera);
	}

	private static void AddGrabBagRecipes()
	{
		RecipeHelper.CreateSimpleRecipe(1869, 1927, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1923, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1921, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1870, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1909, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1915, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1869, 1918, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1774, 1810, 18, 10, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1774, 1800, 18, 25, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1774, 1809, 18, 2, 25, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 313, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 314, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 315, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 317, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 316, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 318, 18, 1, 5, true, false);
		RecipeHelper.CreateSimpleRecipe(3093, 2358, 18, 1, 5, true, false);
	}

	private static void AddCrateRecipes()
	{
		CreateCrateRecipe(3200, 2334, 3, 3979, -1);
		CreateCrateRecipe(3201, 2334, 3, 3979, -1);
		CreateCrateRecipe(997, 2334, 3, 3979, -1);
		CreateCrateRecipe(285, 2334, 3, 3979, -1);
		CreateCrateRecipe(3068, 2334, 3, 3979, -1);
		CreateCrateRecipe(946, 2334, 3, 3979, -1);
		CreateCrateRecipe(953, 2334, 3, 3979, -1);
		CreateCrateRecipe(3084, 2334, 3, 3979, -1);
		CreateCrateRecipe(284, 2334, 3, 3979, -1);
		CreateCrateRecipe(3069, 2334, 3, 3979, -1, Condition.NotRemixWorld);
		CreateCrateRecipe(280, 2334, 3, 3979, -1);
		CreateCrateRecipe(281, 2334, 3, 3979, -1);
		CreateCrateRecipe(4341, 2334, 3, 3979, -1);
		CreateCrateRecipe(4429, 2334, 3, 3979, -1);
		CreateCrateRecipe(4427, 2334, 3, 3979, -1);
		CreateCrateRecipe(2424, -1, 3, 3979, -1);
		CreateCrateRecipe(2608, 2335, 3, 3980, -1);
		CreateCrateRecipe(2587, 2335, 3, 3980, -1);
		CreateCrateRecipe(2501, 2335, 3, 3980, -1);
		CreateCrateRecipe(53, 2335, 1, 3980, -1);
		CreateCrateRecipe(49, 2336, 1, 3981, -1);
		CreateCrateRecipe(50, 2336, 1, 3981, -1);
		CreateCrateRecipe(930, 2336, 1, 3981, -1);
		CreateCrateRecipe(54, 2336, 1, 3981, -1);
		CreateCrateRecipe(975, 2336, 1, 3981, -1);
		CreateCrateRecipe(5011, 2336, 1, 3981, -1);
		CreateCrateRecipe(29, 2336, 5, 3981, -1);
		CreateCrateRecipe(2491, -1, 5, 3981, -1);
		CreateCrateRecipe(989, 2336, 5, 3981, -1);
		CreateCrateRecipe(3064, 2336, 1, 3981, -1);
		CreateCrateRecipe(212, 3208, 1, 3987, -1);
		CreateCrateRecipe(964, 3208, 1, 3987, -1);
		CreateCrateRecipe(211, 3208, 1, 3987, -1);
		CreateCrateRecipe(213, 3208, 1, 3987, -1);
		CreateCrateRecipe(2292, 3208, 1, 3987, -1);
		CreateCrateRecipe(4426, 3208, 1, 3987, -1);
		CreateCrateRecipe(753, 3208, 5, 3987, -1);
		CreateCrateRecipe(3017, 3208, 5, 3987, -1);
		CreateCrateRecipe(2204, 3208, 5, 3987, -1);
		CreateCrateRecipe(159, 3206, 1, 3985, -1);
		CreateCrateRecipe(65, 3206, 1, 3985, -1);
		CreateCrateRecipe(4978, 3206, 1, 3985, -1);
		CreateCrateRecipe(2197, 3206, 1, 3985, -1);
		CreateCrateRecipe(158, 3206, 1, 3985, -1);
		CreateCrateRecipe(2219, 3206, 1, 3985, -1);
		CreateCrateRecipe(162, 3203, 1, 3982, -1);
		CreateCrateRecipe(111, 3203, 1, 3982, -1);
		CreateCrateRecipe(115, 3203, 1, 3982, -1);
		CreateCrateRecipe(96, 3203, 1, 3982, -1);
		CreateCrateRecipe(64, 3203, 1, 3982, -1);
		CreateCrateRecipe(800, 3204, 1, 3983, -1);
		CreateCrateRecipe(802, 3204, 1, 3983, -1);
		CreateCrateRecipe(1256, 3204, 1, 3983, -1);
		CreateCrateRecipe(1290, 3204, 1, 3983, -1);
		CreateCrateRecipe(3062, 3204, 1, 3983, -1);
		CreateCrateRecipe(165, 3205, 1, 3984, 327);
		CreateCrateRecipe(155, 3205, 1, 3984, 327);
		CreateCrateRecipe(156, 3205, 1, 3984, 327);
		CreateCrateRecipe(113, 3205, 1, 3984, 327);
		CreateCrateRecipe(157, 3205, 1, 3984, 327, Condition.NotRemixWorld);
		CreateCrateRecipe(3317, 3205, 1, 3984, 327);
		CreateCrateRecipe(164, 3205, 1, 3984, 327);
		CreateCrateRecipe(329, 3205, 1, 3984, 327);
		CreateCrateRecipe(163, 3205, 1, 3984, 327);
		CreateCrateRecipe(2192, 3205, 1, 3984, 327);
		CreateCrateRecipe(3000, 3205, 1, 3984, 327);
		CreateCrateRecipe(2999, 3205, 1, 3984, 327);
		CreateCrateRecipe(1319, 4405, 1, 4406, -1, Condition.NotRemixWorld);
		CreateCrateRecipe(987, 4405, 1, 4406, -1);
		CreateCrateRecipe(724, 4405, 1, 4406, -1);
		CreateCrateRecipe(950, 4405, 1, 4406, -1);
		CreateCrateRecipe(3199, 4405, 1, 4406, -1);
		CreateCrateRecipe(1579, 4405, 1, 4406, -1);
		CreateCrateRecipe(670, 4405, 1, 4406, -1);
		CreateCrateRecipe(2198, 4405, 1, 4406, -1);
		CreateCrateRecipe(669, 4405, 5, 4406, -1);
		CreateCrateRecipe(4055, 4407, 1, 4408, -1);
		CreateCrateRecipe(4056, 4407, 1, 4408, -1);
		CreateCrateRecipe(4061, 4407, 1, 4408, -1);
		CreateCrateRecipe(4442, 4407, 1, 4408, -1);
		CreateCrateRecipe(4062, 4407, 1, 4408, -1);
		CreateCrateRecipe(4276, 4407, 1, 4408, -1);
		CreateCrateRecipe(4263, 4407, 1, 4408, -1);
		CreateCrateRecipe(4262, 4407, 1, 4408, -1);
		CreateCrateRecipe(4066, 4407, 1, 4408, -1);
		CreateCrateRecipe(4346, 4407, 1, 4408, -1);
		CreateCrateRecipe(934, 4407, 3, 4408, -1);
		CreateCrateRecipe(857, 4407, 3, 4408, -1);
		CreateCrateRecipe(274, 4877, 1, 4878, 329);
		CreateCrateRecipe(3019, 4877, 1, 4878, 329);
		CreateCrateRecipe(218, 4877, 1, 4878, 329);
		CreateCrateRecipe(112, 4877, 1, 4878, 329, Condition.NotRemixWorld);
		CreateCrateRecipe(220, 4877, 1, 4878, 329);
		CreateCrateRecipe(5010, 4877, 1, 4878, 329);
		CreateCrateRecipe(906, 4877, 5, 4878, -1);
		CreateCrateRecipe(4551, 4877, 5, 4878, -1);
		CreateCrateRecipe(4737, 4877, 5, 4878, -1);
		CreateCrateRecipe(4828, 4877, 1, 4878, -1);
		CreateCrateRecipe(4822, 4877, 1, 4878, -1);
		CreateCrateRecipe(4881, 4877, 1, 4878, -1);
		CreateCrateRecipe(4443, 4877, 1, 4878, -1);
		CreateCrateRecipe(4824, 4877, 1, 4878, -1);
		CreateCrateRecipe(4819, 4877, 1, 4878, -1);
		CreateCrateRecipe(277, 5002, 1, 5003, -1);
		CreateCrateRecipe(186, 5002, 1, 5003, -1);
		CreateCrateRecipe(187, 5002, 1, 5003, -1);
		CreateCrateRecipe(4404, 5002, 1, 5003, -1);
		CreateCrateRecipe(863, 5002, 5, 5003, -1);
		CreateCrateRecipe(4425, 5002, 5, 5003, -1);
	}

	private static void CreateCrateRecipe(int result, int crate, int crateAmount, int hardmodeCrate, int extraItem = -1, params Condition[] conditions)
	{
		if (crate != -1)
		{
			Recipe recipe = Recipe.Create(result);
			recipe.AddIngredient(crate, crateAmount);
			if (extraItem != -1)
			{
				recipe.AddIngredient(extraItem);
			}
			recipe.AddTile(18);
			foreach (Condition condition in conditions)
			{
				recipe.AddCondition(condition);
			}
			recipe.DisableDecraft();
			recipe.Register();
		}
		if (hardmodeCrate != -1)
		{
			Recipe recipe = Recipe.Create(result);
			recipe.AddIngredient(hardmodeCrate, crateAmount);
			if (extraItem != -1)
			{
				recipe.AddIngredient(extraItem);
			}
			recipe.AddTile(18);
			foreach (Condition condition in conditions)
			{
				recipe.AddCondition(condition);
			}
			recipe.DisableDecraft();
			recipe.Register();
		}
	}

	private static void CreateTreasureGroupRecipe(int input, params int[] outputs)
	{
		foreach (int output in outputs)
		{
			RecipeHelper.CreateSimpleRecipe(input, output, 220, 1, 1, true, false);
		}
	}
}
