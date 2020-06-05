using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileTree : Tile
    {

        public TileTree(int x, int y, int width, int height) : base("tree", false, 12, x, y, width, height, Assets.LevelTileTree)
        {
            
        }

    }
}
