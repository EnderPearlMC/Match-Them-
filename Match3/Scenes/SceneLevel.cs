using CodeEasier;
using CodeEasier.Polish;
using CodeEasier.Scene;
using CodeEasier.Scene.UI;
using Match3.Datas;
using Match3.Levels.Tiles;
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
    class SceneLevel : CEScene
    {

        // Image Elements
        CEImageElement background;
        CEImageElement spawnTilesBar;
        CEImageElement leftPan;

        // Buttons
        CEUIButton pauseButton;

        // polish
        CETransition transitionIn;

        struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        Tweening tweeningSpawBar;
        Tweening tweeningLeftPan;

        // tiles
        public List<List<Tile>> tiles;

        string[] possibleTiles;

        public int tileWidth;
        public int tileHeight;

        public int tileXOffset;
        public int tileYOffset;
        public float tileScrollY;

        public SceneLevel(Main main) : base("level", main)
        {

            tweeningSpawBar = new Tweening();    
            tweeningSpawBar.time = 0;
            tweeningSpawBar.duration = 1;

            tweeningLeftPan = new Tweening();
            tweeningLeftPan.time = 0;
            tweeningLeftPan.duration = 2;

            tiles = new List<List<Tile>>();

            possibleTiles = new string[] { "written_1", "num_1", "num_2", "num_3", "num_4", "num_5" };

            tileWidth = 0;
            tileHeight = 0;

            tileXOffset = 0;
            tileYOffset = 0;
            tileScrollY = 0;

        }

        public override void Load()
        {

            LoadDrawables();

            transitionIn = new CETransition(3f, CETransition.Type.In);

            GenerateTiles();

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {

            tileWidth = (int) Math.Round(BaseGame.game.ScreenWidth / 17.6);
            tileHeight = BaseGame.game.ScreenHeight / 10;

            tileXOffset = (int) Math.Round(BaseGame.game.ScreenWidth / 2.5);
            tileYOffset = BaseGame.game.ScreenHeight;

            tileScrollY -= (float) gameTime.ElapsedGameTime.TotalSeconds * BaseGame.game.ScreenHeight / 20;

            UpdateDrawables();

            UpdateTweenings(gameTime);

            DetectMatches();

            UpdateTiles(gameTime);

            if (transitionIn.Alpha > 0)
            {
                transitionIn.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {

            base.Draw();

            DrawTiles();

            spawnTilesBar.Draw(BaseGame.spriteBatch);

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

            pauseButton = CEUI.Button(Langs.Texts["pause_button"][BaseGame.game.Lang], Assets.ButtonThemePath1, Color.White, BaseGame);

            AddDrawable(background);
            AddDrawable(leftPan);
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

            if (tweeningSpawBar.time < tweeningSpawBar.duration)
                tweeningSpawBar.time += gameTime.ElapsedGameTime.TotalSeconds;
            if (tweeningLeftPan.time < tweeningLeftPan.duration)
                tweeningLeftPan.time += gameTime.ElapsedGameTime.TotalSeconds;

        }

        /**
         *  Generate tiles
         */
        private void GenerateTiles()
        {

            Random r = new Random();

            for (int row = 0; row < 30; row++)
            {
                List<Tile> l = new List<Tile>();
                for (int col = 0; col < 8; col++)
                {
                    int nbr = r.Next(0, LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles.Count);
                    string t = LevelsData.LevelsList[BaseGame.game.Player.Level - 1].PossibleTiles[nbr];

                    switch (t)
                    {
                        case "written_1":
                            l.Add(new TileWritten1(col * 70, row * 70, tileWidth, tileHeight));
                            break;

                        case "written_2":
                            l.Add(new TileWritten2(col * 70, row * 70, tileWidth, tileHeight));
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
         *  Update Tiles
         */
        private void UpdateTiles(GameTime gameTime)
        {
            for (int row = tiles.Count - 1; row >= 0; row--)
            {
                for (int col = 0; col < tiles[row].Count; col++)
                {
                    if (tiles[row][col] != null)
                    {
                        tiles[row][col].Width = tileWidth + tileWidth / 20;
                        tiles[row][col].Height = tileHeight + tileHeight / 20;
                        tiles[row][col].X = (int) Math.Round(col * tileWidth + tileXOffset + tiles[row][col].XOff);
                        tiles[row][col].Y = (int) Math.Round(row * tileHeight + tileYOffset + tileScrollY + tiles[row][col].YOff);
                        tiles[row][col].Update(this, gameTime);
                    }
                }
            }

            // remove tiles


        }

        /**
         * Draw Tiles
         */
        private void DrawTiles()
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

        private void DetectMatches()
        {

            /**
             * 
             *  Detect basic matches
             * 
             */
            for (int row = 0; row < tiles.Count - 2; row++)
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
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving)
                                {
                                    tiles[row][col] = null;
                                    tiles[row][col + 1] = null;
                                    tiles[row][col + 2] = null;
                                }
                            }
                        }
                    }

                }

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
                                if (!tile1.isTileMoving && !tile2.isTileMoving && !tile3.isTileMoving)
                                {
                                    tiles[row][col] = null;
                                    tiles[row + 1][col] = null;
                                    tiles[row + 2][col] = null;
                                }
                            }
                        }
                    }
                }

            }

        }

    }
}
