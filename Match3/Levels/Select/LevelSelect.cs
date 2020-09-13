using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Select
{
    class LevelSelect
    {

        public int Number { get; set; }
        public bool IsLocked { get; set; }
        public bool IsCurrent { get; set; }
        public Vector2 Position { get; set; }

    }
}
