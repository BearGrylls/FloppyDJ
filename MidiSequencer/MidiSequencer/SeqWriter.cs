using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MidiSequencer
{
    public class SeqWriter
    {
        public List<SeqPart> Parts;
        public Dictionary<int, SeqPart> PartDict; 

        private int LowNoteID = 21;
        private int HighNoteID = 109;
        private Stopwatch stopwatch;

        public SeqWriter()
        {
            Parts = new List<SeqPart>();
            PartDict = new Dictionary<int, SeqPart>();
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void Send(ChannelMessage message)
        {
            AddToPart(message);
        }

        private void AddToPart(ChannelMessage message)
        {
            int noteId = message.Data1 - LowNoteID;

            if (!PartDict.Keys.Contains(message.MidiChannel))
            {
                PartDict.Add(message.MidiChannel, new SeqPart());
            }

            SeqData data = null;
            
            if (message.Command == ChannelCommand.NoteOn &&
                   message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                if (message.Data2 > 0)
                {
                    //data = new SeqData(stopwatch.ElapsedTicks, (double)noteId, true);
                }
                else
                {
                    //data = new SeqData(stopwatch.ElapsedTicks, (double)noteId, false);
                }
            }
            else if (message.Command == ChannelCommand.NoteOff &&
                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                //data = new SeqData(stopwatch.ElapsedTicks, (double)noteId, false);
            }
            
            if(data != null)
            {
                PartDict[message.MidiChannel].Data.Add(data);
            }
        }

        private void AddToPart(SeqData data)
        {
            bool added = false;
            int partNum = 0;

            for (int i = 0; i < Parts.Count; i++)
            {
                var part = Parts[i];

                if (part.Data.Count > 0)
                {
                    var last = part.Data.Last();

                    //if ((!data.Play && last.Play && last.Frequency == data.Frequency) ||
                    //   (last != null && !last.Play && data.Play))
                    //{
                    //    part.Data.Add(data);
                    //    added = true;
                    //    partNum = i;
                    //}
                    //else if (data.TickTime - last.TickTime > 2000)
                    //{
                    //    var newStop = new SeqData(data.TickTime, last.Frequency, false);
                    //    part.Data.Add(newStop);
                    //    part.Data.Add(data);
                    //    added = true;
                    //    partNum = i;
                    //}
                }
                else
                {
                    part.Data.Add(data);
                    added = true;
                    partNum = i;
                }
            }

            if(!added && data.Play)
            {
                var newPart = new SeqPart();
                newPart.Data.Add(data);
                Parts.Add(newPart);
                partNum = Parts.Count - 1;
            }

            //Console.WriteLine("[" + partNum + "] " + "TickTime: " + data.TickTime + ", Frequency: " + data.Frequency + ",  Play: " + data.Play);
        }

        public static void WriteSequenceToFiles(Sequence sequence, string filename = "output")
        {
            int LowNoteID = 21;
            int HighNoteID = 109;

            for (int i = 0; i < sequence.Count; i++)
            {
                Console.Write("Sequencing track " + i + "... ");

                Track track = sequence[i];

                List<SeqData> data = new List<SeqData>();

                foreach (MidiEvent midiEvent in track.Iterator())
                {
                    if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                    {
                        var message = midiEvent.MidiMessage as ChannelMessage;
                        if (message != null)
                        {
                            int noteId = message.Data1 - LowNoteID;

                            if (message.Command == ChannelCommand.NoteOn &&
                                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
                            {
                                if (message.Data2 > 0)
                                {
                                    data.Add(new SeqData(midiEvent.AbsoluteTicks, noteId, true));
                                }
                                else
                                {
                                    data.Add(new SeqData(midiEvent.AbsoluteTicks, noteId, false));
                                }
                            }
                            else if (message.Command == ChannelCommand.NoteOff &&
                                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
                            {
                                data.Add(new SeqData(midiEvent.AbsoluteTicks, noteId, false));
                            }
                        }
                    }
                }

                if (data.Count > 0)
                {
                    XmlSerializer x = new XmlSerializer(data.GetType());
                    TextWriter writer = new StreamWriter(filename + "_" + i + ".xml");
                    x.Serialize(writer, data);
                    writer.Close();
                }

                Console.WriteLine("Done.");
            }
        }
    }

    public class SeqPart
    {
        public List<SeqData> Data = new List<SeqData>();
    }

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
