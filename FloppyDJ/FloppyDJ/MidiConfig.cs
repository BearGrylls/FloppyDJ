using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage.Streams;
using Windows.Storage;

namespace FloppyDJ
{
    public class MidiConfig
    {
        public string[] TrackXmlPaths;
        public int[] OctaveOffsets;
        public double PlaySpeed = 1.0;
        public static Dictionary<string, MidiConfig> Configs = new Dictionary<string, MidiConfig>()
        {
            { "Undertale - Megalovania", new MidiConfig(new string[]
                    {
                        @"assets\midi\Undertale_-_Megalovania_0.xml",
                        @"assets\midi\Undertale_-_Megalovania_1.xml",
                        @"assets\midi\Undertale_-_Megalovania_2.xml",
                        @"assets\midi\Undertale_-_Megalovania_3.xml",
                    },
                    new int[] { }, 1 ) },

            { "Hyrule Temple", new MidiConfig(new string[]
                    {
                        @"assets\midi\Hyrule_Temple_0.xml",
                        @"assets\midi\Hyrule_Temple_1.xml",
                        @"assets\midi\Hyrule_Temple_2.xml",
                        @"assets\midi\Hyrule_Temple_3.xml",
                    },
                    new int[] {  }, 1.25) },

            { "Trainer Battle", new MidiConfig(new string[]
                    {
                        @"assets\midi\Trainer_Battle_Theme_0.xml",
                        @"assets\midi\Trainer_Battle_Theme_1.xml",
                        @"assets\midi\Trainer_Battle_Theme_2.xml",
                        @"assets\midi\Trainer_Battle_Theme_3.xml",
                    },
                    new int[] { -2, -1, -1, -1 }, 1.25) },

            { "Pokemon", new MidiConfig(new string[]
                    {
                        @"assets\midi\pokemon_0.xml",
                        @"assets\midi\pokemon_1.xml",
                        @"assets\midi\pokemon_2.xml",
                        @"assets\midi\pokemon_3.xml",
                    },
                    new int[] { }, 1.15) },

            { "Decisive Battle", new MidiConfig(new string[]
                    {
                        @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_0.xml",
                        @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_1.xml",
                        @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_2.xml",
                        @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_3.xml",
                    },
                    new int[] { }, 1.5) },

            { "Mortal Kombat Theme", new MidiConfig(new string[]
                    {
                        @"assets\midi\MK_Theme_0.xml",
                        @"assets\midi\MK_Theme_1.xml",
                        @"assets\midi\MK_Theme_2.xml",
                        @"assets\midi\MK_Theme_3.xml",
                    },
                    new int[] { -2, -2, -2, -2 }, 1) },

            { "Doctor Who - I am the Doctor", new MidiConfig(new string[]
                    {
                        @"assets\midi\I am the Doctor_0.xml",
                        @"assets\midi\I am the Doctor_1.xml",
                        @"assets\midi\I am the Doctor_2.xml",
                        @"assets\midi\I am the Doctor_3.xml",
                        @"assets\midi\I am the Doctor_4.xml",
                    },
                    new int[] { 0, 0, 0, 0, 0 }, 1.25 ) },

            { "Dimrain47 - Forsaken Neon", new MidiConfig(new string[]
                    {
                        @"assets\midi\Forsaken_Neon_by_Dimrain47_0.xml",
                        @"assets\midi\Forsaken_Neon_by_Dimrain47_1.xml",
                        @"assets\midi\Forsaken_Neon_by_Dimrain47_2.xml",
                    },
                    new int[] { -1, -2, -2 }, 1) },

            { "Journey - Don't Stop Believing", new MidiConfig(new string[]
                    {
                        @"assets\midi\Dont_Stop_Believing_0.xml",
                        @"assets\midi\Dont_Stop_Believing_1.xml",
                        @"assets\midi\Dont_Stop_Believing_2.xml",
                        @"assets\midi\Dont_Stop_Believing_3.xml",
                    },
                    new int[] {-1, -1, 0, 0 }, 1) },

            { "Phoenix Wright - Pursuit Cornered", new MidiConfig(new string[]
                    {
                        @"assets\midi\Pursuit__Cornered_0.xml",
                        @"assets\midi\Pursuit__Cornered_1.xml",
                        @"assets\midi\Pursuit__Cornered_2.xml",
                        @"assets\midi\Pursuit__Cornered_3.xml",
                        @"assets\midi\Pursuit__Cornered_4.xml",
                        @"assets\midi\Pursuit__Cornered_5.xml",
                        @"assets\midi\Pursuit__Cornered_6.xml",
                        @"assets\midi\Pursuit__Cornered_7.xml",
                    },
                    new int[] { }, 1.2) },

            { "Power Rangers", new MidiConfig(new string[]
                    {
                        @"assets\midi\Horn_Rangers_0.xml",
                        @"assets\midi\Horn_Rangers_1.xml",
                        @"assets\midi\Horn_Rangers_2.xml",
                        @"assets\midi\Horn_Rangers_3.xml",
                    },
                    new int[] { }, 1.5 ) },

            { "Ocarina of Time - Song of Storms", new MidiConfig(new string[]
                    {
                        @"assets\midi\SongOfStorms_0.xml",
                        @"assets\midi\SongOfStorms_1.xml",
                        @"assets\midi\SongOfStorms_2.xml",
                        @"assets\midi\SongOfStorms_3.xml",
                    },
                    new int[] { }, 1 ) },

            { "Final Fantasy VII Battle Theme", new MidiConfig(new string[]
                    {
                        @"assets\midi\Let_the_Battles_Begin_Final_Fantasy_VII_Battle_Theme_for_Brass_Quartet_0.xml",
                        @"assets\midi\Let_the_Battles_Begin_Final_Fantasy_VII_Battle_Theme_for_Brass_Quartet_1.xml",
                        @"assets\midi\Let_the_Battles_Begin_Final_Fantasy_VII_Battle_Theme_for_Brass_Quartet_2.xml",
                        @"assets\midi\Let_the_Battles_Begin_Final_Fantasy_VII_Battle_Theme_for_Brass_Quartet_3.xml",
                    },
                    new int[] { -2 }, 0.9 ) },

            { "Mulan - I'll Make a Man Out of You", new MidiConfig(new string[]
                    {
                        @"assets\midi\Ill_Make_a_Man_out_of_You_0.xml",
                        @"assets\midi\Ill_Make_a_Man_out_of_You_1.xml",
                        @"assets\midi\Ill_Make_a_Man_out_of_You_2.xml",
                        @"assets\midi\Ill_Make_a_Man_out_of_You_3.xml",
                    },
                    new int[] { -1, 0, 0, 0 }, 1 ) },

            { "Portal - Still Alive", new MidiConfig(new string[]
                    {
                        @"assets\midi\Still_Alive_0.xml",
                        @"assets\midi\Still_Alive_1.xml",
                        @"assets\midi\Still_Alive_2.xml",
                        @"assets\midi\Still_Alive_3.xml",
                    },
                    new int[] { -1, -1, -2, -1 }, 1 ) },

            { "Street Fighter - Guile Theme", new MidiConfig(new string[]
                    {
                        @"assets\midi\Street_Fighter_II_Guile_Theme_0.xml",
                        @"assets\midi\Street_Fighter_II_Guile_Theme_1.xml",
                        @"assets\midi\Street_Fighter_II_Guile_Theme_2.xml",
                        @"assets\midi\Street_Fighter_II_Guile_Theme_3.xml",
                    },
                    new int[] { -2, -1, -1, -1 }, 1 ) },

            { "The Flash", new MidiConfig(new string[]
                    {
                        @"assets\midi\The_Flash_Main_Theme_0.xml",
                        @"assets\midi\The_Flash_Main_Theme_0.xml",
                        @"assets\midi\The_Flash_Main_Theme_1.xml",
                        @"assets\midi\The_Flash_Main_Theme_2.xml",
                        @"assets\midi\The_Flash_Main_Theme_3.xml",
                    },
                    new int[] { -2, -1, -1, -1, -1 }, 1) },

            { "The Incredibles", new MidiConfig(new string[]
                    {
                        @"assets\midi\The_Incredibles_0.xml",
                        @"assets\midi\The_Incredibles_1.xml",
                        @"assets\midi\The_Incredibles_2.xml",
                        @"assets\midi\The_Incredibles_3.xml",
                        @"assets\midi\The_Incredibles_4.xml",
                        @"assets\midi\The_Incredibles_5.xml",
                        @"assets\midi\The_Incredibles_7.xml",
                        @"assets\midi\The_Incredibles_9.xml",
                    },
                    new int[] { -2, -2, -2, -1, -1, -1, -1, -1 }, 1.25) },

            { "Haddaway - What is Love", new MidiConfig(new string[]
                    {
                        @"assets\midi\What_is_Love_4.xml",  // Singing
                        @"assets\midi\What_is_Love_2.xml",  // Synth bass
                        @"assets\midi\What_is_Love_3.xml",  // Organ
                        @"assets\midi\What_is_Love_6.xml",  // Bass
                        @"assets\midi\What_is_Love_1.xml",  // Saw wave
                        @"assets\midi\What_is_Love_5.xml",  // 5th Saw wave
                        @"assets\midi\What_is_Love_7.xml",  // Tappeto
                        @"assets\midi\What_is_Love_8.xml",  // Archi
                        @"assets\midi\What_is_Love_9.xml",  // Drum
                        @"assets\midi\What_is_Love_17.xml", // Singing
                    },
                    new int[] { }, 0.9) },

            { "Pentatonix - Save the World / Don't You Worry Child", new MidiConfig(new string[]
                    {
                        @"assets\midi\Save_The_WorldDont_You_Worry_Child_1.xml",
                        @"assets\midi\Save_The_WorldDont_You_Worry_Child_0.xml",
                        @"assets\midi\Save_The_WorldDont_You_Worry_Child_2.xml",
                        @"assets\midi\Save_The_WorldDont_You_Worry_Child_3.xml",
                    },
                    new int[] { }, 1.1) },

            { "a-ha - Take On Me", new MidiConfig(new string[]
                    {
                        @"assets\midi\Take_On_Me_0.xml",
                        @"assets\midi\Take_On_Me_1.xml",
                        @"assets\midi\Take_On_Me_2.xml",
                        @"assets\midi\Take_On_Me_3.xml",
                        @"assets\midi\Take_On_Me_4.xml",
                        @"assets\midi\Take_On_Me_5.xml",
                    },
                    new int[] { }, 1.25) },

            { "Jurassic Park", new MidiConfig(new string[]
                    {
                        @"assets\midi\jurassicpark_1.xml",
                        @"assets\midi\jurassicpark_4.xml",
                        @"assets\midi\jurassicpark_3.xml",
                        @"assets\midi\jurassicpark_6.xml",
                        @"assets\midi\jurassicpark_2.xml",
                        @"assets\midi\jurassicpark_5.xml",
                    },
                    new int[] { -2, -2 }, 0.1) },

            //{ "Take On Me (2)", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\a-ha - take on me (4)_1.xml",
            //            @"assets\midi\a-ha - take on me (4)_2.xml",
            //            @"assets\midi\a-ha - take on me (4)_3.xml",
            //            @"assets\midi\a-ha - take on me (4)_4.xml",
            //            @"assets\midi\a-ha - take on me (4)_5.xml",
            //            @"assets\midi\a-ha - take on me (4)_6.xml",
            //            @"assets\midi\a-ha - take on me (4)_7.xml",
            //            @"assets\midi\a-ha - take on me (4)_8.xml",
            //            @"assets\midi\a-ha - take on me (4)_9.xml",
            //            @"assets\midi\a-ha - take on me (4)_10.xml",
            //            @"assets\midi\a-ha - take on me (4)_11.xml",
            //            @"assets\midi\a-ha - take on me (4)_12.xml",
            //        },
            //        new int[] { }, 0.25) },

            { "Michael Jackson - Billie Jean", new MidiConfig(new string[]
                    {
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_1.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_7.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_2.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_6.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_3.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_4.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_5.xml",
                        @"assets\midi\Michael_Jackson_-_Billie_Jean_8.xml",
                    },
                    new int[] { -1, 0, -1, 0, -1, -1, -1, -1 }, 0.45) },

            { "Star Wars Medley", new MidiConfig(new string[]
                    {
                        @"assets\midi\Star_Wars_Medley_0.xml",
                        @"assets\midi\Star_Wars_Medley_1.xml",
                        @"assets\midi\Star_Wars_Medley_2.xml",
                        @"assets\midi\Star_Wars_Medley_3.xml",
                    },
                    new int[] { }, 1) },

            //{ "Supreme Star Wars Medley", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\Supreme_Star_Wars_Medley_0.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_0.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_1.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_1.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_2.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_2.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_3.xml",
            //            @"assets\midi\Supreme_Star_Wars_Medley_3.xml",
            //        },
            //        new int[] { }, 1.0) },

            //{ "Star Wars", new MidiConfig(new string[]
            //        {
            //            //@"assets\midi\Star_Wars_Medley (2)_1.xml",
            //            //@"assets\midi\Star_Wars_Medley (2)_2.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_4.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_8.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_3.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_5.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_6.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_7.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_9.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_10.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_11.xml",
            //            @"assets\midi\Star_Wars_Medley (2)_12.xml",
            //        },
            //        new int[] { }, 0.2) },

            //{ "Star Wars (Hexabass)", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\Hexabass_Star_Wars_Medley_0.xml",
            //            @"assets\midi\Hexabass_Star_Wars_Medley_1.xml",
            //            @"assets\midi\Hexabass_Star_Wars_Medley_2.xml",
            //            @"assets\midi\Hexabass_Star_Wars_Medley_3.xml",
            //            @"assets\midi\Hexabass_Star_Wars_Medley_4.xml",
            //            @"assets\midi\Hexabass_Star_Wars_Medley_5.xml",
            //        },
            //        new int[] { }, 2) },

            //{ "Star Wars (Trombone)", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_0.xml",
            //            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_0.xml",
            //            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_1.xml",
            //            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_2.xml",
            //            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_3.xml",
            //        },
            //        new int[] { }, 1) },

            //{ "John Williams - Star Wars", new MidiConfig(new string[]
            //        {
            //            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_1.xml",
            //            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_2.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_4.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_3.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_8.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_5.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_6.xml",
            //            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_7.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_9.xml",
            //            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_10.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_11.xml",
            //            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_12.xml",
            //        },
            //        new int[] { }, 0.25) },

            { "Derezzed", new MidiConfig(new string[]
                    {
                        @"assets\midi\derezzed_5.xml",
                        @"assets\midi\derezzed_6.xml",
                        @"assets\midi\derezzed_7.xml",
                        @"assets\midi\derezzed_8.xml",
                    },
                    new int[] { }, 0.2) },

            { "Video Game Symphony", new MidiConfig(new string[]
                    {
                        @"assets\midi\Video_Game_Symphony_0.xml",
                        @"assets\midi\Video_Game_Symphony_1.xml",
                        @"assets\midi\Video_Game_Symphony_2.xml",
                        @"assets\midi\Video_Game_Symphony_3.xml",
                        @"assets\midi\Video_Game_Symphony_4.xml",
                        @"assets\midi\Video_Game_Symphony_5.xml",
                        @"assets\midi\Video_Game_Symphony_6.xml",
                    },
                    new int[] { -2, 0, 0, 0, 0, 0, -2 }, 1.5) },

            //{ "Disney Medley", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\Disney_Medley_0.xml",
            //            @"assets\midi\Disney_Medley_1.xml",
            //            @"assets\midi\Disney_Medley_2.xml",
            //            @"assets\midi\Disney_Medley_3.xml",
            //        },
            //        new int[] { }, 1) },

            { "Game of Thrones", new MidiConfig(new string[]
                    {
                        @"assets\midi\Game_of_Thrones_Theme_0.xml",
                        @"assets\midi\Game_of_Thrones_Theme_1.xml",
                        @"assets\midi\Game_of_Thrones_Theme_2.xml",
                        @"assets\midi\Game_of_Thrones_Theme_3.xml",
                    },
                    new int[] {-2, -2, -2, 0 }, 0.75) },

            { "Flight of the Bumblebee", new MidiConfig(new string[]
                    {
                        @"assets\midi\Flight of the Bumblebee_0.xml",
                        @"assets\midi\Flight of the Bumblebee_1.xml",
                    },
                    new int[] { -2, -1 }, 1.25) },

            { "DragonForce - Through the Fire and the Flames", new MidiConfig(new string[]
                    {
                        @"assets\midi\Through_The_Fire_and_The_Flames_0.xml",
                        @"assets\midi\Through_The_Fire_and_The_Flames_1.xml",
                        @"assets\midi\Through_The_Fire_and_The_Flames_2.xml",
                        @"assets\midi\Through_The_Fire_and_The_Flames_3.xml",
                        @"assets\midi\Through_The_Fire_and_The_Flames_4.xml",
                    },
                    new int[] { -2, -2, -1, -1, -1 }, 1.5) },

            //{ "Fairy Tail Main Theme", new MidiConfig(new string[]
            //        {
            //            @"assets\midi\Fairy_Tail_Main_Theme_brass_0.xml",
            //            @"assets\midi\Fairy_Tail_Main_Theme_brass_1.xml",
            //            @"assets\midi\Fairy_Tail_Main_Theme_brass_2.xml",
            //            @"assets\midi\Fairy_Tail_Main_Theme_brass_3.xml",
            //        },
            //        new int[] { }, 1.1) },

            { "Scales", new MidiConfig(new string[]
                    {
                        @"assets\midi\Scales_0.xml",
                    },
                    new int[] { }, 1.0) },

            //{ "Final Fantasy VIII - Force Your Way", new MidiConfig(new string[]
            //        {
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_0.xml",   // F. Horn
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_1.xml",   // C Trumpet
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_14.xml",  // Synth Treble
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_3.xml",   // El. Guitar 1
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_4.xml",   // El. Guitar 2
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_5.xml",   // El. Bass
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_6.xml",   // Harp Treble
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_7.xml",   // Harp Bass
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_8.xml",   // Organ Treble
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_9.xml",   // Organ Bass
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_10.xml",  // Hm. Org. Treble
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_11.xml",  // Hm. Org. Bass
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_12.xml",  // Violin 1
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_13.xml",  // Violin 2
            //            //@"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_15.xml",    // Synth Bass
            //            @"assets\midi\Final_Fantasy_VIII_-_Force_Your_Way_2.xml",   // Drums
            //        },
            //        new int[] { }, 1) },
        };

        public MidiConfig(string[] files, int[] octaveOffsets, double playSpeed)
        {
            TrackXmlPaths = files;
            OctaveOffsets = (octaveOffsets != null) ? octaveOffsets : new int[] { 0 };
            PlaySpeed = playSpeed;
        }

        public static MidiConfig GetConfig(string midiName)
        {
            if(Configs.Keys.Contains(midiName))
            {
                return Configs[midiName];
            }

            return null;
        }
    }

    public class TrackConfig
    {
        public StepperMotor[] Motors;
        public int OctaveOffset = 0;
        public string XmlPath = "";

        public TrackConfig()
        {

        }

        public TrackConfig(string xmlPath, int octaveOffset, params StepperMotor[] motors)
        {
            XmlPath = xmlPath;
            OctaveOffset = octaveOffset;
            Motors = motors;
        }
    }

}
