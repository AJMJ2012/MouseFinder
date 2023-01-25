using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace MouseFinder
{
	public class MouseFinder : Mod {
		public MouseFinder() {}
		public override void Load() {
			if (Main.netMode == 2) { return; }
			On.Terraria.Main.DrawInterface_12_IngameFancyUI += (DrawInterface_12_IngameFancyUI) => {
				if (DrawInterface_12_IngameFancyUI()) {
					DrawMouseFinder();
					return true;
				}
				return false;
			};
		}
		
		const float angleOffset45 = (float)(Math.PI * 45.0 / 180.0);
		const float angleOffset90 = (float)(Math.PI * 90.0 / 180.0);
		int segmentLength = 2;

		public void DrawMouseFinder() {
			try {
				if (Main.netMode != 2 && !Main.gameMenu) {
					Player player = Main.player[Main.myPlayer];
					Vector2 playerScreen = (player.Center - Main.screenPosition) / Main.UIScale;
					Vector2 cursorScreen = (Main.MouseWorld - Main.screenPosition);
					int minDistance = 32;
					int maxDistance = 96;
					int drawDistance = (maxDistance - minDistance);
					float playerDistance = Vector2.Distance(playerScreen, cursorScreen);
					float playerAngle = (float)Math.Atan2(playerScreen.Y - cursorScreen.Y, playerScreen.X - cursorScreen.X);
					float configScale = (Config.Client.Scale / 100f);
					float configAlpha = (Config.Client.Alpha / 100f);
					if (Config.Client.Method == 1) {
						float scale = 1f * configScale;
						float distance = MathHelper.Clamp(playerDistance / 2f, 0, maxDistance);
						float alpha = MathHelper.Clamp((playerDistance / (float)maxDistance) - 1, 0, 1) * configAlpha;
						if (alpha > 0 && distance > 0) {
							Vector2 cursorPos = new Vector2((float)Math.Cos(playerAngle), (float)Math.Sin(playerAngle)) * -distance + playerScreen;
							if (Main.ThickMouse) {
								for (float j = 0; j < (float)Math.Tau; j += (float)(Math.Tau / 4.0)) {
									Vector2 cursorOffset = new Vector2((float)Math.Cos(playerAngle+j), (float)Math.Sin(playerAngle+j)) * 2f;
									for (float k = 0; k < 4; k++)
										Main.spriteBatch.Draw(TextureAssets.Cursors[11].Value, cursorPos + cursorOffset, new Rectangle?(new Rectangle(0, 0, TextureAssets.Cursors[11].Value.Width, TextureAssets.Cursors[11].Value.Height)), Main.MouseBorderColor * alpha, playerAngle - angleOffset45, new Vector2(2, 2), scale, SpriteEffects.None, 0f);
								}
							}
//							Main.spriteBatch.Draw(TextureAssets.Cursors[0].Value, cursorPos + 2, new Rectangle?(new Rectangle(0, 0, TextureAssets.Cursors[0].Value.Width, TextureAssets.Cursors[0].Value.Height)), new Color(0f, 0f, 0f, 0.2f) * alpha, playerAngle - angleOffset45, new Vector2(0, 0), scale * 1.1f, SpriteEffects.None, 0f);
							for (float k = 0; k < 4; k++)
								Main.spriteBatch.Draw(TextureAssets.Cursors[0].Value, cursorPos, new Rectangle?(new Rectangle(0, 0, TextureAssets.Cursors[0].Value.Width, TextureAssets.Cursors[0].Value.Height)), Main.cursorColor * alpha, playerAngle - angleOffset45, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
						}
					}

					if (Config.Client.Method == 2) {
						float scale = 0.5f * configScale;
						for (int t = 0; t <= 1; t++) {
							bool thicc = t == 0;
							if (thicc && !Main.ThickMouse) continue;
							for (int i = 0; i < playerDistance; i += (int)(segmentLength * scale)) {
								float alpha = MathHelper.Clamp((i / (float)drawDistance) - 1, 0, 1) * configAlpha;
								if (playerDistance - i <= (playerDistance) / 2f) {
									alpha = MathHelper.Clamp(((playerDistance - i) / (float)drawDistance) - 1, 0, 1) * configAlpha;
								}
								Vector2 linePos = new Vector2((float)Math.Cos(playerAngle), (float)Math.Sin(playerAngle)) * -i + playerScreen;
								for (float j = angleOffset90; j < (thicc ? (float)Math.Tau : 1) + angleOffset90; j += (thicc ? (float)Math.Tau / 2f : 1)) {
									Vector2 lineOffset = (thicc ? new Vector2((float)Math.Cos(playerAngle+j), (float)Math.Sin(playerAngle+j)) : Vector2.Zero) * 2f * scale;
									Main.spriteBatch.Draw(TextureAssets.Extra[47].Value, linePos + lineOffset, new Rectangle?(new Rectangle(0, 2, TextureAssets.Extra[47].Value.Width, segmentLength)), (thicc ? Main.MouseBorderColor : Main.cursorColor) * 2f * alpha, playerAngle + angleOffset90, new Vector2(TextureAssets.Extra[47].Value.Width / 2f, segmentLength / 2f), scale, SpriteEffects.None, 0f);
								}
							}
						}
					}

					if (Config.Client.Method == 3) {
						float scale = 0.5f * configScale;
						for (int t = 0; t <= 1; t++) {
							bool thicc = t == 0;
							if (thicc && !Main.ThickMouse) continue;
							for (int o = 0; o <= 1; o++) {
								bool vert = o == 0;
								for (int i = 0; i < (vert ? Main.screenHeight : Main.screenWidth); i += (int)(segmentLength * scale)) {
									Vector2 linePos = new Vector2(vert ? cursorScreen.X : i, vert ? i : cursorScreen.Y);
									float distance = Vector2.Distance(linePos, Main.MouseWorld - Main.screenPosition);
									float alpha = MathHelper.Clamp((distance / (float)drawDistance) - 1, 0, 1) * MathHelper.Clamp((Vector2.Distance(playerScreen, linePos) / (float)drawDistance) - 1, 0, 1) * configAlpha;
									if (alpha > 0) {
										for (float j = (vert ? 0 : angleOffset90); j < (thicc ? (float)Math.Tau : 1) + (vert ? 0 : angleOffset90); j += (thicc ? (float)Math.Tau / 2f : 1)) {
											Vector2 lineOffset = (thicc ? new Vector2((float)Math.Cos(j), (float)Math.Sin(j)) : Vector2.Zero) * 2f * scale;
											Main.spriteBatch.Draw(TextureAssets.Extra[47].Value, linePos + lineOffset, new Rectangle?(new Rectangle(0, 2, TextureAssets.Extra[47].Value.Width, segmentLength)), (thicc ? Main.MouseBorderColor : Main.cursorColor) * 2f * alpha, (vert ? 0 : angleOffset90), new Vector2(TextureAssets.Extra[47].Value.Width / 2f, segmentLength / 2f), scale, SpriteEffects.None, 0f);
										}
									}
								}
							}
						}
					}

					if (Config.Client.Method == 4 || Config.Client.Method == 5) {
						float scale = 0.5f * configScale;
						float alpha = MathHelper.Clamp((playerDistance / (float)drawDistance) - 1, 0, 1) * configAlpha;
						float distance = MathHelper.Clamp((playerDistance - minDistance) / (float)maxDistance, 0, 1) * drawDistance;
						if (Config.Client.Method == 5) {
							alpha = 1f * configAlpha;
							distance = maxDistance;
						}
						int sides = 360;
						float baseLength = (distance / (float)sides) * (float)Math.Tau / scale;
						segmentLength = (int)baseLength;
						while (segmentLength == 0 && baseLength - segmentLength < 1 && sides >= 3) {
							sides--;
							baseLength = (distance / (float)sides) * (float)Math.Tau / scale;
							segmentLength = (int)baseLength;
						}
						segmentLength = (int)Math.Ceiling(baseLength);
						if (alpha > 0 && distance > 0) {
							for (int t = 0; t <= 1; t++) {
								bool thicc = t == 0;
								if (thicc && !Main.ThickMouse) continue;
								for (float i = 0; i < sides; i++) {
									double angle = (i / sides) * (float)Math.Tau;
									Vector2 linePos = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -distance + cursorScreen;
									if (Config.Client.Method == 5) {
										alpha = MathHelper.Clamp((Vector2.Distance(playerScreen, linePos) / (float)drawDistance) - 1, 0, 1) * configAlpha;
									}
									for (double j = angle; j < (thicc ? Math.Tau : 1) + angle; j += (thicc ? Math.Tau / 2.0 : 1)) {
										Vector2 lineOffset = (thicc ? new Vector2((float)Math.Cos(j), (float)Math.Sin(j)) : Vector2.Zero) * 2f * scale;
										Main.spriteBatch.Draw(TextureAssets.Extra[47].Value, linePos + lineOffset, new Rectangle?(new Rectangle(0, 2, TextureAssets.Extra[47].Value.Width, segmentLength)), (thicc ? Main.MouseBorderColor : Main.cursorColor) * 2f * alpha, (float)angle, new Vector2(TextureAssets.Extra[47].Value.Width / 2f, segmentLength / 2f), scale, SpriteEffects.None, 0f);
									}
								}
							}
						}
					}
				}
			}
			catch {}
		}
	}
}
