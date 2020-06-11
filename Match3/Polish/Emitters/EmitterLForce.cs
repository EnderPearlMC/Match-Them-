using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterLForce : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {
                if (p.Timer <= 1.5 && p.Timer > 0.5)
                {
                    p.Texture = Assets.FXForceStorm2;
                }
                else if (p.Timer <= 0.5)
                {
                    p.Texture = Assets.FXForceStorm1;
                }

            }

            base.Update(gameTime);
        }

    }
}
