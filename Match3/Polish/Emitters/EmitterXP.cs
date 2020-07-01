using CodeEasier;
using CodeEasier.Polish;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterXP : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach(CEParticle p in Particles)
            {

                double time = Convert.ToDouble(p.Props["time"]);
                double beginx = Convert.ToDouble(p.Props["beginx"]);
                double beginy = Convert.ToDouble(p.Props["beginy"]);
                double distancex = Convert.ToDouble(p.Props["distancex"]);
                double distancey = Convert.ToDouble(p.Props["distancey"]);
                double duration = Convert.ToDouble(p.Props["duration"]);
                SceneLevel scene = (SceneLevel)p.Props["scene"];

                p.Rect = new Rectangle((int) Math.Round(Utils.EaseOutSin(time, beginx, distancex, duration)), (int) Math.Round(Utils.EaseInSin(time, beginy, distancey, duration)), p.Rect.Width, p.Rect.Height);

                if (time < duration)
                {
                    time += gameTime.ElapsedGameTime.TotalSeconds;
                    p.Props["time"] = time;
                }
                if (time >= duration && p.Props.ContainsKey("xp"))
                {
                    scene.xp += (int) p.Props["xp"];
                }

                Random r = new Random();

                if (r.Next(0, 100) < 20)
                {

                    CEParticle smokeP = new CEParticle(Assets.FXSmoke1, p.Rect.X - p.Rect.Width, p.Rect.Y - p.Rect.Height, r.Next(p.Rect.Width, p.Rect.Width * 2), r.Next(p.Rect.Height, p.Rect.Height * 2), (float)r.NextDouble() * 2, 200, r.Next(-50, 50), 0);

                    smokeP.AlphaAmount = 1;

                    scene.emitter1.AddParticle(smokeP);

                }

                for (int i = 0; i < 2; i++)
                {

                    Texture2D texture = null;

                    if (i == 0)
                    {
                        texture = Assets.FXXP;
                    }
                    if (i == 1)
                    {
                        texture = Assets.FXYellowOrange;
                    }

                    CEParticle xpP = new CEParticle(texture, p.Rect.X, p.Rect.Y - p.Rect.Height / 2, r.Next(p.Rect.Width / 3, (int)Math.Round(p.Rect.Width / 2.0)), r.Next(p.Rect.Height / 3, (int)Math.Round(p.Rect.Height / 2.0)), (float)r.NextDouble() * 2, 200, r.Next(-100, 100), r.Next(-70, 70));

                    xpP.AlphaAmount = 1;

                    scene.emitter1.AddParticle(xpP);
                }

                if (r.Next(0, 100) < 5)
                {
                    CEParticle starP = new CEParticle(Assets.FXStar, p.Rect.X, p.Rect.Y, p.Rect.Width, p.Rect.Height, (float)r.NextDouble() * 2, 0, 0, 100);
                    starP.Center = true;
                    starP.GrowingAmount = 2;
                    starP.AlphaAmount = 0.3f;
                    scene.emitter1.AddParticle(starP);
                }

            }

            base.Update(gameTime);
        }

    } 
}
