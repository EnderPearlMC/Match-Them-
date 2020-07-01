using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterFallingTiles : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {
                p.Alpha -= (float) gameTime.ElapsedGameTime.TotalSeconds / 1.5f;
            }

            base.Update(gameTime);
        }

    }
}
