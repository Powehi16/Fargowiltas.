using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace Fargowiltas.Common.Configs;

public sealed class FargoServerConfig : ModConfig
{
	public static FargoServerConfig Instance;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.TownNPCs")]
	[DefaultValue(true)]
	public bool Mutant;

	[DefaultValue(true)]
	public bool Abom;

	[DefaultValue(true)]
	public bool Devi;

	[DefaultValue(true)]
	public bool Lumber;

	[DefaultValue(true)]
	public bool Squirrel;

	[DefaultValue(true)]
	[ReloadRequired]
	public bool CatchNPCs;

	[DefaultValue(true)]
	public bool NPCSales;

	[DefaultValue(true)]
	public bool SaferBoundNPCs;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Seasons")]
	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections Halloween;

	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections Christmas;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Seeds")]
	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections DrunkWorld;

	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections BeeWorld;

	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections WorthyWorld;

	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections CelebrationWorld;

	[DefaultValue(0)]
	[DrawTicks]
	public SeasonSelections ConstantWorld;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Unlimited")]
	[DefaultValue(true)]
	public bool UnlimitedAmmo;

	[DefaultValue(true)]
	public bool UnlimitedConsumableWeapons;

	[DefaultValue(true)]
	public bool UnlimitedPotionBuffsOn120;

	private const uint maxExtraBuffSlots = 99u;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.StatMultipliers")]
	[Range(1f, 10f)]
	[Increment(0.1f)]
	[DefaultValue(1f)]
	public float EnemyHealth;

	[Range(1f, 10f)]
	[Increment(0.1f)]
	[DefaultValue(1f)]
	public float BossHealth;

	[Range(1f, 10f)]
	[Increment(0.1f)]
	[DefaultValue(1f)]
	public float EnemyDamage;

	[Range(1f, 10f)]
	[Increment(0.1f)]
	[DefaultValue(1f)]
	public float BossDamage;

	[DefaultValue(true)]
	public bool BossApplyToAllWhenAlive;

	[Header("$Mods.Fargowiltas.Configs.FargoServerConfig.Headers.Misc")]
	[Range(0u, 99u)]
	[DefaultValue(22)]
	[ReloadRequired]
	public uint ExtraBuffSlots;

	[DefaultValue(true)]
	public bool AnglerQuestInstantReset;

	[DefaultValue(true)]
	public bool ExtraLures;

	[DefaultValue(true)]
	public bool StalkerMoneyTrough;

	[DefaultValue(true)]
	public bool RottenEggs;

	[DefaultValue(true)]
	[ReloadRequired]
	public bool BannerRecipes;

	[DefaultValue(true)]
	public bool IncreaseMaxStack;

	[DefaultValue(true)]
	public bool ExtractSpeed;

	[DefaultValue(true)]
	public bool Fountains;

	[DefaultValue(true)]
	public bool PermanentStationsNearby;

	[DefaultValue(true)]
	public bool BossZen;

	[DefaultValue(true)]
	public bool PiggyBankAcc;

	[DefaultValue(true)]
	public bool FasterLavaFishing;

	[DefaultValue(true)]
	public bool TorchGodEX;

	[DefaultValue(true)]
	public bool PylonsIgnoreEvents;

	[DefaultValue(false)]
	public bool SafeTerraformers;

	[DefaultValue(false)]
	public bool DisableTombstones;

	public override ConfigScope Mode => ConfigScope.ServerSide;

	public override void OnLoaded()
	{
		Instance = this;
	}

	public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
	{
		return false;
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		ExtraBuffSlots = Utils.Clamp(ExtraBuffSlots, 0u, 99u);
	}
}
