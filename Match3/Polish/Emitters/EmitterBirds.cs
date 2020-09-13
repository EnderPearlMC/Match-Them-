using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterBirds : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {

                double time = Convert.ToDouble(p.Props["time_anim"]);
                double anim = Convert.ToInt32(p.Props["anim"]);

                time += gameTime.ElapsedGameTime.TotalSeconds;

                if (time >= 0.3)
                {
                    if (anim == 1)
                    {
                        anim = 2;
                        p.Texture = Assets.Bird2;
                    }
                    else if (anim == 2)
                    {
                        anim = 1;
                        p.Texture = Assets.Bird1;
                    }
                    time = 0;
                }

                p.Props["time_anim"] = time;
                p.Props["anim"] = anim;

            }

            base.Update(gameTime);
        }

    }
}
