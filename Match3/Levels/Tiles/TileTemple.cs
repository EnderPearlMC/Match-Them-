using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileTemple : Tile
    {

        public TileTemple(int x, int y, int width, int height) : base("temple", false, 35, x, y, width, height, Assets.LevelTileTemple)
        {
            
        }

    }
}
