using CodeEasier.Scene;
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

namespace CodeEasier.Scene
{

    /*

        Scene class

        Usage : Create scene classes that extends this one.

    */

    abstract class CEScene
    {

        /*
         *  Properties 
         */

        public string Identifier { get; private set; }
        public Main BaseGame { get; private set; }
        public List<ICESceneDrawable> Drawables { get; private set; }

        /*
         *  Constructor
         */

        /**
         * <param type="string" name="identifier">The identifier of the scene</param>
         * <param type="Main" name="main">The monogame main class</param>
         */
        public CEScene(string identifier, Main main)
        {
            Identifier = identifier;
            BaseGame = main;
            Drawables = new List<ICESceneDrawable>();
        }

        /*
         *  Virtual methods 
         */

        /**
         *  The init method of the scene
         */
        public virtual void Initialize()
        {
            
        }

        /**
         *  The load method of the scene 
         */
        public virtual void Load(Dictionary<string, Object> parameters)
        {
            foreach (ICESceneDrawable drawable in Drawables)
            {
                drawable.Load();
            }
        }

        /**
         *  The update method of the scene
         *  <param type="GameTime" name="gameTime">The game time that will be used to get the delta time</param>
         */
        public virtual void Update(GameTime gameTime)
        {
            foreach (ICESceneDrawable drawable in Drawables)
            {
                drawable.Update(gameTime);
            }

            Drawables.RemoveAll(item => item.ToRemove);

        }

        /**
         *  The draw method of the scene
         */
        public virtual void Draw()
        {
            foreach (ICESceneDrawable drawable in Drawables)
            {
                drawable.Draw(BaseGame.spriteBatch);
            }
        }

        /*
         *  Methods 
         */

        public void AddDrawable(ICESceneDrawable drawable)
        {
            Drawables.Add(drawable);
        }

    }
}
