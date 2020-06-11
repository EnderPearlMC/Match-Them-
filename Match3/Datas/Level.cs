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
        public int ScrollingSpeed { get; set; }
        public float ScrollingStartDistance { get; set; }
        public List<string> PossibleTiles { get; set; }
        public int MaxXp { get; set; }
        public Dictionary<int, string> Forces { get; set; }

        public Level(int levelNbr, List<string> possibleTiles, int scrollingSpeed, float scrollingStartDistance, int maxXp, Dictionary<int, string> forces)
        {
            LevelNbr = levelNbr;
            PossibleTiles = possibleTiles;
            ScrollingSpeed = scrollingSpeed;
            ScrollingStartDistance = scrollingStartDistance;
            MaxXp = maxXp;
            Forces = forces;
        }

    }
}
