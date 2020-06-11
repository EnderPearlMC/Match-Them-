using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Match3;

/*
   _____          _        ______          _           
  / ____|        | |      |  ____|        (_)          
 | |     ___   __| | ___  | |__   __ _ ___ _  ___ _ __ 
 | |    / _ \ / _` |/ _ \ |  __| / _` / __| |/ _ \ '__|
 | |___| (_) | (_| |  __/ | |___| (_| \__ \ |  __/ |   
  \_____\___/ \__,_|\___| |______\__,_|___/_|\___|_|   
                                                      

    Made by EnderPearl MC

     This framework allows you to create games very quickly.
     Made with monogame.

     You are free to use this framework in all your projects
     but you cannot REDISTRIBUTE it. 

 */

namespace CodeEasier.Scene
{
    /*

    ImageElement class

    Create an image element (can be used for backgrounds)

    Usage : Instance it.

    */

    class CESpriteElement : ICESceneDrawable
    {

        /*
         *  Properties 
         */

        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }
        public float VX { get; set; }
        public float VY { get; set; }
        public bool ToRemove { get; set; }

        /*
         *  Constructor
         */

        /**
         * <param type="Texture2D" name="texture">The texture of the image</param>
         * <param type="Vector2" name="position">The position of the image</param>
         * <param type="int" name="width">The width of the sprite</param>
         * <param type="int" name="height">The height of the sprite</param>
         */
        public CESpriteElement(Texture2D texture, Vector2 position, int width, int height)
        {
            Texture = texture;
            Position = position;
            Width = width;
            Height = height;
        }

        /*
         *  Methods 
         */

        public virtual void Load()
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = new Vector2(Position.X + VX * dt, Position.Y + VY * dt);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Width, Height), Color.White);
        }

    }
}
