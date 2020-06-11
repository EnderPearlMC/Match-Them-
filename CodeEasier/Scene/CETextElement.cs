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

    TextElement class

    Create an text element

    Usage : Instance it.

    */

    class CETextElement : ICESceneDrawable
    {

        /*
         *  Properties 
         */

        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public Rectangle Rect { get; set; }
        public bool ToRemove { get; set; }

        /*
         *  Constructor
         */

        /**
         * <param type="string" name="text">The text</param>
         * <param type="SpriteFont" name="font">The font of the text</param>
         * <param type="Color" name="color">The color of the text</param>
         * <param type="Rectangle" name="rect">The rect of the text</param>
         */
        public CETextElement(string text, SpriteFont font, Color color, Rectangle rect)
        {
            Text = text;
            Font = font;
            Color = color;
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
            spriteBatch.DrawString(Font, Text, new Vector2(Rect.X, Rect.Y), Color, 0, Vector2.Zero, new Vector2(Rect.Width / Font.MeasureString(Text).X, Rect.Height / Font.MeasureString(Text).Y), SpriteEffects.None, 0);
        }

    }
}
