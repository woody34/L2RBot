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
        private QuestHelper Helper;

        private Pixel _scrollQuest;

        private bool _isQuestFound;

        private Pixel[] _questScroll = new Pixel[2];

        //Properties

        Pixel[] fulfillRequest = new Pixel[2];

        Pixel[] ProceedOk = new Pixel[2];

        Pixel QuestScroll;

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

            Helper = new QuestHelper(base.App) { Quest = QuestType.Weekly };

            BuildPixels();

        }

        private void BuildPixels()
        {
            //Pixel Objects.
            //Select Quest Scroll popup menu.
            _questScroll[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(521, 107)
            };
            _questScroll[1] = new Pixel
            {
                Color = Color.FromArgb(255, 162, 166, 172),
                Point = new Point(903, 110)
            };
            QuestScroll = new Pixel
            {
                Point = new Point(425, 240),
                Color = Color.White
            };
            //Scroll Quest on the quest pane.
            LocateQuestButton();

            //Select Quest Scroll.

            //Fulfill Request button on Quest Scroll popup menu.
            fulfillRequest[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(726, 601)
            };
            fulfillRequest[1] = new Pixel
            {
                Color = Colors.CompleteOk,//reusing color.
                Point = new Point(747, 584)
            };

            //Proceed Ok button after Fulfill Request is pressed.
            ProceedOk[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(750, 491)//The O on the 'Ok' button.
            };
            ProceedOk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 39, 75, 118),
                Point = new Point(755, 491)//middle of the O on the 'Ok' button.
            };


        }



        //Logic    
        public void Start()
        {
            UpdateScreen();
            User32.SetForegroundWindow(App.MainWindowHandle);
            Thread.Sleep(200);
            //looks to see if the quest has been started
            if (_IsQuestAvailable())
            {
                Click(_scrollQuest.Point);

                Thread.Sleep(100);
            }
            if (_IsQuestScrollPresent())
            {
                Click(QuestScroll.Point);
                Thread.Sleep(100);
            }
            if (_IsFulfillRequestPresent())
            {
                if (LocateAGradeScroll())
                {
                    Click(QuestScroll.Point);
                    Thread.Sleep(TimeSpan.FromSeconds(.1));
                }

                Click(fulfillRequest[0].Point);
                Thread.Sleep(100);
            }
            if (_IsProceedOKPresent())
            {
                Click(ProceedOk[0].Point);
                Thread.Sleep(100);
            }
            if (Helper.IsStuckLoading())
            {
                ClickMapThenQuest();
            }

            Helper.Start();


            IsIdle();
        }

        private void LocateQuestButton()
        {
            User32.SetForegroundWindow(App.MainWindowHandle);//focus drawn to the screen so that we can look for a pixel
            _scrollQuest = L2RBot.Screen.SearchPixelVerticalStride(Screen, new Point(13, 210), 211, Colors.ScrollQuest, 2);
        }

        private void LocateSGradeScroll()
        {
            //
        }

        private bool LocateAGradeScroll()
        {
            bool found;
            Pixel temp = L2RBot.Screen.SearchPixelHorizontalStride(Screen, new Point(360, 176), 500, Color.FromArgb(255, 113, 160, 14), out found, 10);
            if (found)
            {
                QuestScroll = temp;
                return true;
            }
            return false;
        }

        private void ClickMapThenQuest()
        {
            Click(Nav.Map);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Click(Nav.BtnSubClanHall);//reusing points, this clicks a random spot on map

            Click(Nav.MapClose);

            Click(_scrollQuest.Point);//clicks on scroll quest again        
        }

        private void IsIdle()
        {
            if (TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds) > TimeSpan.FromMinutes(5))
            {
                //open bag and click scroll quest
            }
        }

        private bool _IsQuestAvailable()
        {
            _scrollQuest.UpdateColor(Screen);

            //if scrollQuest pixels are detected this means the quest has NOT been started.
            _isQuestFound = (L2RBot.Screen.CompareColor(_scrollQuest.Color, Colors.ScrollQuest, 2)) ? true : false;
            return _isQuestFound;
        }

        private bool _IsQuestScrollPresent()
        {
            _scrollQuest.UpdateColor(Screen);

            return (_questScroll[0].IsPresent(Screen, 2) &&
                    _questScroll[1].IsPresent(Screen, 2)) ?
                    true : false;
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

