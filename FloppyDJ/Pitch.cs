using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloppyDJ
{
    public class NoteLength
    {
        public static double
            Quarter = 1, //0.02,
            Eighth = Quarter / 2,
            Half = Quarter * 2,
            Whole = Quarter * 4,
            Sixteenth = Eighth / 2,
            ThirtySecond = Sixteenth / 2;

        public static double Dotted(double length)
        {
            return length * 1.5;
        }
    }

    public struct Note
    {
        public string Name;
        public double Pitch;
        public double Length;
    }

    public class Pitch
    {
        static Dictionary<string, double> pitchDictionary
        {
            get
            {
                if (_pitchDict == null)
                {
                    _pitchDict = new Dictionary<string, double>();
                    _pitchDict.Add("C0", 16.35);
                    _pitchDict.Add("CSharp0", 17.32);
                    _pitchDict.Add("Db0", 17.32);
                    _pitchDict.Add("D0", 18.35);
                    _pitchDict.Add("DSharp0", 19.45);
                    _pitchDict.Add("Eb0", 19.45);
                    _pitchDict.Add("E0", 20.6);
                    _pitchDict.Add("F0", 21.83);
                    _pitchDict.Add("FSharp0", 23.12);
                    _pitchDict.Add("Gb0", 23.12);
                    _pitchDict.Add("G0", 24.5);
                    _pitchDict.Add("GSharp0", 25.96);
                    _pitchDict.Add("Ab0", 25.96);
                    _pitchDict.Add("A0", 27.5);
                    _pitchDict.Add("ASharp0", 29.14);
                    _pitchDict.Add("Bb0", 29.14);
                    _pitchDict.Add("B0", 30.87);
                    _pitchDict.Add("C1", 32.7);
                    _pitchDict.Add("CSharp1", 34.65);
                    _pitchDict.Add("Db1", 34.65);
                    _pitchDict.Add("D1", 36.71);
                    _pitchDict.Add("DSharp1", 38.89);
                    _pitchDict.Add("Eb1", 38.89);
                    _pitchDict.Add("E1", 41.2);
                    _pitchDict.Add("F1", 43.65);
                    _pitchDict.Add("FSharp1", 46.25);
                    _pitchDict.Add("Gb1", 46.25);
                    _pitchDict.Add("G1", 49);
                    _pitchDict.Add("GSharp1", 51.91);
                    _pitchDict.Add("Ab1", 51.91);
                    _pitchDict.Add("A1", 55);
                    _pitchDict.Add("ASharp1", 58.27);
                    _pitchDict.Add("Bb1", 58.27);
                    _pitchDict.Add("B1", 61.74);
                    _pitchDict.Add("C2", 65.41);
                    _pitchDict.Add("CSharp2", 69.3);
                    _pitchDict.Add("Db2", 69.3);
                    _pitchDict.Add("D2", 73.42);
                    _pitchDict.Add("DSharp2", 77.78);
                    _pitchDict.Add("Eb2", 77.78);
                    _pitchDict.Add("E2", 82.41);
                    _pitchDict.Add("F2", 87.31);
                    _pitchDict.Add("FSharp2", 92.5);
                    _pitchDict.Add("Gb2", 92.5);
                    _pitchDict.Add("G2", 98);
                    _pitchDict.Add("GSharp2", 103.83);
                    _pitchDict.Add("Ab2", 103.83);
                    _pitchDict.Add("A2", 110);
                    _pitchDict.Add("ASharp2", 116.54);
                    _pitchDict.Add("Bb2", 116.54);
                    _pitchDict.Add("B2", 123.47);
                    _pitchDict.Add("C3", 130.81);
                    _pitchDict.Add("CSharp3", 138.59);
                    _pitchDict.Add("Db3", 138.59);
                    _pitchDict.Add("D3", 146.83);
                    _pitchDict.Add("DSharp3", 155.56);
                    _pitchDict.Add("Eb3", 155.56);
                    _pitchDict.Add("E3", 164.81);
                    _pitchDict.Add("F3", 174.61);
                    _pitchDict.Add("FSharp3", 185);
                    _pitchDict.Add("Gb3", 185);
                    _pitchDict.Add("G3", 196);
                    _pitchDict.Add("GSharp3", 207.65);
                    _pitchDict.Add("Ab3", 207.65);
                    _pitchDict.Add("A3", 220);
                    _pitchDict.Add("ASharp3", 233.08);
                    _pitchDict.Add("Bb3", 233.08);
                    _pitchDict.Add("B3", 246.94);
                    _pitchDict.Add("C4", 261.63);
                    _pitchDict.Add("CSharp4", 277.18);
                    _pitchDict.Add("Db4", 277.18);
                    _pitchDict.Add("D4", 293.66);
                    _pitchDict.Add("DSharp4", 311.13);
                    _pitchDict.Add("Eb4", 311.13);
                    _pitchDict.Add("E4", 329.63);
                    _pitchDict.Add("F4", 349.23);
                    _pitchDict.Add("FSharp4", 369.99);
                    _pitchDict.Add("Gb4", 369.99);
                    _pitchDict.Add("G4", 392);
                    _pitchDict.Add("GSharp4", 415.3);
                    _pitchDict.Add("Ab4", 415.3);
                    _pitchDict.Add("A4", 440);
                    _pitchDict.Add("ASharp4", 466.16);
                    _pitchDict.Add("Bb4", 466.16);
                    _pitchDict.Add("B4", 493.88);
                    _pitchDict.Add("C5", 523.25);
                    _pitchDict.Add("CSharp5", 554.37);
                    _pitchDict.Add("Db5", 554.37);
                    _pitchDict.Add("D5", 587.33);
                    _pitchDict.Add("DSharp5", 622.25);
                    _pitchDict.Add("Eb5", 622.25);
                    _pitchDict.Add("E5", 659.25);
                    _pitchDict.Add("F5", 698.46);
                    _pitchDict.Add("FSharp5", 739.99);
                    _pitchDict.Add("Gb5", 739.99);
                    _pitchDict.Add("G5", 783.99);
                    _pitchDict.Add("GSharp5", 830.61);
                    _pitchDict.Add("Ab5", 830.61);
                    _pitchDict.Add("A5", 880);
                    _pitchDict.Add("ASharp5", 932.33);
                    _pitchDict.Add("Bb5", 932.33);
                    _pitchDict.Add("B5", 987.77);
                    _pitchDict.Add("C6", 1046.5);
                    _pitchDict.Add("CSharp6", 1108.73);
                    _pitchDict.Add("Db6", 1108.73);
                    _pitchDict.Add("D6", 1174.66);
                    _pitchDict.Add("DSharp6", 1244.51);
                    _pitchDict.Add("Eb6", 1244.51);
                    _pitchDict.Add("E6", 1318.51);
                    _pitchDict.Add("F6", 1396.91);
                    _pitchDict.Add("FSharp6", 1479.98);
                    _pitchDict.Add("Gb6", 1479.98);
                    _pitchDict.Add("G6", 1567.98);
                    _pitchDict.Add("GSharp6", 1661.22);
                    _pitchDict.Add("Ab6", 1661.22);
                    _pitchDict.Add("A6", 1760);
                    _pitchDict.Add("ASharp6", 1864.66);
                    _pitchDict.Add("Bb6", 1864.66);
                    _pitchDict.Add("B6", 1975.53);
                    _pitchDict.Add("C7", 2093);
                    _pitchDict.Add("CSharp7", 2217.46);
                    _pitchDict.Add("Db7", 2217.46);
                    _pitchDict.Add("D7", 2349.32);
                    _pitchDict.Add("DSharp7", 2489.02);
                    _pitchDict.Add("Eb7", 2489.02);
                    _pitchDict.Add("E7", 2637.02);
                    _pitchDict.Add("F7", 2793.83);
                    _pitchDict.Add("FSharp7", 2959.96);
                    _pitchDict.Add("Gb7", 2959.96);
                    _pitchDict.Add("G7", 3135.96);
                    _pitchDict.Add("GSharp7", 3322.44);
                    _pitchDict.Add("Ab7", 3322.44);
                    _pitchDict.Add("A7", 3520);
                    _pitchDict.Add("ASharp7", 3729.31);
                    _pitchDict.Add("Bb7", 3729.31);
                    _pitchDict.Add("B7", 3951.07);
                    _pitchDict.Add("C8", 4186.01);
                    _pitchDict.Add("CSharp8", 4434.92);
                    _pitchDict.Add("Db8", 4434.92);
                    _pitchDict.Add("D8", 4698.63);
                    _pitchDict.Add("DSharp8", 4978.03);
                    _pitchDict.Add("Eb8", 4978.03);
                    _pitchDict.Add("E8", 5274.04);
                    _pitchDict.Add("F8", 5587.65);
                    _pitchDict.Add("FSharp8", 5919.91);
                    _pitchDict.Add("Gb8", 5919.91);
                    _pitchDict.Add("G8", 6271.93);
                    _pitchDict.Add("GSharp8", 6644.88);
                    _pitchDict.Add("Ab8", 6644.88);
                    _pitchDict.Add("A8", 7040);
                    _pitchDict.Add("ASharp8", 7458.62);
                    _pitchDict.Add("Bb8", 7458.62);
                    _pitchDict.Add("B8", 7902.13);
                    _pitchDict.Add("Rest", 0);
                }

                return _pitchDict;
            }
        }

        private static Dictionary<string, double> _pitchDict = null;

        public static double Rest = 0;

        public static double GetFrequency(string pitch)
        {
            pitch = pitch.Replace("Fb", "E");
            pitch = pitch.Replace("Cb", "B");
            pitch = pitch.Replace("BSharp", "C");
            pitch = pitch.Replace("ESharp", "F");

            return pitchDictionary[pitch];
        }

        public static double GetFrequency(int noteId)
        {
            return Math.Pow(2, (noteId - 49.0) / 12.0) * 440;
        }
    }
}
