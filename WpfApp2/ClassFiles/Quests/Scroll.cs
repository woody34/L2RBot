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

            Helper = new QuestHelper(base.App) { Quest = QuestType.Weekly };

            BuildPixels();
        }

        private void BuildPixels()
        {
            //Pixel Objects
            //Select Quest Scroll popup menu
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
            //Scroll Quest on the quest pane
            LocateQuestButton();

            //Select Quest Scroll

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


        }

        private void LocateQuestButton()
        {
            User32.SetForegroundWindow(App.MainWindowHandle);//focus drawn to the screen so that we can look for a pixel
            _scrollQuest = L2RBot.Screen.SearchPixelVerticalStride(Screen, new Point(16, 210), 211, Colors.ScrollQuest, 2);
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
            if (Helper.IsStuckLoading())
            {
                ClickMapThenQuest();
            }

            Helper.Start();


            IsIdle();
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

