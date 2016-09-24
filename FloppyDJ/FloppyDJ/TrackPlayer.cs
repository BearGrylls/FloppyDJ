using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloppyDJ
{
    public abstract class TrackPlayerBase
    {
        public double BeatsRemaining;
        public int OctaveOffset;
        public bool IsFinished;

        public abstract bool GoToNextNote();

        public abstract void Play();
    }
}
