using CodeEasier;
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
        CEImageElement leftPan;
        CEImageElement star;

        // Text elements
        CETextElement textStars;

        // Buttons
        CEUIButton pauseButton;

        // polish
        CETransition transitionIn;
        EmitterFallingTiles emitterFallingTiles;
        EmitterSpark1 emitterSpark1;

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

        public bool isPaused;

        // tiles
        public bool tilesGenerated;
        public List<List<Tile>> tiles;

        public int tileWidth;
        public int tileHeight;

        public int tileXOffset;
        public int tileYOffset;
        public float tileScrollY;

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

            isPaused = true;

            tilesGenerated = false;
            tiles = new List<List<Tile>>();

            tileWidth = 0;
            tileHeight = 0;

            tileXOffset = 0;
            tileYOffset = 0;
            tileScrollY = 0;

            starsWon = 0;

            xp = 0;

            xpBarWidth = 0;
            xpBarHeight = 0;

            forces = new List<Force>();

            forcesInventory = new List<Force>();

            LoadDrawables();

            transitionIn = new CETransition(3f, CETransition.Type.In);
            emitterFallingTiles = new EmitterFallingTiles();
            emitterSpark1 = new EmitterSpark1();

            GenerateTiles();

            GenerateForces();

            oldMouseState = Mouse.GetState();

            base.Load(parameters);
        }

        public override void Update(GameTime gameTime)
        {

            tileWidth = (int) Math.Round(BaseGame.game.ScreenWidth / 17.6);
            tileHeight = BaseGame.game.ScreenHeight / 10;

            tileXOffset = (int) Math.Round(BaseGame.game.ScreenWidth / 2.5);
            tileYOffset = BaseGame.game.ScreenHeight;

            UpdateDrawables();

            UpdateTweenings(gameTime);

            DetectMatches();

            UpdateTiles(gameTime);

            UpdateForces();

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            emitterFallingTiles.Update(gameTime);
            emitterSpark1.Update(gameTime);

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

            xpBarWidth = leftPan.Rect.Width / 13;
            xpBarHeight = (int) Math.Round(BaseGame.game.ScreenHeight / 2.7);

            if (xp >= LevelsData.LevelsList[BaseGame.game.Player.Level - 1].MaxXp)
            {
                isPaused = true;
                Dictionary<string, Object> p = new Dictionary<string, object>
                {
                    { "stars_won", starsWon }
                };

                BaseGame.game.ChangeScene("win", p);
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            DrawTiles();

            spawnTilesBar.Draw(BaseGame.spriteBatch);

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
            emitterSpark1.Draw(BaseGame.spriteBatch);

            transitionIn.Draw(BaseGame);
            
        }

        /**
         * Load drawables
         */
        private void LoadDrawables()
        {

            background = new CEImageElement(Assets.Background2, new Rectangle(0, 0, 0, 0));
            spawnTilesBar = new CEImageElement(Assets.LevelSpawnTilesBar, new Rectangle(0, 0, 0, 0));
            leftPan = new CEImageElement(Assets.LevelLeftPan, new Rectangle(0, 0, 0, 0));
            star = new CEImageElement(Assets.Star, new Rectangle(0, 0, 0, 0));

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
            spawnTilesBar.Rect = new Rectangle((int) Math.Round(Utils.EaseOutSin(tweeningSpawBar.time, tweeningSpawBar.value, tweeningSpawBar.distance, tweeningSpawBar.duration)), BaseGame.game.ScreenHeight - spawnTilesBar.Rect.Height, (int) Math.Round(BaseGame.game.ScreenWidth / 2.2), (int) Math.Round(BaseGame.game.ScreenHeight / 9.5));
            leftPan.Rect = new Rectangle(0, (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)), BaseGame.game.ScreenWidth / 3, (int) Math.Round(BaseGame.game.ScreenHeight / 1.2));
            star.Rect = new Rectangle((int) Math.Round(leftPan.Rect.Width / 1.7 - star.Rect.Width / 2), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)) + BaseGame.game.ScreenWidth / 13, leftPan.Rect.Width / 6, leftPan.Rect.Height / 9);
            textStars.Rect = new Rectangle((int)Math.Round(leftPan.Rect.Width / 1.9 - (textStars.Font.MeasureString(textStars.Text).X / textStars.Rect.Width) / 2), (int)Math.Round(Utils.EaseOutSin(tweeningLeftPan.time, tweeningLeftPan.value, tweeningLeftPan.distance, tweeningLeftPan.duration)) + BaseGame.game.ScreenWidth / 7, leftPan.Rect.Width / 9, leftPan.Rect.Height / 14);

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
            tweeningSpawBar.distance = (int) Math.Round(BaseGame.game.ScreenWidth / 2.5);

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
                        Force s = new ForceAxe();
                        s.XpToHave = item.Key;
                        forces.Add(s);
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
                        tiles[row][col].Width = tileWidth + tileWidth / 20;
                        tiles[row][col].Height = tileHeight + tileHeight / 20;
                        tiles[row][col].X = (int) Math.Round(col * tileWidth + tileXOffset + tiles[row][col].XOff);
                        tiles[row][col].Y = (int) Math.Round(row * tileHeight + tileYOffset + tileScrollY + tiles[row][col].YOff);
                        tiles[row][col].Update(this, gameTime, BaseGame);

                        // Increase counter if the tile has to be removed
                        if (tiles[row][col] != null)
                        {
                            if (tiles[row][col].toRemove)
                            {
                                tiles[row][col].removeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                                if (tiles[row][col].removeCounter <= 0)
                                {
                                    Random r = new Random();
                                    emitterFallingTiles.AddParticle(tiles[row][col].Texture, tiles[row][col].X, tiles[row][col].Y, tiles[row][col].Width, tiles[row][col].Height, 5, r.Next(-500, -300), 700, r.Next(-70, 70));

                                    // emitterSpark1.AddParticle(Assets.FXSpark1, tiles[row][col].X + tiles[row][col].Width, tiles[row][col].Y + tiles[row][col].Height, tiles[row][col].Width * 2, tiles[row][col].Height * 2, 2, 0, 0, 50);

                                    tiles[row][col] = null;
                                }

                            }
                        }
                    }
                }
            }

        }

        /**
         *  Update forces
         */
        private void UpdateForces()
        {

            MouseState newMouseState = Mouse.GetState();

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
                }
                else
                {
                    fi.IsHovered = false;
                }

                i++;

            }

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
                    for (int col = 0; col < tiles[row].Count; col++)
                    {
                        if (tiles[row][col] != null)
                        {
                            if (tiles[row][col].Y > -tileHeight - 10 && tiles[row][col].Y < BaseGame.game.ScreenHeight + 10)
                                tiles[row][col].Draw(BaseGame.spriteBatch);
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
                else
                {
                    BaseGame.spriteBatch.Draw(fi.button.Texture, fi.button.Rect, Color.White);
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

                        if (tile1.Id == tile2.Id && tile1.Id == tile3.Id && tile3.Id == tile2.Id)
                        {
                            if (!tile1.Numbered && !tile2.Numbered && !tile3.Numbered)
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove)
                                {
                                    /**tiles[row][col] = null;
                                    tiles[row][col + 1] = null;
                                    tiles[row][col + 2] = null;**/
                                    tiles[row][col].toRemove = true;
                                    tiles[row][col + 1].toRemove = true;
                                    tiles[row][col + 2].toRemove = true;
                                    tiles[row][col].removeCounter = 0.1f;
                                    tiles[row][col + 1].removeCounter = 0.3f;
                                    tiles[row][col + 2].removeCounter = 0.5f;

                                    if (!isPaused && tilesGenerated)
                                    {
                                        BaseGame.game.Player.Stars += tile1.StarsToGet;
                                        starsWon += tile1.StarsToGet;
                                        DataManager.WriteFile("player.json", BaseGame.game.Player);
                                        BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                        xp += 50;

                                    }

                                }
                            }
                            else
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving)
                                {
                                    tiles[row][col] = null;
                                    tiles[row][col + 2] = null;

                                    xp += 20;

                                    switch (tile2.Id)
                                    {
                                        case "num_1":
                                            tiles[row][col + 1] = new TileNum2(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_2":
                                            tiles[row][col + 1] = new TileNum3(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_3":
                                            tiles[row][col + 1] = new TileNum4(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_4":
                                            tiles[row][col + 1] = new TileNum5(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_5":
                                            tiles[row][col + 1] = null;
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

                        if (tile1.Id == tile2.Id && tile1.Id == tile3.Id && tile3.Id == tile2.Id)
                        {
                            if (!tile1.Numbered && !tile2.Numbered && !tile3.Numbered)
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving && !tile1.toRemove && !tile2.toRemove && !tile3.toRemove)
                                {
                                    tiles[row][col].toRemove = true;
                                    tiles[row + 1][col].toRemove = true;
                                    tiles[row + 2][col].toRemove = true;
                                    tiles[row][col].removeCounter = 0.1f;
                                    tiles[row + 1][col].removeCounter = 0.3f;
                                    tiles[row + 2][col].removeCounter = 0.5f;

                                    if (!isPaused && tilesGenerated)
                                    {
                                        BaseGame.game.Player.Stars += tile1.StarsToGet;
                                        starsWon += tile1.StarsToGet;
                                        DataManager.WriteFile("player.json", BaseGame.game.Player);
                                        BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                                        xp += 50;

                                    }

                                }
                            }
                            else
                            {
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving)
                                {
                                    tiles[row][col] = null;
                                    tiles[row + 2][col] = null;

                                    xp += 20;

                                    switch (tile2.Id)
                                    {
                                        case "num_1":
                                            tiles[row + 1][col] = new TileNum2(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_2":
                                            tiles[row + 1][col] = new TileNum3(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_3":
                                            tiles[row + 1][col] = new TileNum4(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_4":
                                            tiles[row + 1][col] = new TileNum5(tile2.X, tile2.Y, tileWidth, tileHeight);
                                            break;

                                        case "num_5":
                                            tiles[row + 1][col] = null;
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

    }
}
