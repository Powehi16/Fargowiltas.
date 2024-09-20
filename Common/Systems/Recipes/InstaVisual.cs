using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Fargowiltas.Common.Systems;

public class InstaVisual : ModSystem
{
	public enum DrawOrigin
	{
		Center,
		TopLeft,
		Top,
		TopRight,
		Left,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	public static void DrawInstaVisual(Player player, Vector2 drawPosition, Vector2 scale, DrawOrigin drawOrigin = DrawOrigin.Center)
	{
		InstaDrawPlayer drawPlayer = player.GetModPlayer<InstaDrawPlayer>();
		drawPlayer.Draw = true;
		drawPlayer.DrawPosition = drawPosition;
		Vector2 right = Vector2.UnitX * (scale.X * 8f - 16f);
		Vector2 left = -Vector2.UnitX * scale.X * 8f;
		Vector2 y = Vector2.UnitY * scale.Y * 8f;
		InstaDrawPlayer instaDrawPlayer = drawPlayer;
		InstaDrawPlayer instaDrawPlayer2 = instaDrawPlayer;
		Vector2 drawPosition2 = instaDrawPlayer.DrawPosition;
		if (1 == 0)
		{
		}
		Vector2 vector = drawOrigin switch
		{
			DrawOrigin.TopLeft => left - y, 
			DrawOrigin.Top => -y, 
			DrawOrigin.TopRight => right - y, 
			DrawOrigin.Left => left, 
			DrawOrigin.Right => right, 
			DrawOrigin.BottomLeft => left + y, 
			DrawOrigin.Bottom => y, 
			DrawOrigin.BottomRight => right + y, 
			_ => Vector2.Zero, 
		};
		if (1 == 0)
		{
		}
		instaDrawPlayer2.DrawPosition = drawPosition2 - vector;
		drawPlayer.Scale = scale;
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int index = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Interface Logic 1"));
		if (index != -1)
		{
			layers.Insert(index, new LegacyGameInterfaceLayer("Fargowiltas: Insta Item Visual", delegate
			{
				DrawVisual(Main.spriteBatch);
				return true;
			}));
		}
	}

	public static void DrawVisual(SpriteBatch spriteBatch)
	{
		InstaDrawPlayer drawPlayer = Main.LocalPlayer.GetModPlayer<InstaDrawPlayer>();
		if (drawPlayer.Draw)
		{
			Texture2D texture = ModContent.Request<Texture2D>("Fargowiltas/Assets/InstaVisualSquare").Value;
			Vector2 drawPos = drawPlayer.DrawPosition.ToTileCoordinates().ToWorldCoordinates() - Main.screenPosition - Vector2.One * 8f;
			drawPos -= drawPlayer.Scale * 8f;
			if (drawPlayer.Scale.X % 2f != 0f)
			{
				drawPos.X += 8f;
			}
			if (drawPlayer.Scale.Y % 2f != 0f)
			{
				drawPos.Y += 8f;
			}
			Vector2 position = drawPos;
			Color black = Color.Black;
			black.A = 100;
			spriteBatch.Draw(texture, position, null, black, 0f, Vector2.Zero, drawPlayer.Scale, SpriteEffects.None, 0f);
		}
	}
}
