using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using System.IO;

namespace FloppyDJ
{
    public class TrackPlayerV2 : TrackPlayerBase
    {
        // XML
        private MeasureElementInfo[] meiList;
        private TimeInfo timeInfo;

        private int currentIndex = 0;

        private StepperMotor motor;

        public TrackPlayerV2(StepperMotor motor)
        {
            this.motor = motor;
            this.motor.AutoChangeDirection = true;
            OctaveOffset = 0;
            IsFinished = false;
        }

        public void SetMusic(MeasureElementInfo[] notes)
        {
            currentIndex = 0;
            IsFinished = false;
            meiList = notes;
            while (currentIndex < meiList.Length && !SetNote(meiList[currentIndex++])) ;
        }

        public bool SetNote(MeasureElementInfo mei)
        {
            try
            {
                switch (mei.Type)
                {
                    case MeasureElementInfoType.Time:
                        timeInfo = (TimeInfo)mei.Element;
                        return false;
                    case MeasureElementInfoType.Note:
                        var noteInfo = (NoteInfo)mei.Element;
                        motor.Speed = Pitch.GetFrequency(noteInfo.Name);
                        if (OctaveOffset < 0)
                        {
                            motor.Speed /= 2 * Math.Abs(OctaveOffset);
                        }
                        else if (OctaveOffset > 0)
                        {
                            motor.Speed *= 2 * Math.Abs(OctaveOffset);
                        }
                        BeatsRemaining = noteInfo.Duration / timeInfo.Divisions;
                        return true;
                }
            }catch(Exception)
            {

            }

            return false;
        }

        public override bool GoToNextNote()
        {
            if (currentIndex < meiList.Length)
            {
                while (!SetNote(meiList[currentIndex++])) ;

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

        public static async Task<MeasureElementInfo[]> ReadNotesFromXmlFile(string filePath)
        {
            List<MeasureElementInfo> elements;

            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(filePath);
            var inputStream = await file.OpenReadAsync();
            var serializer = new XmlSerializer(typeof(List<MeasureElementInfo>));

            elements = (List<MeasureElementInfo>)serializer.Deserialize(inputStream.AsStreamForRead());
            inputStream.Dispose();

            return elements.ToArray();
        }
    }
}
