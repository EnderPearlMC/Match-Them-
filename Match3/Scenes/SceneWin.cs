using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using CodeEasier.Scene.UI;
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
    class SceneWin : CEScene
    {

        // Image Elements
        CEImageElement background;
        CEImageElement winPan;
        CEImageElement star;
        CEImageElement starPileFirst;

        // Text elements
        CETextElement levelCompletedText;
        CETextElement starsWonText;

        // buttons
        CEUIButton continueButton;

        // polish
        CETransition transitionIn;
        CETransition transitionOut;

        // Add falling stars
        List<CESpriteElement> fallingStars;
        int starsAmount;
        int starsAmountI;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        Tweening tweeningWinPan;

        public SceneWin(Main main) : base("win", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {

            fallingStars = new List<CESpriteElement>();
            starsAmountI = 0;

            if ((int) parameters["stars_won"] <= 50)
            {
                starsAmount = 3;
            }
            else if ((int)parameters["stars_won"] > 50 && (int)parameters["stars_won"] <= 100)
            {
                starsAmount = 5;
            }
            else if ((int)parameters["stars_won"] > 100 && (int)parameters["stars_won"] <= 300)
            {
                starsAmount = 7;
            }
            else if ((int)parameters["stars_won"] > 300 && (int)parameters["stars_won"] < 800)
            {
                starsAmount = 9;
            }
            else if ((int)parameters["stars_won"] >= 800)
            {
                starsAmount = 13;
            }

            tweeningWinPan = new Tweening();
            tweeningWinPan.time = 0;
            tweeningWinPan.duration = 1;

            LoadDrawables(parameters);

            transitionIn = new CETransition(3f, CETransition.Type.In);
            transitionOut = new CETransition(2, CETransition.Type.Out);

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            Random r = new Random();

            if (starsAmountI <= starsAmount)
            {
                int nbr = r.Next(0, 100);
                if (nbr < 10)
                {
                    fallingStars.Add(new CESpriteElement(Assets.StarForPile, new Vector2(0, 0), 0, 0));
                    starsAmountI++;
                }
            }

            int index = 0;
            foreach (CESpriteElement s in fallingStars)
            {
                s.Width = BaseGame.game.ScreenWidth / 5; 
                s.Height = BaseGame.game.ScreenHeight / 3;
                if (s.Position.Y >= starPileFirst.Rect.Y - (s.Height / 15 + index * s.Height / 15))
                {
                    s.Position = new Vector2(BaseGame.game.ScreenWidth / 3, starPileFirst.Rect.Y - (s.Height / 15 + index * s.Height / 15));
                    s.VY = 0;
                }
                else
                {
                    s.Position = new Vector2(BaseGame.game.ScreenWidth / 3, s.Position.Y);
                    s.VY = BaseGame.game.ScreenHeight / 2;
                }
                s.Update(gameTime);
                index++;
            }

            UpdateDrawables();

            UpdateTweenings(gameTime);

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            foreach (CESpriteElement s in fallingStars)
            {
                s.Draw(BaseGame.spriteBatch);
            }

            transitionIn.Draw(BaseGame);
            transitionOut.Draw(BaseGame);
        }

        /**
         * Load drawables
         */
        private void LoadDrawables(Dictionary<string, Object> parameters)
        {

            background = new CEImageElement(Assets.WinBackground, new Rectangle(0, 0, 0, 0));
            winPan = new CEImageElement(Assets.WinPan, new Rectangle(0, 0, 0, 0));
            star = new CEImageElement(Assets.Star, new Rectangle(0, 0, 0, 0));
            starPileFirst = new CEImageElement(Assets.StarForPile, new Rectangle(0, 0, 0, 0));

            levelCompletedText = new CETextElement(Langs.Texts["level_completed"][BaseGame.game.Lang], Assets.MainFont, new Color(231, 76, 60), new Rectangle(0, 0, 0, 0));
            starsWonText = new CETextElement("+" + parameters["stars_won"], Assets.MainFont, new Color(205, 220, 57), new Rectangle(0, 0, 0, 0));

            continueButton = CEUI.Button(Langs.Texts["continue"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            continueButton.OnClickEvent = Quit;

            AddDrawable(background);
            AddDrawable(winPan);
            AddDrawable(star);
            AddDrawable(starPileFirst);
            AddDrawable(levelCompletedText);
            AddDrawable(starsWonText);
            AddDrawable(continueButton);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {

            background.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            winPan.Rect = new Rectangle((int)Utils.EaseOutSin(tweeningWinPan.time, tweeningWinPan.value, tweeningWinPan.distance, tweeningWinPan.duration), -winPan.Rect.Height / 20, (int) Math.Round(BaseGame.game.ScreenWidth / 2.7), BaseGame.game.ScreenHeight);
            star.Rect = new Rectangle((int)Math.Round(Utils.EaseOutSin(tweeningWinPan.time, tweeningWinPan.value, tweeningWinPan.distance, tweeningWinPan.duration) + winPan.Rect.Width / 1.65), winPan.Rect.Height / 3, (int)Math.Round(winPan.Rect.Width / 4.7), winPan.Rect.Height / 10);
            starPileFirst.Rect = new Rectangle(BaseGame.game.ScreenWidth / 3, (int) Math.Round(BaseGame.game.ScreenHeight / 1.4), BaseGame.game.ScreenWidth / 5, BaseGame.game.ScreenHeight / 3);

            levelCompletedText.Rect = new Rectangle((int)Utils.EaseOutSin(tweeningWinPan.time, tweeningWinPan.value, tweeningWinPan.distance, tweeningWinPan.duration) + winPan.Rect.Width / 6, winPan.Rect.Height / 6, (int) Math.Round(winPan.Rect.Width / 1.6), winPan.Rect.Height / 10);
            starsWonText.Rect = new Rectangle((int)Utils.EaseOutSin(tweeningWinPan.time, tweeningWinPan.value, tweeningWinPan.distance, tweeningWinPan.duration) + winPan.Rect.Width / 6, winPan.Rect.Height / 3, (int) Math.Round(winPan.Rect.Width / 2.7), winPan.Rect.Height / 10);

            continueButton.Rect = new Rectangle((int)Utils.EaseOutSin(tweeningWinPan.time, tweeningWinPan.value, tweeningWinPan.distance, tweeningWinPan.duration) + winPan.Rect.Width / 6, (int) Math.Round(winPan.Rect.Height / 1.4), (int) Math.Round(winPan.Rect.Width / 1.7), winPan.Rect.Height / 7);

        }

        /**
         * Update Tweenings
         */
        private void UpdateTweenings(GameTime gameTime)
        {

            tweeningWinPan.value = BaseGame.game.ScreenWidth * 1.3;
            tweeningWinPan.distance = (int) Math.Round(-BaseGame.game.ScreenWidth / 1.5);

            if (tweeningWinPan.time < tweeningWinPan.duration)
                tweeningWinPan.time += gameTime.ElapsedGameTime.TotalSeconds;

        }

        private void Quit()
        {
            BaseGame.game.ChangeScene("main_menu");
        }

    }
}
