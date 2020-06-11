using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

namespace CodeEasier.Scene.UI
{

    /*

        UITextScroller class

        Makes text that scrolls on the window

        Usage : Call CEUI.TextScroller(); (returns an instance)

    */

    class CEUITextScroller : ICESceneDrawable
    {

        /*
         *  Properties 
         */
        public Main BaseGame { get; private set; }
        public string[] Lines { get; private set; }
        public SpriteFont Font { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Speed { get; set; }

        public bool ToRemove { get; set; }

        /*
         *  Constructor
         */

        /**
         *  <param type="string[]" name="lines">The lines of text of the scroller</param>
         *  <param type="SpriteFont" name="font">The font of the scroller</param>
         *  <param type="int" name="speed">The speed of the scroller</param>
         *  <param type="Main" name="baseGame">The base game</param>
         */
        public CEUITextScroller(string[] lines, SpriteFont font, int speed, Main baseGame)
        {
            BaseGame = baseGame;
            Lines = lines;
            Font = font;
            Width = 0;
            Height = 0;
            Speed = speed;
        }

        /*
         *  Methods 
         */
        
        public void Load()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            Y -= (float) gameTime.ElapsedGameTime.TotalSeconds * Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int counter = 0;
            foreach (string line in Lines)
            {
                spriteBatch.DrawString(Font, line, new Vector2(X, Y + (counter * Height)), Color.White, 0, Vector2.Zero, new Vector2(Width / Font.MeasureString(line).X, Height / Font.MeasureString(line).Y), SpriteEffects.None, 0);
                counter++;
            }
        }

    }
}
