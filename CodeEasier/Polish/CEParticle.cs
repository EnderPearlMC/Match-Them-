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

        Particle class

        Usage : Use a ParticleEmitter

    */

    class CEParticle
    {

        public Texture2D Texture { get; set; }
        public Rectangle Rect { get; set; }
        public float Timer { get; set; }
        public float VX { get; set; }
        public float VY { get; set; }
        public float VAngle { get; set; }
        public float Angle { get; set; }
        public float Alpha { get; set; }
        public float AlphaAmount { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public float Growing { get; set; }
        public float GrowingAmount { get; set; }
        public bool Center { get; set; }

        public CEParticle(Texture2D texture, int x, int y, int w, int h, float timer, float vx, float vy, float vAngle)
        {
            Texture = texture;
            Timer = timer;
            VX = vx;
            VY = vy;
            VAngle = vAngle;
            Rect = new Rectangle(x, y, w, h);
            Alpha = 1;
            AlphaAmount = 0;
            Scale = Vector2.Zero;
            Origin = Vector2.Zero;
            Growing = 0;
            GrowingAmount = 0;
            Center = false;
        }

    }
}
