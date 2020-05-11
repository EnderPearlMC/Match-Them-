using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileNum1 : Tile
    {

        public TileNum1(int x, int y, int width, int height) : base("num_1", true, x, y, width, height, Assets.LevelTileNum1)
        {
            
        }

    }
}
