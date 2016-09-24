using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FloppyDJ
{    
    public class SoundPlayer
    {
        public List<TrackPlayerBase> Tracks;
        Stopwatch stopwatch;
        long ticksPerSecond;

        public SoundPlayer(long ticksPerSecond)
        {
            Tracks = new List<TrackPlayerBase>();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            this.ticksPerSecond = ticksPerSecond;
        }
        
        public void Play(int bpm)
        {
            double ticksPerMinute = ticksPerSecond * 60;

            long last = 0, elapsed = 0;
            int finished = 0;

            while (finished < Tracks.Count)
            {
                elapsed = stopwatch.ElapsedTicks - last;
                last = stopwatch.ElapsedTicks;

                finished = 0;

                foreach (TrackPlayerBase track in Tracks)
                {
                    track.BeatsRemaining -= bpm / ticksPerMinute * elapsed;

                    if(track.BeatsRemaining <= 0)
                    {
                        track.GoToNextNote();
                    }
                    else if(track.BeatsRemaining >= 0.03125)
                    {
                        track.Play();
                    }

                    if (track.IsFinished) finished++;
                }
            }
        }
    }

}
