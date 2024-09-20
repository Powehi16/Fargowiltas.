using System.Linq;
using Fargowiltas.Items.Summons;
using Fargowiltas.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems.Recipes;

public class MiscRecipeSystem : ModSystem
{
	public override void AddRecipes()
	{
		AddStatueRecipes();
		AddMiscRecipes();
	}

	public override void PostAddRecipes()
	{
		foreach (Recipe recipe in Main.recipe.Where((Recipe recipe) => recipe.HasIngredient(2218)))
		{
			if (recipe.TryGetIngredient(1316, out var head))
			{
				recipe.RemoveIngredient(head);
				recipe.AddIngredient(1001);
			}
			if (recipe.TryGetIngredient(1317, out var body))
			{
				recipe.RemoveIngredient(body);
				recipe.AddIngredient(1004);
			}
			if (recipe.TryGetIngredient(1318, out var legs))
			{
				recipe.RemoveIngredient(legs);
				recipe.AddIngredient(1005);
			}
		}
		foreach (Recipe recipe in Main.recipe.Where((Recipe recipe) => recipe.createItem.ModItem != null && recipe.createItem.ModItem is BaseSummon))
		{
			recipe.DisableDecraft();
		}
	}

	private static void AddStatueRecipes()
	{
		AddStatueRecipe(443, 1621);
		AddStatueRecipe(463, 1630);
		AddStatueRecipe(454, 1634);
		AddStatueRecipe(459, 1665);
		AddStatueRecipe(478, 1675);
		AddStatueRecipe(2672, 1680);
		AddStatueRecipe(446, 1681);
		AddStatueRecipe(3712, 1681);
		AddStatueRecipe(440, 1683);
		AddStatueRecipe(3708, 1685);
		AddStatueRecipe(3709, 1691);
		AddStatueRecipe(3710, 3410);
		AddStatueRecipe(3711, 1699);
		AddStatueRecipe(3713, 2988);
		AddStatueRecipe(3714, 3405);
		AddStatueRecipe(3715, 1658);
		AddStatueRecipe(3716, 1674);
		AddStatueRecipe(3717, 3406);
		AddStatueRecipe(3718, 3408);
		AddStatueRecipe(3720, 3409);
		AddStatueRecipe(453, 166, 99);
		AddStatueRecipe(473, 29, 6);
		AddStatueRecipe(438, 109, 6);
		AddStatueRecipe(3719, 1701);
		AddStatueRecipe(466, 1641);
		AddStatueRecipe(471, 1639);
		AddStatueRecipe(441, 1654);
		AddStatueRecipe(452, 1661);
		AddStatueRecipe(449, 1664);
		AddStatueRecipe(442);
		AddStatueRecipe(468);
		AddStatueRecipe(465);
		AddStatueRecipe(461);
		AddStatueRecipe(462);
		AddStatueRecipe(460);
		AddStatueRecipe(455);
		AddStatueRecipe(469);
		AddStatueRecipe(457);
		AddStatueRecipe(475);
		AddStatueRecipe(439);
		AddStatueRecipe(456);
		AddStatueRecipe(52);
		AddStatueRecipe(458);
		AddStatueRecipe(450);
		AddStatueRecipe(451);
		AddStatueRecipe(472);
		AddStatueRecipe(474);
		AddStatueRecipe(447);
		AddStatueRecipe(448);
		AddStatueRecipe(467);
		AddStatueRecipe(1154, 1667, 1, isLihzahrdStatue: true);
		AddStatueRecipe(1152, 1667, 1, isLihzahrdStatue: true);
		AddStatueRecipe(1153, 1667, 1, isLihzahrdStatue: true);
		Recipe recipe = Recipe.Create(476);
		recipe.AddIngredient(355);
		recipe.AddIngredient(2351);
		recipe.AddIngredient(3, 50);
		recipe.AddTile(283);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(477);
		recipe.AddIngredient(355);
		recipe.AddIngredient(2351);
		recipe.AddIngredient(3, 50);
		recipe.AddTile(283);
		recipe.DisableDecraft();
		recipe.Register();
	}

	private static void AddStatueRecipe(int statue, int extraIngredient = -1, int extraIngredientAmount = 1, bool isLihzahrdStatue = false)
	{
		Recipe recipe = Recipe.Create(statue);
		if (extraIngredient != -1)
		{
			recipe.AddIngredient(extraIngredient, extraIngredientAmount);
		}
		recipe.AddIngredient(isLihzahrdStatue ? 1101 : 3, 50);
		recipe.AddTile(283);
		recipe.DisableDecraft();
		recipe.Register();
	}

	private static void AddMiscRecipes()
	{
		RecipeHelper.CreateSimpleRecipe(724, 989, 125, 1, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(1725, 1799, 304, 500, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(2338, 753, 304, 5, 1, true, false);
		RecipeHelper.CreateSimpleRecipe(316, 5114, 85, 5, 1, true, false, Condition.InGraveyard);
		RecipeHelper.CreateSimpleRecipe(989, 4144, 125, 1, 1, true, false, Condition.Hardmode);
		Recipe recipe = Recipe.Create(4837);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(999, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4831);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(181, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4836);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(182, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4834);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(179, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4835);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(178, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4833);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(177, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4832);
		recipe.AddRecipeGroup(RecipeGroups.AnySquirrel);
		recipe.AddIngredient(180, 5);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(3017);
		recipe.AddIngredient(54);
		recipe.AddIngredient(313);
		recipe.AddIngredient(315);
		recipe.AddIngredient(2358);
		recipe.AddIngredient(314);
		recipe.AddIngredient(317);
		recipe.AddIngredient(316);
		recipe.AddIngredient(318);
		recipe.AddTile(304);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(2196);
		recipe.AddIngredient(332);
		recipe.AddIngredient(210, 10);
		recipe.AddTile(18);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4281);
		recipe.AddIngredient(9, 10);
		recipe.AddIngredient(2015);
		recipe.AddTile(304);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(29);
		recipe.AddIngredient(183, 15);
		recipe.AddIngredient(188, 3);
		recipe.AddIngredient(331, 3);
		recipe.AddTile(26);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(208);
		recipe.AddIngredient(223);
		recipe.AddIngredient(1115);
		recipe.AddTile(304);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(223);
		recipe.AddIngredient(208);
		recipe.AddIngredient(1116);
		recipe.AddTile(304);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(1242);
		recipe.AddIngredient(999, 15);
		recipe.AddIngredient(1992);
		recipe.AddTile(96);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(223);
		recipe.AddIngredient(314, 15);
		recipe.AddIngredient(109);
		recipe.AddTile(355);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(857);
		recipe.AddIngredient(169, 50);
		recipe.AddIngredient(31);
		recipe.AddTile(355);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(1552);
		recipe.AddIngredient(1006);
		recipe.AddIngredient(783);
		recipe.AddTile(247);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(939);
		recipe.AddIngredient(84);
		recipe.AddIngredient(3080, 8);
		recipe.AddTile(96);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(857);
		recipe.AddIngredient(848);
		recipe.AddIngredient(866);
		recipe.AddIngredient(73, 10);
		recipe.AddTile(114);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(934);
		recipe.AddIngredient(848);
		recipe.AddIngredient(866);
		recipe.AddIngredient(73, 10);
		recipe.AddTile(114);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(1863);
		recipe.AddIngredient(399);
		recipe.AddIngredient(215);
		recipe.AddTile(114);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(3250);
		recipe.AddIngredient(1250);
		recipe.AddIngredient(215);
		recipe.AddTile(114);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4951);
		recipe.AddIngredient(4919);
		recipe.AddIngredient(4916);
		recipe.AddIngredient(4875);
		recipe.AddIngredient(4921);
		recipe.AddIngredient(4918);
		recipe.AddIngredient(4876);
		recipe.AddIngredient(4920);
		recipe.AddIngredient(4917);
		recipe.AddIngredient(74);
		recipe.AddTile(26);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4368);
		recipe.AddIngredient(4367);
		recipe.AddIngredient(4371);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
		recipe = Recipe.Create(4370);
		recipe.AddIngredient(4369);
		recipe.AddIngredient(4371);
		recipe.AddTile(220);
		recipe.DisableDecraft();
		recipe.Register();
	}
}
