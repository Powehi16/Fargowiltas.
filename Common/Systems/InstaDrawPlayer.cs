using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Fargowiltas.Common.Systems;

public class InstaDrawPlayer : ModPlayer
{
	public bool Draw = false;

	public Vector2 DrawPosition = Vector2.Zero;

	public Vector2 Scale = Vector2.Zero;

	public override void ResetEffects()
	{
		Draw = false;
		DrawPosition = Vector2.Zero;
		Scale = Vector2.Zero;
	}

	public override void UpdateDead()
	{
		ResetEffects();
	}
}
