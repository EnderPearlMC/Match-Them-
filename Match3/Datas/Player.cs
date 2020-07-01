using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Datas
{
    class Player
    {

        public string Name { get; set; }
        public int Level { get; set; }
        public int Stars { get; set; }

        public Player()
        {
            Name = "Player";
            Level = 1;
            Stars = 0;
        }

    }
}
