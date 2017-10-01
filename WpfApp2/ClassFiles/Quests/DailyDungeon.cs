using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    class DailyDungeon : Quest
    {
        //Globals
        QuestHelper Helper;
        //Pixel Objects

        Pixel[] completeQuest = new Pixel[2];

        public DailyDungeon(Process APP) : base(APP)
        {
            App = APP;
            //Globals
            Helper = new QuestHelper(App) { Quest = QuestType.Weekly };

            //Pixel Objects
            //Quest Complete
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

        private bool _IsComplete()
        {
            return (completeQuest[0].IsPresent(Screen, 2) &&
                    completeQuest[1].IsPresent(Screen, 2));
        }
    }

}

