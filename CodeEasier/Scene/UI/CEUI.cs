using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        UI class

        Usage : Call static method to get differents UI elements

    */

    class CEUI
    {

        public enum Type
        {
            None,
            Button,
            CheckBox
        }

        /**
         *  Static methods
         */

        /**
         *  <param type="string" name="text">The text of the button</param>
         *  <param type="string" name="themePath">The path of the file that represents the theme of the button</param>
         *  <param type="Color" name="color">The color of the text</param>
         *  <param type="Main" name="baseGame">The base game</param>
         */
        public static CEUIButton Button(string text, string themePath, Color color, Main baseGame)
        {
            return new CEUIButton(text, themePath, color, baseGame);
        }

        /**
         *  <param type="string[]" name="lines">The lines of text of the scroller</param>
         *  <param type="SpriteFont" name="font">The font of the scroller</param>
         *  <param type="int" name="speed">The speed of the scroller</param>
         *  <param type="Main" name="baseGame">The base game</param>
         */
        public static CEUITextScroller TextScroller(string[] lines, SpriteFont font, int speed, Main baseGame)
        {
            return new CEUITextScroller(lines, font, speed, baseGame);
        }

        /**
         *  <param type="string" name="themePath">The path of the theme file</param>
         *  <param type="Main" name="baseGame">The base game</param>
         */
        public static CEUICheckBox CheckBox(string themePath, Main baseGame)
        {
            return new CEUICheckBox(themePath, baseGame);
        }

    }
}
