using CodeEasier.Polish;
using CodeEasier.Scene;
using Match3.Levels.Tiles;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Forces
{
    class ForceAxe : Force
    {

        private Rectangle axeRect;
        private Main BaseGame;

        private bool toBreak;
        private Tile tileToBreak;
        private float angle;

        private MouseState oldMouseState;

        public ForceAxe(Main baseGame) : base("axe", "force_axe_title", Assets.ForceAxeButton)
        {
            BaseGame = baseGame;
            oldMouseState = Mouse.GetState();
            toBreak = false;
            tileToBreak = null;
        }

        public override void Update(GameTime gameTime, SceneLevel scene)
        {

            MouseState newMouseState = Mouse.GetState();

            if (!toBreak)
            {
                axeRect = new Rectangle(newMouseState.X + axeRect.Width, newMouseState.Y + (int)Math.Round(axeRect.Height / 1.5), BaseGame.game.ScreenWidth / 13, BaseGame.game.ScreenHeight / 7);
                BaseGame.game.ShowCursor = false;
            }
            else
            {
                axeRect = new Rectangle(tileToBreak.X + ((int)Math.Round(axeRect.Width * 1.3)), tileToBreak.Y + ((int)Math.Round(axeRect.Height / 1.7)), BaseGame.game.ScreenWidth / 13, BaseGame.game.ScreenHeight / 7);
                BaseGame.game.ShowCursor = true;
            }

            // detect click
            if (!toBreak)
            {
                int mouseRow = (int)Math.Floor((newMouseState.Y - (scene.tileYOffset + scene.tileScrollY)) / scene.tileHeight);
                int mouseCol = (newMouseState.X - scene.tileXOffset) / scene.tileWidth;

                if (mouseRow >= 0 && mouseRow < scene.tiles.Count)
                {
                    if (mouseCol >= 0 && mouseCol < scene.tiles[mouseRow].Count)
                    {
                        if (scene.tiles[mouseRow][mouseCol] != null)
                        {

                            scene.tiles[mouseRow][mouseCol].selected = true;

                            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                            {
                                toBreak = true;
                                tileToBreak = scene.tiles[mouseRow][mouseCol];
                                Assets.SndForceAxe.Play();

                            }
                        }
                        else
                        {
                            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !button.Rect.Contains(newMouseState.Position))
                            {
                                Used = false;
                                toBreak = false;
                                BaseGame.game.ShowCursor = true;
                            }
                        }
                    }
                    else
                    {
                        if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !button.Rect.Contains(newMouseState.Position))
                        {
                            Used = false;
                            toBreak = false;
                            BaseGame.game.ShowCursor = true;
                        }
                    }
                }
                else
                {
                    if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !button.Rect.Contains(newMouseState.Position))
                    {
                        Used = false;
                        toBreak = false;
                        BaseGame.game.ShowCursor = true;
                    }
                }
            }

            // update angle
            if (toBreak)
            {
                angle -= (float)gameTime.ElapsedGameTime.TotalSeconds * 200;

                if (angle <= -50)
                {
                    int row = (int)Math.Round((tileToBreak.Y - (scene.tileYOffset + scene.tileScrollY - tileToBreak.YOff)) / scene.tileHeight);
                    int col = (int)Math.Floor((tileToBreak.X - scene.tileXOffset - tileToBreak.XOff) / scene.tileWidth);

                    Random r = new Random();

                    CEParticle pw = new CEParticle(Assets.FXWaveCircle, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 3, tileToBreak.Height / 3, 1, 0, 0, 0);
                    pw.AlphaAmount = 1;
                    pw.GrowingAmount = 30;
                    pw.Center = true;

                    scene.emitter1.AddParticle(pw);

                    scene.tiles[row][col].toRemove = true;
                    scene.tiles[row][col].removeCounter = 1;

                    scene.emitterL.AddParticle(Assets.FXL1, tileToBreak.X - tileToBreak.Width / 2, tileToBreak.Y - tileToBreak.Height / 2, tileToBreak.Width * 2, tileToBreak.Height * 2, .5f, 0, 0, 0);

                    scene.AddCircleParticle(25, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 2, tileToBreak.Height / 2, 1, tileToBreak.Width / 3, tileToBreak.Width * 3);
                    scene.AddCircleParticle(30, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 3, tileToBreak.Height / 3, 1.2f, tileToBreak.Width / 3, tileToBreak.Width * 2);

                    toBreak = false;

                    Done = true;

                }

            }

            oldMouseState = newMouseState;

            base.Update(gameTime, scene);
        }

        public override void Draw()
        {

            BaseGame.spriteBatch.Draw(Assets.ForceAxe, null, axeRect, null, new Vector2(Assets.ForceAxe.Width, Assets.ForceAxe.Height), MathHelper.ToRadians(angle), null, Color.White, SpriteEffects.FlipHorizontally, 0);
            
            base.Draw();
        }

    }
}
