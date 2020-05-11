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

        protected string movingDirection;
        protected Vector2 positionToGo;

        protected Tile oldTile;
        protected Tile newTile;

        protected bool canFall;

        /**
         *  <param name="id">The id of the tile</param>
         *  <param name="numbered">If the tile is numbered and can be stacked</param>
         *  <param name="x">The X position of the tile</param>
         *  <param name="y">The Y position of the tile</param>
         *  <param name="width">The width position of the tile</param>
         *  <param name="height">The height position of the tile</param>
         *  <param name="texture">The texture of the tile</param>
         */
        public Tile(string id, bool numbered, int x, int y, int width, int height, Texture2D texture)
        {
            Id = id;
            Numbered = numbered;
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
        }

        /**
         *  Update the tile
         */
        public virtual void Update(SceneLevel scene, GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            Rectangle rect = new Rectangle(X, Y, scene.tileWidth, scene.tileHeight);

            int row = (int)Math.Floor((Y - (scene.tileYOffset + scene.tileScrollY) - YOff) / scene.tileHeight);
            int col = (int)Math.Floor((X - scene.tileXOffset - XOff) / scene.tileWidth);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isTileMoving)
            {
                canFall = true;
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
                        XOff -= dt * 300;

                        // Verify if the tile has reached its position
                        if (X <= positionToGo.X)
                        {

                            isTileMoving = false;
                            XOff = 0;
                            canFall = true;

                        }
                    }
                    if (movingDirection == "right")
                    {
                        XOff += dt * 300;
                        // Verify if the tile has reached its position
                        if (X >= positionToGo.X)
                        {

                            isTileMoving = false;
                            XOff = 0;
                            canFall = true;

                        }
                    }
                    if (movingDirection == "down")
                    {
                        YOff += dt * 300;
                        // Verify if the tile has reached its position
                        if (Y >= positionToGo.Y)
                        {

                            isTileMoving = false;
                            YOff = 0;
                            canFall = true;

                        }
                    }
                }
            }

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

            if (newMouseState.LeftButton == ButtonState.Pressed && !isTileMoving) 
            {
                if (initialMousePos != null && initialMousePos.X != 0 && initialMousePos.Y != 0)
                {
                    int mouseRow = (int)Math.Floor((newMouseState.Y - (scene.tileYOffset + scene.tileScrollY - YOff)) / scene.tileHeight);
                    int mouseCol = (int)Math.Floor((newMouseState.X - scene.tileXOffset - XOff) / scene.tileWidth);

                    if (mouseCol != col && mouseRow == row && isMouseMoving)
                    {

                        newTile = scene.tiles[row][col];
                        oldTile = scene.tiles[row][mouseCol];

                        if (oldTile != newTile)
                        {
                            int diff = mouseCol - col;
                            Console.WriteLine(diff);
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
                            else if (diff == 1)
                            {
                                scene.tiles[mouseRow][mouseCol] = newTile;
                                scene.tiles[row][col] = oldTile;
                                isTileMoving = true;
                                oldTile.isTileMoving = true;
                                movingDirection = "right";
                                oldTile.movingDirection = "left";
                                XOff = -scene.tileHeight;
                                oldTile.XOff = scene.tileHeight;
                                positionToGo = new Vector2(mouseCol * scene.tileWidth + scene.tileXOffset, Y);
                                oldTile.positionToGo = new Vector2(col * scene.tileWidth + scene.tileXOffset, Y);
                                Assets.SndTileSlide.Play();
                                canFall = false;
                            }
                        }

                        isMouseMoving = false;
                    }
                }
            }

            /**
             * 
             *  Gravity
             * 
             */
             
            if (row < scene.tiles.Count - 1)
            {
                if (scene.tiles[row + 1][col] == null)
                {
                    if (scene.tiles[row][col].canFall)
                    {
                        Tile upTile = scene.tiles[row][col];
                        Tile downTile = scene.tiles[row + 1][col];
                        scene.tiles[row + 1][col] = upTile;
                        scene.tiles[row][col] = null;

                        isTileMoving = true;
                        movingDirection = "down";
                        YOff = -scene.tileHeight;
                        positionToGo = new Vector2(X, (row + 1) * scene.tileHeight + scene.tileYOffset + scene.tileScrollY);
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
            spriteBatch.Draw(Texture, new Rectangle(X, Y, Width, Height), Color.White);
        }

        private void UpdateGrid(SceneLevel scene, int row, int col)
        {

            int newCol = (int)(initialMousePos.X - scene.tileXOffset) / scene.tileWidth;
            Tile newTile = scene.tiles[row][newCol];

            scene.tiles[row][newCol] = scene.tiles[row][col];
            scene.tiles[row][col] = newTile;

            initialMousePos = new Vector2(0, 0);

            XOff = 0;

            isTileMoving = false;
        }

    }
}
