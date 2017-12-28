using System;
using System.Diagnostics;
using L2RBot.Common.Enum;
using System.Drawing;
using L2RBot.Common;

namespace L2RBot
{
    class ExpDungeon : Quest
    {
        //globals
        private bool _dungeonInProgress;

        Color _green;

        private Pixel[] _hamburger;//The hamburger navigation button on the top right of game screen.

        private Pixel[] _dungeon;//The dungeon subnavigation button.

        private Pixel[] _enter;//The Green 'Enter' button for starting the dungeon.

        private Pixel[] _difficulty;//The seven boxes that indicates difficulty.

        private Pixel[] _finishedOk;

        private Pixel[] _recharge;

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

        public Pixel[] Recharge
        {
            get
            {
                if (_recharge == null)
                {
                    _recharge = new Pixel[2];
                }

                return _recharge;
            }
            set
            {
                _recharge = value;
            }
        }

        public Point[] DungeonOptions = new Point[4];

        //constructors
        public ExpDungeon(Process App, L2RDevice AdbApp) : base(App, AdbApp)
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
            Helper = new QuestHelper(App, AdbApp)
            {
                Quest = QuestType.Dungeon,

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
                Color = Color.FromArgb(255, 251, 251, 251),

                Point = new Point(1041, 598)//Center of the E of the 'Enter' button.
            };

            Enter[1] = new Pixel
            {
                Color = Color.FromArgb(255, 186, 186, 187),

                Point = new Point(1032, 592)//The green BG of the'Enter' button.
            };

            Difficulty[0] = new Pixel
            {
                Color = Color.White,

                Point = new Point(615, 418)//The first of the seven difficulty squares.
            };//Only using 1 square atm.

            FinishedOk[0] = new Pixel
            {
                Color = Colors.OffWhite,

                Point = new Point(957, 633)//The O on the 'OK' button when finished.
            };

            FinishedOk[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(935, 612)//The blue BG of the 'OK' button.
            };

            Recharge[0] = new Pixel
            {
                Color = Colors.OffWhite,

                Point = new Point(974, 599)//The R on the 'Recharge'.
            };

            Recharge[1] = new Pixel
            {
                Color = Color.FromArgb(255, 80, 125, 85),

                Point = new Point(979, 580)//The Green BG of the 'Recharge' button.
            };

        }

        private void BuildPoints()
        {
            DungeonOptions[0] = new Point(203, 251);//Easy

            DungeonOptions[1] = new Point(203, 349);//Normal

            DungeonOptions[2] = new Point(203, 436);//Hard

            DungeonOptions[3] = new Point(203, 522);//Very Hard
        }

        //logic
        public void Start()
        {
            UpdateScreen();

            //need to look into halting code until this User32 method has returned.
            if (BringToFront == true)
            {
                BringWindowToFront();
            }

            Sleep();//A short sleep before actions prevents timing issues.

            if (!_dungeonInProgress)
            {
                OpenHamburger();

                OpenDungeon();
            }

            if (Enter[0].IsPresent(Screen, 2) && Enter[0].IsPresent(Screen, 2))
            {
                ChoseDifficulty();
            }

            Finished();

            IsRecharge();

            Sleep();//A short sleep after actions prevents timing issues.
        }

        private void OpenHamburger()
        {
            if (IsCombatScreenUp() && Hamburger[0].IsPresent(Screen, 2) && !Hamburger[1].IsPresent(Screen, 2))
            {
                Click(Hamburger[0].Point);
            }
        }

        private void OpenDungeon()
        {
            if (Dungeon[0].IsPresent(Screen, 2) && !Dungeon[1].IsPresent(Screen, 2))
            {
                Click(Hamburger[0].Point);
            }
        }

        private void ChoseDifficulty()
        {
            Click(DungeonOptions[3]);//Very Hard.

            Difficulty[0].UpdateColor(Screen);

            if(Object.Equals(Difficulty[0].Color, _green))
            {
                Click(Enter[0].Point);
            }

            Click(DungeonOptions[2]);//Hard.

            Difficulty[0].UpdateColor(Screen);

            if (Object.Equals(Difficulty[0].Color, _green))
            {
                Click(Enter[0].Point);
            }

            Click(DungeonOptions[1]);//Normal

            Difficulty[0].UpdateColor(Screen);

            if (Object.Equals(Difficulty[0].Color, _green))
            {
                Click(Enter[0].Point);
            }

            Click(DungeonOptions[0]);//Easy

            Difficulty[0].UpdateColor(Screen);

            if (Object.Equals(Difficulty[0].Color, _green))
            {
                Click(Enter[0].Point);
            }

            _dungeonInProgress = true;
        }

        private void Finished()
        {
            if(FinishedOk[0].IsPresent(Screen, 2) && FinishedOk[1].IsPresent(Screen, 2))
            {
                Click(FinishedOk[0].Point);
            }

            _dungeonInProgress = false;
        }

        private void IsRecharge()
        {
            if (Recharge[0].IsPresent(Screen, 2))
            {
                Complete = true;
                //at some point I can add options to auto recharge according to preferences.
            }

        }

    }

}

