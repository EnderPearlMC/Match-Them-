using Match3.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Tiles
{
    class TileCard : Tile
    {

        public TileCard(int x, int y, int width, int height) : base("card", false, 4, x, y, width, height, Assets.LevelTileCard)
        {
            
        }

        public override void OnMatch(SceneLevel scene)
        {

            scene.PlayCard();

            base.OnMatch(scene);
        }

    }
}
