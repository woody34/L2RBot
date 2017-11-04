using L2RBot.Common;
using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    public class Weekly : Quest
    {
        //globals
        private Pixel[] _weeklyQuest;

        private Pixel[] _weeklyComplete;

        private Pixel[] _weeklyDone;

        private QuestHelper _helper;

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

        public Pixel[] WeeklyComplete
        {
            get
            {
                if (_weeklyComplete == null)
                {
                    _weeklyComplete = new Pixel[2];

                }

                return _weeklyComplete;
            }
        }

        public Pixel[] WeeklyDone
        {
            get
            {
                if (_weeklyDone == null)
                {
                    _weeklyDone = new Pixel[4];

                }

                return _weeklyDone;
            }
        }

        public QuestHelper Helper
        {
            get
            {
                if(_helper == null)
                {
                    _helper = new QuestHelper(App) { Quest = QuestType.Weekly };
                }

                return _helper;
            }
        }

        //constructors
        public Weekly(Process APP) : base(APP)
        {
            _BuildQuest();
            _BuildDone();
            _BuildComplete();
        }

        /// <summary>
        /// Builds the collection of quest pixels.
        /// </summary>
        private void _BuildQuest()
        {
            //Pixel Objects
            //Weekly Quest on the quest pane
            WeeklyQuest[0] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(16, 309)
            };
            WeeklyQuest[1] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(104, 309)
            };
            WeeklyQuest[2] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(12, 282)
            };
            WeeklyQuest[3] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(103, 285)
            };
        }

        /// <summary>
        /// Builds the collection of quest done pixels.
        /// </summary>
        private void _BuildDone()
        {
            //Done graphic for Weekly Quest on the quest pane. Due to main quest size variations upon completion I had to add a second set of pixels to detect    
            WeeklyDone[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(242, 329)
            };
            WeeklyDone[1] = new Pixel//needs to NOT be present, used to prevent triggering the event if the whole screen goes this color
            {
                Color = Colors.White,
                Point = new Point(245, 329)
            };
            WeeklyDone[2] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(242, 295)
            };
            WeeklyDone[3] = new Pixel//needs to NOT be present, used to prevent triggering the event if the whole screen goes this color
            {
                Color = Colors.White,
                Point = new Point(245, 295)
            };
        }

        /// <summary>
        /// Builds the collection of quest complete pixels.
        /// </summary>
        private void _BuildComplete()
        {
            //All Weekly Quests complete
            WeeklyComplete[0] = new Pixel
            {
                Color = Colors.WeeklyDoneHigh,
                Point = new Point(848, 494)
            };
            WeeklyComplete[1] = new Pixel
            {
                Color = Colors.WeeklyDoneLow,
                Point = new Point(852, 494)
            };
        }

        //logic
        public void Start()
        {
            UpdateScreen();
            User32.SetForegroundWindow(App.MainWindowHandle);
            //looks to see if the quest has been started
            if (_IsWeeklyInProgress())
            {
                Helper.Start();
            }
            if (!_IsWeeklyInProgress())
            {
                if (WeeklyQuest[0].IsPresent(Screen, 2))
                {
                    Click(WeeklyQuest[0].Point);
                }
                if (WeeklyQuest[2].IsPresent(Screen, 2))
                {
                    Click(WeeklyQuest[2].Point);
                }
            }
            if (_IsQuestDone())
            {
                for (int i = WeeklyDone.Length; i > 0; i--)
                {
                    if (WeeklyDone[i - 1].IsPresent(Screen, 2))
                    {
                        Click(WeeklyDone[i - 1].Point);
                        i = 0;//stop once it is found
                    }
                }
            }
            if (_IsComplete())
            {
                System.Threading.Thread.Sleep(1000);
                if (_IsComplete())
                {
                    Complete = true;
                    MainWindow.main.UpdateLog = App.MainWindowTitle + " has completed the weekly quests";
                }
            }
            else
            {
                Bot.PopUpKiller(App);
            }
        }

        private bool _IsWeeklyInProgress()
        {
            //if weeklyQuest pixels are detected this means the quest has NOT been started.
            return (WeeklyQuest[0].IsPresent(Screen, 2) &&
                    WeeklyQuest[1].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App) ||
                    WeeklyQuest[2].IsPresent(Screen, 2) &&
                    WeeklyQuest[3].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App)) ? false : true;
        }

        private bool _IsQuestDone()
        {
            //if WeeklyDone[0] is detected and WeeklyDone[1] is not detected this means the quest HAS been completed
            return (WeeklyDone[0].IsPresent(Screen, 10) &&
                    !WeeklyDone[1].IsPresent(Screen, 10) &&
                    Bot.IsCombatScreenUp(App) ||
                    WeeklyDone[2].IsPresent(Screen, 10) &&
                    !WeeklyDone[3].IsPresent(Screen, 10) &&
                    Bot.IsCombatScreenUp(App)) ? true : false;
        }

        private bool _IsComplete()
        {
            return (WeeklyComplete[0].IsPresent(Screen, 2) &&
                    WeeklyComplete[1].IsPresent(Screen, 2));
        }
    }

}

