using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace FloppyDJ
{
    public class TrackPlayerV1 : TrackPlayerBase
    {
        private StepperMotor motor;
        private Note[] notes;
        private int currentNoteIndex = 0;

        public TrackPlayerV1(StepperMotor motor)
        {
            this.motor = motor;
            this.motor.AutoChangeDirection = true;
            OctaveOffset = 0;
        }

        public void SetMusic(Note[] notes)
        {
            currentNoteIndex = 0;
            IsFinished = false;
            this.notes = notes;
            if (notes.Length > 0)
            {
                SetNote(notes[currentNoteIndex]);
            }
        }

        public void SetNote(Note note)
        {
            motor.Speed = note.Pitch;
            if (OctaveOffset < 0)
            {
                motor.Speed /= 2 * Math.Abs(OctaveOffset);
            }
            else if (OctaveOffset > 0)
            {
                motor.Speed *= 2 * Math.Abs(OctaveOffset);
            }

            BeatsRemaining = note.Length;
        }

        public override bool GoToNextNote()
        {
            ++currentNoteIndex;
            if (currentNoteIndex < notes.Length)
            {
                SetNote(notes[currentNoteIndex]);
                return true;
            }

            IsFinished = true;
            motor.Speed = 0;
            return false;
        }

        public override void Play()
        {
            motor.Run();
        }

        public static async Task<Note[]> ReadNotesFromFile(string filePath)
        {
            //string musicFile = @"Assets\Do_You_Wanna_Build_A_Snowman_piano_Part1_1.txt";
            try
            {
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await InstallationFolder.GetFileAsync(filePath);
                var read = await FileIO.ReadLinesAsync(file);

                List<Note> notes = new List<Note>();
                foreach (string str in read)
                {
                    var temp = str.Split(',');
                    if (temp.Length == 2)
                    {
                        double pitch = Pitch.GetFrequency(temp[0]);
                        double duration = Convert.ToDouble(temp[1]);
                        notes.Add(new Note { Name = temp[0], Pitch = pitch, Length = duration });
                    }
                }

                return notes.ToArray();

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
