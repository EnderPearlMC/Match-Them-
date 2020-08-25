﻿using System;
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
            p1.Add("written_1");
            p1.Add("written_2");
            p1.Add("written_3");
            p1.Add("star");
            p1.Add("japan");
            p1.Add("bamboo");
            p1.Add("tree");
            p1.Add("temple");
            Dictionary<int, string> f1 = new Dictionary<int, string>();
            LevelsList.Add(new Level(1, p1, 99999999, 1.2f, 500, fe));
            // ================================================ //
            List<string> p2 = new List<string>();
            p2.Add("written_1");
            p2.Add("written_2");
            p2.Add("written_3");
            p2.Add("star");
            p2.Add("japan");
            p2.Add("bamboo");
            p2.Add("tree");
            Dictionary<int, string> f2 = new Dictionary<int, string>();
            f2.Add(300, "axe");
            f2.Add(400, "storm");
            f2.Add(600, "storm");
            f2.Add(800, "axe");
            LevelsList.Add(new Level(2, p2, 160, 2.1f, 900, f2));
            // ================================================ //
            List<string> p3 = new List<string>();
            p3.Add("written_1");
            p3.Add("written_2");
            p3.Add("written_3");
            /***p3.Add("star");
            p3.Add("japan");
            p3.Add("bamboo");**/
            p3.Add("tree");
            p3.Add("num_3");
            p3.Add("num_4");
            p3.Add("num_5");
            p3.Add("card");
            Dictionary<int, string> f3 = new Dictionary<int, string>();
            f3.Add(50, "storm");
            f3.Add(300, "axe");
            f3.Add(400, "storm");
            f3.Add(600, "storm");
            LevelsList.Add(new Level(3, p3, 180, 2.1f, 700, f3));
            // ================================================ //

        }

    }
}
