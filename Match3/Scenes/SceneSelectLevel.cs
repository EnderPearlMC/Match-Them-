using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using CodeEasier.Scene.UI;
using Match3.Datas;
using Match3.Polish.Emitters;
using Match3.Select;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Scenes
{
    class SceneSelectLevel : CEScene
    {

        // Image elements
        CEImageElement bg;
        CEImageElement star;

        CETextElement starsText;

        CEUIButton playButton;

        // polish
        CETransition transitionIn;
        CETransition transitionClose;
        CETransition transitionOut;

        EmitterCircle emitterCircle;

        bool isCloseRequest;

        float scroll;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        List<LevelSelect> levelsList;

        MouseState oldMouseState;

        public SceneSelectLevel(Main main) : base("select_level", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {

            LoadDrawables();

            transitionIn = new CETransition(.5f, CETransition.Type.In);
            transitionClose = new CETransition(1, CETransition.Type.Out);
            transitionOut = new CETransition(2, CETransition.Type.Out);

            emitterCircle = new EmitterCircle();

            isCloseRequest = false;

            oldMouseState = Mouse.GetState();

            levelsList = new List<LevelSelect>();
            StoreLevelsIntoObjects();

            scroll = 0;

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
                transitionOut.Update(gameTime);
                if (transitionOut.Alpha >= 1)
                {
                    BaseGame.game.ChangeScene("level");
                }
            }

            emitterCircle.Update(gameTime);

            MouseState newMouseState = Mouse.GetState();

            UpdateLevels();

            if (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                scroll += 30;
            if (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue && scroll > 0)
                scroll -= 30;

            if (scroll < 0)
                scroll = 0;

            // add fxs
            Random r = new Random();

            LevelSelect current = levelsList.Find(item => item.IsCurrent == true);

            if (r.NextDouble() * 100 < 0.2f)
            {
                AddCircleParticle(15, (int) current.Position.X, (int)current.Position.Y, BaseGame.game.ScreenWidth / 14 / 4, BaseGame.game.ScreenWidth / 14 / 4, 2, BaseGame.game.ScreenWidth / 14 / 3, BaseGame.game.ScreenWidth / 14 * 7, false, new Color(255, 204, 0), true);
                AddCircleParticle(25, (int)current.Position.X, (int)current.Position.Y, BaseGame.game.ScreenWidth / 14 / 4, BaseGame.game.ScreenWidth / 14 / 4, 2, BaseGame.game.ScreenWidth / 14, BaseGame.game.ScreenWidth / 14 * 8, false, new Color(255, 204, 0), true);
            }

            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw()
        {

            bg.Draw(BaseGame.spriteBatch);

            DrawLevels();

            base.Draw();

            emitterCircle.Draw(BaseGame.spriteBatch);

            transitionIn.Draw(BaseGame);
            transitionOut.Draw(BaseGame);
            transitionClose.Draw(BaseGame);
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {

            bg = new CEImageElement(Assets.LevelBackgrounds[1], new Rectangle(0, 0, 0, 0));
            star = new CEImageElement(Assets.Star, new Rectangle(0, 0, 0, 0));

            starsText = new CETextElement("", Assets.MainFont, Color.White, new Rectangle(0, 0, 0, 0));

            playButton = CEUI.Button(Langs.Texts["play_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);
            playButton.OnClickEvent = OnPlayClick;

            AddDrawable(star);
            AddDrawable(playButton);
            AddDrawable(starsText);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {

            Console.WriteLine(starsText.Text.Length * 50);

            bg.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            star.Rect = new Rectangle((int)Math.Round(playButton.Rect.Height / 3.5f), (int)Math.Round(playButton.Rect.Height / 2.8f), BaseGame.game.ScreenWidth / 20, BaseGame.game.ScreenHeight / 13);
            
            starsText.Rect = new Rectangle((int)Math.Round(playButton.Rect.Height / 0.7f), (int)Math.Round(playButton.Rect.Height / 3.5f), starsText.Text.Length * (BaseGame.game.ScreenWidth / 30), BaseGame.game.ScreenHeight / 9);
            starsText.Text = BaseGame.game.Player.Stars + "";

            playButton.Rect = new Rectangle((int)Math.Round(BaseGame.game.ScreenWidth - playButton.Rect.Width * 1.2f), (int)Math.Round(playButton.Rect.Height / 3.5f), BaseGame.game.ScreenWidth / 9, BaseGame.game.ScreenHeight / 9);

        }

        /**
         * Update Tweenings
         */
        private void UpdateTweenings(GameTime gameTime)
        {

        }

        /**
         * 
         *  Store levels into objects
         * 
         */
        private void StoreLevelsIntoObjects()
        {

            List<Level> levels = LevelsData.LevelsList;

            foreach (Level l in levels)
            {

                LevelSelect ls = new LevelSelect();

                ls.Number = l.LevelNbr;

                if (BaseGame.game.Player.Level > ls.Number)
                    ls.IsLocked = true;
                if (BaseGame.game.Player.Level == ls.Number)
                    ls.IsCurrent = true;

                levelsList.Add(ls);

            }

        }

        private void UpdateLevels()
        {
            foreach (LevelSelect l in levelsList)
            {

                float totalWidth = (BaseGame.game.ScreenWidth / 10) * (l.Number % 10);
                l.Position = new Vector2((l.Number % 10) * (BaseGame.game.ScreenWidth / 10) + (BaseGame.game.ScreenWidth / 60), (BaseGame.game.ScreenHeight - (BaseGame.game.ScreenHeight / 6) * (float)(Math.Floor((double)l.Number / 10) + 1)) + scroll);

            }

        }

        private void DrawLevels()
        {
            foreach (LevelSelect l in levelsList)
            {

                Texture2D texture = null;

                if (l.IsCurrent)
                    texture = Assets.SelectLevelBtnCurrent;
                else
                    texture = Assets.SelectLevelBtn;

                BaseGame.spriteBatch.Draw(texture, new Rectangle((int) l.Position.X, (int) l.Position.Y, BaseGame.game.ScreenWidth / 14, BaseGame.game.ScreenHeight / 10), Color.White);
                float w = 20;
                float h = 14;
                if (l.Number < 10)
                {
                    w = 30;
                    h = 17;
                }

                Color c = Color.White;

                if (l.IsCurrent)
                    c = Color.Black;

                if (l.Number <= BaseGame.game.Player.Level)
                    BaseGame.spriteBatch.DrawString(Assets.MainFont, l.Number + "", new Vector2(l.Position.X + w / 1.5f, l.Position.Y + h / 1.5f), c, 0, Vector2.Zero, new Vector2(BaseGame.game.ScreenWidth / w / Assets.MainFont.MeasureString(l.Number + "").X, BaseGame.game.ScreenHeight / h / Assets.MainFont.MeasureString(l.Number + "").Y), SpriteEffects.None, 0);
                else
                    BaseGame.spriteBatch.Draw(Assets.Locked, new Rectangle((int)l.Position.X + BaseGame.game.ScreenWidth / 145, (int)l.Position.Y, BaseGame.game.ScreenWidth / 14, BaseGame.game.ScreenHeight / 10), Color.White);

            }
        }

        private void OnPlayClick()
        {
            isCloseRequest = true;
        }

        // FXS

        public void AddCircleParticle(int nbrStars, int x, int y, int width, int height, float time, int begin, int end)
        {
            AddCircleParticle(nbrStars, x, y, width, height, time, begin, end, true, new Color(0, 0, 0), false);
        }

        public void AddCircleParticle(int nbrStars, int x, int y, int width, int height, float time, int begin, int end, bool growing, Color color, bool useExternColor)
        {
            float pAngle = 0;

            Random r = new Random();

            for (int i = 0; i < nbrStars; i++)
            {

                Texture2D rectangle = new Texture2D(BaseGame.GraphicsDevice, 1, 1);

                rectangle.SetData(new Color[] { Color.White });

                CEParticle pStar = new CEParticle(rectangle, 0, 0, width, height, time, 0, 0, 0);
                CEParticle pTriangle = new CEParticle(Assets.FXTriangle, 0, 0, (int)Math.Round(width * 1.5f), (int)Math.Round(height / 1.5f), time, 0, 0, 0);
                pTriangle.Angle = pAngle + 180;

                //if (growing)
                pStar.GrowingAmount = -4;
                pTriangle.AlphaAmount = 0.8f;
                pTriangle.GrowingAmount = -4f;
                pTriangle.Origin = new Vector2(0, pTriangle.Texture.Height / 2);
                pStar.Center = true;

                if (i % 2 == 0 && !useExternColor)
                {
                    pStar.Color = new Color(255, 204, 255);
                    pTriangle.Color = new Color(255, 204, 255);
                }

                if (useExternColor)
                {
                    pStar.Color = color;
                    pTriangle.Color = color;
                }

                pStar.Props.Add("angle", pAngle);
                pStar.Props.Add("distance", 0);
                pStar.Props.Add("ix", x);
                pStar.Props.Add("iy", y);
                pStar.Props.Add("t_time", 0);
                pStar.Props.Add("t_begin", begin);
                pStar.Props.Add("t_end", end);
                pStar.Props.Add("t_duration", time);

                emitterCircle.AddParticle(pStar);

                float xOff = (float)Math.Cos(MathHelper.ToRadians(pAngle - 90));
                float yOff = (float)Math.Sin(MathHelper.ToRadians(pAngle - 90));

                pTriangle.Props.Add("angle", pAngle);
                pTriangle.Props.Add("distance", 0);
                pTriangle.Props.Add("ix", x - (width * 1.5f) * xOff);
                pTriangle.Props.Add("iy", y - (height * 1.5f) * yOff);
                pTriangle.Props.Add("t_time", 0);
                pTriangle.Props.Add("t_begin", begin);
                pTriangle.Props.Add("t_end", end);
                pTriangle.Props.Add("t_duration", time);

                emitterCircle.AddParticle(pTriangle);

                pAngle += 360 / nbrStars;

            }

        }

    }
}
