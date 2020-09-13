using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileOneDot : Tile
    {

        public TileOneDot(int x, int y, int width, int height) : base("one_dot", false, 4, x, y, width, height, Assets.LevelTileOneDot)
        {
            
        }

    }
}
