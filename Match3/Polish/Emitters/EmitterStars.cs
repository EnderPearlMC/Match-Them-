using CodeEasier;
using CodeEasier.Polish;
using Match3.Datas;
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
    class EmitterStars : CEParticleEmitter
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
                if (time >= duration && p.Props.ContainsKey("stars"))
                {
                    scene.starsWon += (int) p.Props["stars"];
                    scene.BaseGame.game.Player.Stars += (int) p.Props["stars"];
                    DataManager.WriteFile("player.json", scene.BaseGame.game.Player);
                    scene.BaseGame.game.Player = DataManager.ReadFile<Player>("player.json");
                }

                Random r = new Random();

                if (r.Next(0, 100) < 50)
                {
                    Texture2D rectangle = new Texture2D(scene.BaseGame.GraphicsDevice, 1, 1);

                    Color c;

                    if (r.Next(0, 100) < 50)
                    {
                        c = new Color(241, 196, 15);
                    }
                    else
                    {
                        c = new Color(225, 177, 44);
                    }

                    rectangle.SetData(new Color[] { c });

                    int width = r.Next((int)Math.Round(p.Rect.Width / 2.5f), (int)Math.Round(p.Rect.Width / 1.5f));

                    CEParticle starP = new CEParticle(rectangle, p.Rect.X - p.Rect.Width / 2, p.Rect.Y - p.Rect.Height / 2, width, width, (float)r.NextDouble() * 2, 0, r.Next(-60, 60), 200, 0, r.Next(500, 2000));

                    starP.GrowingAmount = -1f;

                    scene.emitter1.AddParticle(starP);
                }

            }

            base.Update(gameTime);
        }

    } 
}
