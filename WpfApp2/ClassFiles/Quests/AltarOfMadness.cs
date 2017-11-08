using L2RBot.Common;
using L2RBot.Common.Enum;
using System;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    public class AltarOfMadness : Quest
    {
        //globals
        private Pixel[] _weeklyQuest;

        private QuestHelper _helper;

        private Difficulty _difficulty;

        private bool _finished;//using this because Complete is used to track and drive the bot

        private bool _startQuest;

        private Pixel[] _btnSpotRevive;

        private Pixel[] _completeOk;

        private Pixel[] _closeRechargeWindow;

        private Pixel[] _closeGadgetWindow;

        private Pixel[] _partyAccept;

        private Pixel[] _partyLeader;

        private bool _isLeader;

        private Pixel[] _partyFull;

        private bool _isPartyFull;

        private bool _riftMenuOpen;

        private Pixel _mapPoint;


        //properties
        public Pixel[] WeeklyQuest
        {
            get
            {
                if (_weeklyQuest == null)
                {
                    _weeklyQuest = new Pixel[4];
                }

                return _weeklyQuest;
            }
        }

        public QuestHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new QuestHelper(App) { Quest = QuestType.AltarOfMadness };
                }

                return _helper;
            }
        }

        public Difficulty DifficultyLevel
        {
            get
            {
                return _difficulty;
            }
        }

        public bool Finished
        {
            get
            {
                return _finished;
            }
            set
            {
                _finished = value;
            }
        }

        public bool StartQuest
        {
            get
            {
                return _startQuest;
            }
            set
            {
                _startQuest = value;
            }
        }

        public Pixel[] SpotRevive
        {
            get
            {
                if (_btnSpotRevive == null)
                {
                    _btnSpotRevive = new Pixel[2];
                }
                return _btnSpotRevive;
            }
            set
            {
                _btnSpotRevive = value;
            }
        }

        public Pixel[] CompleteOk
        {
            get
            {
                if (_completeOk == null)
                {
                    _completeOk = new Pixel[2];
                }
                return _completeOk;
            }
            set
            {
                _completeOk = value;
            }
        }

        public Pixel[] CloseRechargeWindow
        {
            get
            {
                if (_closeRechargeWindow == null)
                {
                    _closeRechargeWindow = new Pixel[3];
                }
                return _closeRechargeWindow;
            }
            set
            {
                _closeRechargeWindow = value;
            }
        }

        public Pixel[] CloseGadgetWindow
        {
            get
            {
                if (_closeGadgetWindow == null)
                {
                    _closeGadgetWindow = new Pixel[2];
                }
                return _closeGadgetWindow;
            }
            set
            {
                _closeGadgetWindow = value;
            }
        }

        public Pixel[] PartyAccept
        {
            get
            {
                if (_partyAccept == null)
                {
                    _partyAccept = new Pixel[2];
                }
                return _partyAccept;
            }
            set
            {
                _partyAccept = value;
            }
        }

        public Pixel[] PartyLeader
        {
            get
            {
                if (_partyLeader == null)
                {
                    _partyLeader = new Pixel[2];
                }
                return _partyLeader;
            }
            set
            {
                _partyLeader = value;
            }
        }

        public bool IsLeader
        {
            get
            {
                return _isLeader;
            }
            set
            {
                _isLeader = value;
            }
        }

        public Pixel[] PartyFull
        {
            get
            {
                if (_partyFull == null)
                {
                    _partyFull = new Pixel[2];
                }
                return _partyFull;
            }
            set
            {
                _partyFull = value;
            }
        }

        public bool IsPartyFull
        {
            get
            {
                return _isPartyFull;
            }
        }

        public bool RiftMenuOpen
        {
            get
            {
                return _riftMenuOpen;
            }
        }

        public Pixel MapPoint
        {
            get
            {
                if (_mapPoint == null)
                {
                    _mapPoint = new Pixel();
                }
                return _mapPoint;
            }
            set
            {
                _mapPoint = value;
            }
        }


        //constructors
        public AltarOfMadness(Process APP) : base(APP)
        {
            _difficulty = Difficulty.Easy;
            _finished = true;
            BuildPixels();
        }

        //logic
        /// <summary>
        /// initializes the valies of the pixel objects
        /// </summary>
        private void BuildPixels()
        {
            SpotRevive[0] = new Pixel
            {
                Color = Color.White,
                Point = new Point(1064, 417)
            };

            SpotRevive[1] = new Pixel
            {
                Color = Colors.SpotReviveBtn,
                Point = new Point(1088, 407)
            };

            CompleteOk[0] = new Pixel
            {
                Color = Colors.OffWhite,
                Point = new Point(955, 631)
            };

            CompleteOk[1] = new Pixel
            {
                Color = Colors.CompleteOk,
                Point = new Point(954, 612)
            };

            CloseRechargeWindow[0] = new Pixel
            {
                Color = Colors.CloseRechargeWindowX,
                Point = new Point(852, 223)
            };

            CloseRechargeWindow[1] = new Pixel // button bg
            {
                Color = Colors.SpotReviveBtn,
                Point = new Point(652, 480)
            };

            CloseRechargeWindow[2] = new Pixel // 1
            {
                Color = Colors.White,
                Point = new Point(716, 419)
            };

            CloseGadgetWindow[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(626, 480)
            };

            CloseGadgetWindow[1] = new Pixel
            {
                Color = Colors.CompleteOk,
                Point = new Point(632, 462)
            };

            PartyAccept[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(897, 45)
            };

            PartyAccept[1] = new Pixel
            {
                Color = Colors.CompleteOk,
                Point = new Point(911, 30)
            };

            PartyLeader[0] = new Pixel
            {
                Color = Color.FromArgb(255, 5, 88, 176),
                Point = new Point(10, 7)
            };

            PartyLeader[1] = new Pixel
            {
                Color = Color.FromArgb(91, 189, 252),
                Point = new Point(12, 10)
            };

            PartyFull[0] = new Pixel
            {
                Color = Color.FromArgb(255, 104, 0, 0),
                Point = new Point(57, 394)
            };

            PartyFull[1] = new Pixel
            {
                Color = Color.White,
                Point = new Point(229, 180)
            };

            MapPoint = new Pixel()
            {
                Color = Color.White,
                Point = new Point(1205, 90)
            };

        }

        /// <summary>
        /// Starts the Alter of Madness Quest logic
        /// </summary>
        public void Start()
        {
            UpdateScreen(); //updates Screen object for parent class with latest window size and x,y location.

            User32.SetForegroundWindow(App.MainWindowHandle);//brings window to front.
            Thread.Sleep(TimeSpan.FromSeconds(.5));

            if (_finished == true && _riftMenuOpen == false)
            {
                if (Bot.IsCombatScreenUp(App))
                {
                    OpenRiftMenu();
                }

            }

            if (_finished == true && _riftMenuOpen == true)
            {
                EnterDungeon();
            }

            if (Bot.IsCombatScreenUp(App) && _startQuest == false && _finished == false) //look for combat screen, starts Quest once it is detected.
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if (IsRechargeUp())//added it here because of timing issues
                {
                    Click(_closeRechargeWindow[0].Point);//closes recharge window if its present.
                    Thread.Sleep(TimeSpan.FromSeconds(.1));
                }
                _startQuest = true;
                Click(new Point(128, 370)); // clicks the quest go button
                _mapPoint.UpdateColor(Screen);
            }

            if (Bot.IsCombatScreenUp(App) && _startQuest == true && _finished == false)
            {
                if (TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds) > TimeSpan.FromSeconds(15))
                {
                    //checks if there has been any map movement in the last 15 seconds
                    if (_mapPoint.IsPresent(Screen, 0))
                    {
                        //resets quest status by clicking on the auto combat button 3 times 
                        //then changes _startQuest to false so i will start the quest again. 
                        Click(new Point(876, 684));
                        Thread.Sleep(TimeSpan.FromSeconds(.1));
                        Click(new Point(876, 684));
                        Thread.Sleep(TimeSpan.FromSeconds(.1));
                        Click(new Point(876, 684));
                        Thread.Sleep(TimeSpan.FromSeconds(.1));
                        _startQuest = false;
                    }
                    _mapPoint.UpdateColor(Screen);
                }
            }
            if (NeedRevived())
            {
                Click(SpotRevive[0].Point);
                _startQuest = false;
            }
            if (IsGadgetUp())
            {
                Click(_closeGadgetWindow[0].Point);
                Thread.Sleep(TimeSpan.FromSeconds(.1));
            }
            if (IsRechargeUp())
            {
                Click(_closeRechargeWindow[0].Point);//closes recharge window if its present
                Thread.Sleep(TimeSpan.FromSeconds(.1));
            }
            if (_partyAccept[0].IsPresent(Screen, 2) && _partyAccept[1].IsPresent(Screen, 2))
            {
                Click(PartyAccept[0].Point);
                Thread.Sleep(TimeSpan.FromSeconds(.1));
            }
            if (IsFinished())
            {
                DoAgain();
            }
            Helper.Start();

        }

        private bool NeedRevived()
        {
            if (_btnSpotRevive[0].IsPresent(Screen, 2) && _btnSpotRevive[1].IsPresent(Screen, 2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Wraps up the quest and prepares to start it again.
        /// </summary>
        private void DoAgain()
        {
            _finished = true;
            _startQuest = false;
            _riftMenuOpen = false;
            Click(_completeOk[0].Point);
        }



        /// <summary>
        /// clicks all buttons to navigate to the Alter of madness temporal rift.
        /// </summary>
        public void OpenRiftMenu()
        {
            UpdatePartyFullStatus();

            UpdateLeaderStatus();

            Click(Menus.BtnHamburger);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            Click(Menus.BtnDungeons);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Click(Menus.BtnSubTemporalRift); // selects temporal rift dungeon type, reuing point

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Click(Menus.BtnSubTemporalRift); //selects the alter of madness rift type, reusuing point

            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (DifficultyLevel == Difficulty.Easy)
            {
                Click(Menus.BtnSubNormalDungeon);//easy mode, reusing point

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            if (DifficultyLevel == Difficulty.Normal)
            {
                Click(Menus.BtnSubTemporalRift);//normal mode, reusing point

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            _riftMenuOpen = true;
            Thread.Sleep(TimeSpan.FromSeconds(.1));

        }


        /// <summary>
        /// checks for the OK button after the dungeon is finnished 
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            return (CompleteOk[0].IsPresent(Screen, 2) && CompleteOk[1].IsPresent(Screen, 2)) ? true : false;
        }

        /// <summary>
        /// looks for the recharge window that appears after you have completed the dungeon once
        /// </summary>
        /// <returns></returns>
        public bool IsRechargeUp()
        {
            return (CloseRechargeWindow[1].IsPresent(Screen, 3) && CloseRechargeWindow[2].IsPresent(Screen, 3)) ? true : false;
        }

        /// <summary>
        /// looks for the window that randomly pops up mentioning gadgets.
        /// </summary>
        /// <returns></returns>
        public bool IsGadgetUp()
        {
            return (CloseGadgetWindow[0].IsPresent(Screen, 2) && CloseGadgetWindow[1].IsPresent(Screen, 2)) ? true : false;
        }

        /// <summary>
        /// updates the party leader status by detecting the party leader crown on your characters name
        /// </summary>
        public void UpdateLeaderStatus()
        {
            if (Bot.IsCombatScreenUp(App))
            {
                _isLeader = (PartyLeader[0].IsPresent(Screen, 2) && PartyLeader[1].IsPresent(Screen, 2)) ? true : false;
                MainWindow.main.UpdateLog = App.MainWindowTitle + " has been detected as party leader.";
            }
        }

        /// <summary>
        /// detects wether you are party of a full party by looking for the 4th members HP bar
        /// </summary>
        public void UpdatePartyFullStatus()
        {
            if (Bot.IsCombatScreenUp(App))
            {
                Click(PartyFull[1].Point);
                Thread.Sleep(TimeSpan.FromSeconds(.1));
                _isPartyFull = (PartyFull[0].IsPresent(Screen, 2)) ? true : false;
            }
        }

        /// <summary>
        /// enters the dungeon based on leader and party status
        /// </summary>
        public void EnterDungeon()
        {
            if (_isLeader && _isPartyFull)
            {
                //this delay gives all party member bots time to get where they need to be, waits 30+ seconds from last click
                if (TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds) > TimeSpan.FromSeconds(32))
                {
                    Click(Menus.BtnRanking);//enter dungeon, reusing point
                    _finished = false;
                }
            }

            if (_isLeader && !IsPartyFull)
            {
                //this delay gives all party member bots time to get where they need to be, waits 30+ seconds from last click
                if (TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds) > TimeSpan.FromSeconds(32))
                {
                    Click(Menus.BtnTradingPost);//party matching, reusing point
                    _finished = false;
                }
            }
            if (!_isLeader && !IsPartyFull)
            {
                //wait 
                _finished = false;
            }
            if (!_isLeader && _isPartyFull)
            {
                //wait
                _finished = false;
            }
        }
    }

}

