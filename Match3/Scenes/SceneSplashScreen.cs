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
    class SceneSplashScreen : CEScene
    {

        // Image elements
        CEImageElement screen;

        float counter;

        bool sndPlayed;

        // polish
        CETransition transitionIn;
        CETransition transitionOut;

        public SceneSplashScreen(Main main) : base("splash_screen", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {

            LoadDrawables();

            transitionIn = new CETransition(.2f, CETransition.Type.In);
            transitionOut = new CETransition(.5f, CETransition.Type.Out);

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            UpdateDrawables();

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= 8)
            {
                if (transitionOut.Alpha < 1)
                {
                    transitionOut.Update(gameTime);
                }
                else
                {
                    BaseGame.game.ChangeScene("main_menu");
                }
            }

            if (counter > 2)
            {
                if (!sndPlayed)
                {
                    Assets.SndSplashScreen.Play();
                    sndPlayed = true;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            transitionIn.Draw(BaseGame);
            transitionOut.Draw(BaseGame);
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {

            screen = new CEImageElement(Assets.SplashScreen, new Rectangle(0, 0, 0, 0));

            AddDrawable(screen);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {

            screen.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);

        }
    }
}
