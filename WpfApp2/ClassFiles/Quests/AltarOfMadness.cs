using L2RBot.Common;
using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    public class AltarOfMadness : Quest
    {
        //globals
        private Pixel[] _weeklyQuest;

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

        //constructors
        public AltarOfMadness(Process APP) : base(APP)
        {
            _BuildQuest();

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
    }

}

