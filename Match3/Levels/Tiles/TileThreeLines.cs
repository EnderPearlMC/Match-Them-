using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileThreeLines : Tile
    {

        public TileThreeLines(int x, int y, int width, int height) : base("three_lines", false, 20, x, y, width, height, Assets.LevelTileThreeLines)
        {
            
        }

    }
}
