using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileStar : Tile
    {

        public TileStar(int x, int y, int width, int height) : base("star", false, 20, x, y, width, height, Assets.LevelTileStar)
        {
            
        }

    }
}
