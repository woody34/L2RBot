using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace L2RBot
{
    class Bot
    {
        /// <summary>
        /// opens Nox Player
        /// </summary>
        /// <returns>Process object for Nox Player</returns>
        public Process OpenNox()
        {
            Process[] noxPlayer = Process.GetProcesses();
            Process[] nPID = new Process[20];// max of 20 nox players open
            int i = 0;
            Thread.Sleep(100);
            foreach (Process arry in noxPlayer)
            {
                if (arry.ProcessName == "Nox" | arry.ProcessName == "NoxPlayer1")
                {
                    nPID[i] = Process.GetProcessById(arry.Id);
                    i++;
                }
            }
            if(nPID[0] == null)
            {
                Process Nox = new Process();
                Nox.StartInfo.FileName = @"C:\Program Files (x86)\Nox\bin\Nox.exe";
                Nox.Start();
                Nox.WaitForInputIdle();
                return Nox;
            }
            return nPID[0];




        }
        /// <summary>
        /// Looks at pixel locations to determine if the game has crashed and the Nox Player has returned to the desktop
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>True: upon detecting the Lineage 2 Revolutions Logo on the Nox Player desktop at R2:C5</returns>
        /// <returns>False: upon failing to detect the Lineage 2 Revolutions Logo on the Nox Player desktop at R2:C5</returns>
        public Boolean IsAppClosed(Process proc)
        {
            Rectangle rect = Screen.GetRect(proc);

            Color[] isAppClosed = new Color[2];
            isAppClosed[0] = Color.FromArgb(255,87,179,183);
            isAppClosed[1] = Color.FromArgb(255, 161, 161, 161);

            Color pixel1 = Screen.GetColor(rect, 83.046875, 47.63888888888889);
            Color pixel2 = Screen.GetColor(rect, 84.453125, 44.44444444444444);
            if (Screen.CompareColor(pixel1, isAppClosed[0], 2) & Screen.CompareColor(pixel2, isAppClosed[1], 2))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// checks to see if bag is full
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>true: if full bag icon is detected</returns>
        public Boolean IsItemFull(Process proc)
        {
            Rectangle rect = Screen.GetRect(proc);
            Color[] isItemFull = new Color[2];
            isItemFull[0] = Color.FromArgb(255, 64, 218, 250);
            isItemFull[1] = Color.FromArgb(255, 219, 220, 217);

            Color pixel1 = Screen.GetColor(rect, 75.78125, 7.083333333333333);
            Color pixel2 = Screen.GetColor(rect, 76.640625, 2.5);

            if (Screen.CompareColor(pixel1, isItemFull[0], 2) & Screen.CompareColor(pixel2, isItemFull[1], 2))
            {
                return true;
            }
            else { return false; }

        }
        /// <summary>
        /// Checks to see if the green wifi logo is visible indicating that the combat Screen is displayed.
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>true: once wifi logo is detected</returns>
        public Boolean IsCombatScreenUp(Process proc)
        {
            Rectangle rect = Screen.GetRect(proc);
            Color wifi = new Color();
            wifi = Color.FromArgb(255, 79, 136, 42);
            Color pixel1 = Screen.GetColor(rect, 0.78125, 98.47222222222222);

            if (Screen.CompareColor(pixel1, wifi, 2))
                if (pixel1 == wifi) { return true; }

            return false;
        }
        /// <summary>
        /// Opens Lineage 2 Revolutions from within Nox Plater.
        /// </summary>
        /// <param name="app">Nox Player's Process</param>
        /// <returns>true: upon completion</returns>
        //public Boolean OpenApp(Process app)
        //{
        //    #region OpenApp variables
        //    Mouse mouse = new Mouse();
        //    Point clickPoint;
        //    Rectangle rect = Screen.GetRect(app);
        //    //Lineage 2 Revolutions Icon
        //    Color[] isAppClosed = new Color[2];
        //    isAppClosed[0] = Color.FromArgb(255, 87, 179, 183);
        //    isAppClosed[1] = Color.FromArgb(255, 161, 161, 161);
        //    //Blue login buttons
        //    Color[] btnBlue = new Color[2];
        //    btnBlue[0] = Color.FromArgb(255, 35, 112, 143);
        //    btnBlue[1] = Color.FromArgb(255, 255, 255, 255);
        //    //Annoying login popups
        //    Color[] btnClose = new Color[2];
        //    btnClose[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnClose[1] = Color.FromArgb(255, 0, 0, 0);
        //    //Green play button
        //    Color[] btnPlay = new Color[2];
        //    btnPlay[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnPlay[1] = Color.FromArgb(255, 80, 131, 85);
        //    //Green Wifi Logo which is visible after logging into the game
        //    Color[] wifi = new Color[2];
        //    wifi[0] = Color.FromArgb(255, 79, 136, 42);
        //    //wifi[1] = Color.FromArgb(255, 80, 131, 85);//not used atm
        //    #endregion


        //    //Look for Lineage 2 Revolutions Application Icon
        //    Color pixel1 = Screen.GetColor(rect, 83.046875, 47.63888888888889, out clickPoint);
        //    Color pixel2 = Screen.GetColor(rect, 84.453125, 44.44444444444444);

        //    if (pixel1 == isAppClosed[0] & pixel2 == isAppClosed[1])
        //    {
                
        //        //Thread.Sleep(5000);
        //        //Click L2R app icon
        //        //Mouse.LeftClick(clickPoint, app);
        //        User32.SetFocus(app.MainWindowHandle);
        //        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);

        //        //System.Windows.MessageBox.Show(pixel1.ToString() + "==" + isAppClosed[0].ToString() + ":" + pixel2.ToString() + "==" + isAppClosed[1].ToString() + " Click:" + clickPoint.ToString());
        //        Thread.Sleep(1000);
        //        bool b = false;
        //        while (b == false)
        //        {
        //            //Look for the blue buttons that indicate login potential
        //            if (pixel1 != btnBlue[0] & pixel2 != btnBlue[1])
        //            {
        //                pixel1 = Screen.GetColor(rect, 53.125, 67.08333333333333, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 56.71875, 68.88888888888889);
        //                //System.Windows.MessageBox.Show(pixel1.ToString() + "=="+ btnBlue[0].ToString() + ":" + pixel2.ToString() + "==" + btnBlue[1].ToString() + " Click:" +clickPoint.ToString());
        //                Thread.Sleep(100);
        //            }
        //            //Click 'Tap to Start' upon finding the blue buttons
        //            if (pixel1 == btnBlue[0] & pixel2 == btnBlue[1])
        //            {
        //                Screen.GetColor(rect, 47.578125, 84.58333333333333, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Thread.Sleep(100);
        //                //Mouse.LeftClick(clickPoint, app);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            }
        //            //Look for annoying login pop-ups
        //            if (pixel1 != btnClose[0] & pixel2 != btnClose[1])
        //            {
        //                pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 91.484375, 5);
        //                Thread.Sleep(100);
        //            }
        //            //Hit escape to close anooying login pop-ups
        //            if (pixel1 == btnClose[0] & pixel2 == btnClose[1])
        //            {
        //                //I had to do this for now since i havent figured out how to get SendInput to subwindows
        //                pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Thread.Sleep(100);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //                //start-test
        //                //WindowsEnumerator enumerator = new WindowsEnumerator();
        //                //foreach (ApiWindow top in enumerator.GetTopLevelWindows())
        //                //{
        //                //    if (top.MainWindowTitle == "NoxPlayer")
        //                //    {

        //                //        System.Windows.MessageBox.Show(top.MainWindowTitle);
        //                //        foreach (ApiWindow child in enumerator.GetChildWindows(top.hWnd))
        //                //            System.Windows.MessageBox.Show("Child MainWindowTitle:" + child.MainWindowTitle + " Class Name:" + child.ClassName + " hWnd:" + child.hWnd);
        //                //    }
        //                //}
        //                //end-test
        //            }
        //            //Look for green 'Play' button
        //            if (pixel1 != btnPlay[0] & pixel2 != btnPlay[1])
        //            {
        //                pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 87.5, 88.61111111111111);
        //                Thread.Sleep(100);
        //            }
        //            //Click green 'Play' button
        //            if (pixel1 == btnPlay[0] & pixel2 == btnPlay[1])
        //            {
        //                pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //                Thread.Sleep(100);
        //            }
        //            //Look for the green wifi logo upon succesful login
        //            if (pixel1 != wifi[0])
        //            {
        //                pixel1 = Screen.GetColor(rect, 0.78125, 98.47222222222222, out clickPoint);
        //                Thread.Sleep(100);
        //            }
        //            //End while loop
        //            if (pixel1 == wifi[0])
        //            {
        //                b = true;
        //            }
        //        }
        //    }
        //    return true;
        //}
        public Boolean OpenApp(Process app)
        {
            #region OpenApp variables
            Mouse mouse = new Mouse();
            Point clickPoint;
            Rectangle rect = Screen.GetRect(app);
            //Lineage 2 Revolutions Icon
            Color[] isAppClosed = new Color[2];
            isAppClosed[0] = Color.FromArgb(255, 87, 179, 183);
            isAppClosed[1] = Color.FromArgb(255, 161, 161, 161);
            //Blue login buttons
            Color[] btnBlue = new Color[2];
            btnBlue[0] = Color.FromArgb(255, 35, 112, 143);
            btnBlue[1] = Color.FromArgb(255, 255, 255, 255);
            //Annoying login popups
            Color[] btnClose = new Color[2];
            btnClose[0] = Color.FromArgb(255, 255, 255, 255);
            btnClose[1] = Color.FromArgb(255, 0, 0, 0);
            //Green play button
            Color[] btnPlay = new Color[2];
            btnPlay[0] = Color.FromArgb(255, 255, 255, 255);
            btnPlay[1] = Color.FromArgb(255, 80, 131, 85);
            //Green Wifi Logo which is visible after logging into the game
            Color[] wifi = new Color[2];
            wifi[0] = Color.FromArgb(255, 79, 136, 42);
            //wifi[1] = Color.FromArgb(255, 80, 131, 85);//not used atm
            #endregion


            //Look for Lineage 2 Revolutions Application Icon
            Color pixel1 = Screen.GetColor(rect, 83.046875, 47.63888888888889, out clickPoint);
            Color pixel2 = Screen.GetColor(rect, 84.453125, 44.44444444444444);

            if (Screen.CompareColor(pixel1, isAppClosed[0], 2) & Screen.CompareColor(pixel2, isAppClosed[1], 2))
            {
                
                //Thread.Sleep(5000);
                //Click L2R app icon
                //Mouse.LeftClick(clickPoint, app);
                User32.SetFocus(app.MainWindowHandle);
                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);

                //System.Windows.MessageBox.Show(pixel1.ToString() + "==" + isAppClosed[0].ToString() + ":" + pixel2.ToString() + "==" + isAppClosed[1].ToString() + " Click:" + clickPoint.ToString());
                Thread.Sleep(1000);
                bool b = false;
                while (b == false)
                {
                    //Look for the blue buttons that indicate login potential
                    if (Screen.CompareColor(pixel1, btnBlue[0], 2) & Screen.CompareColor(pixel2, btnBlue[1], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 53.125, 67.08333333333333, out clickPoint);
                        pixel2 = Screen.GetColor(rect, 56.71875, 68.88888888888889);
                        Thread.Sleep(100);
                    }
                    //Click 'Tap to Start' upon finding the blue buttons
                    if (Screen.CompareColor(pixel1, btnBlue[0], 2) & Screen.CompareColor(pixel2, btnBlue[1], 2))
                    {
                        Screen.GetColor(rect, 47.578125, 84.58333333333333, out clickPoint);
                        User32.SetFocus(app.MainWindowHandle);
                        Thread.Sleep(100);
                        //Mouse.LeftClick(clickPoint, app);
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    }
                    //Look for annoying login pop-ups
                    if (!Screen.CompareColor(pixel1, btnClose[0], 2) & !Screen.CompareColor(pixel2, btnClose[1], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
                        pixel2 = Screen.GetColor(rect, 91.484375, 5);
                        Thread.Sleep(100);
                    }
                    //Hit escape to close anooying login pop-ups
                    if (Screen.CompareColor(pixel1, btnClose[0], 2) & Screen.CompareColor(pixel2, btnClose[1], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
                        User32.SetFocus(app.MainWindowHandle);
                        Thread.Sleep(100);
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);

                    }
                    //Look for green 'Play' button
                    if (!Screen.CompareColor(pixel1, btnPlay[0], 2) & !Screen.CompareColor(pixel2, btnPlay[1], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
                        pixel2 = Screen.GetColor(rect, 87.5, 88.61111111111111);
                        Thread.Sleep(100);
                    }
                    //Click green 'Play' button
                    if (Screen.CompareColor(pixel1, btnPlay[0], 2) & Screen.CompareColor(pixel2, btnPlay[1], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
                        User32.SetFocus(app.MainWindowHandle);
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                        Thread.Sleep(100);
                    }
                    //Look for the green wifi logo upon succesful login
                    if (!Screen.CompareColor(pixel1, wifi[0], 2))
                    {
                        pixel1 = Screen.GetColor(rect, 0.78125, 98.47222222222222, out clickPoint);
                        Thread.Sleep(100);
                    }
                    //End while loop
                    if (Screen.CompareColor(pixel1, wifi[0], 2))
                    {
                        b = true;
                    }
                }
            }
            return true;
        }

        public static Process[] BindNoxPlayer()
        {
            Process[] noxPlayers;
            Process[] WindowsProcesses = Process.GetProcesses();
            int i = 0;

            foreach (Process pro in WindowsProcesses)
            {
                //Debug.WriteLine(pro.Id.ToString());
                if (pro.ProcessName == "Nox" | pro.ProcessName == "NoxPlayer1")
                {
                    i++;
                }
            }
            noxPlayers = new Process[i];
            i = 0;
            foreach (Process pro in WindowsProcesses)
            {
                if (pro.ProcessName == "Nox" | pro.ProcessName == "NoxPlayer1")
                {
                    noxPlayers[i] = Process.GetProcessById(pro.Id);
                    MainWindow.main.UpdateLog = pro.MainWindowTitle + " detected!";
                    i++;
                }
            }
            return noxPlayers;
        }
        /// <summary>
        /// Performs a bulk sale operation when bag is detected to be full
        /// </summary>
        /// <param name="app"></param>
        /// <returns>true: once complete</returns>
        public Boolean BulkSale(Process app)
        {
            Mouse mouse = new Mouse();
            Point clickPoint;
            Rectangle rect = Screen.GetRect(app);
            Color pixel1;
            Color pixel2;
            bool saleComplete = false;
            User32.SetFocus(app.MainWindowHandle);
            while (saleComplete == false)
            {
                //full bag icon
                Color[] isItemFull = new Color[2];
                isItemFull[0] = Color.FromArgb(255, 64, 218, 250);
                isItemFull[1] = Color.FromArgb(255, 219, 220, 217);

                User32.SetFocus(app.MainWindowHandle);
                pixel1 = Screen.GetColor(rect, 75.78125, 7.083333333333333, out clickPoint);
                pixel2 = Screen.GetColor(rect, 76.640625, 2.5);
                if (Screen.CompareColor(pixel1, isItemFull[0], 2) & Screen.CompareColor(pixel2, isItemFull[1], 2))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    Thread.Sleep(100);
                }

                //'Bulk Sale' button
                Color[] btnBulkSale = new Color[2];
                btnBulkSale[0] = Color.FromArgb(255, 255, 255, 255);
                btnBulkSale[1] = Color.FromArgb(255, 52, 66, 82);
                User32.SetFocus(app.MainWindowHandle);
                pixel1 = Screen.GetColor(rect, 71.09375, 94.02777777777778, out clickPoint);
                pixel2 = Screen.GetColor(rect, 72.421875, 91.80555555555556);
                if (Screen.CompareColor(pixel1, btnBulkSale[0], 2) & Screen.CompareColor(pixel2, btnBulkSale[1], 2))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    Thread.Sleep(100);
                }

                //'Sale' button.green
                Color[] btnSellGreen = new Color[2];
                btnSellGreen[0] = Color.FromArgb(255, 251, 248, 237);
                btnSellGreen[1] = Color.FromArgb(255, 80, 130, 84);
                User32.SetFocus(app.MainWindowHandle);
                pixel1 = Screen.GetColor(rect, 91.5625, 93.75, out clickPoint);
                pixel2 = Screen.GetColor(rect, 90.078125, 93.61111111111111);
                if (Screen.CompareColor(pixel1, btnSellGreen[0], 2) & Screen.CompareColor(pixel2, btnSellGreen[1], 2))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    Thread.Sleep(100);
                }

                //'Sale' button.blue
                Color[] btnSellBlue = new Color[2];
                btnSellBlue[0] = Color.FromArgb(255, 255, 255, 255);
                btnSellBlue[1] = Color.FromArgb(255, 49, 85, 127);
                User32.SetFocus(app.MainWindowHandle);
                pixel1 = Screen.GetColor(rect, 58.125, 68.19444444444444, out clickPoint);
                pixel2 = Screen.GetColor(rect, 57.109375, 68.19444444444444);
                if (Screen.CompareColor(pixel1, btnSellBlue[0], 2) & Screen.CompareColor(pixel2, btnSellBlue[1], 2))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    Thread.Sleep(100);
                }

                //'Ok' button
                Color[] btnOk = new Color[2];
                btnOk[0] = Color.FromArgb(255, 255, 255, 255);
                btnOk[1] = Color.FromArgb(255, 49, 85, 127);
                User32.SetFocus(app.MainWindowHandle);
                pixel1 = Screen.GetColor(rect, 48.90625, 66.94444444444444, out clickPoint);
                pixel2 = Screen.GetColor(rect, 47.96875, 66.25);
                if (Screen.CompareColor(pixel1, btnOk[0], 2) & Screen.CompareColor(pixel2, btnOk[1], 2))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                    Thread.Sleep(2000);
                    Color[] btnClose = new Color[2];
                    btnClose[0] = Color.FromArgb(255, 28, 31, 40);
                    btnClose[1] = Color.FromArgb(255, 216, 216, 218);
                    User32.SetFocus(app.MainWindowHandle);
                    pixel1 = Screen.GetColor(rect, 96.71875, 2.777777777777778, out clickPoint);
                    pixel2 = Screen.GetColor(rect, 96.640625, 4.027777777777778);
                    if (Screen.CompareColor(pixel1, btnClose[0], 2) & Screen.CompareColor(pixel2, btnClose[1], 2))
                    {
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                        Thread.Sleep(1000);
                        saleComplete = true;
                        return true;
                    }
                }

                //close button; top right, looks like a door with an arrow



            }
            return false;
        }//WIP
        /// <summary>
        /// Keeps quests going to navigating through the popup menus and driving the quest
        /// </summary>
        /// <param name="app">the process object for Nox Player/Android emulator</param>
        /// <param name="input"></param>
        //public void QuestHelper(Process app, string input = "")
        //{
        //    Rectangle rect = Screen.GetRect(app); //capture Screen location and demensions

        //    //variables
        //    Color pixel1;
        //    Color pixel2;
        //    Color pixel3;
        //    Point point = new Point();

        //    //Skip
        //    Color[] btnSkip = new Color[2];
        //    btnSkip[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnSkip[1] = Color.FromArgb(255, 0, 0, 0);
        //    pixel1 = Screen.GetColor(rect, 90.78125, 69.86111111111111, out point);
        //    pixel2 = Screen.GetColor(rect, 97.109375, 82.5);
        //    if (btnSkip[0] == pixel1 & btnSkip[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //    }

        //    //Accept Quest
        //    Color[] btnAcceptQuest = new Color[2];
        //    btnAcceptQuest[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnAcceptQuest[1] = Color.FromArgb(255, 55, 91, 133);
        //    User32.SetFocus(app.MainWindowHandle);
        //    pixel1 = Screen.GetColor(rect, 60.3125, 84.44444444444444, out point);
        //    pixel2 = Screen.GetColor(rect, 53.75, 82.63888888888889);
        //    if (btnAcceptQuest[0] == pixel1 & btnAcceptQuest[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //    }

        //    //Claim Reward
        //    Color[] btnClaimReward = new Color[2];
        //    btnClaimReward[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnClaimReward[1] = Color.FromArgb(255, 59, 95, 136);
        //    pixel1 = Screen.GetColor(rect, 45.078125, 84.58333333333333, out point);
        //    pixel2 = Screen.GetColor(rect, 47.578125, 81.94444444444444);
        //    if (btnClaimReward[0] == pixel1 & btnClaimReward[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        //main quest extras
        //        if (input == "main")
        //        {
        //            Thread.Sleep(4000);
        //            point = Screen.PercentToPoint(rect, 5.00, 34.58333333333333);
        //            Mouse.LeftMouseClick(point.X, point.Y);

        //        }
        //    }


        //    //Continue
        //    Color[] btnContinue = new Color[2];
        //    btnContinue[0] = Color.FromArgb(255, 251, 251, 251);
        //    btnContinue[1] = Color.FromArgb(255, 51, 88, 130);
        //    User32.SetFocus(app.MainWindowHandle);
        //    pixel1 = Screen.GetColor(rect, 82.421875, 87.91666666666667, out point);
        //    pixel2 = Screen.GetColor(rect, 79.0625, 86.52777777777778);
        //    if (btnContinue[0] == pixel1 & btnContinue[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        //main quest extras
        //        if (input == "main")
        //        {
        //            Thread.Sleep(7000);
        //            point = Screen.PercentToPoint(rect, 5.00, 34.58333333333333);
        //            Mouse.LeftMouseClick(point.X, point.Y);

        //        }
        //    }

        //    //Skip Tutorial
        //    Color[] btnSkipTutorial = new Color[3];
        //    btnSkipTutorial[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnSkipTutorial[1] = Color.FromArgb(255, 255, 255, 255);
        //    btnSkipTutorial[2] = Color.FromArgb(255, 255, 255, 255);
        //    pixel1 = Screen.GetColor(rect, 90.703125, 5.972222222222222, out point);
        //    pixel2 = Screen.GetColor(rect, 90.703125, 5.416666666666667);
        //    pixel3 = Screen.GetColor(rect, 96.5625, 8.055555555555556);
        //    if (btnSkipTutorial[0] == pixel1 & btnSkipTutorial[1] != pixel2 & btnSkipTutorial[1] == pixel3)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //    }
        //    //Skip Tutorial>Ok
        //    Color[] btnSkipOk = new Color[2];
        //    btnSkipOk[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnSkipOk[1] = Color.FromArgb(255, 55, 91, 133);
        //    User32.SetFocus(app.MainWindowHandle);
        //    pixel1 = Screen.GetColor(rect, 58.515625, 66.66666666666667, out point);
        //    pixel2 = Screen.GetColor(rect, 57.578125, 65);
        //    if (btnSkipOk[0] == pixel1 & btnSkipOk[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        Thread.Sleep(1000);
        //    }
        //    //Weekly>Quest Compelte
        //    Color[] questComplete = new Color[2];
        //    questComplete[0] = Color.FromArgb(255, 49, 85, 127);
        //    questComplete[1] = Color.FromArgb(255, 255, 255, 255);
        //    pixel1 = Screen.GetColor(rect, 65.703125, 68.61111111111111);
        //    pixel2 = Screen.GetColor(rect, 66.71875, 69.44444444444444, out point);
        //    if (pixel1 == questComplete[0] & pixel2 == questComplete[1])
        //    {
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        Thread.Sleep(300);
        //    }
        //    //Weekly>Start Quest
        //    Color[] questStart = new Color[2];
        //    questStart[0] = Color.FromArgb(255, 255, 255, 255);
        //    questStart[1] = Color.FromArgb(255, 41, 56, 76);
        //    pixel1 = Screen.GetColor(rect, 68.046875, 68.19444444444444);
        //    pixel2 = Screen.GetColor(rect, 72.34375, 68.88888888888889, out point);
        //    MainWindow.main.UpdateLog = pixel1.ToString() + "?=" + questStart[0].ToString() + ", " + pixel2.ToString() + " ?= " + questStart[1].ToString();
        //    if (pixel1 == questStart[0] & pixel2 == questStart[1])
        //    {
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        Thread.Sleep(300);
        //    }
        //    //Weekly>Go Now
        //    Color[] questGoNow = new Color[2];
        //    questGoNow[0] = Color.FromArgb(255, 41, 55, 76);
        //    questGoNow[1] = Color.FromArgb(255, 255, 255, 255);
        //    pixel1 = Screen.GetColor(rect, 71.171875, 68.88888888888889);
        //    pixel2 = Screen.GetColor(rect, 72.265625, 68.47222222222222, out point);
        //    //MainWindow.main.UpdateLog = pixel1.ToString() + "?=" + questGoNow[0].ToString() + ", " + pixel2.ToString() + " ?= " + questGoNow[1].ToString();
        //    if (pixel1 == questGoNow[0] & pixel2 == questGoNow[1])
        //    {
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        Thread.Sleep(300);
        //    }
        //    //Weekly>Walk
        //    Color[] questWalk = new Color[2];
        //    questWalk[0] = Color.FromArgb(255, 41, 54, 72);
        //    questWalk[1] = Color.FromArgb(255, 120, 130, 140);
        //    pixel1 = Screen.GetColor(rect, 35.78125, 71.52777777777778);
        //    pixel2 = Screen.GetColor(rect, 36.953125, 72.63888888888889, out point);
        //    if (pixel1 == questWalk[0] & pixel2 == questWalk[1])
        //    {
        //        Mouse.LeftMouseClick(point.X, point.Y);
        //        Thread.Sleep(300);
        //    }


        //}
        public void QuestHelper(Process app, string input = "")
        {
            Rectangle rect = Screen.GetRect(app); //capture Screen location and demensions

            //variables
            Color pixel1;
            Color pixel2;
            Color pixel3;
            Point point = new Point();

            //Skip
            Color[] btnSkip = new Color[2];
            btnSkip[0] = Color.FromArgb(255, 255, 255, 255);
            btnSkip[1] = Color.FromArgb(255, 0, 0, 0);
            pixel1 = Screen.GetColor(rect, 90.78125, 69.86111111111111, out point);
            pixel2 = Screen.GetColor(rect, 97.109375, 82.5);
            if (Screen.CompareColor(pixel1, btnSkip[0], 2) & Screen.CompareColor(pixel2, btnSkip[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
            }

            //Accept Quest
            Color[] btnAcceptQuest = new Color[2];
            btnAcceptQuest[0] = Color.FromArgb(255, 255, 255, 255);
            btnAcceptQuest[1] = Color.FromArgb(255, 55, 91, 133);
            User32.SetFocus(app.MainWindowHandle);
            pixel1 = Screen.GetColor(rect, 60.3125, 84.44444444444444, out point);
            pixel2 = Screen.GetColor(rect, 53.75, 82.63888888888889);
            if (Screen.CompareColor(pixel1, btnAcceptQuest[0], 2) & Screen.CompareColor(pixel2, btnAcceptQuest[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
            }

            //Claim Reward
            Color[] btnClaimReward = new Color[2];
            btnClaimReward[0] = Color.FromArgb(255, 255, 255, 255);
            btnClaimReward[1] = Color.FromArgb(255, 59, 95, 136);
            pixel1 = Screen.GetColor(rect, 45.078125, 84.58333333333333, out point);
            pixel2 = Screen.GetColor(rect, 47.578125, 81.94444444444444);
            if (Screen.CompareColor(pixel1, btnClaimReward[0], 2) & Screen.CompareColor(pixel2, btnClaimReward[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
                //main quest extras
                if (input == "main")
                {
                    Thread.Sleep(4000);
                    point = Screen.PercentToPoint(rect, 5.00, 34.58333333333333);
                    Mouse.LeftMouseClick(point.X, point.Y);

                }
            }


            //Continue
            Color[] btnContinue = new Color[2];
            btnContinue[0] = Color.FromArgb(255, 251, 251, 251);
            btnContinue[1] = Color.FromArgb(255, 51, 88, 130);
            User32.SetFocus(app.MainWindowHandle);
            pixel1 = Screen.GetColor(rect, 82.421875, 87.91666666666667, out point);
            pixel2 = Screen.GetColor(rect, 79.0625, 86.52777777777778);
            if (Screen.CompareColor(pixel1, btnContinue[0], 2) & Screen.CompareColor(pixel2, btnContinue[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
                //main quest extras
                if (input == "main")
                {
                    Thread.Sleep(7000);
                    point = Screen.PercentToPoint(rect, 5.00, 34.58333333333333);
                    Mouse.LeftMouseClick(point.X, point.Y);

                }
            }

            //Skip Tutorial
            Color[] btnSkipTutorial = new Color[3];
            btnSkipTutorial[0] = Color.FromArgb(255, 255, 255, 255);
            btnSkipTutorial[1] = Color.FromArgb(255, 255, 255, 255);
            btnSkipTutorial[2] = Color.FromArgb(255, 255, 255, 255);
            pixel1 = Screen.GetColor(rect, 90.703125, 5.972222222222222, out point);
            pixel2 = Screen.GetColor(rect, 90.703125, 5.416666666666667);
            pixel3 = Screen.GetColor(rect, 96.5625, 8.055555555555556);
            if (Screen.CompareColor(pixel1, btnSkipTutorial[0], 2) & !Screen.CompareColor(pixel2, btnSkipTutorial[1], 2) & Screen.CompareColor(pixel3, btnSkipTutorial[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
            }
            //Skip Tutorial>Ok
            Color[] btnSkipOk = new Color[2];
            btnSkipOk[0] = Color.FromArgb(255, 255, 255, 255);
            btnSkipOk[1] = Color.FromArgb(255, 55, 91, 133);
            User32.SetFocus(app.MainWindowHandle);
            pixel1 = Screen.GetColor(rect, 58.515625, 66.66666666666667, out point);
            pixel2 = Screen.GetColor(rect, 57.578125, 65);
            if (Screen.CompareColor(pixel1, btnSkipOk[0], 2) & Screen.CompareColor(pixel2, btnSkipOk[1], 2))
            {
                Thread.Sleep(300);
                Mouse.LeftMouseClick(point.X, point.Y);
                Thread.Sleep(1000);
            }
            //Weekly>Quest Compelte
            Color[] questComplete = new Color[2];
            questComplete[0] = Color.FromArgb(255, 49, 85, 127);
            questComplete[1] = Color.FromArgb(255, 255, 255, 255);
            pixel1 = Screen.GetColor(rect, 65.703125, 68.61111111111111);
            pixel2 = Screen.GetColor(rect, 66.71875, 69.44444444444444, out point);
            if (Screen.CompareColor(pixel1, questComplete[0], 2) & Screen.CompareColor(pixel2, questComplete[1], 2))
            {
                Mouse.LeftMouseClick(point.X, point.Y);
                Thread.Sleep(300);
            }
            //Weekly>Start Quest
            Color[] questStart = new Color[2];
            questStart[0] = Color.FromArgb(255, 255, 255, 255);
            questStart[1] = Color.FromArgb(255, 41, 56, 76);
            pixel1 = Screen.GetColor(rect, 68.046875, 68.19444444444444);
            pixel2 = Screen.GetColor(rect, 72.34375, 68.88888888888889, out point);
            //MainWindow.main.UpdateLog = pixel1.ToString() + "?=" + questStart[0].ToString() + ", " + pixel2.ToString() + " ?= " + questStart[1].ToString();
            if (Screen.CompareColor(pixel1, questStart[0], 2) & Screen.CompareColor(pixel2, questStart[1], 2))
            {
                Mouse.LeftMouseClick(point.X, point.Y);
                Thread.Sleep(300);
            }
            //Weekly>Go Now
            Color[] questGoNow = new Color[2];
            questGoNow[0] = Color.FromArgb(255, 41, 55, 76);
            questGoNow[1] = Color.FromArgb(255, 255, 255, 255);
            pixel1 = Screen.GetColor(rect, 71.171875, 68.88888888888889);
            pixel2 = Screen.GetColor(rect, 72.265625, 68.47222222222222, out point);
            //MainWindow.main.UpdateLog = pixel1.ToString() + "?=" + questGoNow[0].ToString() + ", " + pixel2.ToString() + " ?= " + questGoNow[1].ToString();
            if (Screen.CompareColor(pixel1, questGoNow[0], 2) & Screen.CompareColor(pixel2, questGoNow[1], 2))
            {
                Mouse.LeftMouseClick(point.X, point.Y);
                Thread.Sleep(300);
            }
            //Weekly>Walk
            Color[] questWalk = new Color[2];
            questWalk[0] = Color.FromArgb(255, 41, 54, 72);
            questWalk[1] = Color.FromArgb(255, 120, 130, 140);
            pixel1 = Screen.GetColor(rect, 35.78125, 71.52777777777778);
            pixel2 = Screen.GetColor(rect, 36.953125, 72.63888888888889, out point);
            if (Screen.CompareColor(pixel1, questWalk[0], 2) & Screen.CompareColor(pixel2, questWalk[1], 2))
            {
                Mouse.LeftMouseClick(point.X, point.Y);
                Thread.Sleep(300);
            }


        }

        public void PopUpKiller(Process app)//WIP
        {
            Mouse mouse = new Mouse();
            Point clickPoint;
            Rectangle rect = Screen.GetRect(app);
            Color pixel1;
            Color pixel2;

            //Template
            Color[] btnTemp = new Color[2];
            btnTemp[0] = Color.FromArgb(255, 255, 255, 255);
            btnTemp[1] = Color.FromArgb(255, 0, 0, 0);
            pixel1 = Screen.GetColor(rect, 90.78125, 69.86111111111111, out clickPoint);
            pixel2 = Screen.GetColor(rect, 97.109375, 82.5);
            if (btnTemp[0] == pixel1 & btnTemp[1] == pixel2)
            {
                Thread.Sleep(300);
                User32.SetFocus(app.MainWindowHandle);
                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
            }

        }

        /// <summary>
        /// performs the main quest
        /// </summary>
        /// <param name="app">the process object for Nox Player/Android emulator</param>
        public void MainQuest(Process app)
        {
            User32.SetForegroundWindow(app.MainWindowHandle);
            bool appIsClosed = IsAppClosed(app);
            Rectangle rect = Screen.GetRect(app);

            Mouse mouse = new Mouse();
            Point clickPoint = Screen.PercentToPoint(rect, 5.00, 34.58333333333333);
            Color movePixel;
            Stopwatch mainTimer;

            mainTimer = new Stopwatch();
            mainTimer.Start();
            movePixel = Screen.GetColor(rect, 580, 450);
            rect = Screen.GetRect(app);
            if (mainTimer.ElapsedMilliseconds > 60000)
            {
                mainTimer.Stop();
                mainTimer.Reset();
                if (movePixel == Screen.GetColor(rect, 580, 450))
                {
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
                }
                if (movePixel != Screen.GetColor(rect, 580, 450))
                {
                    movePixel = Screen.GetColor(rect, 580, 450);
                }
                mainTimer.Start();
            }
            //QuestHelper(app, "main");
            //while(appIsClosed == false)
            //{
            //    appIsClosed = IsAppClosed(app);
            //    QuestHelper(app, "main");
            //    }
            if (appIsClosed == true)
            {
                OpenApp(app);
                appIsClosed = IsAppClosed(app);
                Thread.Sleep(750);
            }
            QuestHelper(app, "main");
        }
        //public void WeeklyQuest(Process app)
        //{
        //    User32.SetForegroundWindow(app.MainWindowHandle);
        //    //MainWindow.main.UpdateLog = app.MainWindowTitle + " to the front";
        //    bool appIsClosed = IsAppClosed(app);
        //    if (appIsClosed == false)
        //    {
               
        //        appIsClosed = IsAppClosed(app);
        //        Mouse mouse = new Mouse();
        //        Rectangle rect = Screen.GetRect(app);
        //        Point clickPoint = Screen.PercentToPoint(rect, 6.171875, 43.33333333333333);

        //        //Weekly Quest present
        //        Color[] weekly = new Color[2];
        //        weekly[0] = Color.FromArgb(255, 75, 154, 255);
        //        weekly[1] = Color.FromArgb(255, 75, 154, 255);
               
        //        Color pixel1 = Screen.GetColor(rect, 0.9375, 41.94444444444444);
        //        Color pixel2 = Screen.GetColor(rect, 6.171875, 43.33333333333333);
        //        if( pixel1 != weekly[0] & pixel2 != weekly[1] & IsCombatScreenUp(app) )//if weekly quest is NOT present then compplete these actions
        //        {
        //            MainWindow.main.UpdateLog = app.MainWindowTitle + ", no quest detected, clicking  quests, then the weekly tab";
        //            clickPoint = Screen.PercentToPoint(rect, 72.265625, 12.08333333333333);//quest log
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click quest log
        //            Thread.Sleep(2000);

                   
        //            clickPoint = Screen.PercentToPoint(rect, 8.671875, 40.41666666666667);//weekly
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click weekly
        //            Thread.Sleep(2000);
        //            QuestHelper(app);
        //        }

        //        if (pixel1 == weekly[0] & pixel2 == weekly[1] & IsCombatScreenUp(app))//if weekly quest IS present then compplete these actions
        //        {
        //            Color[] questDone = new Color[2];
        //            questDone[0] = Color.FromArgb(255, 255, 255, 255);
                   
        //            Color pixel3 = Screen.GetColor(rect, 243, 333);
        //            Color pixel4 = Screen.GetColor(rect, 245, 334);

        //            if (pixel3 == questDone[0] & pixel4 != questDone[0])
        //            {
        //                MainWindow.main.UpdateLog = app.MainWindowTitle + ", weekly quest done detected, clicking  quests, then the weekly tab";
        //                clickPoint = Screen.PercentToPoint(rect, 72.265625, 12.08333333333333);//quest log
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click quest log
        //                Thread.Sleep(2000);


        //                clickPoint = Screen.PercentToPoint(rect, 8.671875, 40.41666666666667);//weekly
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click weekly
        //                Thread.Sleep(2000);
        //                QuestHelper(app);
        //            }
        //        }
        //        QuestHelper(app);
        //    }
           
        //    if (appIsClosed == true)
        //    {
        //        OpenApp(app);
        //        appIsClosed = IsAppClosed(app);
        //    }



        //}
        public void WeeklyQuest(Process app)
        {
            User32.SetForegroundWindow(app.MainWindowHandle);
            //MainWindow.main.UpdateLog = app.MainWindowTitle + " to the front";
            bool appIsClosed = IsAppClosed(app);
            if (appIsClosed == false)
            {
               
                appIsClosed = IsAppClosed(app);
                Mouse mouse = new Mouse();
                Rectangle rect = Screen.GetRect(app);
                Point clickPoint = Screen.PercentToPoint(rect, 6.171875, 43.33333333333333);

                //Weekly Quest present
                Color[] weekly = new Color[2];
                weekly[0] = Color.FromArgb(255, 75, 154, 255);
                weekly[1] = Color.FromArgb(255, 75, 154, 255);
               
                Color pixel1 = Screen.GetColor(rect, 0.9375, 41.94444444444444);
                Color pixel2 = Screen.GetColor(rect, 6.171875, 43.33333333333333);
                if (!Screen.CompareColor(pixel1, weekly[0], 2) & !Screen.CompareColor(pixel2, weekly[1], 2) & IsCombatScreenUp(app))//if weekly quest is NOT present then compplete these actions
                {
                    MainWindow.main.UpdateLog = app.MainWindowTitle + ", no quest detected, clicking  quests, then the weekly tab";
                    clickPoint = Screen.PercentToPoint(rect, 72.265625, 12.08333333333333);//quest log
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click quest log
                    Thread.Sleep(2000);

                   
                    clickPoint = Screen.PercentToPoint(rect, 8.671875, 40.41666666666667);//weekly
                    Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click weekly
                    Thread.Sleep(2000);
                    QuestHelper(app);
                }

                if (Screen.CompareColor(pixel1, weekly[0], 2) & Screen.CompareColor(pixel2, weekly[1], 2) & IsCombatScreenUp(app))//if weekly quest IS present then compplete these actions
                {
                    Color[] questDone = new Color[2];
                    questDone[0] = Color.FromArgb(255, 255, 255, 255);
                   
                    Color pixel3 = Screen.GetColor(rect, 243, 333);
                    Color pixel4 = Screen.GetColor(rect, 245, 334);
                    if (Screen.CompareColor(pixel3, questDone[0], 2) & !Screen.CompareColor(pixel4, questDone[0], 2))
                    {
                        MainWindow.main.UpdateLog = app.MainWindowTitle + ", weekly quest done detected, clicking  quests, then the weekly tab";
                        clickPoint = Screen.PercentToPoint(rect, 72.265625, 12.08333333333333);//quest log
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click quest log
                        Thread.Sleep(2000);


                        clickPoint = Screen.PercentToPoint(rect, 8.671875, 40.41666666666667);//weekly
                        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);//click weekly
                        Thread.Sleep(2000);
                        QuestHelper(app);
                    }
                }
                QuestHelper(app);
            }
           
            if (appIsClosed == true)
            {
                OpenApp(app);
                appIsClosed = IsAppClosed(app);
            }



        }

    }
}
