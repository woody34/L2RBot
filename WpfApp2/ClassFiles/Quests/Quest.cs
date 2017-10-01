using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    class Quest
    {
        public Process App; //nox player

        private Stopwatch _timer = new Stopwatch(); //timer for tracking objects

        public Rectangle Screen; //game screen rectangle

        public bool InitialClick = false; //for tracking the very first click to start the quest

        public bool Complete = false;

        //public int TimeoutInMilliS; //used to drive the first click //1 min.

        public Quest(Process APP)
        {
            App = APP;
        }

        public void Click(Point GamePoint)
        {
            Point screenPoint = L2RBot.Screen.PointToScreenPoint(Screen, GamePoint.X, GamePoint.Y); //Convert game Point to Screen Point

            Mouse.LeftMouseClick(screenPoint.X, screenPoint.Y);//click screen point

            if (_timer.IsRunning)
            {
                ResetTimer();
            }
            StartTimer();
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public void ResetTimer()
        {
            _timer.Stop();
            _timer.Reset();
        }

        public void UpdateScreen()
        {
            if (App == null)
            {
                Debug.WriteLine("Process is null");
            }
            else
            {
                Screen = L2RBot.Screen.GetRect(App); //game window screen object(nox players screen location and demensions)
            }

        }
    }
}
