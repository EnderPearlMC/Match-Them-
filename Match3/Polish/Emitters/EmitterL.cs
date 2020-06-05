using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterL : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {
                if (p.Timer <= 0.3 && p.Timer > 0.2)
                {
                    p.Texture = Assets.FXL2;
                }
                else if (p.Timer <= 0.2 && p.Timer > 0)
                {
                    p.Texture = Assets.FXL3;
                }
                else if (p.Timer <= 0)
                {
                    p.Texture = Assets.FXL4;
                }

            }

            base.Update(gameTime);
        }

    }
}
