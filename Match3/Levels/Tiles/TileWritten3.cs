using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileWritten3 : Tile
    {

        public TileWritten3(int x, int y, int width, int height) : base("written_3", false, 11, x, y, width, height, Assets.LevelTileWritten3)
        {
            
        }

    }
}
