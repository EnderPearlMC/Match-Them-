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

    class CEImageElement : ICESceneDrawable
    {

        /*
         *  Properties 
         */

        public Texture2D Texture { get; set; }
        public Rectangle Rect { get; set; }
        public bool ToRemove { get; set; }

        /*
         *  Constructor
         */

        /**
         * <param type="Texture2D" name="texture">The texture of the image</param>
         * <param type="Rectangle" name="rect">The rect of the image</param>
         */
        public CEImageElement(Texture2D texture, Rectangle rect)
        {
            Texture = texture;
            Rect = rect;
        }

        /*
         *  Methods 
         */

        public void Load()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.White);
        }

    }
}
