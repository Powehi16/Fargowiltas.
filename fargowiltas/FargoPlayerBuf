using System;
using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Fargowiltas;

public class FargoPlayerBuffDrawLayer : PlayerDrawLayer
{
	private readonly int[] debuffsToIgnore = new int[12]
	{
		87, 89, 146, 157, 158, 25, 147, 28, 34, 215,
		321, 332
	};

	private Dictionary<int, Tuple<int, int>> memorizedDebuffDurations = new Dictionary<int, Tuple<int, int>>();

	public override bool IsHeadLayer => false;

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		return !Main.hideUI && drawInfo.drawPlayer.whoAmI == Main.myPlayer && drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead && !drawInfo.drawPlayer.ghost && drawInfo.shadow == 0f && FargoClientConfig.Instance.DebuffOpacity > 0f && drawInfo.drawPlayer.buffType.Count((int d) => Main.debuff[d] && !debuffsToIgnore.Contains(d)) > 0;
	}

	public override Position GetDefaultPosition()
	{
		return new Between();
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		Player player = drawInfo.drawPlayer;
		List<int> debuffs = player.buffType.Where((int d) => Main.debuff[d]).Except(debuffsToIgnore).ToList();
		int yOffset = 0;
		for (int j = 0; j < debuffs.Count; j += 10)
		{
			int maxForThisLine = Math.Min(10, debuffs.Count - j);
			float midpoint = (float)maxForThisLine / 2f - 0.5f;
			for (int i = 0; i < maxForThisLine; i++)
			{
				int debuffID = debuffs[j + i];
				Vector2 drawPos = ((player.gravDir > 0f) ? player.Top : player.Bottom);
				drawPos.Y -= (32f + (float)yOffset) * player.gravDir;
				drawPos.X += 32f * ((float)i - midpoint);
				drawPos -= player.MountedCenter;
				drawPos = drawPos.RotatedBy(0f - player.fullRotation);
				drawPos += player.MountedCenter;
				drawPos -= Main.screenPosition;
				drawPos += Vector2.UnitY * player.gfxOffY;
				if (!TextureAssets.Buff[debuffID].IsLoaded)
				{
					continue;
				}
				Texture2D buffIcon = TextureAssets.Buff[debuffID].Value;
				Color buffColor = Color.White * FargoClientConfig.Instance.DebuffOpacity;
				int index = Array.FindIndex(player.buffType, (int id) => id == debuffID);
				int currentDuration = player.buffTime[index];
				float rotation = ((player.gravDir > 0f) ? 0f : ((float)Math.PI)) - player.fullRotation;
				SpriteEffects effects = ((!(player.gravDir > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				float faderRatio = FargoClientConfig.Instance.DebuffFaderRatio;
				if (faderRatio > 0f && !Main.buffNoTimeDisplay[debuffID])
				{
					Tuple<int, int> knownDurations;
					if (currentDuration <= 1)
					{
						if (memorizedDebuffDurations.TryGetValue(debuffID, out knownDurations))
						{
							memorizedDebuffDurations.Remove(debuffID);
							buffColor *= 1f - faderRatio;
						}
					}
					else if (memorizedDebuffDurations.TryGetValue(debuffID, out knownDurations) && knownDurations.Item1 >= currentDuration && knownDurations.Item2 > currentDuration)
					{
						int maxDuration = knownDurations.Item2;
						float ratio = (float)currentDuration / (float)maxDuration;
						int x = 0;
						int y = (int)((float)buffIcon.Bounds.Height * (1f - ratio));
						int width = buffIcon.Bounds.Width;
						int height = (int)((float)buffIcon.Bounds.Height * ratio);
						if (y + height > buffIcon.Bounds.Height)
						{
							y = buffIcon.Bounds.Height - height;
						}
						Rectangle buffIconPortion = new Rectangle(x, y, width, height);
						Vector2 drawPortion = drawPos + y * Vector2.UnitY.RotatedBy(rotation);
						Color portionColor = buffColor * faderRatio;
						drawInfo.DrawDataCache.Add(new DrawData(buffIcon, drawPortion, buffIconPortion, buffColor, rotation, buffIcon.Bounds.Size() / 2f, 1f, effects));
						buffColor *= 1f - faderRatio;
						memorizedDebuffDurations[debuffID] = new Tuple<int, int>(currentDuration, maxDuration);
					}
					else
					{
						memorizedDebuffDurations[debuffID] = new Tuple<int, int>(currentDuration, currentDuration);
					}
				}
				drawInfo.DrawDataCache.Add(new DrawData(buffIcon, drawPos, buffIcon.Bounds, buffColor, rotation, buffIcon.Bounds.Size() / 2f, 1f, effects));
			}
			yOffset += (int)(32f * player.gravDir);
		}
	}
}
