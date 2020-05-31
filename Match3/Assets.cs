using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    class Assets
    {

        // fonts
        public static SpriteFont MainFont { get; private set; }

        // images
        public static Texture2D Cursor { get; private set; }
        public static Texture2D JapanTemple1 { get; private set; }
        public static Texture2D Background1 { get; private set; }
        public static Texture2D Background2 { get; private set; }
        public static Texture2D LevelSpawnTilesBar { get; private set; }
        public static Texture2D LevelLeftPan { get; private set; }
        public static Texture2D Star { get; private set; }

        public static Texture2D LevelTileWritten1 { get; private set; }
        public static Texture2D LevelTileWritten2 { get; private set; }
        public static Texture2D LevelTileWritten3 { get; private set; }
        public static Texture2D LevelTileStar { get; private set; }
        public static Texture2D LevelTileJapan { get; private set; }
        public static Texture2D LevelTileBamboo { get; private set; }
        public static Texture2D LevelTileTree { get; private set; }
        public static Texture2D LevelTileTemple { get; private set; }

        public static Texture2D LevelTileNum1 { get; private set; }
        public static Texture2D LevelTileNum2 { get; private set; }
        public static Texture2D LevelTileNum3 { get; private set; }
        public static Texture2D LevelTileNum4 { get; private set; }
        public static Texture2D LevelTileNum5 { get; private set; }

        public static Texture2D WinBackground { get; private set; }
        public static Texture2D WinPan { get; private set; }
        public static Texture2D StarForPile { get; private set; }

        // forces
        public static Texture2D ForceAxeButton { get; private set; }

        // particles
        public static Texture2D ParticleSmoke1 { get; private set; }

        // musics
        public static Song MusicMainMenu { get; private set; }

        // sounds
        public static SoundEffect SndTileSlide { get; private set; }
        public static SoundEffect SndButtonHovered { get; private set; }

        // theme files
        public static string ButtonThemePath1 { get; private set; }

        public static void Load(ContentManager content)
        {

            MainFont = content.Load<SpriteFont>("main_font");

            Cursor = content.Load<Texture2D>("cursor");
            JapanTemple1 = content.Load<Texture2D>("japan_temple_1");
            Background1 = content.Load<Texture2D>("background_1");
            Background2 = content.Load<Texture2D>("background_2");
            LevelSpawnTilesBar = content.Load<Texture2D>("level_spawn_tiles_bar");
            LevelLeftPan = content.Load<Texture2D>("level_left_pan");
            Star = content.Load<Texture2D>("star");

            LevelTileWritten1 = content.Load<Texture2D>("tile_written_1");
            LevelTileWritten2 = content.Load<Texture2D>("tile_written_2");
            LevelTileWritten3 = content.Load<Texture2D>("tile_written_3");
            LevelTileStar = content.Load<Texture2D>("tile_star");
            LevelTileJapan = content.Load<Texture2D>("tile_japan");
            LevelTileBamboo = content.Load<Texture2D>("tile_bamboo");
            LevelTileTree = content.Load<Texture2D>("tile_tree");
            LevelTileTemple = content.Load<Texture2D>("tile_temple");

            LevelTileNum1 = content.Load<Texture2D>("tile_num_1");
            LevelTileNum2 = content.Load<Texture2D>("tile_num_2");
            LevelTileNum3 = content.Load<Texture2D>("tile_num_3");
            LevelTileNum4 = content.Load<Texture2D>("tile_num_4");
            LevelTileNum5 = content.Load<Texture2D>("tile_num_5");

            WinBackground = content.Load<Texture2D>("background_win");
            WinPan = content.Load<Texture2D>("win_pan");
            StarForPile = content.Load<Texture2D>("star_for_pile");

            ForceAxeButton = content.Load<Texture2D>("force_axe_button");

            ParticleSmoke1 = content.Load<Texture2D>("particle_smoke_1");

            MusicMainMenu = content.Load<Song>("music_menu");

            SndTileSlide = content.Load<SoundEffect>("snd_tile_slide");
            SndButtonHovered = content.Load<SoundEffect>("snd_button_hover");

            ButtonThemePath1 = @"D:/jeux/Match3/Match3/Match3/Match3/themes/button_theme_1.thm";

        }

    }
}
