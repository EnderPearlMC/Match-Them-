using CodeEasier;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    abstract class Tile
    {

        public string Id { get; private set; }
        public bool Numbered { get; private set; }
        public bool ToRemove { get; set; }
        public int StarsToGet { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public float XOff { get; set; }
        public float YOff { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; set; }

        protected MouseState oldMouseState;

        protected Vector2 initialMousePos;
        protected bool isMouseMoving;
        public bool isTileMoving;

        public string movingDirection;
        public Vector2 positionToGo;

        protected Tile oldTile;
        protected Tile newTile;

        protected bool canFall;

        public bool toRemove;
        public float removeCounter;

        public bool toStack;
        public bool toBeStacked;
        public float toBeStackedCounter;
        public string stackAxe;
        public Tile stackTileToGet;

        public struct Tweening
        {
            public double time;
            public double value;
            public int distance;
            public double duration;
        }

        public Tweening tweeningStack;

        /**
         *  <param name="id">The id of the tile</param>
         *  <param name="numbered">If the tile is numbered and can be stacked</param>
         *  <param name="starsToGet">The amount of stars to get</param>
         *  <param name="x">The X position of the tile</param>
         *  <param name="y">The Y position of the tile</param>
         *  <param name="width">The width position of the tile</param>
         *  <param name="height">The height position of the tile</param>
         *  <param name="texture">The texture of the tile</param>
         */
        public Tile(string id, bool numbered, int starsToGet, int x, int y, int width, int height, Texture2D texture)
        {
            Id = id;
            Numbered = numbered;
            StarsToGet = starsToGet;
            X = x;
            Y = y;
            XOff = 0;
            YOff = 0;
            Width = width;
            Height = height;
            Texture = texture;
            oldMouseState = Mouse.GetState();
            isMouseMoving = false;
            isTileMoving = false;
            movingDirection = "none";
            positionToGo = new Vector2(0, 0);
            toRemove = false;
            toStack = false;
            toBeStacked = false;

        }

        /**
         *  Update the tile
         */
        public virtual void Update(SceneLevel scene, GameTime gameTime, Main BaseGame)
        {
            MouseState newMouseState = Mouse.GetState();

            Rectangle rect = new Rectangle(X, Y, scene.tileWidth, scene.tileHeight);

            int row = (int)Math.Floor((Y - (scene.tileYOffset + scene.tileScrollY) - YOff) / scene.tileHeight);
            int col = (int)Math.Floor((X - scene.tileXOffset - XOff) / scene.tileWidth);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (toRemove)
                removeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isTileMoving)
            {
                canFall = true;
            }

            if (toStack)
            {
                if (tweeningStack.time < tweeningStack.duration)
                    tweeningStack.time += gameTime.ElapsedGameTime.TotalSeconds;

                if (stackAxe == "x")
                    XOff = (int)Math.Round(Utils.EaseOutSin(tweeningStack.time, tweeningStack.value, tweeningStack.distance, tweeningStack.duration));
                else if (stackAxe == "y")
                    YOff = (int)Math.Round(Utils.EaseOutSin(tweeningStack.time, tweeningStack.value, tweeningStack.distance, tweeningStack.duration));

            }

            if (toBeStacked)
            {
                toBeStackedCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (toBeStackedCounter <= 0)
                {
                    toBeStacked = false;
                }

            }

            /*
             *  
             *  Move tile
             * 
             */
            if (isTileMoving)
            {

                if (movingDirection != "")
                {

                    if (movingDirection == "left")
                    {
                        XOff -= dt * (BaseGame.game.ScreenWidth / 3);
                        
                        // Verify if the tile has reached its position
                        if (X <= positionToGo.X)
                        {

                            isTileMoving = false;
                            XOff = 0;
                            YOff = 0;
                            canFall = true;

                        }
                    }
                    else if (movingDirection == "right")
                    {
                        XOff += dt * (BaseGame.game.ScreenWidth / 3);
                        // Verify if the tile has reached its position
                        if (X >= positionToGo.X)
                        {

                            isTileMoving = false;
                            XOff = 0;
                            YOff = 0;
                            canFall = true;

                        }
                    }
                    else if (movingDirection == "down")
                    {
                        YOff += dt * (BaseGame.game.ScreenWidth / 3);
                        // Verify if the tile has reached its position
                        if (Y >= positionToGo.Y)
                        {

                            isTileMoving = false;
                            XOff = 0;
                            YOff = 0;

                        }
                    }
                }
            }

            if (toStack)
            {

            }

            if (!scene.isPaused)
            {

                /*
                 *  Detect mouse 
                 * 
                 */
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !isTileMoving)
                {

                    if (rect.Contains(newMouseState.Position))
                    {

                        initialMousePos = new Vector2(newMouseState.X, newMouseState.Y);
                        isMouseMoving = true;

                    }

                }

                int mouseRow = (int)Math.Floor((newMouseState.Y - (scene.tileYOffset + scene.tileScrollY - YOff)) / scene.tileHeight);
                int mouseCol = (int)Math.Floor((newMouseState.X - scene.tileXOffset - XOff) / scene.tileWidth);

                if (newMouseState.LeftButton == ButtonState.Pressed && !isTileMoving)
                {
                    if (initialMousePos != null && initialMousePos.X != 0 && initialMousePos.Y != 0)
                    {

                        if (mouseCol != col && mouseRow == row && isMouseMoving && mouseRow >= 0 && mouseRow < scene.tiles.Count && mouseCol >= 0 && mouseCol < scene.tiles[row].Count)
                        {

                            if (col >= 0 && col < scene.tiles[row].Count)
                            {
                                newTile = scene.tiles[row][col];
                                oldTile = scene.tiles[row][mouseCol];

                                int initialCol = (int)Math.Floor((initialMousePos.X - scene.tileXOffset - XOff) / scene.tileWidth);

                                if (oldTile != newTile)
                                {
                                    int diff = mouseCol - col;

                                    if (row < scene.tiles.Count - 1)
                                    {
                                        if (scene.tiles[row + 1][col] != null)
                                        {
                                            if (diff == -1)
                                            {
                                                scene.tiles[mouseRow][mouseCol] = newTile;
                                                scene.tiles[row][col] = oldTile;
                                                isTileMoving = true;
                                                XOff = scene.tileHeight;
                                                movingDirection = "left";

                                                if (oldTile != null)
                                                {
                                                    oldTile.movingDirection = "right";
                                                    oldTile.isTileMoving = true;
                                                    oldTile.XOff = -scene.tileHeight;
                                                    oldTile.positionToGo = new Vector2(col * scene.tileWidth + scene.tileXOffset, Y);
                                                }

                                                positionToGo = new Vector2(mouseCol * scene.tileWidth + scene.tileXOffset, Y);

                                                canFall = false;

                                                Assets.SndTileSlide.Play();
                                            }

                                            if (diff == 1 && mouseCol != initialCol)
                                            {
                                                scene.tiles[mouseRow][mouseCol] = newTile;
                                                scene.tiles[row][col] = oldTile;
                                                isTileMoving = true;
                                                XOff = -scene.tileHeight;
                                                movingDirection = "right";

                                                if (oldTile != null)
                                                {
                                                    oldTile.movingDirection = "left";
                                                    oldTile.isTileMoving = true;
                                                    oldTile.XOff = scene.tileHeight;
                                                    oldTile.positionToGo = new Vector2(col * scene.tileWidth + scene.tileXOffset, Y);
                                                }

                                                positionToGo = new Vector2(mouseCol * scene.tileWidth + scene.tileXOffset, Y);

                                                canFall = false;

                                                Assets.SndTileSlide.Play();

                                            }
                                        }
                                    }
                                }

                                initialMousePos = new Vector2(newMouseState.X, newMouseState.Y);
                            }
                        }
                    }
                }

                if (newMouseState.LeftButton == ButtonState.Released)
                {
                    isMouseMoving = false;
                    initialMousePos = new Vector2(0, 0);
                }

            }

            /**
            * 
            *  Gravity
            * 
            */

            if (row < scene.tiles.Count - 1 && row >= 0)
            {
                if (col >= 0 && col < scene.tiles[row].Count)
                {
                    if (scene.tiles[row + 1][col] == null && scene.tiles[row][col] != null)
                    {
                        if (scene.tiles[row][col].canFall && !scene.tiles[row][col].isTileMoving && !scene.tiles[row][col].toBeStacked)
                        {
                            Tile upTile = scene.tiles[row][col];
                            Tile downTile = scene.tiles[row + 1][col];
                            scene.tiles[row + 1][col] = upTile;
                            scene.tiles[row][col] = null;

                            if (!scene.isPaused)
                            {
                                isTileMoving = true;
                                movingDirection = "down";
                                YOff = -scene.tileHeight;
                                positionToGo = new Vector2(X, (row + 1) * scene.tileHeight + scene.tileYOffset + scene.tileScrollY);
                            }
                        }
                    }
                }
            }

            oldMouseState = newMouseState;

        }

        /**
         *  Draw the tile
         */
        public virtual void Draw(SpriteBatch spriteBatch)
        {

            Color c = new Color(255, 255, 255);

            if (toRemove)
                c = new Color(255, 203, 0);

            spriteBatch.Draw(Texture, new Rectangle(X, Y, Width, Height), c);
        }
    }
}
