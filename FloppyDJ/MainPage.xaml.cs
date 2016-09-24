using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FloppyDJ
{
    public sealed partial class MainPage : Page
    {
        MidiPlayer player;

        public MainPage()
        {
            this.InitializeComponent();
            titleTextBlock.Text = "";
        }

        private long getTps()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long sum = 0;
            for (int i = 0; i < 100; i++)
            {
                if (i > 0 || i < 99)
                {
                    sum += (long)((double)stopwatch.ElapsedTicks / stopwatch.ElapsedMilliseconds * 1000);
                }
            }

            return sum / 100;
        }

        StepperMotor[] motors;
        Instrument[] instruments;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            long tps = getTps();

            motors = new StepperMotor[]
            {
                new StepperMotor(16, 12, tps),  // 2
                new StepperMotor(17, 26, tps),  // 3
                new StepperMotor(21, 20, tps),  // 1
                new StepperMotor(13, 19, tps),  // 5
                new StepperMotor(18, 23, tps),  // 7
                new StepperMotor(6, 5, tps),  // 8
                new StepperMotor(22, 27, tps),  // 4
                new StepperMotor(25, 24, tps),  // 6
            };

            List<Instrument> instruments = new List<Instrument>();
            foreach(StepperMotor m in motors)
            {
                instruments.Add(new Instrument()
                {
                    Motor = m,
                    Note = null
                });
            }
            this.instruments = instruments.ToArray();

            //motors[0].DirChangeSteps = 0;

            int count = 0;
            foreach(StepperMotor motor in motors)
            {
                var button = new Button()
                {
                    Content = "Motor " + (count++).ToString()
                };

                button.Click += async (s, args) =>
                {
                    await ThreadPool.RunAsync(async (s1) =>
                    {
                        MidiPlayer scalePlayer = await MidiPlayer.LoadConfig("scales", new StepperMotor[] { motor });
                        scalePlayer.Play();
                    }, WorkItemPriority.Normal);
                };

                stackPanel.Children.Add(button);

                ThreadPool.RunAsync((s) =>
                {
                    motor.Reset();
                }, WorkItemPriority.High);
            }
            //Global.Watch = new Stopwatch();

            //await ThreadPool.RunAsync(async (s1) =>
            //{
            //    MidiPlayer scalePlayer = await MidiPlayer.LoadConfig("scales", new StepperMotor[] { motors[0] });
            //    scalePlayer.PlayGlobal();
            //}, WorkItemPriority.Normal);

            //await ThreadPool.RunAsync(async (s1) =>
            //{
            //    MidiPlayer scalePlayer = await MidiPlayer.LoadConfig("trainer_battle", new StepperMotor[] { motors[0] });
            //    scalePlayer.PlayGlobal();
            //}, WorkItemPriority.Normal);

            //Global.Watch.Start();

            await Task.Delay(10);

            ////Scales to hear volume
            //foreach (StepperMotor motor in motors)
            //{
            //    MidiPlayer player = await MidiPlayer.LoadConfig("scales", new StepperMotor[] { motor });
            //    player.Play();
            //}

            string configName = "decisive_battle";

            //player = await MidiPlayer.LoadConfig(configName, motors);
            player = await LoadSong("jurassic_park");
            //titleTextBlock.Text = configName;

            await Task.Delay(2);

            player.Play();
        }
        
        public async Task<MidiPlayer> LoadSong(string song)
        {
            MidiPlayer player = await MidiPlayer.LoadConfig(song, motors);
            if (player != null) return player;

            switch (song)
            {
                case "guren_no_yumiya":
                    return await MidiPlayer.LoadTrackConfigs(
                        0.75,
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__0.xml", -1, motors[0]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__1.xml", -1, motors[1], motors[2]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__2.xml", 0, motors[3], motors[4]),
                        new TrackConfig(@"assets\midi\Guren_no_Yumiya_Finished__3.xml", -1, motors[5], motors[6])
                    );
                case "star_wars":
                    return await MidiPlayer.LoadTrackConfigs(
                        0.25,
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_4.xml", -1, motors[0], motors[1]),  // trumpets
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_3.xml", 0, motors[1], motors[2]),  // trombones
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_5.xml", 0, motors[3]),  // more trombones?
                        //new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_6.xml", 0, motors[3]),
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_8.xml", -1, motors[4], motors[5]),
                        new TrackConfig(@"assets\midi\Movie_Themes_-_Star_Wars_-_by_John_Willams_9.xml", 0, motors[6], motors[7])
                    );
                case "this_game":
                    return await MidiPlayer.LoadTrackConfigs(
                        2,
                        new TrackConfig(@"assets\midi\This Game (1)_1.xml", 0, motors)
                        );
                case "the_incredibles":
                    return await MidiPlayer.LoadTrackConfigs(
                        1.25,
                        new TrackConfig(@"assets\midi\The_Incredibles_0.xml", -1, motors[0]),
                        new TrackConfig(@"assets\midi\The_Incredibles_1.xml", 0, motors[1]),
                        new TrackConfig(@"assets\midi\The_Incredibles_2.xml", 0, motors[2]),
                        new TrackConfig(@"assets\midi\The_Incredibles_3.xml", 0, motors[3]),
                        new TrackConfig(@"assets\midi\The_Incredibles_4.xml", 0, motors[4]),
                        new TrackConfig(@"assets\midi\The_Incredibles_5.xml", 0, motors[5]),
                        //new TrackConfig(@"assets\midi\The_Incredibles_6.xml", 0, motors[6]),
                        new TrackConfig(@"assets\midi\The_Incredibles_7.xml", 0, motors[6]),
                        //new TrackConfig(@"assets\midi\The_Incredibles_8.xml", 0, motors[8]),
                        new TrackConfig(@"assets\midi\The_Incredibles_9.xml", 0, motors[7])
                        );
            }

            return player;
        }

        private async void playButton_Click(object sender, RoutedEventArgs e)
        {
            await ThreadPool.RunAsync(async (s) =>
            {
                if (!player.IsPlaying)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        playButton.Content = "Stop";
                    });

                    player.Play();
                }
                else
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        playButton.Content = "Play";
                    });

                    player.Stop();
                }
            }, WorkItemPriority.High);
        }
    }
}
