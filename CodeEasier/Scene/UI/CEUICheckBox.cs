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

        UIButton class

        Usage : Call CEUI.CheckBox(); (returns an instance)

    */

    class CEUICheckBox : ICESceneDrawable
    {

        /*
         *  Properties 
         */

        public string ButtonMode { get; set; }
        public string ThemePath { get; private set; }
        public Main BaseGame { get; private set; }
        public bool IsChecked { get; set; }

        public string ImgPath { get; private set; }
        public string ImgCheckedPath { get; private set; }

        public Texture2D Img { get; private set; }
        public Texture2D ImgChecked { get; private set; }

        public Rectangle Rect { get; set; }

        private MouseState oldMouseState;

        public bool ToRemove { get; set; }

        /*
         *  Constructor
         */

        /**
         *  <param type="string" name="themePath">The path of the theme file</param>
         *  <param type="Main" name="baseGame">The base game</param>
         */
        public CEUICheckBox(string themePath, Main baseGame)
        {
            ThemePath = themePath;
            BaseGame = baseGame;
            Rect = new Rectangle(0, 0, 0, 0);
            IsChecked = false;

            ParseTheme(System.IO.File.ReadAllLines(themePath));

        }

        /*
         *  Methods 
         */

        private void ParseTheme(string[] lines)
        {
             
            if (lines[0] == "--<thm>--")
            {

                CEUI.Type type = CEUI.Type.None;
                string img = "";
                string imgChecked = "";

                foreach (string line in lines)
                {
                    if (line != "")
                    {
                        string[] parts = line.Split(' ');
                        
                        // means "type : <type>"
                        if (parts[0] == "type")
                        {
                            if (parts[1] == ":")
                            {
                                if (parts[2] == "checkbox")
                                {
                                    type = CEUI.Type.CheckBox;
                                }
                            }
                        }

                        // means "background : <bg>"
                        if (parts[0] == "background")
                        {
                            if (parts[1] == ":")
                            {
                                img = parts[2];
                            }
                        }

                        // means "background-checked : <bg>"
                        if (parts[0] == "background-checked")
                        {
                            if (parts[1] == ":")
                            {
                                imgChecked = parts[2];
                            }
                        }
                    }
                }
                
                if (type == CEUI.Type.CheckBox && img != "" && imgChecked != "")
                {
                    ImgPath = img;
                    ImgCheckedPath = imgChecked;
                }
                else
                {
                    throw new Exception("Theme file incorrect! File : " + ThemePath);
                }

            }
        }
        
        public void Load()
        {

            // load textures and font
            Img = BaseGame.Content.Load<Texture2D>(ImgPath);
            ImgChecked = BaseGame.Content.Load<Texture2D>(ImgCheckedPath);

            oldMouseState = Mouse.GetState();

        }

        public void Update(GameTime gameTime)
        {

            MouseState newMouseState = Mouse.GetState();

            if (Rect.Contains(newMouseState.X, newMouseState.Y))
            {
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                {
                    if (!IsChecked)
                    {
                        IsChecked = true;
                    }
                    else
                    {
                        IsChecked = false;
                    }
                }
            }

            oldMouseState = newMouseState;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsChecked)
            {
                spriteBatch.Draw(ImgChecked, Rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(Img, Rect, Color.White);
            }
        }

    }
}
