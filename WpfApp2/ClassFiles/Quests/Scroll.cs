using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace L2RBot
{
    class Scroll : Quest
    {
        //Globals
        QuestHelper Helper;
        //Pixel Objects
        Pixel[] scrollQuest = new Pixel[2];
        Pixel[] questScroll = new Pixel[2];
        Pixel[] fulfillRequest = new Pixel[2];
        Pixel[] ProceedOk = new Pixel[2];

        //Point Objects
        Point scrollOne = new Point(425, 240);

        public Scroll(Process APP) : base(APP)
        {
            App = APP;
            //Globals
            Helper = new QuestHelper(App) { Quest = QuestType.Weekly };

            //Pixel Objects
            //Scroll Quest on the quest pane
            scrollQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(13, 368)
            };
            scrollQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(94, 363)
            };

            //Select Quest Scroll popup menu
            questScroll[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(521, 107)
            };
            questScroll[1] = new Pixel
            {
                Color = Color.FromArgb(255, 162, 166, 172),
                Point = new Point(903, 110)
            };

            //Fulfill Request button on Quest Scroll popup menu
            fulfillRequest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(715, 608)
            };
            fulfillRequest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 39, 79, 118),
                Point = new Point(711, 609)
            };

            //Proceed Ok button after Fulfill Request is pressed
            ProceedOk[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(749, 494)
            };
            ProceedOk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 39, 79, 118),
                Point = new Point(753, 496)
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
                Click(scrollQuest[0].Point);
                Thread.Sleep(100);
            }
            if (_IsQuestScrollPresent())
            {
                Click(scrollOne);
                Thread.Sleep(100);
            }
            if (_IsFulfillRequestPresent())
            {
                Click(fulfillRequest[0].Point);
                Thread.Sleep(100);
            }
            if (_IsProceedOKPresent())
            {
                Click(ProceedOk[0].Point);
                Thread.Sleep(100);
            }
            else
            {
                Bot.PopUpKiller(App);
            }
        }

        private bool _IsWeeklyInProgress()
        {
            //if scrollQuest pixels are detected this means the quest has NOT been started.
            return (scrollQuest[0].IsPresent(Screen, 2) &&
                    scrollQuest[1].IsPresent(Screen, 2) &&
                    Bot.IsCombatScreenUp(App)) ? false : true;
        }

        private bool _IsQuestScrollPresent()
        {
            return (questScroll[0].IsPresent(Screen, 2) &&
                questScroll[1].IsPresent(Screen, 2)) ? true : false;
        }

        private bool _IsFulfillRequestPresent()
        {
            return (fulfillRequest[0].IsPresent(Screen, 2) &&
                fulfillRequest[1].IsPresent(Screen, 2)) ? true : false;
        }

        private bool _IsProceedOKPresent()
        {
            return (ProceedOk[0].IsPresent(Screen, 2) &&
                    ProceedOk[1].IsPresent(Screen, 2)) ? true : false;
        }
    }

}

