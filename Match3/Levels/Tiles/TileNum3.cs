using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileNum3 : Tile
    {

        public TileNum3(int x, int y, int width, int height) : base("num_3", true, 5, x, y, width, height, Assets.LevelTileNum3)
        {
            
        }

    }
}
