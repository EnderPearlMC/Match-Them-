using CodeEasier.Polish;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Polish.Emitters
{
    class EmitterSpark1 : CEParticleEmitter
    {

        public override void Update(GameTime gameTime)
        {

            foreach (CEParticle p in Particles)
            {

                //p.Scale = new Vector2(p.Scale.X + 0.1f, p.Scale.Y + 0.1f);
                p.Origin = new Vector2(p.Rect.Width / 2, p.Rect.Height / 2);

            }

            base.Update(gameTime);
        }

    }
}
