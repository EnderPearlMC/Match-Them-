using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Datas
{
    class LevelsData
    {

        public static List<Level> LevelsList { get; set; }

        public static void Load()
        {

            LevelsList = new List<Level>();

            // ================================================ //
            List<string> p1 = new List<string>();
            p1.Add("written_1");
            p1.Add("written_2");
            p1.Add("num_1");
            p1.Add("num_2");
            p1.Add("num_3");
            p1.Add("num_4");
            p1.Add("num_5");
            LevelsList.Add(new Level(1, p1));
            // ================================================ //

        }

    }
}
