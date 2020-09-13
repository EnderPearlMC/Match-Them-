using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileFourDot : Tile
    {

        public TileFourDot(int x, int y, int width, int height) : base("four_dot", false, 3, x, y, width, height, Assets.LevelTileFourDot)
        {
            
        }

    }
}
