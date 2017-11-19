using L2RBot.Common;
using System;
using System.Diagnostics;
using System.Drawing;
using L2RBot.Common.Enum;

namespace L2RBot
{
    public class Weekly : Quest
    {
        //globals
        private Pixel[] _weeklyAvailable;

        private Pixel[] _weeklyComplete;

        private Pixel[] _weeklyDone;

        private QuestHelper _helper;

        //properties
        public Pixel[] WeeklyAvailable
        {
            get
            {
                if (_weeklyAvailable == null)
                {
                    _weeklyAvailable = new Pixel[4];
                }

                return _weeklyAvailable;
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
            //Available [Weekly] on the quest pane
            WeeklyAvailable[0] = new Pixel// [0] and [1] are detecting Weekly post Main completion.
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(16, 389)//The top of the A in 'Available [Weekly].'
            };

            WeeklyAvailable[1] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(95, 389)//The [ in 'Available [Weekly].'
            };

            WeeklyAvailable[2] = new Pixel//[2] and [3] are detecting Weekly prior to Main completion.
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(16, 331)//The top of the A in 'Available [Weekly].'
            };

            WeeklyAvailable[3] = new Pixel
            {
                Color = Colors.WeeklyQuest,
                Point = new Point(95, 335)//The [ in 'Available [Weekly].'
            };
        }

        /// <summary>
        /// Builds the collection of quest done pixels.
        /// </summary>
        private void _BuildDone()
        {
            //Done graphic for Weekly Quest on the quest pane. Due to main quest size variations upon completion I had to add a second set of pixels to detect    
            WeeklyDone[0] = new Pixel//[0] and [1] are detecting done prior Main completion.
            {
                Color = Colors.White,
                Point = new Point(222, 345)//White vert line in the D on 'Done.'
            };
            WeeklyDone[1] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(225, 345)//Blue space in Center of D on 'Done.'
            };
            WeeklyDone[2] = new Pixel//[2] and [3] are detecting done post Main completion.
            {
                Color = Colors.White,
                Point = new Point(223, 421)//White vert line in the D on 'Done.'
            };
            WeeklyDone[3] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(225, 421)//Blue space in Center of D on 'Done.'
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
                Color = Color.FromArgb(255, 71, 71, 71),
                Point = new Point(838, 493)//Left side of the Q on 'Quest Complete' button.
            };
            WeeklyComplete[1] = new Pixel
            {
                Color = Color.FromArgb(255, 21, 26, 37),
                Point = new Point(843, 493)//Center blue of the Q on 'Quest Complete' button.
            };
        }

        //logic
        public void Start()
        {
            UpdateScreen();
            User32.SetForegroundWindow(App.MainWindowHandle);
            //looks to see if the quest has been started
            if (_IsWeeklyAvailable())
            {
                for (int i = WeeklyAvailable.Length; i > 0; i--)
                {
                    if (WeeklyAvailable[i - 1].IsPresent(Screen, 2))
                    {
                        Click(WeeklyAvailable[i - 1].Point);
                        i = 0;//stop once it is found
                    }
                }
            }
            if (_IsDone())
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
            Helper.Start();
        }

        private bool _IsWeeklyAvailable()
        {
            //If weeklyQuest pixels are detected this means the quest has NOT been started.
            //This is effective because once the quest begins the text changes from 'Available [Weekly]' to '[Weekly].'
            return (WeeklyAvailable[0].IsPresent(Screen, 2) &&
                    WeeklyAvailable[1].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App) ||
                    WeeklyAvailable[2].IsPresent(Screen, 2) &&
                    WeeklyAvailable[3].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App)) ? true : false;
        }

        private bool _IsDone()
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

