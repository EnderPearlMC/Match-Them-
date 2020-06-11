using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileBamboo : Tile
    {

        public TileBamboo(int x, int y, int width, int height) : base("bamboo", false, 4, x, y, width, height, Assets.LevelTileBamboo)
        {
            
        }

    }
}
