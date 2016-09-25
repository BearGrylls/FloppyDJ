using Microsoft.IoT.Lightning.Providers;
using Windows.Devices.Gpio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Devices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FloppyDJ
{
    public sealed partial class MainPage : Page
    {
        MidiPlayer player;
        List<StepperMotor> motors;
        ObservableCollection<string> songs;
        List<Button> testButtons;
        List<Slider> sliders;

        public MainPage()
        {
            this.InitializeComponent();
            statusTextBlock.Text = "";
            playButton.IsEnabled = false;
            testButtons = new List<Button>();
            sliders = new List<Slider>();

            if (Microsoft.IoT.Lightning.Providers.LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            }
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

        private void resetPlayer()
        {
            if (player != null)
            {
                player.Stop();
            }

            player = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            long tps = TimeSpan.TicksPerSecond; //getTps();

            motors = new List<StepperMotor>()
            {
                new StepperMotor(3, 2, tps),
                new StepperMotor(17, 4, tps),
                new StepperMotor(22, 27, tps),
                new StepperMotor(9, 10, tps),
                new StepperMotor(5, 11, tps),
                new StepperMotor(13, 6, tps),
                new StepperMotor(26, 19, tps),
                new StepperMotor(21, 20, tps),
            };
            
            //motorListView.ItemsSource = motors;
            
            int count = 0;
            foreach(StepperMotor motor in motors)
            {
                motor.Name = "Motor " + (++count);
                motor.OctaveOffset = -1;    // adjust octave

                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };

                var textblock = new TextBlock()
                {
                    Text = motor.Name,
                    Margin = new Thickness(10),
                    VerticalAlignment = VerticalAlignment.Center
                };

                var testButton = new Button()
                {
                    Content = "Test",
                    Margin = new Thickness(10)
                };
                testButton.Click += async (s, args) =>
                {
                    await ThreadPool.RunAsync(async (s1) =>
                    {
                        resetPlayer();
                        MidiPlayer scalePlayer = await MidiPlayer.LoadConfig("Scales", new StepperMotor[] { motor });
                        scalePlayer.Play();
                    }, WorkItemPriority.Normal);
                };
                testButtons.Add(testButton);

                var octaveSlider = new Slider()
                {
                    Maximum = 5,
                    Minimum = -5,
                    Value = motor.OctaveOffset,
                    Margin = new Thickness(10),
                    Width = 150
                };
                octaveSlider.ValueChanged += (s, args) =>
                {
                    motor.OctaveOffset = (int)octaveSlider.Value;
                };
                sliders.Add(octaveSlider);

                stackPanel.Children.Add(textblock);
                stackPanel.Children.Add(testButton);
                stackPanel.Children.Add(octaveSlider);

                driveStackPanel.Children.Add(stackPanel);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ThreadPool.RunAsync((s) =>
                {
                    motor.Reset();
                }, WorkItemPriority.High);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            songs = new ObservableCollection<string>()
            {
                "Hyrule Temple",
                "Trainer Battle",
                "Pokemon",
                "Decisive Battle",
                "What is Love",
                "Pentatonix - Save the World / Don't You Worry Child",
                "Take On Me",
                "Jurassic Park",
                "Take On Me (2)",
                "Billie Jean",
                "Star Wars Medley",
                "Supreme Star Wars Medley",
                "Star Wars",
                "Star Wars (Hexabass)",
                "Star Wars (Trombone)",
                "John Williams - Star Wars",
                "The Incredibles",
                "Attack on Titan - Guren No Yumiya"
            };

            songListView.ItemsSource = songs;
        }
        
        public async Task<MidiPlayer> LoadSong(string song)
        {
            MidiPlayer player = await MidiPlayer.LoadConfig(song, motors.ToArray());
            if (player != null) return player;

            switch (song)
            {
                case "Attack on Titan - Guren No Yumiya":
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
                        new TrackConfig(@"assets\midi\This Game (1)_1.xml", 0, motors.ToArray())
                    );
                case "The Incredibles":
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
            if (player == null) return;

            await ThreadPool.RunAsync(async (s) =>
            {
                if (!player.IsPlaying)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        playButtonTextBlock.Text = "\xE71A";
                        statusTextBlock.Text = "Playing";
                        enableControls(false);
                    });

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    ThreadPool.RunAsync((s1) =>
                    {
                        player.Play();
                    }, WorkItemPriority.High);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
                else
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        playButtonTextBlock.Text = "\xE768";
                        statusTextBlock.Text = "Stopped";
                        enableControls(true);
                    });

                    player.Stop();
                }
            }, WorkItemPriority.High);
        }

        private async void enableControls(bool isEnabled)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                songListView.IsEnabled = resetAllButton.IsEnabled = isEnabled;
                foreach (Button button in testButtons)
                {
                    button.IsEnabled = isEnabled;
                }
            });
        }

        private void resetAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(StepperMotor motor in motors)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ThreadPool.RunAsync((s) =>
                {
                    motor.Reset();
                }, WorkItemPriority.High);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            foreach(Slider slider in sliders)
            {
                slider.Value = -1;
            }
        }

        private async void songListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var songName = songListView.SelectedItem as string;

            if (songName != null)
            {
                resetPlayer();
                player = await LoadSong(songName);
                statusTextBlock.Text = (player != null) ? "Loaded: " + songName : "";
                playButton.IsEnabled = true;
            }
        }
    }
}
