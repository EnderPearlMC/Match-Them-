using CodeEasier.GameSystems;
using CodeEasier.Scene;
using CodeEasier.Window;
using Match3.Datas;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    class MainGame : CEGame
    {

        public string Lang { get; set; }
        public Player Player { get; set; }

        private CETextElement versionText;

        public MainGame(Main main) : base(main, References.NAME, new CEWindowMode(CEWindowMode.Mode.Fullscreen, 1280, 720, 1920, 1080))
        {
            AddScenes();
        }

        public override void Initialize()
        {

            // Change the language
            Lang = "fr";
             
            // File

            if (!File.Exists(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), References.DATAS_FOLDER), "player.json")))
            {
                Player = new Player();
                DataManager.WriteFile("player.json", Player);
            }
            else
            {
                Player = DataManager.ReadFile<Player>("player.json");

                Console.WriteLine(Player.Name);
            }

            // set the scene to main menu
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("stars_won", 50);
            ChangeScene("main_menu", p);

            base.Initialize();
        }

        public override void Load()
        {

            versionText = new CETextElement(References.VERSION, Assets.MainFont, Color.White, new Rectangle(0, 0, 0, 0));

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {

            versionText.Rect = new Rectangle(ScreenWidth - versionText.Rect.Width - 10, ScreenHeight - versionText.Rect.Height - 10, ScreenWidth / 7, ScreenHeight / 10);

            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();

            versionText.Draw(BaseGame.spriteBatch);
            BaseGame.spriteBatch.Draw(Assets.Cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 50, 50), Color.White);

        }

        /*
         *  Add scenes to the game
         */
        private void AddScenes()
        {
            AddScene(new SceneMainMenu(BaseGame));
            AddScene(new SceneLevel(BaseGame));
            AddScene(new SceneWin(BaseGame));
        }

    }
}
