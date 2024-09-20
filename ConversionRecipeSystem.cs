using Fargowiltas.Items.Summons.Mutant;
using Fargowiltas.Items.Summons.VanillaCopy;
using Fargowiltas.Utilities;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes;

public class ConversionRecipeSystem : ModSystem
{
	public override void AddRecipes()
	{
		AddSummonConversions();
		AddEvilConversions();
		AddMetalConversions();
	}

	private static void AddSummonConversions()
	{
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<FleshyDoll>(), 267, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<LihzahrdPowerCell2>(), 1293, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<TruffleWorm2>(), 2673, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<CelestialSigil2>(), 3601, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<MechEye>(), 544, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<MechWorm>(), 556, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<MechSkull>(), 557, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<GoreySpine>(), 1331, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<SlimyCrown>(), 560, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<Abeemination2>(), 1133, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<DeerThing2>(), 5120, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<WormyFood>(), 70, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<SuspiciousEye>(), 43, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<PrismaticPrimrose>(), 4961, 18, 1, 1, false, false);
		RecipeHelper.CreateSimpleRecipe(ModContent.ItemType<JellyCrystal>(), 4988, 18, 1, 1, false, false);
	}

	private static void AddEvilConversions()
	{
		AddConvertRecipe(1330, 68);
		AddConvertRecipe(86, 1329);
		AddConvertRecipe(782, 784);
		AddConvertRecipe(1332, 522);
		AddConvertRecipe(3016, 3015);
		AddConvertRecipe(3007, 3008);
		AddConvertRecipe(3023, 3020);
		AddConvertRecipe(3012, 3013);
		AddConvertRecipe(3014, 3006);
		AddConvertRecipe(115, 3062);
		AddConvertRecipe(96, 800);
		AddConvertRecipe(1290, 111);
		AddConvertRecipe(162, 802);
		AddConvertRecipe(1256, 64);
		AddConvertRecipe(836, 61);
		AddConvertRecipe(911, 619);
		AddConvertRecipe(60, 2887);
		AddConvertRecipe(3211, 3210);
		AddConvertRecipe(1569, 1571);
		AddConvertRecipe(2318, 2305);
		AddConvertRecipe(2319, 2318);
		AddConvertRecipe(3060, 994);
		AddConvertRecipe(2171, 59);
		AddConvertRecipe(1492, 1488);
		AddConvertRecipe(4284, 4285);
	}

	private static void AddMetalConversions()
	{
		AddConvertRecipe(12, 699);
		AddConvertRecipe(20, 703);
		AddConvertRecipe(11, 700);
		AddConvertRecipe(22, 704);
		AddConvertRecipe(14, 701);
		AddConvertRecipe(21, 705);
		AddConvertRecipe(13, 702);
		AddConvertRecipe(19, 706);
		AddConvertRecipe(364, 1104);
		AddConvertRecipe(381, 1184);
		AddConvertRecipe(365, 1105);
		AddConvertRecipe(382, 1191);
		AddConvertRecipe(366, 1106);
		AddConvertRecipe(391, 1198);
		AddConvertRecipe(56, 880);
		AddConvertRecipe(57, 1257);
	}

	private static void AddConvertRecipe(int itemID, int otherItemID)
	{
		RecipeHelper.CreateSimpleRecipe(itemID, otherItemID, 26, 1, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(otherItemID, itemID, 26, 1, 1, true, false);
	}
}
