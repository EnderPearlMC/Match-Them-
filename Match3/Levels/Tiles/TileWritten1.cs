using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileWritten1 : Tile
    {

        public TileWritten1(int x, int y, int width, int height) : base("written_1", false, x, y, width, height, Assets.LevelTileWritten1)
        {
            
        }

    }
}
