using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    class Langs
    {

        public static Dictionary<string, Dictionary<string, string>> Texts;

        public static void Load()
        {

            Texts = new Dictionary<string, Dictionary<string, string>>();

            AddText("play_button", "Play", "Jouer");
            AddText("help_button", "Help", "Aide");
            AddText("settings_button", "Settings", "Paramètres");
            AddText("quit_button", "Quit", "Quitter");
            AddText("welcome", "Welcome ", "Bienvenue ");
            AddText("pause_button", "Pause", "Pause");

        }

        private static void AddText(string key, string en, string fr)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("en", en);
            t.Add("fr", fr);
            Texts.Add(key, t);
        }

    }
}
