using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Datas
{
    public class Level
    {

        public int LevelNbr { get; set; }
        public List<string> PossibleTiles { get; set; }

        public Level(int levelNbr, List<string> possibleTiles)
        {
            LevelNbr = levelNbr;
            PossibleTiles = possibleTiles;
        }

    }
}
