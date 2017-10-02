using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace L2RBot
{
    class TowerOfInsolence : Quest
    {
        //Globals
        private int _SleepTime = 1000;

        private bool _IsMenuOpen = false;

        //Pixel Objects
        Pixel menu;

        Pixel[] dungeon = new Pixel[2];

        Pixel toiDungeon;

        Pixel difficulty;

        Pixel[] autoClear = new Pixel[2];

        Pixel[] completeQuest = new Pixel[2];

        Pixel[] closeMenu = new Pixel[2];

        public TowerOfInsolence(Process APP) : base(APP)
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
            toiDungeon = new Pixel
            {
                Color = Color.FromArgb(255, 202, 5, 27),
                Point = new Point(540, 180)
            };

            //difficulty rating
            difficulty = new Pixel //Green Bar that indicates if the quest is Easy as aposed to normal or hard, we are looking for this to enter
            {
                Color = Color.FromArgb(255, 80, 170, 30), //green
                Point = new Point(635, 420)
            };

            //Auto-Clear button
            autoClear[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(717, 647)
            };
            autoClear[1] = new Pixel
            {
                Color = Color.FromArgb(255, 27, 38, 50),
                Point = new Point(730, 647)
            };

            //Complete
            completeQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 71, 71, 71),
                Point = new Point(717, 647)
            };
            completeQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 21, 21, 21),
                Point = new Point(730, 647)
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
                OpenToiDungeon();
                _IsMenuOpen = true;
            }
            if (_IsAutoClearPresent())
            {
                Click(autoClear[0].Point);
            }
            else
            {
                Bot.PopUpKiller(App);
            }
        }

        private bool _IsAutoClearPresent()
        {
            return (autoClear[0].IsPresent(Screen, 3) &&
                    autoClear[1].IsPresent(Screen, 3)) ? true : false;
        }

        private void CloseMenus()
        {
            Click(closeMenu[0].Point);
        }

        private void OpenToiDungeon()
        {
            Click(toiDungeon.Point);
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

