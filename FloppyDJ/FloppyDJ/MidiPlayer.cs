using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace FloppyDJ
{
    public class MidiPlayer
    {
        public event EventHandler StateChanged;
        public List<MidiTrackPlayer> Tracks = new List<MidiTrackPlayer>();
        public double PlaySpeed = 1;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }

            set
            {
                _isPlaying = value;
                OnStateChanged(new EventArgs());
            }
        }

        private bool stop = true;
        private MidiConfig Config;
        private bool _isPlaying = false;

        public async static Task<MidiPlayer> LoadConfig(MidiConfig config, StepperMotor[] motors, bool fillParts)
        {
            if (config == null) return null;

            MidiPlayer midiPlayer = new MidiPlayer();

            midiPlayer.Config = config;

            MidiTrackPlayer[] midiTracks = new MidiTrackPlayer[motors.Length];

            int partNum = 0;

            for (int i = 0; i < motors.Length; i++)
            {
                midiTracks[i] = new MidiTrackPlayer(motors[i]);

                if (partNum >= config.TrackXmlPaths.Length)
                {
                    if(fillParts)
                        partNum = 0;
                    else
                        break;
                }

                if (config.TrackXmlPaths[partNum] != null)
                {
                    await midiTracks[i].LoadMidiTrack(config.TrackXmlPaths[partNum]);

                    midiPlayer.Tracks.Add(midiTracks[i]);

                    if (partNum < config.OctaveOffsets.Length)
                    {
                        motors[i].OctaveOffset = midiTracks[i].OctaveOffset = config.OctaveOffsets[partNum];
                    }
                }

                partNum++;
            }

            midiPlayer.PlaySpeed = config.PlaySpeed;

            return midiPlayer;
        }

        public async static Task<MidiPlayer> LoadConfig(string config, StepperMotor[] motors, bool fillParts = true)
        {
            var player = await LoadConfig(MidiConfig.GetConfig(config), motors, fillParts);

            if (player != null) return player;

            switch (config)
            {
                case "Attack on Titan - Guren No Yumiya":
                    return await LoadTrackConfigs(
                        0.75,
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__0.xml", -1, motors[0]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__1.xml", -1, motors[1], motors[2]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__2.xml", 0, motors[3], motors[4]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__3.xml", -1, motors[5], motors[6])
                    );
                case "star_wars":
                    return await LoadTrackConfigs(
                        0.25,
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_4.xml", -1, motors[0], motors[1]),  // trumpets
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_3.xml", 0, motors[1], motors[2]),  // trombones
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_5.xml", 0, motors[3]),  // more trombones?
                                                                                                                         //new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_6.xml", 0, motors[3]),
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_8.xml", -1, motors[4], motors[5]),
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_9.xml", 0, motors[6], motors[7])
                    );
                case "this_game":
                    return await LoadTrackConfigs(
                        2,
                        new TrackConfig(@"assets\midi\This Game (1)_1.xml", 0, motors.ToArray())
                    );
                case "The Incredibles":
                    return await LoadTrackConfigs(
                        1.25,
                        new TrackConfig(@"assets\midi\The_Incredibles_0.xml", -1, motors[0]),
                        new TrackConfig(@"assets\midi\The_Incredibles_1.xml", 0, motors[1]),
                        new TrackConfig(@"assets\midi\The_Incredibles_2.xml", 0, motors[2]),
                        new TrackConfig(@"assets\midi\The_Incredibles_3.xml", 0, motors[3]),
                        new TrackConfig(@"assets\midi\The_Incredibles_4.xml", 0, motors[4]),
                        new TrackConfig(@"assets\midi\The_Incredibles_5.xml", 0, motors[5]),
                        //new TrackConfig(@"assets\midi\The_Incredibles_6.xml", 0, motors[6]),
                        new TrackConfig(@"assets\midi\The_Incredibles_7.xml", 0, motors[6]),
                        //new TrackConfig(@"assets\midi\The_Incredibles_8.xml", 0, motors[8]),
                        new TrackConfig(@"assets\midi\The_Incredibles_9.xml", 0, motors[7])
                    );
            }

            return player;
        }

        public async static Task<MidiPlayer> LoadTrackConfigs(double speed = 1, params TrackConfig[] configs)
        {
            MidiPlayer player = new MidiPlayer();
            player.PlaySpeed = speed;

            foreach(TrackConfig config in configs)
            {
                MidiTrackPlayer track = new MidiTrackPlayer(config.Motors);
                await track.LoadMidiTrack(config.XmlPath);
                track.OctaveOffset = config.OctaveOffset;
                player.Tracks.Add(track);

                foreach(StepperMotor motor in config.Motors)
                {
                    motor.OctaveOffset = config.OctaveOffset;
                }
            }

            return player;
        }

        public void Play(double speed = -1)
        {
            IsPlaying = true;

            Debug.WriteLine("Playing midi...");

            stop = false;

            if (speed < 0) speed = PlaySpeed;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!stop)
            {
                int count = 0;
                foreach (MidiTrackPlayer track in Tracks)
                {
                    track.Play((long)(stopwatch.ElapsedMilliseconds * speed));
                    if (track.Done)
                    {
                        count++;
                    }
                }

                if (count == Tracks.Count)
                {
                    IsPlaying = false;
                    return;
                }
            }

            foreach (MidiTrackPlayer track in Tracks)
            {
                track.Reset();
            }

            IsPlaying = false;
        }

        public void PlayGlobal(double speed = -1)
        {
            Debug.WriteLine("Playing midi...");

            stop = false;

            if (speed < 0) speed = PlaySpeed;

            while (!stop)
            {
                int count = 0;
                foreach (MidiTrackPlayer track in Tracks)
                {
                    track.Play((long)(Global.Watch.ElapsedMilliseconds * speed));
                    if (track.Done)
                    {
                        count++;
                    }
                }

                if (count == Tracks.Count)
                {
                    return;
                }
            }

            foreach (MidiTrackPlayer track in Tracks)
            {
                track.Reset();
            }
        }

        public void Stop()
        {
            stop = true;
            while (IsPlaying) ;
        }

        public void MultiPlay(double speed = 1)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                foreach (MidiTrackPlayer track in Tracks)
                {
                    //track.MultiPlay((long)(stopwatch.ElapsedMilliseconds * speed));
                }
            }
        }
        
        protected virtual void OnStateChanged(EventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }
    }
}
