using Microsoft.Xna.Framework;
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

        public static List<Texture2D> LevelBackgrounds { get; set; }

        public static Texture2D SplashScreen { get; private set; }
        public static Texture2D Cursor { get; private set; }
        public static Texture2D JapanTemple1 { get; private set; }
        public static Texture2D LevelSpawnTilesBar { get; private set; }
        public static Texture2D LevelDieBar { get; private set; }
        public static Texture2D LevelLeftPan { get; private set; }
        public static Texture2D Star { get; private set; }
        public static Texture2D Clouds { get; private set; }

        // menu
        public static Texture2D LayerSky { get; private set; }
        public static Texture2D LayerStars { get; private set; }
        public static Texture2D LayerClouds { get; private set; }
        public static Texture2D LayerMountains { get; private set; }
        public static Texture2D LayerFan { get; private set; }
        public static Texture2D Bird1 { get; private set; }
        public static Texture2D Bird2 { get; private set; }

        public static Texture2D LevelTileWritten1 { get; private set; }
        public static Texture2D LevelTileWritten2 { get; private set; }
        public static Texture2D LevelTileWritten3 { get; private set; }
        public static Texture2D LevelTileStar { get; private set; }
        public static Texture2D LevelTileJapan { get; private set; }
        public static Texture2D LevelTileBamboo { get; private set; }
        public static Texture2D LevelTileTree { get; private set; }
        public static Texture2D LevelTileTemple { get; private set; }
        public static Texture2D LevelTileCard { get; private set; }

        public static Texture2D LevelTileNum1 { get; private set; }
        public static Texture2D LevelTileNum2 { get; private set; }
        public static Texture2D LevelTileNum3 { get; private set; }
        public static Texture2D LevelTileNum4 { get; private set; }
        public static Texture2D LevelTileNum5 { get; private set; }

        public static Texture2D SelectedTileIndicator { get; private set; }
        public static Texture2D TilesUp { get; private set; }

        public static Texture2D Match4Text { get; private set; }
        public static Texture2D CascadeText { get; private set; }

        public static Texture2D WinBackground { get; private set; }
        public static Texture2D WinPan { get; private set; }
        public static Texture2D StarForPile { get; private set; }

        // forces
        public static Texture2D ForceAxeButton { get; private set; }
        public static Texture2D ForceAxe { get; private set; }
        public static Texture2D FXForceStormAdd { get; private set; }
        public static Texture2D ForceStormButton { get; private set; }

        // fxs
        public static Texture2D FXSpark1 { get; private set; }
        public static Texture2D FXPink { get; private set; }
        public static Texture2D FXTwirl { get; private set; }
        public static Texture2D FXTileBroken { get; private set; }
        public static Texture2D FXWaveCircle { get; private set; }
        public static Texture2D FXImpact1 { get; private set; }
        public static Texture2D FXForceStorm1 { get; private set; }
        public static Texture2D FXForceStorm2 { get; private set; }
        public static Texture2D FXRainDrop { get; private set; }
        public static Texture2D FXLight1 { get; private set; }
        public static Texture2D FXSmoke1 { get; private set; }
        public static Texture2D FXDirt { get; private set; }
        public static Texture2D FXXP { get; private set; }
        public static Texture2D FXXP2 { get; private set; }
        public static Texture2D FXStar { get; private set; }
        public static Texture2D FXYellowOrange { get; private set; }
        public static Texture2D FXYellow { get; private set; }
        public static Texture2D FXTriangle { get; private set; }

        public static Texture2D FXL1 { get; private set; }
        public static Texture2D FXL2 { get; private set; }
        public static Texture2D FXL3 { get; private set; }
        public static Texture2D FXL4 { get; private set; }

        // musics
        public static Song MusicMainMenu { get; private set; }

        public static List<Song> LevelMusics { get; private set; }

        // sounds
        public static SoundEffect SndSplashScreen { get; private set; }
        public static SoundEffect SndTileSlide { get; private set; }
        public static SoundEffect SndTilePop { get; private set; }
        public static SoundEffect SndTilePop2 { get; private set; }
        public static SoundEffect SndTilePop3 { get; private set; }
        public static SoundEffect SndTilePop4 { get; private set; }
        public static SoundEffect SndMatch { get; private set; }
        public static SoundEffect SndMatch4 { get; private set; }
        public static SoundEffect SndStack { get; private set; }
        public static SoundEffect SndMatchNum { get; private set; }
        public static SoundEffect SndButtonHovered { get; private set; }
        public static SoundEffect SndForceAxe { get; private set; }
        public static SoundEffectInstance StormSounds { get; set; }
        public static SoundEffect SndThunder { get; private set; }

        // theme files
        public static string[] ButtonThemePath1 { get; private set; }

        public static Texture2D Transparent { get; private set; }

        public static void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {

            MainFont = content.Load<SpriteFont>("main_font");

            LevelBackgrounds = new List<Texture2D>()
            {
                content.Load<Texture2D>("level_bg_1"),
                content.Load<Texture2D>("level_bg_2"),
            };

            SplashScreen = content.Load<Texture2D>("splash_screen");
            Cursor = content.Load<Texture2D>("cursor");
            JapanTemple1 = content.Load<Texture2D>("japan_temple_1");
            LevelSpawnTilesBar = content.Load<Texture2D>("level_spawn_tiles_bar");
            LevelDieBar = content.Load<Texture2D>("level_die_bar");
            LevelLeftPan = content.Load<Texture2D>("level_left_pan");
            Star = content.Load<Texture2D>("star");
            Clouds = content.Load<Texture2D>("clouds");

            // menu
            LayerSky = content.Load<Texture2D>("layer_sky");
            LayerStars = content.Load<Texture2D>("layer_stars");
            LayerClouds = content.Load<Texture2D>("layer_clouds");
            LayerMountains = content.Load<Texture2D>("layer_mountains");
            LayerFan = content.Load<Texture2D>("layer_fan");
            Bird1 = content.Load<Texture2D>("bird_1");
            Bird2 = content.Load<Texture2D>("bird_2");

            LevelTileWritten1 = content.Load<Texture2D>("tile_written_1");
            LevelTileWritten2 = content.Load<Texture2D>("tile_written_2");
            LevelTileWritten3 = content.Load<Texture2D>("tile_written_3");
            LevelTileStar = content.Load<Texture2D>("tile_star");
            LevelTileJapan = content.Load<Texture2D>("tile_japan");
            LevelTileBamboo = content.Load<Texture2D>("tile_bamboo");
            LevelTileTree = content.Load<Texture2D>("tile_tree");
            LevelTileTemple = content.Load<Texture2D>("tile_temple");
            LevelTileCard = content.Load<Texture2D>("tile_card");

            LevelTileNum1 = content.Load<Texture2D>("tile_num_1");
            LevelTileNum2 = content.Load<Texture2D>("tile_num_2");
            LevelTileNum3 = content.Load<Texture2D>("tile_num_3");
            LevelTileNum4 = content.Load<Texture2D>("tile_num_4");
            LevelTileNum5 = content.Load<Texture2D>("tile_num_5");

            SelectedTileIndicator = content.Load<Texture2D>("selected_tile_indicator");
            TilesUp = content.Load<Texture2D>("tiles_up");

            Match4Text = content.Load<Texture2D>("match4");
            CascadeText = content.Load<Texture2D>("cascade");

            WinBackground = content.Load<Texture2D>("background_win");
            WinPan = content.Load<Texture2D>("win_pan");
            StarForPile = content.Load<Texture2D>("star_for_pile");

            ForceAxeButton = content.Load<Texture2D>("force_axe_button");
            ForceAxe = content.Load<Texture2D>("force_axe");
            FXForceStormAdd = content.Load<Texture2D>("force_storm_add");
            ForceStormButton = content.Load<Texture2D>("force_storm_button");

            FXSpark1 = content.Load<Texture2D>("fx_spark1");
            FXPink = content.Load<Texture2D>("fx_pink");
            FXTwirl = content.Load<Texture2D>("fx_twirl");
            FXTileBroken = content.Load<Texture2D>("fx_tile_broken");
            FXWaveCircle = content.Load<Texture2D>("fx_circle_wave");
            FXImpact1 = content.Load<Texture2D>("fx_impact1");
            FXForceStorm1 = content.Load<Texture2D>("fx_force_storm1");
            FXForceStorm2 = content.Load<Texture2D>("fx_force_storm2");
            FXRainDrop = content.Load<Texture2D>("fx_rain_drop");
            FXLight1 = content.Load<Texture2D>("fx_light1");
            FXSmoke1 = content.Load<Texture2D>("fx_smoke1");
            FXDirt = content.Load<Texture2D>("fx_dirt");
            FXXP = content.Load<Texture2D>("fx_xp");
            FXXP2 = content.Load<Texture2D>("fx_xp2");
            FXStar = content.Load<Texture2D>("fx_star");
            FXYellowOrange = content.Load<Texture2D>("fx_yellow_orange");
            FXYellow = content.Load<Texture2D>("fx_yellow");
            FXTriangle = content.Load<Texture2D>("fx_triangle");

            FXL1 = content.Load<Texture2D>("fx_l1");
            FXL2 = content.Load<Texture2D>("fx_l2");
            FXL3 = content.Load<Texture2D>("fx_l3");
            FXL4 = content.Load<Texture2D>("fx_l4");

            MusicMainMenu = content.Load<Song>("music_menu");

            LevelMusics = new List<Song>();
            LevelMusics.Add(content.Load<Song>("music_l1"));
            LevelMusics.Add(content.Load<Song>("music_l2"));
            LevelMusics.Add(content.Load<Song>("music_l3"));

            SndSplashScreen = content.Load<SoundEffect>("snd_splash_screen");
            SndTileSlide = content.Load<SoundEffect>("snd_tile_slide");
            SndTilePop = content.Load<SoundEffect>("snd_tile_pop");
            SndTilePop2 = content.Load<SoundEffect>("snd_tile_pop2");
            SndTilePop3 = content.Load<SoundEffect>("snd_tile_pop3");
            SndTilePop4 = content.Load<SoundEffect>("snd_tile_pop4");
            SndMatch = content.Load<SoundEffect>("snd_match");
            SndMatch4 = content.Load<SoundEffect>("snd_match_4");
            SndStack = content.Load<SoundEffect>("snd_tile_stack");
            SndMatchNum = content.Load<SoundEffect>("snd_match_num");
            SndButtonHovered = content.Load<SoundEffect>("snd_button_hover");
            SndForceAxe = content.Load<SoundEffect>("snd_force_axe");
            StormSounds = content.Load<SoundEffect>("storm_sounds").CreateInstance();
            SndThunder = content.Load<SoundEffect>("snd_thunder");

            ButtonThemePath1 = new string[]
            {
                "--<thm>--",

                "type : button",

                "background : button",
                "background-hover : button_hover",
                "background-pressed : button",

                "font : main_font",

                "sound-hover : snd_button_hover"
            };

            Transparent = new Texture2D(graphicsDevice, 1, 1);
            Transparent.SetData(new Color[] { Color.Transparent });

        }
    }
}