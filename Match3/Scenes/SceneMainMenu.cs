using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using CodeEasier.Scene.UI;
using Match3.Polish.Emitters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Scenes
{
    class SceneMainMenu : CEScene
    {

        // Image elements
        CEImageElement LayerSky;
        CEImageElement LayerStars;
        CEImageElement LayerStars2;
        CEImageElement LayerClouds;
        CEImageElement LayerClouds2;
        CEImageElement LayerMountains;
        CEImageElement LayerFan;

        float starsX;
        float stars2X;
        float cloudsX;
        float clouds2X;

        bool l;

        // buttons
        CETextElement ButtonPlay;
        CETextElement ButtonHelp;
        CETextElement ButtonSettings;

        string sounded;

        // polish
        CETransition transitionIn;
        CETransition transitionClose;
        CETransition transitionOut;

        CEParticleEmitter emitter;
        EmitterBirds emitterBirds;

        bool isCloseRequest;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        Tweening tweeningFan;

        MouseState oldMouseState;

        public SceneMainMenu(Main main) : base("main_menu", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {

            LoadDrawables();

            starsX = 0;
            stars2X = 0;
            cloudsX = 0;
            clouds2X = 0;

            transitionIn = new CETransition(.5f, CETransition.Type.In);
            transitionClose = new CETransition(1, CETransition.Type.Out);
            transitionOut = new CETransition(2, CETransition.Type.Out);

            emitter = new CEParticleEmitter();
            emitterBirds = new EmitterBirds();

            tweeningFan = new Tweening();
            tweeningFan.time = 0;
            tweeningFan.duration = 1;

            //MediaPlayer.Play(Assets.MusicMainMenu);
            MediaPlayer.IsRepeating = true;

            l = false;

            sounded = "";

            isCloseRequest = false;

            oldMouseState = Mouse.GetState();

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            UpdateDrawables();

            UpdateTweenings(gameTime);

            emitter.Update(gameTime);
            emitterBirds.Update(gameTime);

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            if (isCloseRequest)
            {
                transitionOut.Update(gameTime);
                if (transitionOut.Alpha >= 1)
                {
                    BaseGame.game.ChangeScene("select_level");
                }
            }

            if (tweeningFan.time >= tweeningFan.duration)
            {
                if (!l)
                {
                    l = true;
                    stars2X = -BaseGame.game.ScreenWidth;
                    clouds2X = -BaseGame.game.ScreenWidth;
                }

                starsX += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                stars2X += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
                cloudsX += (float)gameTime.ElapsedGameTime.TotalSeconds * 30;
                clouds2X += (float)gameTime.ElapsedGameTime.TotalSeconds * 30;

                if (starsX > BaseGame.game.ScreenWidth)
                {
                    starsX = -BaseGame.game.ScreenWidth;
                }

                if (stars2X > BaseGame.game.ScreenWidth)
                {
                    stars2X = -BaseGame.game.ScreenWidth;
                }

                if (cloudsX > BaseGame.game.ScreenWidth)
                {
                    cloudsX = -BaseGame.game.ScreenWidth;
                }

                if (clouds2X > BaseGame.game.ScreenWidth)
                {
                    clouds2X = -BaseGame.game.ScreenWidth;
                }

            }

            MouseState newMouseState = Mouse.GetState();

            if (ButtonPlay.Rect.Contains(newMouseState.Position))
            {
                ButtonPlay.Color = Color.Black;
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                {
                    isCloseRequest = true;
                }
                if (sounded != "play")
                {
                    sounded = "play";
                    Assets.SndButtonHovered.Play();
                }
            }
            else
            {
                if (sounded == "play")
                    sounded = "";
                ButtonPlay.Color = Color.White;
            }

            if (new Rectangle(ButtonHelp.Rect.X, ButtonHelp.Rect.Y - ButtonHelp.Rect.Height / 2, ButtonHelp.Rect.Width, ButtonHelp.Rect.Height).Contains(newMouseState.Position))
            {
                ButtonHelp.Color = new Color(46, 204, 113);
                if (sounded != "help")
                {
                    sounded = "help";
                    Assets.SndButtonHovered.Play();
                }
            }
            else
            {
                if (sounded == "help")
                    sounded = "";
                ButtonHelp.Color = Color.Black;
            }

            if (ButtonSettings.Rect.Contains(newMouseState.Position))
            {
                ButtonSettings.Color = new Color(46, 204, 113);
                if (sounded != "settings")
                {
                    sounded = "settings";
                    Assets.SndButtonHovered.Play();
                }
            }
            else
            {
                if (sounded == "settings")
                    sounded = "";
                ButtonSettings.Color = Color.Black;
            }

            // add birds
            Random r = new Random();

            if (r.Next(0, 100) < 5)
            {
                CEParticle bp = new CEParticle(Assets.Bird1, 0, r.Next(0, BaseGame.game.ScreenWidth / 2), r.Next(BaseGame.game.ScreenHeight / 30, BaseGame.game.ScreenHeight / 20), r.Next(BaseGame.game.ScreenHeight / 30, BaseGame.game.ScreenHeight / 20), 8, r.Next(100, 300), -200, 0);

                bp.Props.Add("time_anim", 0);
                bp.Props.Add("anim", 1);

                emitterBirds.AddParticle(bp);

            }

            // add sparks
            if (r.NextDouble() * 100 < .5)
            {

                int x = 0;
                int y = 0;

                if (r.Next(0, 100) < 50)
                {
                    x = (int) Math.Round(BaseGame.game.ScreenWidth / 1.33);
                    y = (int) Math.Round(BaseGame.game.ScreenHeight / 1.3);
                }
                else
                {
                    x = (int)Math.Round(BaseGame.game.ScreenWidth / 1.45);
                    y = (int)Math.Round(BaseGame.game.ScreenHeight / 2.3);
                }

                CEParticle ps = new CEParticle(Assets.FXStar, x, y, BaseGame.game.ScreenHeight / 20, BaseGame.game.ScreenHeight / 20, 1, 0, 0, 100);
                ps.AlphaAmount = .3f;
                ps.GrowingAmount = 3;
                ps.Center = true;

                emitter.AddParticle(ps);
            }

            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            emitterBirds.Draw(BaseGame.spriteBatch);

            LayerFan.Draw(BaseGame.spriteBatch);
            ButtonPlay.Draw(BaseGame.spriteBatch);
            ButtonHelp.Draw(BaseGame.spriteBatch);
            ButtonSettings.Draw(BaseGame.spriteBatch);

            emitter.Draw(BaseGame.spriteBatch);

            transitionIn.Draw(BaseGame);
            transitionOut.Draw(BaseGame);
            transitionClose.Draw(BaseGame);
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {

            LayerSky = new CEImageElement(Assets.LayerSky, new Rectangle(0, 0, 0, 0));
            LayerStars = new CEImageElement(Assets.LayerStars, new Rectangle(0, 0, 0, 0));
            LayerStars2 = new CEImageElement(Assets.LayerStars, new Rectangle(0, 0, 0, 0));
            LayerClouds = new CEImageElement(Assets.LayerClouds, new Rectangle(0, 0, 0, 0));
            LayerClouds2 = new CEImageElement(Assets.LayerClouds, new Rectangle(0, 0, 0, 0));
            LayerMountains = new CEImageElement(Assets.LayerMountains, new Rectangle(0, 0, 0, 0));
            LayerFan = new CEImageElement(Assets.LayerFan, new Rectangle(0, 0, 0, 0));

            ButtonPlay = new CETextElement(Langs.Texts["play_button"][BaseGame.game.Lang], Assets.MainFont, Color.White, new Rectangle(0, 0, 0, 0));
            ButtonPlay.Rotation = -20;
            ButtonHelp = new CETextElement(Langs.Texts["help_button"][BaseGame.game.Lang], Assets.MainFont, Color.Black, new Rectangle(0, 0, 0, 0));
            ButtonHelp.Rotation = -30;
            ButtonSettings = new CETextElement(Langs.Texts["settings_button"][BaseGame.game.Lang], Assets.MainFont, Color.Black, new Rectangle(0, 0, 0, 0));
            ButtonSettings.Rotation = -10;

            AddDrawable(LayerSky);
            AddDrawable(LayerStars);
            AddDrawable(LayerStars2);
            AddDrawable(LayerClouds);
            AddDrawable(LayerClouds2);
            AddDrawable(LayerMountains);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {

            LayerSky.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerStars.Rect = new Rectangle((int) starsX, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerStars2.Rect = new Rectangle((int) stars2X, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerClouds.Rect = new Rectangle((int)cloudsX, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerClouds2.Rect = new Rectangle((int)clouds2X, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerMountains.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            LayerFan.Rect = new Rectangle((int) Math.Round(Utils.EaseOutSin(tweeningFan.time, tweeningFan.value, tweeningFan.distance, tweeningFan.duration)), 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);

            ButtonPlay.Rect = new Rectangle(BaseGame.game.ScreenWidth / 5, (int) Math.Round(BaseGame.game.ScreenHeight / 1.6), BaseGame.game.ScreenWidth / 10, BaseGame.game.ScreenHeight / 10);
            ButtonHelp.Rect = new Rectangle(BaseGame.game.ScreenWidth / 9, BaseGame.game.ScreenHeight / 2, BaseGame.game.ScreenWidth / 10, BaseGame.game.ScreenHeight / 10);
            ButtonSettings.Rect = new Rectangle(BaseGame.game.ScreenWidth / 5, (int)Math.Round(BaseGame.game.ScreenHeight / 1.2), BaseGame.game.ScreenWidth / 7, BaseGame.game.ScreenHeight / 8);

        }

        /**
         * Update Tweenings
         */
        private void UpdateTweenings(GameTime gameTime)
        {

            tweeningFan.value = -BaseGame.game.ScreenWidth / 3;
            tweeningFan.distance = BaseGame.game.ScreenWidth / 3;

            if (tweeningFan.time < tweeningFan.duration)
                tweeningFan.time += gameTime.ElapsedGameTime.TotalSeconds;

        }
    }
}
