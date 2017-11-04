using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    public class Quest
    {
        //globals
        private Process _app;

        private Stopwatch _timer;

        private Rectangle _screen;

        private bool _initialClick;

        private bool _complete;

        private int _idleTimeInMs;

        //properties
        public Process App//nox player
        {
            get
            {
                return _app;
            }
            set
            {
                _app = value;
            }
        }

        public Stopwatch Timer
        {
            get
            {
                return _timer;
            }
            set
            {
                _timer = value;
            }
        }//timer for tracking objects

        public Rectangle Screen
        {
            get
            {
                return _screen;
            }
            set
            {
                _screen = value;
            }
        }//game screen rectangle

        public bool InitialClick
        {
            get
            {
                return _initialClick;
            }
            set
            {
                _initialClick = value;
            }
        }//for tracking the very first click to start the quest

        public bool Complete
        {
            get
            {
                return _complete;
            }
            set
            {
                _complete = value;
            }
        }// for tracking quest completion

        public int IdleTimeInMs
        {
            get
            {
                return _idleTimeInMs;
            }
            set
            {
                _idleTimeInMs = value;
            }
        }//the duration of time in ms that has to pass between clicks before idle

        //constructor
        public Quest(Process APP)
        {
            App = APP;
            Timer = new Stopwatch();
            Screen = L2RBot.Screen.GetRect(App);
            InitialClick = false;
            Complete = false;
            _idleTimeInMs = 30000;
        }

        //logic
        /// <summary>
        /// Clicks the pixel's point and resets timer object.
        /// </summary>
        /// <param name="GamePoint"></param>
        public void Click(Point GamePoint)
        {
            Point screenPoint = L2RBot.Screen.PointToScreenPoint(Screen, GamePoint.X, GamePoint.Y); //Convert game point to screen point

            Mouse.LeftMouseClick(screenPoint.X, screenPoint.Y);//click screen point

            if (Timer.IsRunning)
            {
                ResetTimer();
            }
            StartTimer();
        }

        /// <summary>
        /// Calls the timers Start() method.
        /// </summary>
        public void StartTimer()
        {
            Timer.Start();
        }

        /// <summary>
        /// Calls the timers Stop() and Reset() methods
        /// </summary>
        public void ResetTimer()
        {
            Timer.Stop();
            Timer.Reset();
        }

        /// <summary>
        /// Updates the screen's rectangle locations and size
        /// </summary>
        public void UpdateScreen()
        {
            Screen = L2RBot.Screen.GetRect(App); //game window screen object(nox players screen location and demensions)
        }
    }
}
