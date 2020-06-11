using CodeEasier.Scene;
using Match3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace CodeEasier.Polish
{

    /*

        Transitions class

    */

    class CETransition
    {

        public enum Type
        {
            In,
            Out
        }

        public Texture2D Rectangle { get; private set; }
        public bool ToRemove { get; set; }
        public float Alpha { get; set; }
        public float Speed { get; set; }
        public Type TypeA { get; set; }

        /**
         *  <param type="float" name="speed">The speed of the transition</param>
         *  <param type="Type" name="type">The type of transition</param>
         */
        public CETransition(float speed, Type type)
        {
            Speed = speed;
            TypeA = type;
            if (TypeA == Type.In)
            {
                Alpha = 1;
            }
            if (TypeA == Type.Out)
            {
                Alpha = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (TypeA == Type.In)
            {
                Alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
            }
            if (TypeA == Type.Out)
            {
                Alpha += (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
            }
        }

        public void Draw(Main baseGame)
        {
            Rectangle = new Texture2D(baseGame.GraphicsDevice, 1, 1);
            Rectangle.SetData(new[] { Color.Black });
            baseGame.spriteBatch.Draw(Rectangle, new Rectangle(0, 0, baseGame.game.ScreenWidth, baseGame.game.ScreenHeight), Color.White * Alpha);
        }

    }
}
