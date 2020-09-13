using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using Match3.Levels.Tiles;
using Match3.Polish.Emitters;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Forces
{
    class ForceStorm : Force
    {

        private Main BaseGame;

        private int lLeft;

        private Rectangle lRect;
        private CEImageElement clouds;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        private Tweening tweeningClouds;

        private bool tweeningChanged;

        private CEParticleEmitter rainEmitter;
        private EmitterLForce lFEmitter;

        private MouseState oldMouseState;

        public ForceStorm(Main baseGame) : base("storm", "force_storm_title", Assets.ForceStormButton)
        {
            BaseGame = baseGame;

            clouds = new CEImageElement(Assets.Clouds, new Rectangle(0, 0, 0, 0));

            tweeningClouds = new Tweening();
            tweeningClouds.time = 0;
            tweeningClouds.duration = 1;

            rainEmitter = new CEParticleEmitter();
            lFEmitter = new EmitterLForce();

            lLeft = 4;

            oldMouseState = Mouse.GetState();

        }

        public override void Load()
        {
            Assets.StormSounds.Play();

            base.Load();
        }

        public override void Update(GameTime gameTime, SceneLevel scene)
        {

            MouseState newMouseState = Mouse.GetState();

            if (lLeft <= 0)
            {

                if (!tweeningChanged)
                {
                    tweeningClouds = new Tweening();
                    tweeningClouds.time = 0;
                    tweeningClouds.duration = 2;
                    tweeningChanged = true;
                }

                tweeningClouds.value = 0;
                tweeningClouds.distance = -BaseGame.game.ScreenHeight / 2;

                BaseGame.game.ShowCursor = true;

                if (tweeningClouds.time >= tweeningClouds.duration)
                {
                    Assets.StormSounds.Stop();
                    Used = false;
                    Done = true;
                }

            }
            else
            {

                tweeningClouds.value = -BaseGame.game.ScreenHeight / 2;
                tweeningClouds.distance = BaseGame.game.ScreenHeight / 2;
            }

            clouds.Rect = new Rectangle(0, (int)Utils.EaseOutSin(tweeningClouds.time, tweeningClouds.value, tweeningClouds.distance, tweeningClouds.duration), BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight / 3);

            if (tweeningClouds.time < tweeningClouds.duration)
                tweeningClouds.time += gameTime.ElapsedGameTime.TotalSeconds;

            // add rain drop particles
            Random r = new Random();

            int nbr = r.Next(0, 100);

            if (lLeft > 0)
            {
                if (tweeningClouds.time >= tweeningClouds.duration)
                {

                    BaseGame.game.ShowCursor = false;

                    lRect = new Rectangle(newMouseState.X, newMouseState.Y, BaseGame.game.ScreenWidth / 13, BaseGame.game.ScreenHeight / 7);

                    rainEmitter.AddParticle(Assets.FXRainDrop, r.Next(0, BaseGame.game.ScreenWidth), BaseGame.game.ScreenHeight / 6, BaseGame.game.ScreenWidth / 200, BaseGame.game.ScreenHeight / 60, 7, 0, r.Next(600, 700), 0);

                    int mouseRow = (int)Math.Floor((newMouseState.Y - (scene.tileYOffset + scene.tileScrollY)) / scene.tileHeight);
                    int mouseCol = (newMouseState.X - scene.tileXOffset) / scene.tileWidth;

                    if (mouseRow >= 0 && mouseRow < scene.tiles.Count)
                    {
                        if (mouseCol >= 0 && mouseCol < scene.tiles[mouseRow].Count)
                        {
                            if (scene.tiles[mouseRow][mouseCol] != null)
                            {

                                if (scene.tiles[mouseRow][mouseCol].activated)
                                {

                                    scene.tiles[mouseRow][mouseCol].selected = true;

                                    if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !scene.tiles[mouseRow][mouseCol].toRemove)
                                    {

                                        scene.canCascade = false;

                                        lFEmitter.AddParticle(Assets.FXForceStorm1, scene.tiles[mouseRow][mouseCol].X - scene.tiles[mouseRow][mouseCol].Width / 2, 0, scene.tiles[mouseRow][mouseCol].Width * 2, scene.tiles[mouseRow][mouseCol].Y + scene.tiles[mouseRow][mouseCol].Height / 2, 2, 0, 0, 0);
                                        scene.tiles[mouseRow][mouseCol].toRemove = true;
                                        scene.tiles[mouseRow][mouseCol].removeCounter = 5;
                                        scene.tiles[mouseRow][mouseCol].popNbr = 1;
                                        Assets.SndThunder.Play();

                                        CEParticle pl = new CEParticle(Assets.FXLight1, scene.tiles[mouseRow][mouseCol].X + scene.tiles[mouseRow][mouseCol].Width / 2, scene.tiles[mouseRow][mouseCol].Y + scene.tiles[mouseRow][mouseCol].Height / 2, scene.tiles[mouseRow][mouseCol].Width / 3, scene.tiles[mouseRow][mouseCol].Height / 3, 2, 0, 0, 0);
                                        pl.AlphaAmount = .5f;
                                        pl.GrowingAmount = 10;
                                        pl.Center = true;

                                        scene.emitter1.AddParticle(pl);

                                        scene.emitterL.AddParticle(Assets.FXL1, scene.tiles[mouseRow][mouseCol].X - scene.tiles[mouseRow][mouseCol].Width / 2, scene.tiles[mouseRow][mouseCol].Y - scene.tiles[mouseRow][mouseCol].Height / 2, scene.tiles[mouseRow][mouseCol].Width * 2, scene.tiles[mouseRow][mouseCol].Height * 2, .5f, 0, 0, 0);

                                        Random r1 = new Random();

                                        /**for (int i = 0; i < 150; i++)
                                        {
                                            scene.emitterL.AddParticle(Assets.FXDirt, r1.Next(scene.tiles[mouseRow][mouseCol].X, scene.tiles[mouseRow][mouseCol].X + scene.tiles[mouseRow][mouseCol].Width), r.Next(scene.tiles[mouseRow][mouseCol].Y, scene.tiles[mouseRow][mouseCol].Y + scene.tiles[mouseRow][mouseCol].Height), scene.tiles[mouseRow][mouseCol].Width / r.Next(3, 4), scene.tiles[mouseRow][mouseCol].Height / r.Next(3, 4), (float)r.NextDouble() * 2, r.Next(-200, 200), 300, r.Next(-60, 60));
                                        }

                                        for (int i = 0; i < 70; i++)
                                        {
                                            scene.emitter1.AddParticle(Assets.FXSmoke1, r1.Next(scene.tiles[mouseRow][mouseCol].X, scene.tiles[mouseRow][mouseCol].X + scene.tiles[mouseRow][mouseCol].Width), r.Next(scene.tiles[mouseRow][mouseCol].Y, scene.tiles[mouseRow][mouseCol].Y + scene.tiles[mouseRow][mouseCol].Height), scene.tiles[mouseRow][mouseCol].Width / r.Next(2, 3), scene.tiles[mouseRow][mouseCol].Height / r.Next(2, 3), (float)r.NextDouble() * 2, r.Next(-50, 50), -200, 0);
                                        }**/

                                        Tile tileToBreak = scene.tiles[mouseRow][mouseCol];

                                        scene.AddCircleParticle(20, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 4, tileToBreak.Height / 4, 2, tileToBreak.Width / 3, tileToBreak.Width * 4, false, new Color(255, 204, 0), true);
                                        scene.AddCircleParticle(30, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 4, tileToBreak.Height / 4, 2, tileToBreak.Width, tileToBreak.Width * 5, false, new Color(255, 230, 128), true);
                                        scene.AddCircleParticle(40, tileToBreak.X + tileToBreak.Width / 2, tileToBreak.Y + tileToBreak.Height / 2, tileToBreak.Width / 4, tileToBreak.Height / 4, 2, tileToBreak.Width * 2, tileToBreak.Width * 6, false, new Color(255, 245, 204), true);

                                        lLeft -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            rainEmitter.Update(gameTime);
            lFEmitter.Update(gameTime);

            oldMouseState = newMouseState;

            base.Update(gameTime, scene);
        }

        public override void Draw()
        {

            rainEmitter.Draw(BaseGame.spriteBatch);

            lFEmitter.Draw(BaseGame.spriteBatch);

            clouds.Draw(BaseGame.spriteBatch);

            if (lLeft > 0)
                BaseGame.spriteBatch.Draw(Assets.FXForceStormAdd, null, lRect, null, new Vector2(Assets.FXL2.Width / 2, Assets.FXL2.Height / 2), 0, null, Color.White, SpriteEffects.None, 0);

            base.Draw();
        }

    }
}
