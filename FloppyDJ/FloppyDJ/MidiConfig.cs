using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage.Streams;
using Windows.Storage;

namespace FloppyDJ
{
    public class MidiConfig
    {
        public string[] TrackXmlPaths;
        public int[] OctaveOffsets;
        public double PlaySpeed = 1.0;

        public MidiConfig(string[] files, int[] octaveOffsets, double playSpeed)
        {
            TrackXmlPaths = files;
            OctaveOffsets = (octaveOffsets != null) ? octaveOffsets : new int[] { 0 };
            PlaySpeed = playSpeed;
        }

        public static MidiConfig GetConfig(string midiName)
        {
            switch(midiName)
            {
                case "hyrule_temple":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Hyrule_Temple_0.xml",
                            @"assets\midi\Hyrule_Temple_1.xml",
                            @"assets\midi\Hyrule_Temple_2.xml",
                            @"assets\midi\Hyrule_Temple_3.xml",
                        }, 
                        new int[] { 0, 0 }, 1.25);

                case "trainer_battle":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Trainer_Battle_Theme_0.xml",
                            @"assets\midi\Trainer_Battle_Theme_1.xml",
                            @"assets\midi\Trainer_Battle_Theme_2.xml",
                            @"assets\midi\Trainer_Battle_Theme_3.xml",
                        },
                        new int[] { 0, 0 }, 1.25);

                case "pokemon":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\pokemon_0.xml",
                            @"assets\midi\pokemon_1.xml",
                            @"assets\midi\pokemon_2.xml",
                            @"assets\midi\pokemon_3.xml",
                        },
                        new int[] { }, 1.15);

                case "decisive_battle":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_0.xml",
                            @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_1.xml",
                            @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_2.xml",
                            @"assets\midi\The_Decisive_Battle_-_Final_Fantasy_VI_3.xml",
                        },
                        new int[] { 0, 0 }, 1.5);

                case "what_is_love":
                    return new MidiConfig(new string[]
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
                        new int[] { 0, 0, 0, 0, 0, 0, }, 0.9);

                case "scales":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Scales_0.xml",
                        },
                        new int[] { 0 }, 1.0);

                case "save_the_world":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Save_The_WorldDont_You_Worry_Child_1.xml",
                            @"assets\midi\Save_The_WorldDont_You_Worry_Child_0.xml",
                            @"assets\midi\Save_The_WorldDont_You_Worry_Child_2.xml",
                            @"assets\midi\Save_The_WorldDont_You_Worry_Child_3.xml",
                        },
                        new int[] { 0, 0 }, 1.1);

                case "take_on_me":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Take_On_Me_0.xml",
                            @"assets\midi\Take_On_Me_1.xml",
                            @"assets\midi\Take_On_Me_2.xml",
                            @"assets\midi\Take_On_Me_3.xml",
                            @"assets\midi\Take_On_Me_4.xml",
                            @"assets\midi\Take_On_Me_5.xml",
                        },
                        new int[] { 0, 0 }, 1.25);

                case "jurassic_park":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\jurassicpark_1.xml",
                            @"assets\midi\jurassicpark_4.xml",
                            @"assets\midi\jurassicpark_3.xml",
                            @"assets\midi\jurassicpark_6.xml",
                            @"assets\midi\jurassicpark_2.xml",
                            @"assets\midi\jurassicpark_5.xml",
                        },
                        new int[] { -1, -1 }, 0.1);

                case "take_on_me_2":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\a-ha - take on me (4)_1.xml",
                            @"assets\midi\a-ha - take on me (4)_2.xml",
                            @"assets\midi\a-ha - take on me (4)_3.xml",
                            @"assets\midi\a-ha - take on me (4)_4.xml",
                            @"assets\midi\a-ha - take on me (4)_5.xml",
                            @"assets\midi\a-ha - take on me (4)_6.xml",
                            @"assets\midi\a-ha - take on me (4)_7.xml",
                            @"assets\midi\a-ha - take on me (4)_8.xml",
                            @"assets\midi\a-ha - take on me (4)_9.xml",
                            @"assets\midi\a-ha - take on me (4)_10.xml",
                            @"assets\midi\a-ha - take on me (4)_11.xml",
                            @"assets\midi\a-ha - take on me (4)_12.xml",
                        },
                        new int[] { -1, -1 }, 0.25);

                case "billie_jean":
                    return new MidiConfig(new string[]
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
                        new int[] { 0, 2 }, 0.45);

                case "star_wars_medley":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Star_Wars_Medley_0.xml",
                            @"assets\midi\Star_Wars_Medley_1.xml",
                            @"assets\midi\Star_Wars_Medley_2.xml",
                            @"assets\midi\Star_Wars_Medley_3.xml",
                        },
                        new int[] { 0, 0 }, 1);

                case "supreme_star_wars_medley":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Supreme_Star_Wars_Medley_0.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_0.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_1.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_1.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_2.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_2.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_3.xml",
                            @"assets\midi\Supreme_Star_Wars_Medley_3.xml",
                        },
                        new int[] { 0,0,0,0,1,1,1,1 }, 1.0);

                case "star_wars":
                    return new MidiConfig(new string[]
                        {
                            //@"assets\midi\Star_Wars_Medley (2)_1.xml",
                            //@"assets\midi\Star_Wars_Medley (2)_2.xml",
                            @"assets\midi\Star_Wars_Medley (2)_4.xml",
                            @"assets\midi\Star_Wars_Medley (2)_8.xml",
                            @"assets\midi\Star_Wars_Medley (2)_3.xml",
                            @"assets\midi\Star_Wars_Medley (2)_5.xml",
                            @"assets\midi\Star_Wars_Medley (2)_6.xml",
                            @"assets\midi\Star_Wars_Medley (2)_7.xml",
                            @"assets\midi\Star_Wars_Medley (2)_9.xml",
                            @"assets\midi\Star_Wars_Medley (2)_10.xml",
                            @"assets\midi\Star_Wars_Medley (2)_11.xml",
                            @"assets\midi\Star_Wars_Medley (2)_12.xml",
                        },
                        new int[] { -1, -2 }, 0.2);

                case "star_wars_hexabass":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Hexabass_Star_Wars_Medley_0.xml",
                            @"assets\midi\Hexabass_Star_Wars_Medley_1.xml",
                            @"assets\midi\Hexabass_Star_Wars_Medley_2.xml",
                            @"assets\midi\Hexabass_Star_Wars_Medley_3.xml",
                            @"assets\midi\Hexabass_Star_Wars_Medley_4.xml",
                            @"assets\midi\Hexabass_Star_Wars_Medley_5.xml",
                        },
                        new int[] { 0,0 }, 2);

                case "star_wars_trombone":
                    return new MidiConfig(new string[]
                        {
                            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_0.xml",
                            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_0.xml",
                            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_1.xml",
                            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_2.xml",
                            @"assets\midi\Star_Wars_Main_Theme_Trombone_Quartet_3.xml",
                        },
                        new int[] { 0,0,0,0,1 }, 1);

                case "star_wars_jw":
                    return new MidiConfig(new string[]
                        {
                            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_1.xml",
                            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_2.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_4.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_3.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_8.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_5.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_6.xml",
                            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_7.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_9.xml",
                            //@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_10.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_11.xml",
                            @"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_12.xml",
                        },
                        new int[] { -1, 0, -1, 0, 0 }, 0.25);
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
