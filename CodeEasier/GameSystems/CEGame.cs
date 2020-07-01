using CodeEasier.Scene;
using CodeEasier.Window;
using Microsoft.Xna.Framework;
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

namespace CodeEasier.GameSystems
{

    /*

        Main class of the framework

        Usage : Create a game class that extends this one.

    */

    abstract class CEGame
    {

        /*
         *  Properties 
         */

        public Main BaseGame { get; private set; }
        public string Title { get; set; }
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public CEWindowMode WindowMode { get; set; }
        public List<CEScene> Scenes { get; private set; }
        public CEScene CurrentScene { get; private set; }

        /*
         *  Constructor
         */

        /**
         * <param type="Main" name="main"> The main file of the monogame project </param>
         * <param type="string" name="title">The title of the window</param>
         */
        public CEGame(Main main, string title, CEWindowMode windowMode)
        {
            BaseGame = main;
            Title = title;
            WindowMode = windowMode;
            Scenes = new List<CEScene>();
        }

        /*
         *  Virtual methods 
         */

        /**
         *  You have to call this method on your monogame project's init method  
         */
        public virtual void Initialize()
        {
            LoadWindowMode();
        }

        /**
         *  You have to call this method on your monogame project's load method  
         */
        public virtual void Load()
        {

        }

        /**
         *  You have to call this method on your monogame project's update method
         *  <param type="GameTime" name="gameTime">The game time that will be used to get the delta time</param>
         */
        public virtual void Update(GameTime gameTime)
        {

            // get the size of the window
            if (WindowMode.WindowModeType == CEWindowMode.Mode.Fullscreen)
            {
                // fullscreen mode
                ScreenWidth = BaseGame.graphics.GraphicsDevice.DisplayMode.Width;
                ScreenHeight = BaseGame.graphics.GraphicsDevice.DisplayMode.Height;
            }
            else if (WindowMode.WindowModeType == CEWindowMode.Mode.Fixed || WindowMode.WindowModeType == CEWindowMode.Mode.Resizable)
            {
                // Fixed size mode
                ScreenWidth = BaseGame.Window.ClientBounds.Width;
                ScreenHeight = BaseGame.Window.ClientBounds.Height;
            }

            // update the current scene
            if (CurrentScene != null)
            {
                CurrentScene.Update(gameTime);
            }
            

        }

        /**
         *  You have to call this method on your monogame project's draw method  
         */
        public virtual void Draw()
        {

            // draw the current scene
            if (CurrentScene != null)
            {
                CurrentScene.Draw();
            }

        }

        /*
         *  Methods 
         */

        /**
         * 
         * Loads window mode using the title, the WindowMode, the Width and the Heigth
         * 
         */
        public void LoadWindowMode()
        {
            BaseGame.Window.Title = Title;

            if (WindowMode.WindowModeType == CEWindowMode.Mode.Fullscreen)
            {
                // fullscreen mode
                BaseGame.graphics.PreferredBackBufferWidth = WindowMode.ResolutionWidth;
                BaseGame.graphics.PreferredBackBufferHeight = WindowMode.ResolutionHeight;
                BaseGame.graphics.IsFullScreen = true;
            }
            else if (WindowMode.WindowModeType == CEWindowMode.Mode.Fixed)
            {
                // Fixed size mode
                BaseGame.graphics.PreferredBackBufferWidth = WindowMode.Width;
                BaseGame.graphics.PreferredBackBufferHeight = WindowMode.Height;
            }
            else if (WindowMode.WindowModeType == CEWindowMode.Mode.Resizable)
            {
                // Resizable mode
                BaseGame.graphics.PreferredBackBufferWidth = WindowMode.Width;
                BaseGame.graphics.PreferredBackBufferHeight = WindowMode.Height;
                BaseGame.Window.AllowUserResizing = true;
            }

            BaseGame.graphics.ApplyChanges();

        }

        /**
         *  Add a scene to your game 
         */
        public void AddScene(CEScene scene)
        {
            Scenes.Add(scene);
        }

        /**
         * Change the current scene and loads it.
         * <param type="string" name="identifier">The identifier of the scene</param>
         */
        public void ChangeScene(string identifier)
        {

            LoadScene(identifier, new Dictionary<string, Object>());

        }

        public void ChangeScene(string identifier, Dictionary<string, Object> parameters)
        {
            LoadScene(identifier, parameters);
        }

        private void LoadScene(string identifier, Dictionary<string, Object> parameters)
        {
            bool sceneFound = false;

            foreach (CEScene scene in Scenes)
            {

                foreach (ICESceneDrawable dr in scene.Drawables)
                {
                    dr.ToRemove = true;
                }

                if (scene.Identifier == identifier && !sceneFound)
                {

                    sceneFound = true;

                    CurrentScene = scene;

                    CurrentScene.Initialize();
                    CurrentScene.Load(parameters);

                }
            }

            if (!sceneFound)
            {
                Console.WriteLine("[CEGame] : The scene you're trying to load doesn't exist!");
            }
        }

    }
}
