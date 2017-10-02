using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace L2RBot
{
    class ExpDungeon : Quest
    {
        //Globals
        private int _SleepTime = 1000;

        private bool _IsMenuOpen = false;

        private bool _IsAutoCombatEnabled = false;

        //Pixel Objects
        Pixel menu;

        Pixel[] dungeon = new Pixel[2];

        Pixel expDungeon;

        Pixel[] difficulty = new Pixel[5];

        Pixel enter;

        Pixel map;

        Pixel location;

        Pixel mapExit;

        Pixel autoCombat;

        Pixel[] ok = new Pixel[2];

        Pixel[] completeQuest = new Pixel[2];

        Pixel[] closeMenu = new Pixel[2];

        public ExpDungeon(Process APP) : base(APP)
        {
            //Globals
            App = APP;
            
            //Points


            //Pixel Objects
            //Menu navigation hamburger button
            menu = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(918, 32)
            };

            //Dungeon sub navigation buttons
            dungeon[0] = new Pixel
            {
                Color = Color.FromArgb(255, 189, 188, 188),
                Point = new Point(240, 667)
            };
            dungeon[1] = new Pixel
            {
                Color = Color.FromArgb(255, 193, 192, 190),
                Point = new Point(239, 549)
            };

            //Daily Dungeon Red Dot that indicates a new quest
            expDungeon = new Pixel
            {
                Color = Color.FromArgb(255, 181, 0, 20),
                Point = new Point(953, 182)
            };

            //difficulty rating
            difficulty[0] = new Pixel //Green Bar that indicates if the quest is Easy as aposed to normal or hard, we are looking for this to enter
            {
                Color = Color.FromArgb(255, 80, 170, 30), //green
                Point = new Point(887, 297)
            };
            difficulty[1] = new Pixel //Very Hard
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(185, 528)
            };
            difficulty[2] = new Pixel //Hard
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(185, 437)
            };
            difficulty[3] = new Pixel //Normal
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(185, 330)
            };
            difficulty[4] = new Pixel //Easy
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(185, 225)
            };

            //Enter button
            enter = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(1050, 645)
            };

            //Map
            map = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(1180, 106)
            };

            //Map Location
            location = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(447, 552)
            };

            //Map exit button
            mapExit = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(1245, 42)
            };

            //The auto combat button that has the character complete the quest automatically
            autoCombat = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(887, 683)
            };

            //Ok menu
            ok[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(940, 634)
            };
            ok[1] = new Pixel
            {
                Color = Color.FromArgb(255, 38, 75, 118),
                Point = new Point(945, 634)
            };

            //Complete
            completeQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(952, 644)
            };
            completeQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 77, 127, 82),
                Point = new Point(957, 640)
            };

            //Close menu and go back to combat screen
            closeMenu[0] = new Pixel
            {
                Color = Color.FromArgb(255, 217, 218, 219),
                Point = new Point(1237, 28)
            };
            closeMenu[1] = new Pixel
            {
                Color = Color.FromArgb(255, 18, 24, 33),
                Point = new Point(1247, 44)
            };
        }

        public void Start()
        {
            UpdateScreen();
            User32.SetForegroundWindow(App.MainWindowHandle);
            Thread.Sleep(_SleepTime);
            if (_IsComplete())
            {
                Complete = true;
                CloseMenus();
            }
            if (!_IsMenuOpen &&
                Bot.IsCombatScreenUp(App))
            {
                Debug.WriteLine("Menu has not been opened, opening menu");
                OpenDungeons();
                ScrollToEnd();
                OpenExpDungeon();
                _IsMenuOpen = true;
            }
            if (enter.IsPresent(Screen, 2))
            {
                Debug.WriteLine("Enter buton detected");
                ChoseDungeon();
            }
            if (_IsMenuOpen &&
                Bot.IsCombatScreenUp(App) &&
                !_IsAutoCombatEnabled)
            {
                EnableAutoCombat();
            }
            if (Bot.IsCombatScreenUp(App) &&
                _IsAutoCombatEnabled)
            {
                Thread.Sleep(_SleepTime);
            }
            if (_IsOkBtnPresent())
            {
                Click(ok[0].Point);
            }
            else
            {
                Bot.PopUpKiller(App);
            }
        }

        private bool _IsOkBtnPresent()
        {
            return (ok[0].IsPresent(Screen, 3) &&
                    ok[1].IsPresent(Screen, 3)) ? true : false;
        }

        private void EnableAutoCombat()
        {
            _IsAutoCombatEnabled = true;
            Click(map.Point);
            Thread.Sleep(_SleepTime * 2);
            Click(location.Point);
            Thread.Sleep(_SleepTime);
            Click(mapExit.Point);
            Thread.Sleep(_SleepTime);
            Click(autoCombat.Point);
            Thread.Sleep(_SleepTime);
        }

        private void CloseMenus()
        {
            Click(closeMenu[0].Point);
        }

        private void ChoseDungeon()
        {
            _IsAutoCombatEnabled = false;

            Click(difficulty[1].Point);

            Thread.Sleep(_SleepTime);

            if (difficulty[0].IsPresent(Screen, 2))
            {
                Click(enter.Point);

                Thread.Sleep(_SleepTime);
            }
            else
            {
                Click(difficulty[2].Point);

                Thread.Sleep(_SleepTime);

                if (difficulty[0].IsPresent(Screen, 2))
                {
                    Click(enter.Point);

                    Thread.Sleep(_SleepTime);
                }
                else
                {
                    Click(difficulty[3].Point);

                    Thread.Sleep(_SleepTime);

                    if (difficulty[0].IsPresent(Screen, 2))
                    {
                        Click(enter.Point);
                        Thread.Sleep(_SleepTime);
                    }
                    else
                    {
                        Click(difficulty[4].Point);

                        Thread.Sleep(_SleepTime);

                        if (difficulty[0].IsPresent(Screen, 2))
                        {
                            Click(enter.Point);

                            Thread.Sleep(_SleepTime);
                        }
                    }

                }

            }

        }

        private void OpenExpDungeon()
        {
            Click(expDungeon.Point);

            Thread.Sleep(_SleepTime);
        }

        private void OpenDungeons()
        {
            Click(menu.Point);

            Thread.Sleep(_SleepTime);

            Click(dungeon[0].Point);

            Thread.Sleep(_SleepTime);

            Click(dungeon[1].Point);

            Thread.Sleep(_SleepTime);

            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            Point ScrollUp = new Point(35, 333);

            Point ScrollDown = new Point(1246, 333);

            Mouse.LeftMouseClickDrag(ScrollDown, ScrollUp);

            Thread.Sleep(_SleepTime);

        }

        private bool _IsComplete()
        {
            return (completeQuest[0].IsPresent(Screen, 2) &&
                    completeQuest[1].IsPresent(Screen, 2));
        }
    }

}

