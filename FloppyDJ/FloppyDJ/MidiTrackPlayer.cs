using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using System.IO;

namespace FloppyDJ
{
    public class MidiTrackPlayer
    {
        //public List<Queue<SeqData>> Tracks;
        public Queue<SeqData> Track;

        public bool Done
        {
            get;
            private set;
        }

        public int OctaveOffset = 0;

        private StepperMotor motor;
        private Instrument[] instruments;
        private List<SeqData> data;

        public MidiTrackPlayer(StepperMotor motor)
        {
            this.motor = motor;
            this.Track = new Queue<SeqData>();
            this.data = new List<SeqData>();
        }

        public MidiTrackPlayer(StepperMotor[] motors)
        {
            List<Instrument> inst = new List<Instrument>();
            foreach(StepperMotor motor in motors)
            {
                inst.Add(new Instrument()
                {
                    Motor = motor,
                    Note = null
                });
            }
            this.instruments = inst.ToArray();
            this.Track = new Queue<SeqData>();
            this.data = new List<SeqData>();
        }

        public async Task LoadMidiTrack(string filePath)
        {
            Debug.WriteLine("Loading midi track: " + filePath);
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(filePath);
            var inputStream = await file.OpenReadAsync();
            var serializer = new XmlSerializer(typeof(List<SeqData>));

            var data = (List<SeqData>)serializer.Deserialize(inputStream.AsStreamForRead());
            inputStream.Dispose();

            Track = new Queue<SeqData>(data);
            this.data = new List<SeqData>(data);
        }

        public void Reset()
        {
            Track = new Queue<SeqData>(data);
        }

        public void Play(long tick)
        {
            if(motor != null)
            {
                SinglePlay(tick);
            }
            else if(instruments != null)
            {
                MultiPlay(tick);
            }
        }

        public void MultiPlay(long tick)
        {
            if (Track.Count > 0)
            {
                SeqData curr = Track.Peek();

                if (tick > curr.TickTime)
                {
                    // Stop playing note
                    if (!curr.Play)
                    {
                        for (int i = 0; i < instruments.Length; i++)
                        {
                            // Find motor that was playing this note and stop it
                            if (instruments[i].Note.NoteId == curr.NoteId)
                            {
                                instruments[i].Motor.Speed = 0;
                                instruments[i].Note = curr;
                                break;
                            }
                        }

                        Track.Dequeue();
                    }
                    // Play note
                    else
                    {
                        // Figure out if this is a chord
                        List<SeqData> chordNotes = new List<SeqData>();
                        while(Track.Peek().TickTime == curr.TickTime && Track.Peek().Play)
                        {
                            chordNotes.Add(Track.Dequeue());
                        }

                        chordNotes = chordNotes.OrderByDescending(n => n.NoteId).ToList();

                        foreach (SeqData chordNote in chordNotes)
                        {
                            for (int i = 0; i < instruments.Length; i++)
                            {
                                // Find a motor that is free and play the note
                                if (instruments[i].Note == null || !instruments[i].Note.Play)
                                //|| (instruments[i].Note.NoteId < curr.NoteId))  // if the new note is a higher pitch, play the new note(I'm assuming it's the melody)
                                {
                                    double freq = Pitch.GetFrequency(chordNote.NoteId);
                                    while(freq < Pitch.GetFrequency(39) / 2)
                                    {
                                        freq *= 2;
                                    }

                                    while(freq > Pitch.GetFrequency(51) * 2)
                                    {
                                        freq /= 2;
                                    }

                                    instruments[i].Motor.Speed = freq /2;

                                    if (OctaveOffset < 0)
                                    {
                                        instruments[i].Motor.Speed /= 2 * Math.Abs(OctaveOffset);
                                    }
                                    else if (OctaveOffset > 0)
                                    {
                                        instruments[i].Motor.Speed *= 2 * Math.Abs(OctaveOffset);
                                    }

                                    instruments[i].Note = chordNote;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Instrument inst in instruments)
                {
                    inst.Motor.Speed = 0;
                }
                Done = true;
                return;
            }

            foreach (Instrument inst in instruments)
            {
                inst.Motor.Run();
            }
        }

        SeqData prev = null;

        public void SinglePlay(long tick)
        {
            if (Track.Count > 0)
            {
                SeqData curr = Track.Peek();

                if (tick > curr.TickTime)
                {
                    Track.Dequeue();

                    if (curr.Play && (prev == null || !prev.Play))
                    {
                        motor.Speed = Pitch.GetFrequency(curr.NoteId) / 2;

                        if (OctaveOffset < 0)
                        {
                            motor.Speed /= 2 * Math.Abs(OctaveOffset);
                        }
                        else if (OctaveOffset > 0)
                        {
                            motor.Speed *= 2 * Math.Abs(OctaveOffset);
                        }

                        prev = curr;
                    }
                    else if (!curr.Play && prev.NoteId == curr.NoteId)
                    {
                        motor.Speed = 0;

                        prev = curr;
                    }
                }
                else if (curr.TickTime - tick < 10)
                {
                    motor.Speed = 0;
                }
            }
            else
            {
                motor.Speed = 0;
                Done = true;
                return;
            }

            motor.Run();
        }

        //int currTrack = 0;
        //public void MultiPlay(long tick)
        //{
        //    for (int i = 0; i < Tracks.Count; i++)
        //    {
        //        var data = Tracks[i];

        //        if (data.Count > 0)
        //        {
        //            SeqData curr = data.Peek();

        //            if (tick > curr.TickTime)
        //            {
        //                data.Dequeue();

        //                if (curr.Play)
        //                {
        //                    if ((i == 0  || i < currTrack || currTrack == -1) && motor.Speed == 0)
        //                    {
        //                        motor.Speed = Pitch.GetFrequency(curr.NoteId);

        //                        if (OctaveOffset < 0)
        //                        {
        //                            motor.Speed /= 2 * Math.Abs(OctaveOffset);
        //                        }
        //                        else if (OctaveOffset > 0)
        //                        {
        //                            motor.Speed *= 2 * Math.Abs(OctaveOffset);
        //                        }

        //                        currTrack = i;
        //                    }
        //                }
        //                else
        //                {
        //                    if (i == 0 || i == currTrack)
        //                    {
        //                        motor.Speed = 0;
        //                        currTrack = -1;
        //                    }
        //                }
        //            }
        //            //else if (curr.TickTime - tick < 10)
        //            //{
        //            //    motor.Speed = 0;
        //            //}
        //        }
        //    }

        //    motor.Run();
        //}

        public void PlayScale()
        {
            int[] scaleNotes =
            {
                40, 42, 44, 45, 47, 49, 51, 52
            };

            Stopwatch time = new Stopwatch();
            time.Start();

            for (int i = 0; i < scaleNotes.Length; i++)
            {
                motor.Speed = Pitch.GetFrequency(40 + i - 12);
                long start = time.ElapsedMilliseconds;
                while (time.ElapsedMilliseconds - start < 100)
                {
                    motor.Run();
                }
            }


            for (int i = scaleNotes.Length - 1; i >= 0; i--)
            {
                motor.Speed = Pitch.GetFrequency(40 + i - 12);
                long start = time.ElapsedMilliseconds;
                while (time.ElapsedMilliseconds - start < 100)
                {
                    motor.Run();
                }
            }
        }
    }
}
