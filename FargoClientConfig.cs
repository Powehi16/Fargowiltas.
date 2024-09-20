using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader.Config;

namespace Fargowiltas.Common.Configs;

public sealed class FargoClientConfig : ModConfig
{
	public static FargoClientConfig Instance;

	[DefaultValue(true)]
	public bool ExpandedTooltips;

	[DefaultValue(false)]
	public bool HideUnlimitedBuffs;

	[DefaultValue(false)]
	public bool DoubleTapDashDisabled;

	[DefaultValue(false)]
	public bool DoubleTapSetBonusDisabled;

	[DefaultValue(1f)]
	[Slider]
	public float TransparentFriendlyProjectiles;

	[DefaultValue(0.75f)]
	[Slider]
	public float DebuffOpacity;

	[DefaultValue(0.75f)]
	[Slider]
	public float DebuffFaderRatio;

	public override ConfigScope Mode => ConfigScope.ClientSide;

	public override void OnLoaded()
	{
		Instance = this;
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		TransparentFriendlyProjectiles = Utils.Clamp(TransparentFriendlyProjectiles, 0f, 1f);
	}
}
