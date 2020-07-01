﻿using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using CodeEasier.Scene.UI;
using Match3.Datas;
using Match3.Levels.Forces;
using Match3.Levels.Tiles;
using Match3.Polish.Emitters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Scenes
{
    class SceneLevel : CEScene
    {

        // Image Elements
        CEImageElement background;
        CEImageElement spawnTilesBar;
        CEImageElement spawnTilesBar2;
        CEImageElement leftPan;
        CEImageElement star;
        CEImageElement tilesUp;

        // Text elements
        CETextElement textStars;

        // Buttons
        CEUIButton pauseButton;

        // polish
        CETransition transitionIn;
        public CEParticleEmitter emitter1;
        EmitterFallingTiles emitterFallingTiles;
        public EmitterL emitterL;
        EmitterXP emitterXP;
        public EmitterCircle emitterCircle;
        public EmitterLevelTexts emitterTexts;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        Tweening tweeningSpawBar;
        Tweening tweeningLeftPan;
        Tweening tweeningTilesSpawn;
        Tweening tweeningTilesUp;

        public bool isPaused;

        // tiles
        public bool tilesGenerated;
        public List<List<Tile>> tiles;

        public int tileWidth;
        public int tileHeight;

        public int tileXOffset;
        public int tileYOffset;
        public float tileScrollY;

        private CEImageElement selectedTileIndicator;
        public bool showIndicator;
        public int indicatorRow, indicatorCol;

        private bool tilesTweening;
        private float tilesTweeningIScroll;

        // match 4
        private bool match4Circles;
        private float match4CirclesTimer;

        // stars
        public int starsWon;

        // xp bar
        public int xp;

        public int xpBarWidth;
        public int xpBarHeight;

        // forces
        public List<Force> forces;

        // forces inventory
        public List<Force> forcesInventory;

        // mouse
        MouseState oldMouseState;

        public SceneLevel(Main main) : base("level", main)
        {

        }

        public override void Load(Dictionary<string, Object> parameters)
        {


            tweeningSpawBar = new Tweening();
            tweeningSpawBar.time = 0;
            tweeningSpawBar.duration = 1;

            tweeningLeftPan = new Tweening();
            tweeningLeftPan.time = 0;
            tweeningLeftPan.duration = 2;

            tweeningTilesSpawn = new Tweening();
            tweeningTilesSpawn.time = 0;
            tweeningTilesSpawn.duration = 1;

            tweeningTilesUp = new Tweening();
            tweeningTilesUp.time = 0;
            tweeningTilesUp.duration = .5;

            isPaused = true;

            tilesGenerated = false;
            tiles = new List<List<Tile>>();

            tileWidth = 0;
            tileHeight = 0;

            tileXOffset = 0;
            tileYOffset = 0;
            tileScrollY = 0;

            selectedTileIndicator = new CEImageElement(Assets.SelectedTileIndicator, new Rectangle(0, 0, 0, 0));

            match4Circles = false;
            match4CirclesTimer = 0;

            starsWon = 0;

            xp = 0;

            xpBarWidth = 0;
            xpBarHeight = 0;

            forces = new List<Force>();

            forcesInventory = new List<Force>();

            LoadDrawables();

            transitionIn = new CETransition(3f, CETransition.Type.In);
            emitterFallingTiles = new EmitterFallingTiles();
            emitter1 = new CEParticleEmitter();
            emitterL = new EmitterL();
            emitterXP = new EmitterXP();
            emitterCircle = new EmitterCircle();
            emitterTexts = new EmitterLevelTexts();

            GenerateTiles();

            GenerateForces();

            oldMouseState = Mouse.GetState();

            BaseGame.game.Song += 1;

            if (BaseGame.game.Song >= Assets.LevelMusics.Count)
            {
                BaseGame.game.Song = 0;
            }

            Song song = Assets.LevelMusics[BaseGame.game.Song];

            

            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            MouseState newMouseState = Mouse.GetState();

            tileWidth = (int) Math.Round(BaseGame.game.ScreenWidth / 17.6);
            tileHeight = BaseGame.game.ScreenHeight / 10;

            tileXOffset = (int) Math.Round(BaseGame.game.ScreenWidth / 2.5);
            tileYOffset = BaseGame.game.ScreenHeight;

            UpdateDrawables();

            UpdateTweenings(gameTime);

            DetectMatches();

            UpdateTiles(gameTime);

            UpdateForces(newMouseState, gameTime);

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            emitterFallingTiles.Update(gameTime);
            emitter1.Update(gameTime);
            emitterL.Update(gameTime);
            emitterXP.Update(gameTime);
            emitterCircle.Update(gameTime);
            emitterTexts.Update(gameTime);

            if (!isPaused)
            {
                tileScrollY -= (float)gameTime.ElapsedGameTime.TotalSeconds * BaseGame.game.ScreenHeight / LevelsData.LevelsList[BaseGame.game.Player.Level - 1].ScrollingSpeed;
            }
            else
            {

                while (tiles[0].Contains(null))
                {
                    GenerateTiles();
                }

                if (!tiles[0].Contains(null))
                    tilesGenerated = true;
                else
                    tilesGenerated = false;

                if (tweeningSpawBar.time >= 1 && tilesGenerated)
                {
                    tileScrollY = Utils.EaseOutSin(tweeningTilesSpawn.time, tweeningTilesSpawn.value, tweeningTilesSpawn.distance, tweeningTilesSpawn.duration);
                    if (tweeningTilesSpawn.time >= 1)
                    {
                        isPaused = false;
                    }
                }

            }

            if (tilesTweening)
            {
                tileScrollY = Utils.EaseOutSin(tweeningTilesUp.time, tweeningTilesUp.value, tweeningTilesUp.distance, tweeningTilesUp.duration) + tilesTweeningIScroll;
            }

            xpBarWidth = leftPan.Rect.Width / 13;
            xpBarHeight = (int) Math.Round(BaseGame.game.ScreenHeight / 2.7);

            if (xp >= LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp)
            {

                BaseGame.game.ShowCursor = true;

                isPaused = true;
                Dictionary<string, Object> p = new Dictionary<string, object>
                {
                    { "stars_won", starsWon }
                };

                /**BaseGame.game.Player.Level += 1;

                DataManager.WriteFile("player.json", BaseGame.game.Player);**/

                BaseGame.game.ChangeScene("win", p);
            }

            int mouseRow = (int)Math.Floor((newMouseState.Y - (tileYOffset + tileScrollY)) / tileHeight);
            int mouseCol = (newMouseState.X - tileXOffset) / tileWidth;

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseRow >= 0 && mouseRow < tiles.Count)
                {
                    if (mouseCol >= 0 && mouseCol < tiles[mouseRow].Count)
                    {
                        if (tiles[mouseRow][mouseCol] != null)
                        {
                            if (tiles[mouseRow][mouseCol].activated)
                            {
                                showIndicator = true;

                                indicatorRow = mouseRow;
                                indicatorCol = mouseCol;
                            }
                            else
                            {
                                showIndicator = false;
                            }
                        }
                        else
                        {
                            showIndicator = false;
                        }
                    }
                    else
                    {
                        showIndicator = false;
                    }
                }
                else
                {
                    showIndicator = false;
                }
            }

            if (showIndicator)
            {
                
                if (tiles[indicatorRow][indicatorCol] != null)
                {
                    int x = (int)Math.Round(indicatorCol * tileWidth + tileXOffset + tiles[indicatorRow][indicatorCol].XOff);
                    int y = (int)Math.Round(indicatorRow * tileHeight + tileYOffset + tileScrollY + tiles[indicatorRow][indicatorCol].YOff);
                    selectedTileIndicator.Rect = new Rectangle(x, y, tileWidth + tileWidth / 5, tileHeight + tileHeight / 20);
                }
                else
                {
                    showIndicator = false;
                }

            }

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {

                if (tilesUp.Rect.Contains(newMouseState.Position))
                {
                    tweeningTilesUp.time = 0;
                    tweeningTilesUp.value = 0;
                    tweeningTilesUp.distance = -tileHeight / 2;
                    tilesTweeningIScroll = tileScrollY;
                    tilesTweening = true;
                    Assets.SndTileSlide.Play();
                }

            }

            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            BaseGame.spriteBatch.Draw(spawnTilesBar.Texture, spawnTilesBar.Rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);

            DrawTiles();

            spawnTilesBar2.Draw(BaseGame.spriteBatch);

            if (showIndicator)
                selectedTileIndicator.Draw(BaseGame.spriteBatch);

            tilesUp.Draw(BaseGame.spriteBatch);

            Texture2D rectXpBarEmpty = new Texture2D(BaseGame.GraphicsDevice, 1, 1);
            rectXpBarEmpty.SetData(new[] { new Color(127, 140, 141) });

            Texture2D rectXpBarFilled = new Texture2D(BaseGame.GraphicsDevice, 1, 1);
            rectXpBarFilled.SetData(new[] { new Color(46, 204, 113) });

            BaseGame.spriteBatch.Draw(rectXpBarEmpty, new Rectangle(leftPan.Rect.X + (leftPan.Rect.Width / 3), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4), xpBarWidth, xpBarHeight), Color.White);

            if (xp > 0)
            {
                float a = (float) LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / xp;

                if (a > 0)
                    BaseGame.spriteBatch.Draw(rectXpBarFilled, new Rectangle(leftPan.Rect.X + (leftPan.Rect.Width / 3), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)), xpBarWidth, (int) Math.Round(xpBarHeight / a)), Color.White);
            }

            DrawForces();

            emitterFallingTiles.Draw(BaseGame.spriteBatch);
            emitterL.Draw(BaseGame.spriteBatch);
            emitterXP.Draw(BaseGame.spriteBatch);
            emitter1.Draw(BaseGame.spriteBatch);
            emitterCircle.Draw(BaseGame.spriteBatch);
            emitterTexts.Draw(BaseGame.spriteBatch);

            transitionIn.Draw(BaseGame);
            
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {

            background = new CEImageElement(Assets.Background2, new Rectangle(0, 0, 0, 0));
            spawnTilesBar = new CEImageElement(Assets.LevelSpawnTilesBar, new Rectangle(0, 0, 0, 0));
            spawnTilesBar2 = new CEImageElement(Assets.LevelSpawnTilesBar, new Rectangle(0, 0, 0, 0));
            leftPan = new CEImageElement(Assets.LevelLeftPan, new Rectangle(0, 0, 0, 0));
            star = new CEImageElement(Assets.Star, new Rectangle(0, 0, 0, 0));
            tilesUp = new CEImageElement(Assets.TilesUp, new Rectangle(0, 0, 0, 0));

            textStars = new CETextElement(BaseGame.game.Player.Stars + "", Assets.MainFont, Color.Black, new Rectangle(0, 0, 0, 0));

            pauseButton = CEUI.Button(Langs.Texts["pause_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);

            AddDrawable(background);
            AddDrawable(leftPan);
            AddDrawable(star);
            AddDrawable(textStars);
            AddDrawable(pauseButton);

        }

        /**
         * Update drawables
         */
        private void UpdateDrawables()
        {

            // Image elements
            background.Rect = new Rectangle(0, 0, BaseGame.game.ScreenWidth, BaseGame.game.ScreenHeight);
            spawnTilesBar.Rect = new Rectangle((int) Math.Round(Utils.EaseOutSin(tweeningSpawBar.time, tweeningSpawBar.value, tweeningSpawBar.distance, tweeningSpawBar.duration)), BaseGame.game.ScreenHeight - spawnTilesBar.Rect.Height * 2, (int) Math.Round(BaseGame.game.ScreenWidth / 1.7), (int) Math.Round(BaseGame.game.ScreenHeight / 9.2));
            spawnTilesBar2.Rect = new Rectangle((int) Math.Round(Utils.EaseOutSin(tweeningSpawBar.time, tweeningSpawBar.value, tweeningSpawBar.distance, tweeningSpawBar.duration)), BaseGame.game.ScreenHeight - spawnTilesBar.Rect.Height, (int) Math.Round(BaseGame.game.ScreenWidth / 1.7), (int) Math.Round(BaseGame.game.ScreenHeight / 9.2));
            leftPan.Rect = new Rectangle(0, (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)), BaseGame.game.ScreenWidth / 3, (int) Math.Round(BaseGame.game.ScreenHeight / 1.2));
            star.Rect = new Rectangle((int) Math.Round(leftPan.Rect.Width / 1.7 - star.Rect.Width / 2), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)) + BaseGame.game.ScreenWidth / 13, leftPan.Rect.Width / 6, leftPan.Rect.Height / 9);
            textStars.Rect = new Rectangle((int)Math.Round(leftPan.Rect.Width / 1.9 - (textStars.Font.MeasureString(textStars.Text).X / textStars.Rect.Width) / 2), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)) + BaseGame.game.ScreenWidth / 7, leftPan.Rect.Width / 9, leftPan.Rect.Height / 14);
            tilesUp.Rect = new Rectangle(spawnTilesBar2.Rect.X + (spawnTilesBar2.Rect.Width / 2 - tilesUp.Rect.Width / 2), spawnTilesBar2.Rect.Y, BaseGame.game.ScreenWidth / 15, BaseGame.game.ScreenHeight / 15);

            textStars.Text = BaseGame.game.Player.Stars + "";

            // buttons
            if (tweeningLeftPan.time >= 2)
            {
                pauseButton.Rect = new Rectangle(10, 10, BaseGame.game.ScreenWidth / 6, BaseGame.game.ScreenHeight / 8);
            }

        }

        /**
         * Update Tweenings
         */
        private void UpdateTweenings(GameTime gameTime)
        {

            tweeningSpawBar.value = 0;
            tweeningSpawBar.distance = (int) Math.Round(BaseGame.game.ScreenWidth / 3.0);

            tweeningLeftPan.value = 0;
            tweeningLeftPan.distance = BaseGame.game.ScreenHeight / 8;

            tweeningTilesSpawn.value = 0;
            tweeningTilesSpawn.distance = (int) Math.Round(-BaseGame.game.ScreenHeight / LevelsData.LevelsList[BaseGame.game.Player.Level - 1].ScrollingStartDistance);

            if (tweeningSpawBar.time < tweeningSpawBar.duration)
                tweeningSpawBar.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningLeftPan.time < tweeningLeftPan.duration)
                tweeningLeftPan.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningTilesSpawn.time < tweeningTilesSpawn.duration && tweeningSpawBar.time >= 1)
                tweeningTilesSpawn.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningTilesUp.time < tweeningTilesUp.duration && tilesTweening)
                tweeningTilesUp.time += gameTime.ElapsedGameTime.TotalSeconds;
            else
                tilesTweening = false;

        }

        /**
         *  Generate tiles
         */
        private void GenerateTiles()
        {

            Random r = new Random();

            tiles.Clear();

            for (int row = 0; row < 200; row++)
            {

                int oldCol = 0;

                List<Tile> l = new List<Tile>();
                for (int col = 0; col < 8; col++)
                {

                    int nbr = r.Next(0, LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles.Count);

                    while(nbr == oldCol)
                    {
                        nbr = r.Next(0, LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles.Count);
                    }

                    string t = LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles[nbr];
                    oldCol = nbr;

                    if (row > 0)
                    {
                        string upT = tiles[row - 1][col].Id;
                        while (t == upT)
                        {
                            nbr = r.Next(0, LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles.Count);
                            t = LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles[nbr];
                        }
                        oldCol = nbr;

                    }

                    switch (t)
                    {
                        case "written_1":
                            l.Add(new TileWritten1(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "written_2":
                            l.Add(new TileWritten2(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "written_3":
                            l.Add(new TileWritten3(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "star":
                            l.Add(new TileStar(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "japan":
                            l.Add(new TileJapan(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "bamboo":
                            l.Add(new TileBamboo(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "tree":
                            l.Add(new TileTree(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "temple":
                            l.Add(new TileTemple(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "card":
                            l.Add(new TileCard(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "num_1":
                            l.Add(new TileNum1(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "num_2":
                            l.Add(new TileNum2(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "num_3":
                            l.Add(new TileNum3(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "num_4":
                            l.Add(new TileNum4(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "num_5":
                            l.Add(new TileNum5(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        default:
                            break;
                    }

                }
                tiles.Add(l);
            }

        }

        /**
         *  Generate  Level's Forces
         */
        private void GenerateForces()
        {
            foreach (KeyValuePair<int, string> item in LevelsData.LevelsList[BaseGame.game.Player.Level - 1].Forces)
            {
                switch (item.Value)
                {

                    case "axe":
                        Force s = new ForceAxe(BaseGame);
                        s.XpToHave = item.Key;
                        forces.Add(s);
                        break;

                    case "storm":
                        Force s1 = new ForceStorm(BaseGame);
                        s1.XpToHave = item.Key;
                        forces.Add(s1);
                        break;

                    default:
                        break;
                }
            }
        }

        /**
         *  Update Tiles
         */
        private void UpdateTiles(GameTime gameTime)
        {

            for (int row = tiles.Count - 1; row >= 0; row--)
            {

                for (int col = 0; col < tiles[row].Count; col++)
                {
                    if (tiles[row][col] != null && tilesGenerated)
                    {
                        tiles[row][col].Width = tileWidth + tileWidth / 5;
                        tiles[row][col].Height = tileHeight + tileHeight / 7;
                        tiles[row][col].X = (int) Math.Round(col * tileWidth + tileXOffset + tiles[row][col].XOff);
                        tiles[row][col].Y = (int) Math.Round(row * tileHeight + tileYOffset + tileScrollY + tiles[row][col].YOff);

                        if (tiles[row][col].Y < BaseGame.game.ScreenHeight / 1.3)
                        {
                            tiles[row][col].activated = true;
                        }
                        else
                        {
                            tiles[row][col].activated = false;
                        }

                        tiles[row][col].Update(this, gameTime, BaseGame);

                        // Increase counter if the tile has to be removed
                        if (tiles[row][col] != null)
                        {
                            if (tiles[row][col].toRemove)
                            {
                                tiles[row][col].removeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                                if (tiles[row][col].removeCounter <= 0)
                                {

                                    if (tiles[row][col].popNbr == 1)
                                        Assets.SndTilePop.Play();
                                    else if (tiles[row][col].popNbr == 2)
                                        Assets.SndTilePop2.Play();
                                    else if (tiles[row][col].popNbr == 3)
                                        Assets.SndTilePop3.Play();
                                    else if (tiles[row][col].popNbr == 4)
                                        Assets.SndTilePop4.Play();

                                    Random r = new Random();

                                    int vx = 0;

                                    if (r.Next(0, 100) < 50)
                                        vx = 1000;
                                    else
                                        vx = -1000;

                                    emitterFallingTiles.AddParticle(tiles[row][col].Texture, tiles[row][col].X, tiles[row][col].Y, tiles[row][col].Width, tiles[row][col].Height, 5, vx, 700, -80);

                                    //for (int i = 0; i < 20; i++)
                                    //    emitter1.AddParticle(Assets.FXSpark1, tiles[row][col].X, tiles[row][col].Y, tiles[row][col].Width, tiles[row][col].Height, (float)r.NextDouble(), r.Next(-100, 100), r.Next(-100, 100), r.Next(-100, 100));

                                    // emitterSpark1.AddParticle(Assets.FXSpark1, tiles[row][col].X + tiles[row][col].Width, tiles[row][col].Y + tiles[row][col].Height, tiles[row][col].Width * 2, tiles[row][col].Height * 2, 2, 0, 0, 50);

                                    tiles[row][col] = null;
                                }

                            }

                        }

                        if (tiles[row][col] != null)
                        {
                            if (tiles[row][col].toStack)
                            {

                                if (tiles[row][col].tweeningStack.time >= tiles[row][col].tweeningStack.duration)
                                {
                                    Random r = new Random();
                                    //emitterFallingTiles.AddParticle(tiles[row][col].Texture, tiles[row][col].X, tiles[row][col].Y, tiles[row][col].Width, tiles[row][col].Height, 5, r.Next(-500, -300), 700, r.Next(-70, 70));

                                    // emitterSpark1.AddParticle(Assets.FXSpark1, tiles[row][col].X + tiles[row][col].Width, tiles[row][col].Y + tiles[row][col].Height, tiles[row][col].Width * 2, tiles[row][col].Height * 2, 2, 0, 0, 50

                                    if (tiles[row][col].stackAxe == "x" && tiles[row][col].stackTileToGet != null)
                                    {
                                        tiles[row][col + 1] = tiles[row][col].stackTileToGet;
                                        tiles[row][col + 1].toBeStacked = true;

                                        for (int i = 0; i < 50; i++)
                                            emitter1.AddParticle(Assets.FXSpark1, tiles[row][col + 1].X, tiles[row][col + 1].Y, tiles[row][col + 1].Width, tiles[row][col + 1].Height, (float)r.NextDouble(), r.Next(-300, 300), r.Next(-300, 300), r.Next(-100, 100));


                                        for (int i = 0; i < 4; i++)
                                        {
                                            CEParticle pinkP = new CEParticle(Assets.FXPink, tiles[row][col + 1].X + (int) Math.Round(tiles[row][col + 1].Width / 1.4), tiles[row][col + 1].Y + (int) Math.Round(tiles[row][col + 1].Height / 1.6), tiles[row][col + 1].Width, tiles[row][col + 1].Height, 2, 0, 0, r.Next(-100, 100));
                                            pinkP.Center = true;
                                            pinkP.GrowingAmount = 2;
                                            pinkP.AlphaAmount = 1;
                                            emitter1.AddParticle(pinkP);
                                        }


                                    }
                                    else if (tiles[row][col].stackAxe == "y" && tiles[row][col].stackTileToGet != null)
                                    {
                                        tiles[row + 1][col] = tiles[row][col].stackTileToGet;
                                        tiles[row + 1][col].toBeStacked = true;
                                        tiles[row + 1][col].toBeStackedCounter = 2;
                                        for (int i = 0; i < 50; i++)
                                            emitter1.AddParticle(Assets.FXSpark1, tiles[row][col].X, tiles[row][col].Y, tiles[row][col].Width, tiles[row][col].Height, (float)r.NextDouble(), r.Next(-300, 300), r.Next(-300, 300), r.Next(-100, 100));

                                        for (int i = 0; i < 4; i++)
                                        {
                                            CEParticle pinkP = new CEParticle(Assets.FXPink, tiles[row][col].X + (int)Math.Round(tiles[row][col].Width / 1.4), tiles[row][col].Y + (int)Math.Round(tiles[row][col].Height / 1.6), tiles[row][col].Width, tiles[row][col].Height, 2, 0, 0, r.Next(-100, 100));
                                            pinkP.Center = true;
                                            pinkP.GrowingAmount = 2;
                                            pinkP.AlphaAmount = 1;
                                            emitter1.AddParticle(pinkP);
                                        }

                                    }

                                    tiles[row][col] = null;

                                }

                            }
                        }
                    }
                }
            }

            if (match4Circles)
            {

                Random r = new Random();

                if (r.Next(0, 100) < 5)
                    AddCircleParticle(30, r.Next(0, BaseGame.game.ScreenWidth), r.Next(0, BaseGame.game.ScreenHeight), r.Next(BaseGame.game.ScreenWidth / 30, BaseGame.game.ScreenWidth / 20), r.Next(BaseGame.game.ScreenWidth / 30, BaseGame.game.ScreenWidth / 20), 1.2f, r.Next(BaseGame.game.ScreenWidth / 30, BaseGame.game.ScreenWidth / 20), r.Next(BaseGame.game.ScreenWidth / 4, BaseGame.game.ScreenWidth / 3));

                match4CirclesTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (match4CirclesTimer <= 0)
                {
                    match4Circles = false;
                }

            }

        }

        /**
         *  Update forces
         */
        private void UpdateForces(MouseState newMouseState, GameTime gameTime)
        {

            foreach (Force f in forces)
            {

                // Update the text of the force

                f.text.Text = Langs.Texts[f.LangTitlePath][BaseGame.game.Lang];

                float a = (float)LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / (f.XpToHave + 1);

                f.text.Rect = new Rectangle((int) Math.Round(leftPan.Rect.Width / 2.3), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)), (int) Math.Round(leftPan.Rect.Width / 8 - (textStars.Font.MeasureString(textStars.Text).X / textStars.Rect.Width) / 2), (int)Math.Round(leftPan.Rect.Height / 19 - (textStars.Font.MeasureString(textStars.Text).Y / textStars.Rect.Height) / 2));

                // Detect if a force is owned
                if (xp >= f.XpToHave)
                {

                    f.text.Color = new Color(46, 204, 113);

                    if (!forcesInventory.Contains(f))
                        forcesInventory.Add(f);
                }

            }

            int i = 0;
            foreach (Force fi in forcesInventory)
            {


                fi.button.Rect = new Rectangle(BaseGame.game.ScreenWidth - fi.button.Rect.Width - (fi.button.Rect.Width / 4), BaseGame.game.ScreenHeight - (fi.button.Rect.Height * (i + 1) + (fi.button.Rect.Height / 4 * (i + 1))), BaseGame.game.ScreenWidth / 11, BaseGame.game.ScreenHeight / 6);

                // mouse
                if (fi.button.Rect.Contains(newMouseState.Position))
                {
                    if (!fi.IsHovered)
                    {
                        Assets.SndButtonHovered.Play();
                    }
                    fi.IsHovered = true;

                    // detect click
                    if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        fi.Used = true;
                        fi.Load();
                    }

                }
                else
                {
                    fi.IsHovered = false;
                }

                if(fi.Used)
                {
                    fi.Update(gameTime, this);
                }

                i++;

            }

            forcesInventory.RemoveAll(item => item.Done == true);
            forces.RemoveAll(item => item.Done == true);

        }

        /**
         * Draw Tiles
         */
        private void DrawTiles()
        {
            if (tilesGenerated)
            {
                for (int row = tiles.Count - 1; row >= 0; row--)
                {
                    for (int col = tiles[row].Count - 1; col >= 0; col--)
                    {
                        
                        if (row < tiles.Count - 1)
                        {
                            if (tiles[row + 1][col] != null)
                            {
                                if (tiles[row + 1][col].isTileMoving && tiles[row + 1][col].movingDirection == "down")
                                {
                                    //if (tiles[row + 1][col].Y > -tileHeight - 10 && tiles[row + 1][col].Y < BaseGame.game.ScreenHeight + 10)
                                        tiles[row + 1][col].Draw(BaseGame.spriteBatch);
                                    tiles[row + 1][col].fallingDrawn = true;
                                }
                            }
                        }
                        if (tiles[row][col] != null)
                        {

                                //if (tiles[row][col].Y > -tileHeight - 10 && tiles[row][col].Y < BaseGame.game.ScreenHeight + 10 && !tiles[row][col].fallingDrawn)
                                tiles[row][col].Draw(BaseGame.spriteBatch);
                            tiles[row][col].fallingDrawn = false;
                        }
                    }
                }
            }
        }

        /**
         *  Draw forces
         */
        private void DrawForces()
        {
            foreach (Force f in forces)
            {
                f.text.Draw(BaseGame.spriteBatch);
            }

            foreach (Force fi in forcesInventory)
            {
                if (fi.IsHovered)
                {
                    BaseGame.spriteBatch.Draw(fi.button.Texture, fi.button.Rect, new Color(100, 255, 255));
                }
                else if (fi.Used)
                {
                    BaseGame.spriteBatch.Draw(fi.button.Texture, fi.button.Rect, new Color(200, 50, 50));
                }
                else
                {
                    BaseGame.spriteBatch.Draw(fi.button.Texture, fi.button.Rect, Color.White);
                }

                if (fi.Used)
                {
                    fi.Draw();
                }

            }

        }
        
        /**
         * 
         *  Detect matches
         * 
         */
        private void DetectMatches()
        {
            for (int row = 0; row < tiles.Count; row++)
            {
                for (int col = 0; col < tiles[row].Count - 2; col++)
                {

                    // check line

                    if (tiles[row][col] != null && tiles[row][col + 1] != null && tiles[row][col + 2] != null)
                    {
                        Tile tile1 = tiles[row][col];
                        Tile tile2 = tiles[row][col + 1];
                        Tile tile3 = tiles[row][col + 2];

                        if (tile1.Id == tile2.Id && tile1.Id == tile3.Id && tile3.Id == tile2.Id && tile1.activated && tile2.activated && tile3.activated)
                        {
                            if (!tile1.Numbered && !tile2.Numbered && !tile3.Numbered)
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove)
                                {

                                    bool match4 = false;

                                    int xp = 0;

                                    if (col < tiles[row].Count - 3)
                                    {
                                        if (tiles[row][col + 3] != null)
                                        {
                                            Tile tile4 = tiles[row][col + 3];
                                            if (tile3.Id == tile4.Id && tile4.activated)
                                            {
                                                tiles[row][col + 3].toRemove = true;
                                                tiles[row][col + 3].removeCounter = 2.3f;
                                                tiles[row][col + 3].popNbr = 4;

                                                CEParticle p = new CEParticle(Assets.FXTwirl, tiles[row][col + 3].X + tiles[row][col + 3].Width / 2, tiles[row][col + 3].Y + tiles[row][col + 3].Height / 2, tiles[row][col + 3].Width * 2, tiles[row][col + 3].Height * 2, 1.5f, 0, 0, 500);
                                                p.Center = true;
                                                p.AlphaAmount = 1f;
                                                emitter1.AddParticle(p);

                                                match4 = true;

                                                if (!isPaused && tilesGenerated)
                                                {
                                                    BaseGame.game.Player.Stars += tile3.StarsToGet;
                                                    starsWon += tile3.StarsToGet;
                                                    DataManager.WriteFile("player.json", BaseGame.game.Player);
                                                    BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                                    Assets.SndMatch.Play();

                                                    AddCircleParticle(25, tiles[row][col + 2].X + tiles[row][col + 2].Width / 2, tiles[row][col + 2].Y + tiles[row][col + 2].Height / 2, tiles[row][col + 2].Width / 3, tiles[row][col + 2].Height / 3, 1, tiles[row][col + 2].Width / 3, tiles[row][col + 2].Width * 3);
                                                    AddCircleParticle(30, tiles[row][col + 2].X + tiles[row][col + 2].Width / 2, tiles[row][col + 2].Y + tiles[row][col + 2].Height / 2, tiles[row][col + 2].Width / 3, tiles[row][col + 2].Height / 3, 1.2f, tiles[row][col + 2].Width / 3, tiles[row][col + 2].Width * 2);

                                                    match4Circles = true;
                                                    match4CirclesTimer = 3;

                                                    // match 4 text
                                                    AddText(Assets.Match4Text);

                                                    xp += 50;

                                                }

                                            }
                                        }
                                    }
                                    tiles[row][col].toRemove = true;
                                    tiles[row][col + 1].toRemove = true;
                                    tiles[row][col + 2].toRemove = true;
                                    tiles[row][col].removeCounter = 1.1f;
                                    tiles[row][col + 1].removeCounter = 1.5f;
                                    tiles[row][col + 2].removeCounter = 1.9f;
                                    tiles[row][col].popNbr = 1;
                                    tiles[row][col + 1].popNbr = 2;
                                    tiles[row][col + 2].popNbr = 3;

                                    tiles[row][col + 1].OnMatch(this);
                                    
                                    float a = (float)LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / xp;

                                    CEParticle xpP = new CEParticle(Assets.FXXP, 0, 0, (int) Math.Round(tiles[row][col + 1].Width / 1.5), (int) Math.Round(tiles[row][col + 1].Height / 1.5), 3, 0, 0, 80);

                                    xpP.Center = true;
                                    
                                    xpP.Props.Add("time", 0);
                                    xpP.Props.Add("beginx", tiles[row][col + 1].X);
                                    xpP.Props.Add("beginy", tiles[row][col + 1].Y);
                                    xpP.Props.Add("distancex", -(tiles[row][col + 1].X - (leftPan.Rect.X + (leftPan.Rect.Width / 3))));
                                    xpP.Props.Add("distancey", (BaseGame.game.ScreenHeight - tiles[row][col + 1].Y) - (BaseGame.game.ScreenHeight - ((int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)))));
                                    xpP.Props.Add("duration", 3);
                                    xpP.Props.Add("scene", this);
                                    
                                    for (int i = 0; i < 3; i++)
                                    {
                                        CEParticle p = new CEParticle(Assets.FXTwirl, tiles[row][col + i].X + tiles[row][col + i].Width / 2, tiles[row][col + i].Y + tiles[row][col + i].Height / 2, tiles[row][col + i].Width * 2, tiles[row][col + i].Height * 2, 1.5f, 0, 0, 500);
                                        p.Center = true;
                                        p.AlphaAmount = 1f;
                                        emitter1.AddParticle(p);
                                    }
                                    
                                    if (!isPaused && tilesGenerated)
                                    {
                                        BaseGame.game.Player.Stars += tile1.StarsToGet;
                                        starsWon += tile1.StarsToGet;
                                        DataManager.WriteFile("player.json", BaseGame.game.Player);
                                        BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                        xpP.Props.Add("xp", xp + 50);
                                        Assets.SndMatch.Play();
                                    tiles[row][col + 2].toRemove = true;

                                        if (!match4)
                                        {
                                            AddCircleParticle(25, tiles[row][col + 1].X + tiles[row][col + 1].Width / 2, tiles[row][col + 1].Y + tiles[row][col + 1].Height / 2, tiles[row][col + 1].Width / 3, tiles[row][col + 1].Height / 3, 1, tiles[row][col + 1].Width / 3, tiles[row][col + 1].Width * 3);
                                        }
                                        emitterXP.AddParticle(xpP);
                                    
                                    }
                                }
                            }
                            else
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove && !tile1.toStack && !tile3.toStack)
                                {

                                    Tile.Tweening tweeningToRight = new Tile.Tweening();
                                    tweeningToRight.time = 0;
                                    tweeningToRight.duration = .2f;
                                    tweeningToRight.value = 0;
                                    tweeningToRight.distance = tileWidth;

                                    Tile.Tweening tweeningToLeft = new Tile.Tweening();
                                    tweeningToLeft.time = 0;
                                    tweeningToLeft.duration = .2f;
                                    tweeningToLeft.value = 0;
                                    tweeningToLeft.distance = -tileWidth;

                                    float a = (float)LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / xp;

                                    CEParticle xpP = new CEParticle(Assets.FXXP, 0, 0, (int)Math.Round(tiles[row][col + 1].Width / 1.5), (int)Math.Round(tiles[row][col + 1].Height / 1.5), 3, 0, 0, 80);

                                    xpP.Center = true;

                                    xpP.Props.Add("time", 0);
                                    xpP.Props.Add("beginx", tiles[row][col + 1].X);
                                    xpP.Props.Add("beginy", tiles[row][col + 1].Y);
                                    xpP.Props.Add("distancex", -(tiles[row][col + 1].X - (leftPan.Rect.X + (leftPan.Rect.Width / 3))));
                                    xpP.Props.Add("distancey", (BaseGame.game.ScreenHeight - tiles[row][col + 1].Y) - (BaseGame.game.ScreenHeight - ((int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)))));
                                    xpP.Props.Add("duration", 3);
                                    xpP.Props.Add("scene", this);

                                    xpP.Props.Add("xp", 20);

                                    emitterXP.AddParticle(xpP);

                                    Random r = new Random();

                                    switch (tile2.Id)
                                    {
                                        case "num_1":
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "x";
                                            tiles[row][col].tweeningStack = tweeningToRight;
                                            tiles[row][col].stackTileToGet = new TileNum2(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col + 2].toStack = true;
                                            tiles[row][col + 2].stackAxe = "x";
                                            tiles[row][col + 2].tweeningStack = tweeningToLeft;
                                            break;

                                        case "num_2":
                                            //tiles[row][col + 1] = new TileNum3(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "x";
                                            tiles[row][col].tweeningStack = tweeningToRight;
                                            tiles[row][col].stackTileToGet = new TileNum3(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col + 2].toStack = true;
                                            tiles[row][col + 2].stackAxe = "x";
                                            tiles[row][col + 2].tweeningStack = tweeningToLeft;
                                            break;

                                        case "num_3":
                                            //tiles[row][col + 1] = new TileNum4(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "x";
                                            tiles[row][col].tweeningStack = tweeningToRight;
                                            tiles[row][col].stackTileToGet = new TileNum4(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col + 2].toStack = true;
                                            tiles[row][col + 2].stackAxe = "x";
                                            tiles[row][col + 2].tweeningStack = tweeningToLeft;
                                            break;

                                        case "num_4":
                                            //tiles[row][col + 1] = new TileNum5(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "x";
                                            tiles[row][col].tweeningStack = tweeningToRight;
                                            tiles[row][col].stackTileToGet = new TileNum5(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            tiles[row][col + 2].toStack = true;
                                            tiles[row][col + 2].stackAxe = "x";
                                            tiles[row][col + 2].tweeningStack = tweeningToLeft;
                                            break;

                                        case "num_5":
                                            tiles[row][col].toRemove = true;
                                            tiles[row][col + 1].toRemove = true;
                                            tiles[row][col + 2].toRemove = true;
                                            tiles[row][col].removeCounter = 0.2f;
                                            tiles[row][col + 1].removeCounter = 0.4f;
                                            tiles[row][col + 2].removeCounter = 0.6f;

                                            for (int i = 0; i < 3; i++)
                                            {
                                                emitterL.AddParticle(Assets.FXL1, tiles[row][col + i].X - tiles[row][col + i].Width / 2, tiles[row][col + i].Y - tiles[row][col + i].Height / 2, tiles[row][col + i].Width * 2, tiles[row][col + i].Height * 2, .5f, 0, 0, 0);
                                            }

                                            if (!isPaused && tilesGenerated)
                                            {
                                                BaseGame.game.Player.Stars += tile1.StarsToGet;
                                                starsWon += tile1.StarsToGet;
                                                DataManager.WriteFile("player.json", BaseGame.game.Player);
                                                BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                            }
                                            break;

                                        default:
                                            break;
                                    }

                                }
                            }
                        }
                    }

                }

            }

            for (int row = 0; row < tiles.Count - 2; row++)
            {
                for (int col = 0; col < tiles[row].Count; col++)
                {
                    // check col

                    if (tiles[row][col] != null && tiles[row + 1][col] != null && tiles[row + 2][col] != null)
                    {
                        Tile tile1 = tiles[row][col];
                        Tile tile2 = tiles[row + 1][col];
                        Tile tile3 = tiles[row + 2][col];

                        if (tile1.Id == tile2.Id && tile1.Id == tile3.Id && tile3.Id == tile2.Id && tile1.activated && tile2.activated && tile3.activated)
                        {
                            if (!tile1.Numbered && !tile2.Numbered && !tile3.Numbered)
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove)
                                {

                                    bool match4 = false;

                                    int xp = 0;

                                    if (row < tiles.Count - 3)
                                    {
                                        if (tiles[row + 3][col] != null)
                                        {
                                            Tile tile4 = tiles[row + 3][col];
                                            if (tile3.Id == tile4.Id && tile4.activated)
                                            {
                                                tiles[row + 3][col].toRemove = true;
                                                tiles[row + 3][col].removeCounter = 2.3f;
                                                tiles[row + 3][col].popNbr = 4;

                                                CEParticle p = new CEParticle(Assets.FXTwirl, tiles[row + 3][col].X + tiles[row + 3][col].Width / 2, tiles[row + 3][col].Y + tiles[row + 3][col].Height / 2, tiles[row + 3][col].Width * 2, tiles[row + 3][col].Height * 2, 1.5f, 0, 0, 500);
                                                p.Center = true;
                                                p.AlphaAmount = 1f;
                                                emitter1.AddParticle(p);

                                                match4 = true;

                                                if (!isPaused && tilesGenerated)
                                                {
                                                    BaseGame.game.Player.Stars += tile3.StarsToGet;
                                                    starsWon += tile3.StarsToGet;
                                                    DataManager.WriteFile("player.json", BaseGame.game.Player);
                                                    BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                                    Assets.SndMatch.Play();

                                                    AddCircleParticle(25, tiles[row + 2][col].X + tiles[row + 2][col].Width / 2, tiles[row + 2][col].Y + tiles[row + 2][col].Height / 2, tiles[row + 2][col].Width / 3, tiles[row + 2][col].Height / 3, 1, tiles[row + 2][col].Width / 3, tiles[row + 2][col].Width * 3);
                                                    AddCircleParticle(30, tiles[row + 2][col].X + tiles[row + 2][col].Width / 2, tiles[row + 2][col].Y + tiles[row + 2][col].Height / 2, tiles[row + 2][col].Width / 3, tiles[row + 2][col].Height / 3, 1.2f, tiles[row + 2][col].Width / 3, tiles[row + 2][col].Width * 2);

                                                    match4Circles = true;
                                                    match4CirclesTimer = 3;

                                                    // match 4 text
                                                    AddText(Assets.Match4Text);

                                                    xp += 50;

                                                }

                                            }
                                        }
                                    }

                                    tiles[row][col].toRemove = true;
                                    tiles[row + 1][col].toRemove = true;
                                    tiles[row + 2][col].toRemove = true;
                                    tiles[row][col].removeCounter = 1.1f;
                                    tiles[row + 1][col].removeCounter = 1.5f;
                                    tiles[row + 2][col].removeCounter = 1.9f;
                                    tiles[row][col].popNbr = 1;
                                    tiles[row + 1][col].popNbr = 2;
                                    tiles[row + 2][col].popNbr = 3;

                                    tiles[row + 1][col].OnMatch(this);

                                    for (int i = 0; i < 3; i++)
                                    {
                                        CEParticle p = new CEParticle(Assets.FXTwirl, tiles[row + i][col].X + tiles[row + 1][col].Width / 2, tiles[row + i][col].Y + tiles[row + i][col].Height / 2, tiles[row + i][col].Width * 2, tiles[row + i][col].Height * 2, 1.5f, 0, 0, 500);
                                        p.Center = true;
                                        p.AlphaAmount = 1f;
                                        emitter1.AddParticle(p);
                                    }

                                    float a = (float)LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / xp;

                                    CEParticle xpP = new CEParticle(Assets.FXXP, 0, 0, (int)Math.Round(tiles[row + 1][col].Width / 1.5), (int)Math.Round(tiles[row + 1][col].Height / 1.5), 3, 0, 0, 80);

                                    xpP.Center = true;

                                    xpP.Props.Add("time", 0);
                                    xpP.Props.Add("beginx", tiles[row + 1][col].X);
                                    xpP.Props.Add("beginy", tiles[row + 1][col].Y);
                                    xpP.Props.Add("distancex", -(tiles[row + 1][col].X - (leftPan.Rect.X + (leftPan.Rect.Width / 3))));
                                    xpP.Props.Add("distancey", (BaseGame.game.ScreenHeight - tiles[row + 1][col].Y) - (BaseGame.game.ScreenHeight - ((int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)))));
                                    xpP.Props.Add("duration", 3);
                                    xpP.Props.Add("scene", this);

                                    if (!isPaused && tilesGenerated)
                                    {
                                        BaseGame.game.Player.Stars += tile1.StarsToGet;
                                        starsWon += tile1.StarsToGet;
                                        DataManager.WriteFile("player.json", BaseGame.game.Player);
                                        BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                        xpP.Props.Add("xp", xp + 50);
                                        Assets.SndMatch.Play();

                                        if (!match4)
                                            AddCircleParticle(25, tiles[row + 1][col].X + tiles[row + 1][col].Width / 2, tiles[row + 1][col].Y + tiles[row + 1][col].Height / 2, tiles[row + 1][col].Width / 3, tiles[row + 1][col].Height / 3, 1, tiles[row + 1][col].Width / 3, tiles[row + 1][col].Width * 3);

                                        emitterXP.AddParticle(xpP);

                                    }

                                }
                            }
                            else
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove && !tile1.toStack && !tile3.toStack)
                                {

                                    Tile.Tweening tweeningToBottom = new Tile.Tweening();
                                    tweeningToBottom.time = 0;
                                    tweeningToBottom.duration = .2f;
                                    tweeningToBottom.value = 0;
                                    tweeningToBottom.distance = tileHeight * 2;

                                    Tile.Tweening tweeningToBottom2 = new Tile.Tweening();
                                    tweeningToBottom2.time = 0;
                                    tweeningToBottom2.duration = .2f;
                                    tweeningToBottom2.value = 0;
                                    tweeningToBottom2.distance = tileHeight;

                                    float a = (float)LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp / xp;

                                    CEParticle xpP = new CEParticle(Assets.FXXP, 0, 0, (int)Math.Round(tiles[row + 1][col].Width / 1.5), (int)Math.Round(tiles[row + 1][col].Height / 1.5), 3, 0, 0, 80);

                                    xpP.Center = true;

                                    xpP.Props.Add("time", 0);
                                    xpP.Props.Add("beginx", tiles[row + 1][col].X);
                                    xpP.Props.Add("beginy", tiles[row + 1][col].Y);
                                    xpP.Props.Add("distancex", -(tiles[row + 1][col].X - (leftPan.Rect.X + (leftPan.Rect.Width / 3))));
                                    xpP.Props.Add("distancey", (BaseGame.game.ScreenHeight - tiles[row + 1][col].Y) - (BaseGame.game.ScreenHeight - ((int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration) + leftPan.Rect.Height / 2.4 + (xpBarHeight - xpBarHeight / a)))));
                                    xpP.Props.Add("duration", 3);
                                    xpP.Props.Add("scene", this);
                                    xpP.Props.Add("xp", 20);

                                    emitterXP.AddParticle(xpP);

                                    switch (tile2.Id)
                                    {
                                        case "num_1":
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "y";
                                            tiles[row][col].tweeningStack = tweeningToBottom;
                                            tiles[row + 1][col].toStack = true;
                                            tiles[row + 1][col].stackAxe = "y";
                                            tiles[row + 1][col].tweeningStack = tweeningToBottom2;
                                            tiles[row + 1][col].stackTileToGet = new TileNum2(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_2":
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "y";
                                            tiles[row][col].tweeningStack = tweeningToBottom;
                                            tiles[row + 1][col].toStack = true;
                                            tiles[row + 1][col].stackAxe = "y";
                                            tiles[row + 1][col].tweeningStack = tweeningToBottom2;
                                            tiles[row + 1][col].stackTileToGet = new TileNum3(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_3":
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "y";
                                            tiles[row][col].tweeningStack = tweeningToBottom;
                                            tiles[row + 1][col].toStack = true;
                                            tiles[row + 1][col].stackAxe = "y";
                                            tiles[row + 1][col].tweeningStack = tweeningToBottom2;
                                            tiles[row + 1][col].stackTileToGet = new TileNum4(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_4":
                                            tiles[row][col].toStack = true;
                                            tiles[row][col].stackAxe = "y";
                                            tiles[row][col].tweeningStack = tweeningToBottom;
                                            tiles[row + 1][col].toStack = true;
                                            tiles[row + 1][col].stackAxe = "y";
                                            tiles[row + 1][col].tweeningStack = tweeningToBottom2;
                                            tiles[row + 1][col].stackTileToGet = new TileNum5(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_5":

                                            tiles[row][col].toRemove = true;
                                            tiles[row + 1][col].toRemove = true;
                                            tiles[row + 2][col].toRemove = true;
                                            tiles[row][col].removeCounter = 0.6f;
                                            tiles[row + 1][col].removeCounter = 1;
                                            tiles[row + 2][col].removeCounter = 1.4f;

                                            for (int i = 0; i < 3; i++)
                                            {
                                                emitterL.AddParticle(Assets.FXL1, tiles[row + i][col].X - tiles[row + i][col].Width / 2, tiles[row + i][col].Y - tiles[row + i][col].Height / 2, tiles[row + i][col].Width * 2, tiles[row + i][col].Height * 2, .5f, 0, 0, 0);
                                            }

                                            if (!isPaused && tilesGenerated)
                                            {
                                                BaseGame.game.Player.Stars += tile1.StarsToGet;
                                                starsWon += tile1.StarsToGet;
                                                DataManager.WriteFile("player.json", BaseGame.game.Player);
                                                BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                            }
                                            break;

                                        default:
                                            break;
                                    }

                                }
                            }
                        }
                    }
                } 
            }
        }

        public void AddCircleParticle(int nbrStars, int x, int y, int width, int height, float time, int begin, int end)
        {
            float pAngle = 0;

            for (int i = 0; i < nbrStars; i++)
            {



                CEParticle pStar = new CEParticle(Assets.FXStar, 0, 0, width, height, time, 0, 0, 0);

                pStar.GrowingAmount = 10;
                pStar.AlphaAmount = 1;
                pStar.Center = true;

                pStar.Props.Add("angle", pAngle);
                pStar.Props.Add("distance", 0);
                pStar.Props.Add("ix", x);
                pStar.Props.Add("iy", y);
                pStar.Props.Add("t_time", 0);
                pStar.Props.Add("t_begin", begin);
                pStar.Props.Add("t_end", end);
                pStar.Props.Add("t_duration", time);

                emitterCircle.AddParticle(pStar);

                pAngle += 360 / nbrStars;

            }
        }

        public void AddText(Texture2D texture)
        {
            CEParticle pt = new CEParticle(texture, BaseGame.game.ScreenWidth / 2, BaseGame.game.ScreenHeight / 2, 0, 0, 3, 0, 0, 0);

            pt.Props.Add("ix", BaseGame.game.ScreenWidth * 2);
            pt.Props.Add("iy", BaseGame.game.ScreenHeight);
            pt.Props.Add("time", 0);
            pt.Props.Add("beginx", 0);
            pt.Props.Add("beginy", 0);
            pt.Props.Add("distancex", -BaseGame.game.ScreenWidth * 1.3);
            pt.Props.Add("distancey", -BaseGame.game.ScreenHeight / 1.6);
            pt.Props.Add("duration", 1.5f);

            pt.Center = true;

            emitterTexts.AddParticle(pt);
        }

        /**
         * 
         *  CARDS
         * 
         */
        public void PlayCard()
        {


            
        }

    }
}
