using CodeEasier.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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

        ParticleEmitter class

        Usage : Instance it.

    */

    class CEParticleEmitter
    {

        public List<CEParticle> Particles { get; set; }
        public Vector2 Position { get; set; }

        public CEParticleEmitter()
        {
            Particles = new List<CEParticle>();
        }

        public void AddParticle(Texture2D texture, int x, int y, int w, int h, float timer, float vx, float vy, float vAngle, float gravity = 0, float gravityA = 0)
        {
            CEParticle p = new CEParticle(texture, x, y, w, h, timer, vx, vy, vAngle, gravity, gravityA);
            Particles.Add(p);
        }

        public void AddParticle(CEParticle particle)
        {
            Particles.Add(particle);
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (CEParticle p in Particles)
            {

                p.Gravity += p.GravityAmount * dt;

                p.Growing += p.GrowingAmount * dt;

                p.Alpha -= p.AlphaAmount * dt;

                p.VY += p.Gravity * dt;

                p.Rect = new Rectangle((int) Math.Round(p.Rect.X + (p.VX * dt)), (int) Math.Round(p.Rect.Y + (p.VY * dt)), p.Rect.Width + (int)p.Growing, p.Rect.Height + (int)p.Growing);

                if (p.Center)
                    p.Origin = new Vector2(p.Texture.Width / 2, p.Texture.Height / 2);

                p.Angle += (float)gameTime.ElapsedGameTime.TotalSeconds * p.VAngle;

                p.Timer -= dt;
            }

            Particles.RemoveAll(item => item.Timer <= 0);
            Particles.RemoveAll(item => item.Rect.Width < 0);

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (CEParticle p in Particles)
            {
                spriteBatch.Draw(p.Texture, p.Rect, null, p.Color * p.Alpha, MathHelper.ToRadians(p.Angle), p.Origin, SpriteEffects.None, 0);
            }
        }

    }
}
