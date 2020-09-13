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

            Dictionary<int, string> fe = new Dictionary<int, string>();

            // ================================================ //
            List<string> p1 = new List<string>();
            p1.Add("one_dot");
            p1.Add("two_dot");
            p1.Add("four_dot");
            p1.Add("three_lines");
            p1.Add("two_lines_h");
            p1.Add("square");
            Dictionary<int, string> f1 = new Dictionary<int, string>();
            LevelsList.Add(new Level(1, p1, 99999999, 1.2f, 500, fe));
            // ================================================ //
            List<string> p2 = new List<string>();
            p2.Add("one_dot");
            p2.Add("two_dot");
            p2.Add("four_dot");
            p2.Add("three_lines");
            p2.Add("two_lines_h");
            p2.Add("square");
            Dictionary<int, string> f2 = new Dictionary<int, string>();
            f2.Add(300, "axe");
            f2.Add(400, "storm");
            f2.Add(600, "storm");
            f2.Add(800, "axe");
            LevelsList.Add(new Level(2, p2, 160, 2.1f, 800, f2));
            // ================================================ //
            List<string> p3 = new List<string>();
            p3.Add("one_dot");
            p3.Add("two_dot");
            p3.Add("four_dot");
            p3.Add("three_lines");
            p3.Add("two_lines_h");
            p3.Add("square");
            p3.Add("num_3");
            p3.Add("num_4");
            p3.Add("num_5");
            Dictionary<int, string> f3 = new Dictionary<int, string>();
            f3.Add(50, "storm");
            f3.Add(300, "axe");
            f3.Add(400, "storm");
            f3.Add(600, "storm");
            LevelsList.Add(new Level(3, p3, 180, 2.1f, 900, f3));
            // ================================================ //
            LevelsList.Add(new Level(4, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(5, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(6, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(7, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(8, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(9, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(10, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(11, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(12, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(13, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(14, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(15, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(16, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(17, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(18, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(19, p3, 180, 2.1f, 900, f3));
            LevelsList.Add(new Level(20, p3, 180, 2.1f, 900, f3));
        }

    }
}
