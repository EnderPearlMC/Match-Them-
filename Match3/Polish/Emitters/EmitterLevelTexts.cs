using CodeEasier;
using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterLevelTexts : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {

                double ix = Convert.ToDouble(p.Props["ix"]);
                double iy = Convert.ToDouble(p.Props["iy"]);
                double time = Convert.ToDouble(p.Props["time"]);
                double beginx = Convert.ToDouble(p.Props["beginx"]);
                double beginy = Convert.ToDouble(p.Props["beginy"]);
                double distancex = Convert.ToDouble(p.Props["distancex"]);
                double distancey = Convert.ToDouble(p.Props["distancey"]);
                double duration = Convert.ToDouble(p.Props["duration"]);

                p.Rect = new Rectangle(p.Rect.X, p.Rect.Y, (int) ix + (int)Utils.EaseOutSin(time, beginx, distancex, duration), (int) iy + (int)Utils.EaseOutSin(time, beginy, distancey, duration));

                if (time < duration)
                {
                    time += gameTime.ElapsedGameTime.TotalSeconds;
                    p.Props["time"] = time;
                }

                if (p.Timer < 1.5f)
                {
                    p.Alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }

            base.Update(gameTime);
        }

    }
}
