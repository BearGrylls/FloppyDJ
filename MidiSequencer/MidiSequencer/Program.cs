using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiSequencer
{
    class Program
    {
        static Sequence seq;
        static Sequencer sequencer;
        static Stopwatch stopwatch;
        static SeqWriter writer;
        static OutputDevice outDevice;

        static void Main(string[] args)
        {
            outDevice = new OutputDevice(0);

            stopwatch = new Stopwatch();
            stopwatch.Start();

            seq = new Sequence();
            seq.LoadCompleted += Seq_LoadCompleted;
            seq.Format = 1;

            sequencer = new Sequencer();
            sequencer.Position = 0;
            sequencer.Sequence = seq;
            sequencer.ChannelMessagePlayed += Sequencer_ChannelMessagePlayed;
            sequencer.PlayingCompleted += Sequencer_PlayingCompleted;
            sequencer.Stopped += Sequencer_Stopped;
            sequencer.SysExMessagePlayed += Sequencer_SysExMessagePlayed;
            sequencer.Chased += Sequencer_Chased;

            writer = new SeqWriter();

            var currDirectory = Directory.GetCurrentDirectory();
            var midiDir = Path.Combine(currDirectory, "MidiFiles");
            var files = Directory.GetFiles(midiDir);

            Console.WriteLine("Reading files from " + midiDir + "...");
            foreach(string file in files)
            {
                Console.WriteLine("Sequencing file " + file + "...");
                //seq.Load(@"MidiFiles\The_Decisive_Battle_-_Final_Fantasy_VI.mid");
                seq.Load(file);

                SeqWriter.WriteSequenceToFiles(seq, Path.GetFileNameWithoutExtension(file));

                Console.WriteLine("Done.");
            }

            Console.WriteLine("Finished.");
        }

        private static void Seq_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("LoadCompleted");
            //sequencer.Start();
        }

        private static void Sequencer_Chased(object sender, ChasedEventArgs e)
        {
            Console.WriteLine("Chased");
        }

        private static void Sequencer_SysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            Console.WriteLine("SysExMessagePlayed");
        }

        private static void Sequencer_Stopped(object sender, StoppedEventArgs e)
        {
            Console.WriteLine("Stopped");
        }

        private static void Sequencer_PlayingCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("PlayingCompleted");
        }


        private static void Sequencer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            outDevice.Send(e.Message);
            writer.Send(e.Message);
        }
    }
}
