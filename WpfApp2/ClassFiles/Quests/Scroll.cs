using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using L2RBot.Common.Enum;
using L2RBot.Common;
using System.Collections.Generic;
using static L2RBot.Screen;
using log4net;

namespace L2RBot
{
    class Scroll : Quest
    {
        //Globals

        private Pixel _scrollSearch;

        private Pixel[] _items;

        private Pixel[] _fulfill;

        private Pixel[] _fulfillOk;

        private Pixel[] _scrollQuestMenu;

        private Pixel _scroll;

        private Pixel _checkComplete;

        private List<Grade> _preference;

        private bool _iniClick;

        private bool _reset = false;

        //Log object
        private static readonly ILog log = LogManager.GetLogger(typeof(Scroll));

        //Properties
        public Pixel ScrollSearch
        {
            get
            {
                if (_scrollSearch == null)
                {
                    _scrollSearch = new Pixel();
                }
                return _scrollSearch;
            }
        }

        public Pixel[] Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Pixel[10];
                }
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public Pixel[] Fulfill
        {
            get
            {
                if (_fulfill == null)
                {
                    _fulfill = new Pixel[2];
                }
                return _fulfill;
            }
            set
            {
                _fulfill = value;
            }
        }

        public Pixel[] FulfillOk
        {
            get
            {
                if (_fulfillOk == null)
                {
                    _fulfillOk = new Pixel[2];
                }
                return _fulfillOk;
            }
            set
            {
                _fulfillOk = value;
            }
        }

        public Pixel[] ScrollQuestMenu
        {
            get
            {
                if (_scrollQuestMenu == null)
                {
                    _scrollQuestMenu = new Pixel[2];
                }
                return _scrollQuestMenu;
            }
            set
            {
                _scrollQuestMenu = value;
            }
        }

        public Pixel _Scroll
        {
            get
            {
                if (_scroll == null)
                {
                    _scroll = new Pixel();
                }
                return _scroll;
            }
            set
            {
                _scroll = value;
            }
        }

        public List<Grade> Preference
        {
            get
            {
                if (_preference == null)
                {
                    _preference = new List<Grade>();
                }
                return _preference;
            }

            set
            {
                _preference = value;
            }
        }

        public Pixel CheckComplete
        {
            get
            {
                if (_checkComplete == null)
                {
                    _checkComplete = new Pixel();
                }
                return _checkComplete;
            }
            set
            {
                _checkComplete = value;
            }
        }

        public bool Reset
        {
            get
            {
                return _reset;
            }
            set
            {
                _reset = value;
            }
        }

        //Constructors
        public Scroll(Process App, L2RDevice AdbApp) : base(App, AdbApp)
        {
            Timer.Start();

            Helper = new QuestHelper(App, AdbApp)
            {
                Quest = QuestType.Scroll,

                Deathcount = this.Deathcount,

                Respawn = this.Respawn,

                CloseTVPopup = this.CloseTVPopup
            };

            BuildPixels();

            BuildFulfill();

            BuildScrollQuestMenu();

            _iniClick = false;

            IdleTimeInMs = 60000;

            //testing purposes

        }

        private void BuildPixels()
        {
            Items[0] = new Pixel()
            {
                Point = new Point(735, 238),
                Color = Colors.White
            };
            Items[1] = new Pixel()
            {
                Point = new Point(855, 238),
                Color = Colors.White
            };
            Items[2] = new Pixel()
            {
                Point = new Point(975, 238),
                Color = Colors.White
            };
            Items[3] = new Pixel()
            {
                Point = new Point(1095, 238),
                Color = Colors.White
            };
            Items[4] = new Pixel()
            {
                Point = new Point(1215, 238),
                Color = Colors.White
            };
            Items[5] = new Pixel()
            {
                Point = new Point(735, 358),
                Color = Colors.White
            };
            Items[6] = new Pixel()
            {
                Point = new Point(855, 358),
                Color = Colors.White
            };
            Items[7] = new Pixel()
            {
                Point = new Point(975, 358),
                Color = Colors.White
            };
            Items[8] = new Pixel()
            {
                Point = new Point(1095, 358),
                Color = Colors.White
            };
            Items[9] = new Pixel()
            {
                Point = new Point(1215, 358),
                Color = Colors.White
            };

            CheckComplete = new Pixel
            {
                Point = new Point(996, 226),
                Color = Color.FromArgb(255, 255, 152, 176)
            };
        }

        private void BuildFulfill()
        {
            Fulfill[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(1062, 667)//The white F in the 'Fulfill Request' button.
            };

            Fulfill[1] = new Pixel
            {
                Color = Colors.CompleteOk,
                Point = new Point(1120, 648)//The blue BG of the 'Fulfill Request button.
            };

            FulfillOk[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(750, 486)//The white O on the 'OK' button.
            };

            FulfillOk[1] = new Pixel
            {
                Color = Colors.CompleteOk,
                Point = new Point(754, 472)//The blue BG of the 'OK' button.
            };
        }

        private void BuildScrollQuestMenu()
        {
            ScrollQuestMenu[0] = new Pixel
            {
                Color = Colors.ScrollQuestMenuX,

                Point = new Point(906, 117)//The grey X that you press to Exit the scroll quest menu.
            };

            ScrollQuestMenu[1] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(730, 599)//The white F on the 'Fulfill Request' button.
            };
        }

        //Logic    
        public void Start()
        {
            log.Info(BotName + " calls Scroll.Start()");

            UpdateScreen();

            if (BringToFront == true)
            {
                BringWindowToFront();
            }

            Sleep();//Sleep before to prevent actions from occuring to early.

            if (_iniClick == false)
            {
                if (IsCombatScreenUp())
                {
                    OpenBag();

                    FindScroll();

                    FulfillRequest();

                    _iniClick = true;
                }
                if (IsCombatScreenUp())
                {
                    Helper.ClosePopUps();
                }
            }

            IdleCheck();

            Helper.Start();

            IsHelperComplete();

            Sleep();//Sleep after to prevent actions from occuring on the next active window.

            log.Info(BotName + " Scroll.Start() ends.");
        }

        /// <summary>
        /// Checks the Quest pane for the 'Scroll/Sub Quest.'
        /// </summary>
        /// <returns></returns>
        private bool GrabScrollPoint()
        {
            log.Info(BotName + " Looking for Scroll Quest point on game screen.");

            Pixel _temp = L2RBot.Screen.SearchPixelVerticalStride(Screen, new Point(13, 210), 211, Colors.ScrollQuest, out bool Found, 2);

            if (Found)
            {
                log.Info(BotName + " Found Scroll Quest point, " + _temp.Point.ToString());

                _scrollSearch = _temp;
            }

            return Found;
        }

        /// <summary>
        /// Opens the Potion Bag where the scrolls reside.
        /// </summary>
        private void OpenBag()
        {
            log.Info(BotName + " Opening bag.");

            if (IsCombatScreenUp())
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                Click(Nav.BtnBag);

                Thread.Sleep(TimeSpan.FromSeconds(6));

                Click(Nav.PotionBag);

                Thread.Sleep(TimeSpan.FromSeconds(1.5));

                log.Info(BotName + " Bag opened to Poition bag.");

            }
        }

        /// <summary>
        /// Grabs the pixel colors of the first 10 items calls ComparePref(), clicking the highest prospect. 
        /// </summary>
        private void FindScroll()
        {
            log.Info(BotName + " searching for Quest Scroll.");
            foreach (Pixel Px in _items)//get screen colors
            {
                log.Info(BotName + " updating bag item pixel values.");

                Px.UpdateColor(Screen);
            }

            ComparePref();

            if (_scroll == null)
            {
                Click(Nav.MapClose);

                Complete = true;

                log.Info(BotName + " item pixel values did not match known Quest Scroll pixel values.");

                MainLog(BotName + " has none of the Scrolls of the grade requested. Check 'Settings' tab under 'Scroll Quest.'");

                return;
            }

            Click(_scroll.Point);

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Looks at the first 20 items in your scroll bag and select the highest grade scroll,
        /// within the specifications of the _preference List.
        /// </summary>
        private void ComparePref()
        {
            log.Info(BotName + " Comparing item pixel values to user set preferences.");

            _scroll = null;

            if (_preference.Contains(Grade.C))
            {
                foreach (Pixel item in _items)
                {
                    if (CompareColor(item.Color, Colors.ScrollC, 2))
                    {
                        log.Info(BotName + " C grade Quest Scroll detected at " + item.Point.ToString());

                        _scroll = item;
                    }
                }
            }

            if (_preference.Contains(Grade.B))
            {
                foreach (Pixel item in _items)
                {
                    if (CompareColor(item.Color, Colors.ScrollB, 2))
                    {
                        log.Info(BotName + " B grade Quest Scroll detected at " + item.Point.ToString());

                        _scroll = item;
                    }
                }
            }

            if (_preference.Contains(Grade.A))
            {
                foreach (Pixel item in _items)
                {
                    if (CompareColor(item.Color, Colors.ScrollA, 2))
                    {
                        log.Info(BotName + " A grade Quest Scroll detected at " + item.Point.ToString());

                        _scroll = item;
                    }
                }
            }

            if (_preference.Contains(Grade.S))
            {
                foreach (Pixel item in _items)
                {
                    if (CompareColor(item.Color, Colors.ScrollS, 2))
                    {
                        log.Info(BotName + " S grade Quest Scroll detected at " + item.Point.ToString());

                        _scroll = item;
                    }
                }
            }
        }

        /// <summary>
        /// Click's the Fulfill Request then OK button, checks for 'Scroll Quest' completion.
        /// </summary>
        private void FulfillRequest()
        {
            if (_reset) //If Scroll Quest Reset preference is check and the pixel is detected, reset the quest.
            {
                if (CheckComplete.IsPresent(Screen, 2))
                {
                    log.Info(BotName + " Scroll Quest has '0' remaining for the day.");

                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    ResetScrollQuest();
                }
            }
            if (!_reset)//If Scroll Quest Reset preference is NOT check and the pixel is detected, close the window
            {
                if (CheckComplete.IsPresent(Screen, 2))
                {
                    Click(Nav.MapClose);

                    log.Info(BotName + " Scroll Quest has 0 remaining for the day, closing item menu and ending Scroll Quest.");

                    MainWindow.main.UpdateLog = BotName + " has completed 'Scroll Quest'";

                    Complete = true;

                    return;
                }
            }

            if (_fulfill[0].IsPresent(Screen, 2) && _fulfill[1].IsPresent(Screen, 2))
            {
                log.Info(BotName + " Clicking Fulfill Quest ");

                Click(_fulfill[0].Point);
            }

            Thread.Sleep(1000);

            if (_fulfillOk[0].IsPresent(Screen, 2) && _fulfillOk[1].IsPresent(Screen, 2))
            {
                log.Info(BotName + " Clicking Fulfill Quest> Ok ");

                Click(_fulfillOk[0].Point);
            }
        }


        /// <summary>
        /// Resets Scroll Quest if present
        /// </summary>
        private void ResetScrollQuest()
        {
            log.Info(BotName + " Reseting Quest Scroll as is user set preference.");

            Click(new Point(727, 409));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Click(new Point(763, 495));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Click(_scroll.Point);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            log.Info(BotName + " Reset Complete.");
        }


        private void IdleCheck()
        {
            if (Timer.ElapsedMilliseconds > IdleTimeInMs && IsCombatScreenUp())
            {
                log.Info(BotName + " calls IdleCheck()");

                ResetTimer();

                StartTimer();

                while (!IsCombatScreenUp())
                {
                    Helper.ClosePopUps();
                    if (Timer.ElapsedMilliseconds > 300000)//5 minutes
                    {
                        log.Info(BotName + " is ending 'Scroll Quest' during IdleCheck() due to not being able to see the combat screen.");

                        MainLog(BotName + " has ended 'Scrol Quest' due to an unknown pop-up being detected.");

                        Complete = true;

                        break;
                    }
                }
                if (IsCombatScreenUp() && GrabScrollPoint())//Looks to see if [Sub] is still in the quest options
                {
                    log.Info(BotName + "has detected Scroll Quest in Quest options");

                    ToggleCombat();

                    Click(_scrollSearch.Point);

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                if (IsCombatScreenUp() && !GrabScrollPoint())
                {
                    log.Info(BotName + "Unable to loctate 'Scroll Quest,' iniClick set to false to open quest screen and check for completion.");
                    _iniClick = false;
                }
                QuestMenuCheck();
            }

        }

        private void QuestMenuCheck()
        {
            if (_scrollQuestMenu[0].IsPresent(Screen, 2) && _scrollQuestMenu[1].IsPresent(Screen, 2))
            {
                log.Info(BotName + " Quest Menu detected, closing menu to look at bag.");

                Click(_scrollQuestMenu[0].Point);

                Thread.Sleep(TimeSpan.FromSeconds(1));

                _iniClick = false;
            }
        }
    }
}

