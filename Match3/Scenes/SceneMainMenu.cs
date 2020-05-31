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
    class SceneMainMenu : CEScene
    {

        // Image elements
        CEImageElement background;

        // Texts elements
        CETextElement welcomeText;

        // buttons
        CEUIButton playButton;
        CEUIButton helpButton;
        CEUIButton settingsButton;
        CEUIButton creditsButton;
        CEUIButton quitButton;

        // polish
        CETransition transitionIn;
        CETransition transitionClose;
        CETransition transitionOut;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        Tweening tweeningPlayBtn;
        Tweening tweeningHelpBtn;
        Tweening tweeningSettingsBtn;
        Tweening tweeningWelcomeText;

        bool isCloseRequest = false;
        bool isPlayRequest = false;

        public SceneMainMenu(Main main) : base("main_menu", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {

            LoadDrawables();

            tweeningPlayBtn = new Tweening();
            tweeningPlayBtn.time = 0;
            tweeningPlayBtn.duration = 1;

            tweeningHelpBtn = new Tweening();
            tweeningHelpBtn.time = 0;
            tweeningHelpBtn.duration = 1;

            tweeningSettingsBtn = new Tweening();
            tweeningSettingsBtn.time = 0;
            tweeningSettingsBtn.duration = 1;

            tweeningWelcomeText = new Tweening();
            tweeningWelcomeText.time = 0;
            tweeningWelcomeText.duration = 1;

            transitionIn = new CETransition(3f, CETransition.Type.In);
            transitionClose = new CETransition(1, CETransition.Type.Out);
            transitionOut = new CETransition(2, CETransition.Type.Out);

            MediaPlayer.Play(Assets.MusicMainMenu);
            MediaPlayer.IsRepeating = true;

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            UpdateDrawables();

            UpdateTweenings(gameTime);

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            if (isCloseRequest)
            {
                transitionClose.Update(gameTime);
                if (transitionClose.Alpha > 1)
                {
                    BaseGame.Exit();
                }
            }

            if (isPlayRequest)
            {
                transitionOut.Update(gameTime);
                if (transitionOut.Alpha > 1)
                {
                    BaseGame.game.ChangeScene("level");
                    isPlayRequest = false;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();
            transitionIn.Draw(BaseGame);
            transitionOut.Draw(BaseGame);
            transitionClose.Draw(BaseGame);
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {
            // image element
            background = new CEImageElement(Assets.Background1, new Rectangle(0, 0, 0, 0));

            // text elements
            welcomeText = new CETextElement(Langs.Texts["welcome"][BaseGame.game.Lang] + BaseGame.game.Player.Name + "!", Assets.MainFont, Color.White, new Rectangle(0, 0, 0, 0));

            // buttons
            playButton = CEUI.Button(Langs.Texts["play_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            playButton.OnClickEvent = StartGame;
            helpButton = CEUI.Button(Langs.Texts["help_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            settingsButton = CEUI.Button(Langs.Texts["settings_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            creditsButton = CEUI.Button("Credtis", Assets.ButtonThemePath1, Color.White, BaseGame);
            quitButton = CEUI.Button(Langs.Texts["quit_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            quitButton.OnClickEvent = Close;

            // add to drawables
            AddDrawable(background);
            AddDrawable(welcomeText);
            AddDrawable(playButton);
            AddDrawable(helpButton);
            AddDrawable(settingsButton);
            AddDrawable(creditsButton);
            AddDrawable(quitButton);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {
            // image element
            background.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);

            // text elements
            welcomeText.Rect = new Rectangle((int)Utils.EaseOutSin(tweeningWelcomeText.time, tweeningWelcomeText.value, tweeningWelcomeText.distance, tweeningWelcomeText.duration), (int)Math.Round((float) BaseGame.game.ScreenHeight - BaseGame.game.ScreenHeight / 10 - 10), BaseGame.game.ScreenWidth / 4, BaseGame.game.ScreenHeight / 10);

            // buttons

            playButton.Rect = new Rectangle(BaseGame.game.ScreenWidth / 2 - playButton.Rect.Width / 2, (int)Utils.EaseOutSin(tweeningPlayBtn.time, tweeningPlayBtn.value, tweeningPlayBtn.distance, tweeningPlayBtn.duration), BaseGame.game.ScreenWidth / 6, BaseGame.game.ScreenHeight / 8);

            helpButton.Rect = new Rectangle(BaseGame.game.ScreenWidth / 2 - playButton.Rect.Width / 2, (int)Utils.EaseOutSin(tweeningHelpBtn.time, tweeningHelpBtn.value, tweeningHelpBtn.distance, tweeningHelpBtn.duration), BaseGame.game.ScreenWidth / 6, BaseGame.game.ScreenHeight / 8);
            settingsButton.Rect = new Rectangle(BaseGame.game.ScreenWidth / 2 - playButton.Rect.Width / 2, (int)Utils.EaseOutSin(tweeningSettingsBtn.time, tweeningSettingsBtn.value, tweeningSettingsBtn.distance, tweeningSettingsBtn.duration), BaseGame.game.ScreenWidth / 6, BaseGame.game.ScreenHeight / 8);
            quitButton.Rect = new Rectangle(10, 10, BaseGame.game.ScreenWidth / 6, BaseGame.game.ScreenHeight / 8);

        }

        /**
         * Update Tweenings
         */
        private void UpdateTweenings(GameTime gameTime)
        {
            tweeningPlayBtn.value = BaseGame.game.ScreenHeight;
            tweeningPlayBtn.distance = -BaseGame.game.ScreenHeight / 2;

            tweeningHelpBtn.value = BaseGame.game.ScreenHeight;
            tweeningHelpBtn.distance = -BaseGame.game.ScreenHeight / 3;

            tweeningSettingsBtn.value = BaseGame.game.ScreenHeight;
            tweeningSettingsBtn.distance = -BaseGame.game.ScreenHeight / 6;

            tweeningWelcomeText.value = 0;
            tweeningWelcomeText.distance = BaseGame.game.ScreenHeight / 6;

            if (tweeningPlayBtn.time < tweeningPlayBtn.duration)
                tweeningPlayBtn.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningHelpBtn.time < tweeningHelpBtn.duration)
                tweeningHelpBtn.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningSettingsBtn.time < tweeningSettingsBtn.duration)
                tweeningSettingsBtn.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningWelcomeText.time < tweeningWelcomeText.duration)
                tweeningWelcomeText.time += gameTime.ElapsedGameTime.TotalSeconds;
        }

        /**
         * Close game
         */
        private void Close()
        {
            isCloseRequest = true;
        }

        /**
         *  Play !
         */
        private void StartGame()
        {
            isPlayRequest = true;
        }

    }
}
