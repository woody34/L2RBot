using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    class Weekly : Quest
    {
        //Globals
        QuestHelper Helper;

        //Pixel Objects
        Pixel[] weeklyQuest = new Pixel[4];

        Pixel[] weeklyDone = new Pixel[4];

        Pixel[] completeQuest = new Pixel[2];

        public Weekly(Process APP) : base(APP)
        {
            App = APP;
            //Globals
            Helper = new QuestHelper(App) { Quest = QuestType.Weekly };

            //Pixel Objects
            //Weekly Quest on the quest pane
            weeklyQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 154, 255),
                Point = new Point(16, 309)
            };
            weeklyQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 154, 255),
                Point = new Point(104, 309)
            };
            weeklyQuest[2] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 154, 255),
                Point = new Point(12, 282)
            };
            weeklyQuest[3] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 154, 255),
                Point = new Point(103, 285)
            };

            //Done graphic for Weekly Quest on the quest pane. Due to main quest size variations upon completion I had to add a second set of pixels to detect    
            weeklyDone[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(242, 329)
            };
            weeklyDone[1] = new Pixel//needs to NOT be present, used to prevent triggering the event if the whole screen goes this color
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(245, 329)
            };
            weeklyDone[2] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(242, 295)
            };
            weeklyDone[3] = new Pixel//needs to NOT be present, used to prevent triggering the event if the whole screen goes this color
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(245, 295)
            };

            //All Weekly Quests complete
            completeQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 65, 66, 67),
                Point = new Point(848, 494)
            };
            completeQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 21, 26, 37),
                Point = new Point(852, 494)
            };
        }

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
                if (weeklyQuest[0].IsPresent(Screen, 2))
                {
                    Click(weeklyQuest[0].Point);
                }
                if (weeklyQuest[2].IsPresent(Screen, 2))
                {
                    Click(weeklyQuest[2].Point);
                }
            }
            if (_IsQuestDone())
            {
                for (int i = weeklyDone.Length; i > 0; i--)
                {
                    if (weeklyDone[i - 1].IsPresent(Screen, 2))
                    {
                        Click(weeklyDone[i - 1].Point);
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
                    Debug.WriteLine("Quest Complete");
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
            return (weeklyQuest[0].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App) ||
                    weeklyQuest[2].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App)) ? false : true;
        }

        private bool _IsQuestDone()
        {
            //if weeklyDone[0] is detected and weeklyDone[1] is not detected this means the quest HAS been completed
            return (weeklyDone[0].IsPresent(Screen, 10) &&
                    !weeklyDone[1].IsPresent(Screen, 10) &&
                    Bot.IsCombatScreenUp(App) ||
                    weeklyDone[2].IsPresent(Screen, 10) &&
                    !weeklyDone[3].IsPresent(Screen, 10) &&
                    Bot.IsCombatScreenUp(App)) ? true : false;
        }

        private bool _IsComplete()
        {
            return (completeQuest[0].IsPresent(Screen, 2) &&
                    completeQuest[1].IsPresent(Screen, 2));
        }
    }

}

