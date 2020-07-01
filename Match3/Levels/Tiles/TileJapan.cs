using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileJapan : Tile
    {

        public TileJapan(int x, int y, int width, int height) : base("japan", false, 3, x, y, width, height, Assets.LevelTileJapan)
        {
            
        }

    }
}
