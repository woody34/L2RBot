using System;
using System.Diagnostics;
using L2RBot.Common.Enum;
using System.Drawing;
using L2RBot.Common;
using log4net;

namespace L2RBot
{
    class TowerOfInsolence : Quest
    {
        //globals
        private static readonly ILog log = LogManager.GetLogger(typeof(DailyDungeon));

        private bool _dungeonInProgress;

        Color _green;

        private Pixel[] _hamburger;//The hamburger navigation button on the top right of game screen.

        private Pixel[] _dungeon;//The dungeon subnavigation button.

        private Pixel[] _enter;//The Green 'Enter' button for starting the dungeon.

        private Pixel[] _difficulty;//The seven boxes that indicates difficulty.

        private Pixel[] _finishedOk;

        private Pixel[] _findDungeon;

        private Pixel[] _autoClear;

        private Pixel _tooHard;

        private Point _nextFloor;

        //properties
        public Pixel[] Hamburger
        {
            get
            {
                if (_hamburger == null)
                {
                    _hamburger = new Pixel[2];
                }

                return _hamburger;
            }
            set
            {
                _hamburger = value;
            }
        }

        public Pixel[] Dungeon
        {
            get
            {
                if (_dungeon == null)
                {
                    _dungeon = new Pixel[2];
                }

                return _dungeon;
            }
            set
            {
                _dungeon = value;
            }
        }

        public Pixel[] Enter
        {
            get
            {
                if (_enter == null)
                {
                    _enter = new Pixel[2];
                }

                return _enter;
            }
            set
            {
                _enter = value;
            }
        }

        public Pixel[] Difficulty
        {
            get
            {
                if (_difficulty == null)
                {
                    _difficulty = new Pixel[7];
                }

                return _difficulty;
            }
            set
            {
                _difficulty = value;
            }
        }

        public Pixel[] FinishedOk
        {
            get
            {
                if (_finishedOk == null)
                {
                    _finishedOk = new Pixel[2];
                }

                return _finishedOk;
            }
            set
            {
                _finishedOk = value;
            }
        }

        public Pixel[] FindDungeon
        {
            get
            {
                if (_findDungeon == null)
                {
                    _findDungeon = new Pixel[2];
                }

                return _findDungeon;
            }
            set
            {
                _findDungeon = value;
            }
        }

        public Pixel[] AutoClear
        {
            get
            {
                if (_autoClear == null)
                {
                    _autoClear = new Pixel[2];
                }

                return _autoClear;
            }
            set
            {
                _autoClear = value;
            }
        }

        public Point[] DungeonOptions = new Point[4];

        public Pixel TooHard
        {
            get
            {
                return _tooHard;
            }
            set
            {
                _tooHard = value;
            }
        }


        //constructors
        public TowerOfInsolence(Process App) : base(App)
        {
            BuildHelper();

            Timer.Start();

            IdleTimeInMs = 60000;

            BuildPixels();

            BuildPoints();

            _green = Color.FromArgb(255, 80, 170, 30);

            _dungeonInProgress = false;
        }

        private void BuildHelper()
        {
            Helper = new QuestHelper(App)
            {
                Quest = QuestType.TOI,

                Deathcount = this.Deathcount,

                Respawn = this.Respawn,

                CloseTVPopup = this.CloseTVPopup
            };
        }

        private void BuildPixels()
        {
            Hamburger[0] = new Pixel
            {
                Color = Colors.Navi, //Color.FromArgb(255, 223, 226, 224);

                Point = new Point(917, 21)//Top bun/center of the hamburger button.
            };

            Hamburger[1] = new Pixel
            {
                Color = Colors.Navi,

                Point = new Point(914, 10)//Above the HAmburger button.
            };

            Dungeon[0] = new Pixel
            {
                Color = Color.FromArgb(255, 186, 186, 187),

                Point = new Point(239, 667)//Center of the Dungeon button.
            };

            Dungeon[1] = new Pixel
            {
                Color = Color.FromArgb(255, 186, 186, 187),//!Color

                Point = new Point(240, 680)//Blank space between the icon and text of Dungeon button.
            };

            Enter[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 248, 237),

                Point = new Point(1027, 642)//Top of the E of the 'Enter' button.
            };

            Enter[1] = new Pixel
            {
                Color = Color.FromArgb(255, 81, 126, 85),

                Point = new Point(1025, 626)//The green BG of the'Enter' button.
            };

            Difficulty[0] = new Pixel
            {
                Color = Color.White,

                Point = new Point(874, 252)//The first of the seven difficulty squares.
            };//Only using 1 square atm.

            FinishedOk[0] = new Pixel
            {
                Color = Colors.OffWhite,

                Point = new Point(855, 631)//The O on the 'OK' button when finished.
            };

            FinishedOk[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(1023, 611)//The blue BG of the 'OK' button.
            };

            FindDungeon[0] = new Pixel
            {
                Color = Color.FromArgb(255, 54, 45, 35),

                Point = new Point(431, 377)//The center of the TOI graphic
            };

            FindDungeon[1] = new Pixel
            {
                Color = Color.FromArgb(255, 181, 0, 20),

                Point = new Point(541, 213)//The red dot of TOI.
            };

            AutoClear[0] = new Pixel
            {
                Color = Color.White,

                Point = new Point(728, 646)//The red dot of TOI.
            };

            AutoClear[1] = new Pixel
            {
                Color = Color.FromArgb(255, 41, 51, 68),

                Point = new Point(757, 627)//The red dot of TOI.
            };

            TooHard = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(708, 500)
            };
        }

        private void BuildPoints()
        {
            _nextFloor = new Point(1070, 631);
        }
        //logic
        public void Start()
        {
            UpdateScreen();

            //need to look into halting code until this User32 method has returned.
            User32.SetForegroundWindow(App.MainWindowHandle);//This is slow and causes timing issues.

            Sleep(500);//A short sleep before actions prevents timing issues.

            if (!_dungeonInProgress)
            {
                OpenHamburger();

                OpenDungeon();
            }

            if (_dungeonInProgress)
            {
                Helper.Start();
            }

            if (Enter[0].IsPresent(Screen, 2) && Enter[0].IsPresent(Screen, 2))
            {
                CheckDifficulty();

                _dungeonInProgress = true;
            }

            FindTheDungeon();

            if (TooHard.IsPresent(Screen, 3))
            {
                Click(Nav.MapClose);//clicks outside of box to close it.

                Sleep(500);

                Click(FinishedOk[0].Point);

                Sleep(500);
            }

            Finished();

            Sleep(500);//A short sleep after actions prevents timing issues.
        }

        private void CheckDifficulty()
        {
            Difficulty[0].UpdateColor(Screen);

            if (Difficulty[0].Color.Equals(_green))//Look for green dificulty box.
            {
                EnterDungeon();
            }

            if (!Difficulty[0].Color.Equals(_green))//Complete quest if it is yellow/red.
            {
                if (AutoClear[0].IsPresent(Screen, 3) && AutoClear[1].IsPresent(Screen, 3))
                {
                    Click(AutoClear[0].Point);

                    Sleep(300);

                    Click(Nav.Map);
                }

                Sleep(500);

                CloseDungeon();
            }
        }

        private void OpenHamburger()
        {
            log.Info(App.MainWindowTitle + " is looking for the Hamburger button.");
            if (Bot.IsCombatScreenUp(App) && Hamburger[0].IsPresent(Screen, 15) && !Hamburger[1].IsPresent(Screen, 3))
            {
                Click(Hamburger[0].Point);
            }
        }

        private void OpenDungeon()
        {
            log.Info(App.MainWindowTitle + " is looking for the Dungeon button.");
            if (Dungeon[0].IsPresent(Screen, 15) && !Dungeon[1].IsPresent(Screen, 2))
            {
                Click(Dungeon[0].Point);
            }
        }

        private void FindTheDungeon()
        {
            if (FindDungeon[0].IsPresent(Screen, 3))
            {
                Click(FindDungeon[0].Point);
            }
        }

        private void EnterDungeon()
        {
            Click(Enter[0].Point);

            log.Info(App.MainWindowTitle + " is entering TOI.");
        }

        private void Finished()
        {
            if (FinishedOk[0].IsPresent(Screen, 2) && FinishedOk[1].IsPresent(Screen, 2))
            {
                NextFloor();

                log.Info(App.MainWindowTitle + " has cleared a floor.");
            }
        }

        private void NextFloor()
        {
            Click(_nextFloor);
        }

        private void CloseDungeon()
        {
            Click(Nav.MapClose);//Close out of TOI menu

            _dungeonInProgress = false;

            Complete = true;

            MainLog(App.MainWindowTitle + " has completed TOI.");

            log.Info(App.MainWindowTitle + " has completed TOI, Closing Window.");
        }
    }

}


