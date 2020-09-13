using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileSquare : Tile
    {

        public TileSquare(int x, int y, int width, int height) : base("square", false, 5, x, y, width, height, Assets.LevelTileSquare)
        {
            
        }

    }
}
