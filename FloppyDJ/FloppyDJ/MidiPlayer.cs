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
        public List<MidiTrackPlayer> Tracks = new List<MidiTrackPlayer>();
        public double PlaySpeed = 1;
        public bool IsPlaying
        {
            get
            {
                return !stop;
            }
        }

        private bool stop = true;
        private MidiConfig Config;

        public async static Task<MidiPlayer> LoadConfig(MidiConfig config, StepperMotor[] motors, bool fillParts)
        {
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
                        midiTracks[i].OctaveOffset = config.OctaveOffsets[partNum];
                    }
                }

                partNum++;
            }

            midiPlayer.PlaySpeed = config.PlaySpeed;

            return midiPlayer;
        }

        public async static Task<MidiPlayer> LoadConfig(string config, StepperMotor[] motors, bool fillParts = true)
        {
            return await LoadConfig(MidiConfig.GetConfig(config), motors, fillParts);
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
            }

            return player;
        }

        public void Play(double speed = -1)
        {
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
                    return;
                }
            }

            foreach (MidiTrackPlayer track in Tracks)
            {
                track.Reset();
            }
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

    }
}
