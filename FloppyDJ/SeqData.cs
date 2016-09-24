using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloppyDJ
{


    public class SeqData
    {
        public long TickTime;
        public int NoteId;
        public bool Play;

        public SeqData()
        {

        }

        public SeqData(long tickTime, int noteId, bool play)
        {
            TickTime = tickTime;
            NoteId = noteId;
            Play = play;
        }
    }
}
