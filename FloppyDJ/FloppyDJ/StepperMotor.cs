using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace FloppyDJ
{
    public enum SpinDirection
    {
        Clockwise,
        CounterClockwise
    }

    public class StepperMotor
    {
        public GpioPin StepPin, DirectionPin;
        public double Speed;   // Speed is in steps per second
        public long DirChangeSteps = 50;

        public SpinDirection Direction
        {
            get;
            private set;
        }

        public bool AutoChangeDirection = true;

        private Stopwatch stopwatch;
        private long ticksPerSecond;

        private long stepsTaken = 0;

        private long last;

        public StepperMotor(int stepPinNumber, int dirPinNumber, long ticksPerSecond)
        {
            GpioController controller = GpioController.GetDefault();
            StepPin = controller.OpenPin(stepPinNumber);
            DirectionPin = controller.OpenPin(dirPinNumber);

            StepPin.SetDriveMode(GpioPinDriveMode.Output);
            DirectionPin.SetDriveMode(GpioPinDriveMode.Output);

            SetDirection(SpinDirection.Clockwise);

            this.ticksPerSecond = ticksPerSecond;

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void SetDirection(SpinDirection direction)
        {
            Direction = direction;
            switch(direction)
            {
                case SpinDirection.Clockwise:
                    DirectionPin.Write(GpioPinValue.High);
                    break;
                case SpinDirection.CounterClockwise:
                    DirectionPin.Write(GpioPinValue.Low);
                    break;
            }
        }

        public void Reset()
        {
            AutoChangeDirection = false;
            SetDirection(SpinDirection.Clockwise);
            Speed = 100;
            int steps = 0;
            while (steps < 100)
            {
                if (Run())
                {
                    steps++;
                }
            }

            Speed = 0;
            AutoChangeDirection = true;
            SetDirection(SpinDirection.CounterClockwise);
            stepsTaken = 0;
        }

        public void Step()
        {
            StepPin.Write(GpioPinValue.High);
            StepPin.Write(GpioPinValue.Low);

            stepsTaken++;
        }

        public void ChangeDirection()
        {
            SetDirection((Direction == SpinDirection.Clockwise) ? SpinDirection.CounterClockwise : SpinDirection.Clockwise);
        }

        public bool Run()
        {
            if(Speed == 0)
            {
                return false;
            }
            else if (stopwatch.ElapsedTicks - last > (1.0 / Speed) * ticksPerSecond)
            {
                Step();
                last = stopwatch.ElapsedTicks;

                if(AutoChangeDirection && stepsTaken > DirChangeSteps)
                {
                    ChangeDirection();
                    stepsTaken = 0;
                }

                return true;
            }

            return false;
        }
    }
}
