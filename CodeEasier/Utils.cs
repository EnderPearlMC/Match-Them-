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

namespace CodeEasier
{

    class Utils
    {

        public static float EaseOutSin(double t, double b, double c, double d)
        {
            return (float)(c * Math.Sin(t / d * (Math.PI / 2)) + b);
        }

        public static float EaseInSin(double t, double b, double c, double d)
        {
            return (float)(-c * Math.Cos(t / d * (Math.PI / 2)) + c + b);
        }

        public static float EaseInOutSin(double t, double b, double c, double d)
        {
            return (float)(-c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b);
        }


        public static float Linear (double t, double b, double c, double d)
        {
            return (float)(c * t / d + b);
        }

        public static float EaseInOutQuad(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1) return (float) (c / 2 * t * t + b);
            t--;
            return (float)(-c / 2 * (t * (t - 2) - 1) + b);
        }

        public static float EaseInQuad(double t, double b, double c, double d)
        {
            t /= d;
            return (float)(c * t * t + b);
        }

    }
}
