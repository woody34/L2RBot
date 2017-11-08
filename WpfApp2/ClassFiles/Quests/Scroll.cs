using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using L2RBot.Common.Enum;
using L2RBot.Common;

namespace L2RBot
{
    class Scroll : Quest
    {
        //Globals
        QuestHelper Helper;

        Pixel _scrollQuest;

        //Properties
        Pixel[] scrollQuest = new Pixel[6];

        Pixel[] questScroll = new Pixel[2];

        Pixel[] fulfillRequest = new Pixel[2];

        Pixel[] ProceedOk = new Pixel[2];

        Point scrollOne = new Point(425, 240);

        Pixel ScrollQuest
        {
            get
            {
                if (_scrollQuest == null)
                {
                    _scrollQuest = new Pixel();
                }
                return _scrollQuest;
            }
        }


        //Constructors
        public Scroll(Process APP) : base(APP)
        {
            App = APP;
            User32.SetForegroundWindow(App.MainWindowHandle);
            Helper = new QuestHelper(base.App) { Quest = QuestType.Weekly };

            BuildPixels();
        }

        private void BuildPixels()
        {
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
            scrollQuest[2] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(13, 335)
            };
            scrollQuest[3] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(94, 335)
            };
            scrollQuest[4] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(16, 413)
            };
            scrollQuest[5] = new Pixel
            {
                Color = Color.FromArgb(255, 75, 208, 247),
                Point = new Point(95, 411)
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
                Point = new Point(721, 598)
            };
            fulfillRequest[1] = new Pixel
            {
                Color = Colors.CompleteOk,//reusing color
                Point = new Point(777, 583)
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

            _scrollQuest = L2RBot.Screen.SearchPixelVerticalStride(Screen, new Point(16, 210), 211, Colors.ScrollQuest, 2);
        }

        //Logic    
        public void Start()
        {
            UpdateScreen();
            MainWindow.main.UpdateLog = App.MainWindowHandle.ToString();
            Thread.Sleep(200);
            //looks to see if the quest has been started
            if (_IsQuestAvailable())
            {
                Click(_scrollQuest.Point);

                Thread.Sleep(100);
            }
            if (!_IsQuestAvailable())
            {
                Helper.Start();
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
                //Bot.PopUpKiller(App);
            }
        }

        private bool _IsQuestAvailable()
        {
            _scrollQuest.UpdateColor(Screen);
            //if scrollQuest pixels are detected this means the quest has NOT been started.
            return (L2RBot.Screen.CompareColor(_scrollQuest.Color, Colors.ScrollQuest, 2)) ? true : false;

            //return (scrollQuest[0].IsPresent(Screen, 2) &&
            //        scrollQuest[1].IsPresent(Screen, 2) &&
            //        Bot.IsCombatScreenUp(App) ||
            //        scrollQuest[2].IsPresent(Screen, 2) &&
            //        scrollQuest[3].IsPresent(Screen, 2) &&
            //        Bot.IsCombatScreenUp(App)
            //        ) ? false : true;
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

