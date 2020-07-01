using CodeEasier.Scene;
using CodeEasier.Scene.UI;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Levels.Forces
{
    abstract class Force
    {

        public string Id { get; set; }
        public string LangTitlePath { get; set; }
        public int XpToHave { get; set; }

        public bool IsHovered { get; set; }
        public bool Used { get; set; }
        public bool Done { get; set; }

        public CETextElement text;
        public CEImageElement button;

        public Force(string id, string langTitlePath, Texture2D buttonImage)
        {
            Id = id;
            LangTitlePath = langTitlePath;

            IsHovered = false;

            text = new CETextElement("", Assets.MainFont, Color.Black, new Rectangle(0, 0, 0, 0));
            button = new CEImageElement(buttonImage, new Rectangle(0, 0, 0, 0));

        }

        public virtual void Load()
        {

        }

        public virtual void Update(GameTime gameTime, SceneLevel scene)
        {
            scene.showIndicator = false;
        }

        public virtual void Draw()
        {

        }

    }
}
