using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using log4net;
using L2RBot.Common;
using L2RBot.Common.Enum;

namespace L2RBot
{
    public class Quest
    {
        //globals
        private Screen _screenObj = new L2RBot.Screen();

        private Stopwatch _timer;

        private int? _sleepTime;

        //log object
        private static readonly ILog log = LogManager.GetLogger(typeof(Quest));

        //properties
        public int SleepTime//global to store thread sleep time in ms
        {
            get
            {
                if (_sleepTime == null)
                {
                    _sleepTime = 100;
                }
                return (int) _sleepTime;
            }
            set
            {
                _sleepTime = value;
            }
        }

        public Process App { get; set; }

        public L2RDevice AdbApp { get; set; }

        public QuestHelper Helper { get; set; }

        public Stopwatch Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Stopwatch();
                }
                return _timer;
            }
            set
            {
                _timer = value;
            }
        }//timer for tracking objects

        public dynamic Screen { get; set; }//game screen rectangle

        public bool InitialClick { get; set; }//for tracking the very first click to start the quest

        public bool Complete { get; set; }// for tracking quest completion

        public int IdleTimeInMs { get; set; }//the duration of time in ms that has to pass between clicks before idle

        public bool Respawn { get; set; }

        public uint Deathcount { get; set; }

        public bool BringToFront { get; set; }

        public bool HomePosition { get; set; }

        public bool CloseTVPopup { get; set; }

        public string BotName { get; set; }

        public bool IsAdbBot { get; set; }

        public IntPtr BotWindowHandle { get; set; }

        public Pixel WifiLogo;

        public Screen ScreenObj
        {
            get
            {
                return _screenObj;
            }
            set
            {
                _screenObj = value;
            }
        }

        //constructor
        public Quest(Process App, L2RDevice AdbApp)
        {
            this.App = App;

            this.AdbApp = AdbApp;

            if (App != null)
            {
                IsAdbBot = false;

                BotName = App.MainWindowTitle;

                BotWindowHandle = App.MainWindowHandle;

                UpdateScreen();
            }

            if (AdbApp != null)
            {
                IsAdbBot = true;

                BotName = AdbApp.CharacterName;
            }

            Timer = new Stopwatch();

            InitialClick = false;

            Complete = false;

            IdleTimeInMs = 30000;

            WifiLogo = new Pixel
            {
                Color = Colors.WifiLogo,

                Point = new Point(8, 708)
            };

            UpdateScreen();
        }

        //logic

        /// <summary>
        /// Updates Screen object's Rectangle and Point.
        /// </summary>
        public void UpdateScreen()
        {
            if (IsAdbBot)
            {
                Image test = AdbApp.ScreenCap();
                Screen = new Bitmap(AdbApp.ScreenCap());

                return;
            }

            log.Info("Updated Screen object for " + BotName);

            Screen = ScreenObj.GetRect(App);
        }

        /// <summary>
        /// Updates Screen object's Rectangle and Point.
        /// </summary>
        /// <param name="App"></param>
        public void UpdateScreen(Process App)
        {
            if (IsAdbBot)
            {
                Screen = new Bitmap(AdbApp.ScreenCap());

                return;
            }
            log.Info("Updated Screen object for " + BotName);

            Screen = ScreenObj.GetRect(App);
        }

        /// <summary>
        /// Clicks the pixel's point and resets timer object.
        /// </summary>
        /// <param name="GamePoint"></param>
        public void Click(Point GamePoint)
        {
            log.Info("Clicking Point " + GamePoint.ToString());

            if (Timer.IsRunning)
            {
                ResetTimer();
            }

            StartTimer();

            //ADB bot click
            if (IsAdbBot)
            {
                AdbApp.Click(GamePoint);

                return;
            }

            if(Screen.GetType() == typeof(Rectangle))
            {
                Point screenPoint = ScreenObj.PointToScreenPoint(Screen, GamePoint.X, GamePoint.Y); //Convert game point to screen point

                Mouse.LeftMouseClick(screenPoint.X, screenPoint.Y);//click screen point
            }
        }

        /// <summary>
        /// Calls the timers Start() method.
        /// </summary>
        public void StartTimer()
        {
            log.Info("Starting Timer for " + BotName);

            Timer.Start();
        }

        /// <summary>
        /// Calls the timers Stop() and Reset() methods
        /// </summary>
        public void ResetTimer()
        {
            log.Info("Reseting Timer for " + BotName);

            Timer.Stop();

            Timer.Reset();
        }

        /// <summary>
        /// Checks to see if QuestHelper object needs to complete the Quest
        /// </summary>
        public void IsHelperComplete()
        {
            if (Helper.Complete == true)
            {
                log.Info("Helper Condition has Completed " + BotName);

                MainLog(BotName + " has stopped because it needs help from a Human.");

                Complete = true;
            }
        }

        /// <summary>
        /// Sleeps the thread for SleepTime's value in milliseconds
        /// </summary>
        public void Sleep()
        {
            log.Info("Sleeping thread " + BotName +
                    " for " + SleepTime + " Milliseconds.");

            Thread.Sleep(SleepTime);
        }

        /// <summary>
        /// Sleeps the thread for however many milliseconds are passed to it.
        /// </summary>
        /// <param name="SleepMS">The ammount of time, in milliseconds, to sleep the thread.</param>
        public void Sleep(int SleepMS)
        {
            log.Info("Sleeping thread " + BotName +
            " for " + SleepMS + " Milliseconds.");

            Thread.Sleep(SleepMS);
        }

        /// <summary>
        /// Writes a log entry to the MainWindow's log.
        /// </summary>
        /// <param name="Text">Log message.</param>
        public void MainLog(string Text, bool Debug = true)
        {
            if (Debug == true)
            {
                log.Info(BotName + " has written " +
                Text + " to MainWindow's Log.");

                MainWindow.main.UpdateLog = Text;
            }

        }

        /// <summary>
        /// Brings game window to the front
        /// </summary>
        public void BringWindowToFront()
        {
            if (BringToFront && !IsAdbBot)
            {
                log.Info(BotName + " window brought to the front.");

                User32.SetForegroundWindow(BotWindowHandle);
            }
        }

        /// <summary>
        /// Toggles Auto combat states twice(on then off | off then on.) End state varies depending on unknown start state.
        /// </summary>
        public void ToggleCombat()
        {
            log.Info(BotName + " is toggling combat");

            Click(Nav.AutoCombat);

            Thread.Sleep(TimeSpan.FromSeconds(1));//If you click fast it will just see a single click.

            Click(Nav.AutoCombat);

            Thread.Sleep(50);
        }

        public Boolean IsCombatScreenUp()
        {
            return (WifiLogo.IsPresent(Screen, 4)) ? true : false;
        }
    }
}
