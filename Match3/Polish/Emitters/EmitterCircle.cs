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
    class EmitterCircle : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {

                double angle = Convert.ToDouble(p.Props["angle"]);
                double distance = Convert.ToDouble(p.Props["distance"]);
                double ix = Convert.ToDouble(p.Props["ix"]);
                double iy = Convert.ToDouble(p.Props["iy"]);
                double tTime = Convert.ToDouble(p.Props["t_time"]);
                double tBegin = Convert.ToDouble(p.Props["t_begin"]);
                double tEnd = Convert.ToDouble(p.Props["t_end"]);
                double tDuration = Convert.ToDouble(p.Props["t_duration"]);

                int xOff = (int)(distance * Math.Cos(MathHelper.ToRadians((float) angle)));
                int yOff = (int)(distance * Math.Sin(MathHelper.ToRadians((float) angle)));

                p.Rect = new Rectangle((int) ix + xOff, (int) iy + yOff, p.Rect.Width, p.Rect.Height);

                distance = Utils.EaseOutSin(tTime, tBegin, tEnd, tDuration);

                if (tTime < tDuration)
                    tTime += gameTime.ElapsedGameTime.TotalSeconds;

                p.Props["distance"] = distance;
                p.Props["t_time"] = tTime;

            }

            Particles.RemoveAll(item => item.Rect.Width <= 0);

            base.Update(gameTime);
        }

    }
}
