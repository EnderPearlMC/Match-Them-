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

namespace CodeEasier.Window
{

    /*

        Class that stores the window mode of a game

        Usage : Instance it

    */

    class CEWindowMode
    {

        public enum Mode
        {
            Resizable,
            Fullscreen,
            Fixed
        }

        /*
         *  Properties 
         */
        public Mode WindowModeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }

        /*
         *  Constructor
         */

        /**
         * <param type="Mode" name="mode">The mode of the window</param>
         * <param type="int" name="width">The width of the window</param>
         * <param type="int" name="height">The height of the window</param>
         * <param type="int" name="resolutionWidth">The width resolution of the window (If not fullscreen, put 0)</param>
         * <param type="int" name="resolutionHeight">The height resolution of the window (If not fullscreen, put 0)</param>
         */
        public CEWindowMode(Mode mode, int width, int height, int resolutionWidth, int resolutionHeight)
        {
            WindowModeType = mode;
            Width = width;
            Height = height;
            ResolutionWidth = resolutionWidth;
            ResolutionHeight = resolutionHeight;
        }

    }
}
