using L2RBot.Common;
using System;
using System.Diagnostics;
using System.Drawing;
using L2RBot.Common.Enum;
using System.Threading;

namespace L2RBot
{
    public class Weekly : Quest
    {
        //globals
        private Pixel _weeklySearch;

        private Pixel[] _weeklyComplete;

        private bool _iniClick = false;

        //properties
        public Pixel WeeklySearch
        {
            get
            {
                if (_weeklySearch == null)
                {
                    _weeklySearch = new Pixel();
                }
                return _weeklySearch;
            }
            set
            {
                _weeklySearch = value;
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

        //constructors
        public Weekly(Process App, L2RDevice AdbApp) : base(App, AdbApp)
        {
            Helper = new QuestHelper(App, AdbApp)
            {
                Quest = QuestType.Weekly,

                Deathcount = this.Deathcount,

                Respawn = this.Respawn,

                CloseTVPopup = this.CloseTVPopup
            };

            Timer.Start();

            IdleTimeInMs = 60000;

            _BuildComplete();

            _iniClick = false;
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

                Point = new Point(844, 490)//Left side of the Q on 'Quest Complete' button.
            };
            WeeklyComplete[1] = new Pixel
            {
                Color = Color.FromArgb(255, 21, 26, 37),

                Point = new Point(848, 490)//Center blue of the Q on 'Quest Complete' button.
            };
        }

        //logic
        public void Start()
        {
            UpdateScreen();

            if (BringToFront == true)
            {
                BringWindowToFront();
            }

            Sleep();//Sleep before to prevent actions from occuring to early.

            if (_iniClick == false)
            {
                _OpenWeeklyQuest();
            }

            _IdleCheck();

            _Complete();

            Helper.Start();

            IsHelperComplete();

            Sleep();//Sleep after to prevent actions from occuring on the next active window.
        }

        private void _OpenWeeklyQuest()
        {
            if (IsCombatScreenUp())
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                Click(Nav.QuestMenu);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                Click(Nav.BtnWeekly);

                Thread.Sleep(TimeSpan.FromSeconds(.1));

                _iniClick = true;
            }

        }

        private void _IdleCheck()
        {
            if (Timer.ElapsedMilliseconds > IdleTimeInMs && IsCombatScreenUp())
            {
                ResetTimer();

                StartTimer();

                while (!IsCombatScreenUp())
                {
                    Helper.Start();
                    if (Timer.ElapsedMilliseconds > 300000)//5 minutes
                    {
                        MainWindow.main.UpdateLog = BotName + " has ended 'Weekly Quest' due to an unknown pop-up being detected.";

                        Complete = true;

                        break;
                    }
                }
                if (IsCombatScreenUp() && _GrabWeeklyPoint())//Looks to see if [Weekly] is still in the quest options and clicks if it is. 
                {
                    Click(Nav.AutoCombat);

                    Thread.Sleep(TimeSpan.FromSeconds(1));//If you click to fast it will just see a single click.

                    Click(Nav.AutoCombat);

                    Thread.Sleep(50);

                    Click(_weeklySearch.Point);
                }
                if (IsCombatScreenUp() && !_GrabWeeklyPoint())//Looks to see if [Weekly] is still in the quest options
                {
                    _iniClick = false;
                }
            }

        }

        private bool _GrabWeeklyPoint()
        {
            Pixel _temp = L2RBot.Screen.SearchPixelVerticalStride(Screen, new Point(13, 273), 180, Colors.WeeklyQuest, out bool Found, 2);

            if (Found)
            {
                _weeklySearch = _temp;
            }
            if (!Found)
            {
                _weeklySearch = new Pixel();
            }

            return Found;
        }

        private bool _IsComplete()
        {
            return (WeeklyComplete[0].IsPresent(Screen, 2) &&
            WeeklyComplete[1].IsPresent(Screen, 2));
        }

        private void _Complete()
        {
            if (_IsComplete())
            {
                Complete = true;

                MainWindow.main.UpdateLog = BotName + " has completed the 'Weekly Quests'";
            }
        }
    }

}

