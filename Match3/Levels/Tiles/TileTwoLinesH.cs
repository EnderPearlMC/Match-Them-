using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileTwoLinesH : Tile
    {

        public TileTwoLinesH(int x, int y, int width, int height) : base("two_lines_h", false, 12, x, y, width, height, Assets.LevelTileTwoLinesH)
        {
            
        }

    }
}
