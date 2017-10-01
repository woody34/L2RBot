using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace L2RBot
{
    class DailyDungeon : Quest
    {
        //Globals
        private int _SleepTime = 1000;

        private bool _IsMenuOpen = false;

        private bool _IsAutoCombatEnabled = false;

        //Pixel Objects
        Pixel menu;

        Pixel[] dungeon = new Pixel[2];

        Pixel dailyDungeon;

        Pixel[] difficulty = new Pixel[5];

        Pixel enter;

        Pixel map;

        Pixel location;

        Pixel mapExit;

        Pixel autoCombat;

        Pixel[] ok = new Pixel[2];

        Pixel[] completeQuest = new Pixel[2];

        Pixel[] closeMenu = new Pixel[2];

        public DailyDungeon(Process APP) : base(APP)
        {
            App = APP;
            //Globals

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
            dailyDungeon = new Pixel
            {
                Color = Color.FromArgb(255, 191, 0, 21),
                Point = new Point(274, 181)
            };

            //difficulty rating
            difficulty[0] = new Pixel //Green Bar that indicates if the quest is Easy as aposed to normal or hard, we are looking for this to enter
            {
                Color = Color.FromArgb(255, 80, 170, 30), //green
                Point = new Point(635, 420)
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
                Point = new Point(185, 345)
            };
            difficulty[4] = new Pixel //Easy
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(185, 248)
            };

            //Enter button
            enter = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(1022, 566)
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
                Point = new Point(954, 630)
            };
            ok[1] = new Pixel
            {
                Color = Color.FromArgb(255, 48, 85, 127),
                Point = new Point(959, 630)
            };

            //Complete
            completeQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(962, 544)
            };
            completeQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 78, 124, 83),
                Point = new Point(966, 544)
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
                OpenDailyDungeon();
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

        private void OpenDailyDungeon()
        {
            Click(dailyDungeon.Point);
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
        }

        private bool _IsComplete()
        {
            return (completeQuest[0].IsPresent(Screen, 2) &&
                    completeQuest[1].IsPresent(Screen, 2));
        }
    }

}

