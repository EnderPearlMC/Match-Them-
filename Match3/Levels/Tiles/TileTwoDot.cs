using Match3.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileTwoDot : Tile
    {

        public TileTwoDot(int x, int y, int width, int height) : base("two_dot", false, 4, x, y, width, height, Assets.LevelTileTwoDot)
        {
            
        }

        public override void OnMatch(SceneLevel scene)
        {

            base.OnMatch(scene);
        }

    }
}
