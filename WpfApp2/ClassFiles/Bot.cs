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
        /// Checks to see if the green wifi logo is visible indicating that the combat Screen is displayed.
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>true: once wifi logo is detected</returns>
        public static Boolean IsCombatScreenUp(Process proc)
        {
            Rectangle rect = Screen.GetRect(proc);
            Color wifi = new Color();
            wifi = Color.FromArgb(255, 79, 136, 42);
            Color pixel1 = Screen.GetColor(rect, 10, 708);

            if (Screen.CompareColor(pixel1, wifi, 2))
            {
                if (pixel1 == wifi)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// returns array of open proccesses with matching
        /// </summary>
        /// <param name="ProcName"></param>
        /// <returns></returns>
        public static Process[] GetOpenProcess(string ProcName)
        {
            Process[] noxPlayers;
            Process[] WindowsProcesses = Process.GetProcesses();
            int i = 0;

            foreach (Process pro in WindowsProcesses)
            {
                if (ProcName.Equals(pro.ProcessName))
                {
                    i++;
                }
            }
            noxPlayers = new Process[i];
            i = 0;
            foreach (Process pro in WindowsProcesses)
            {
                if (ProcName.Equals(pro.ProcessName))
                {
                    noxPlayers[i] = Process.GetProcessById(pro.Id);
                    MainWindow.main.UpdateLog = pro.MainWindowTitle + " detected!";
                    i++;
                }
            }
            return noxPlayers;
        }


        //Legacy
        ///// <summary>
        ///// opens Nox Player
        ///// </summary>
        ///// <returns>Process object for Nox Player</returns>
        //public Process OpenNox()
        //{
        //    Process[] noxPlayer = Process.GetProcesses();
        //    Process[] nPID = new Process[20];// max of 20 nox players open

        //    Thread.Sleep(100);
        //    for (int j = 0; j< noxPlayer.Length;j++)
        //    {
        //        // DELTE ME
        //        Console.Write("Line Number:" + j);

        //        var droidProcess = noxPlayer[j];

        //        if(droidProcess!=null)
        //        {
        //            if ("Nox".Equals(droidProcess.ProcessName) || "NoxPlayer1".Equals(droidProcess.ProcessName))
        //            {
        //                nPID[j] = Process.GetProcessById(droidProcess.Id);
        //            }
        //        }

        //    }


        //    if(nPID[0] == null)
        //    {
        //        Process Nox = new Process();
        //        Nox.StartInfo.FileName = @"C:\Program Files (x86)\Nox\bin\Nox.exe";
        //        Nox.Start();
        //        Nox.WaitForInputIdle();
        //        return Nox;
        //    }
        //    return nPID[0];




        //}
        /// <summary>
        /// Looks at pixel locations to determine if the game has crashed and the Nox Player has returned to the desktop
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>True: upon detecting the Lineage 2 Revolutions Logo on the Nox Player desktop at R2:C5</returns>
        /// <returns>False: upon failing to detect the Lineage 2 Revolutions Logo on the Nox Player desktop at R2:C5</returns>
        //public static Boolean IsAppClosed(Process proc)
        //{
        //    bool result = false;

        //    Rectangle rect = Screen.GetRect(proc);

        //    Color[] isAppClosed = new Color[2];
        //    isAppClosed[0] = Color.FromArgb(255,87,179,183);
        //    isAppClosed[1] = Color.FromArgb(255, 161, 161, 161);

        //    Color pixel1 = Screen.GetColor(rect, 83.046875, 47.63888888888889);
        //    Color pixel2 = Screen.GetColor(rect, 84.453125, 44.44444444444444);
        //    if (Screen.CompareColor(pixel1, isAppClosed[0], 2) & Screen.CompareColor(pixel2, isAppClosed[1], 2))
        //    {
        //        result = true;
        //    }

        //    return result;
        //}
        ///// <summary>
        ///// checks to see if bag is full
        ///// </summary>
        ///// <param name="proc"></param>
        ///// <returns>true: if full bag icon is detected</returns>
        //public static Boolean IsItemFull(Process proc)
        //{
        //    bool bagIsFull = false;

        //    Rectangle rect = Screen.GetRect(proc);
        //    Color[] isItemFull = new Color[2];
        //    isItemFull[0] = Color.FromArgb(255, 64, 218, 250);
        //    isItemFull[1] = Color.FromArgb(255, 219, 220, 217);

        //    Color pixel1 = Screen.GetColor(rect, 75.78125, 7.083333333333333);
        //    Color pixel2 = Screen.GetColor(rect, 76.640625, 2.5);

        //    if (Screen.CompareColor(pixel1, isItemFull[0], 2) && Screen.CompareColor(pixel2, isItemFull[1], 2))
        //    {
        //        bagIsFull = true;
        //    }

        //    return bagIsFull;

        //}

        ///// <summary>
        ///// Opens Lineage 2 Revolutions from within Nox Plater.
        ///// </summary>
        ///// <param name="app">Nox Player's Process</param>
        ///// <returns>true: upon completion</returns>
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
        //public static Boolean OpenApp(Process app)
        //{
        //    #region OpenApp variables
        //    Mouse mouse = new Mouse();

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
        //    Color pixel1 = Screen.GetColor(rect, 83.046875, 47.63888888888889, out Point clickPoint);
        //    Color pixel2 = Screen.GetColor(rect, 84.453125, 44.44444444444444);

        //    if (Screen.CompareColor(pixel1, isAppClosed[0], 2) & Screen.CompareColor(pixel2, isAppClosed[1], 2))
        //    {

        //        //Thread.Sleep(5000);
        //        //Click L2R app icon
        //        //Mouse.LeftClick(clickPoint, app);
        //        User32.SetFocus(app.MainWindowHandle);
        //        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);

        //        //System.Windows.MessageBox.Show(pixel1.ToString() + "==" + isAppClosed[0].ToString() + ":" + pixel2.ToString() + "==" + isAppClosed[1].ToString() + " Click:" + clickPoint.ToString());
        //        Thread.Sleep(1000);

        //        do
        //        {
        //            //Look for the blue buttons that indicate login potential
        //            if (Screen.CompareColor(pixel1, btnBlue[0], 2) & Screen.CompareColor(pixel2, btnBlue[1], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 53.125, 67.08333333333333, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 56.71875, 68.88888888888889);
        //                Thread.Sleep(100);
        //            }
        //            //Click 'Tap to Start' upon finding the blue buttons
        //            if (Screen.CompareColor(pixel1, btnBlue[0], 2) & Screen.CompareColor(pixel2, btnBlue[1], 2))
        //            {
        //                Screen.GetColor(rect, 47.578125, 84.58333333333333, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Thread.Sleep(100);
        //                //Mouse.LeftClick(clickPoint, app);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            }
        //            //Look for annoying login pop-ups
        //            if (!Screen.CompareColor(pixel1, btnClose[0], 2) & !Screen.CompareColor(pixel2, btnClose[1], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 91.484375, 5);
        //                Thread.Sleep(100);
        //            }
        //            //Hit escape to close anooying login pop-ups
        //            if (Screen.CompareColor(pixel1, btnClose[0], 2) & Screen.CompareColor(pixel2, btnClose[1], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 92.5, 4.861111111111111, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Thread.Sleep(100);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);

        //            }
        //            //Look for green 'Play' button
        //            if (!Screen.CompareColor(pixel1, btnPlay[0], 2) & !Screen.CompareColor(pixel2, btnPlay[1], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
        //                pixel2 = Screen.GetColor(rect, 87.5, 88.61111111111111);
        //                Thread.Sleep(100);
        //            }
        //            //Click green 'Play' button
        //            if (Screen.CompareColor(pixel1, btnPlay[0], 2) & Screen.CompareColor(pixel2, btnPlay[1], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 82.578125, 89.86111111111111, out clickPoint);
        //                User32.SetFocus(app.MainWindowHandle);
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //                Thread.Sleep(100);
        //            }
        //            //Look for the green wifi logo upon succesful login
        //            if (!Screen.CompareColor(pixel1, wifi[0], 2))
        //            {
        //                pixel1 = Screen.GetColor(rect, 0.78125, 98.47222222222222, out clickPoint);
        //                Thread.Sleep(100);
        //            }
        //        }
        //        while (!IsCombatScreenUp(app));
        //    }
        //    return true;
        //}


        ///// <summary>
        ///// Performs a bulk sale operation when bag is detected to be full
        ///// </summary>
        ///// <param name="app"></param>
        ///// <returns>true: once complete</returns>
        //public static Boolean BulkSale(Process app)
        //{
        //    Mouse mouse = new Mouse();
        //    Rectangle rect = Screen.GetRect(app);
        //    Color pixel1;
        //    Color pixel2;
        //    bool saleComplete = false;
        //    User32.SetFocus(app.MainWindowHandle);
        //    while (saleComplete == false)
        //    {
        //        //full bag icon
        //        Color[] isItemFull = new Color[2];
        //        isItemFull[0] = Color.FromArgb(255, 64, 218, 250);
        //        isItemFull[1] = Color.FromArgb(255, 219, 220, 217);

        //        User32.SetFocus(app.MainWindowHandle);
        //        pixel1 = Screen.GetColor(rect, 75.78125, 7.083333333333333, out Point clickPoint);
        //        pixel2 = Screen.GetColor(rect, 76.640625, 2.5);
        //        if (Screen.CompareColor(pixel1, isItemFull[0], 2) & Screen.CompareColor(pixel2, isItemFull[1], 2))
        //        {
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            Thread.Sleep(100);
        //        }

        //        //'Bulk Sale' button
        //        Color[] btnBulkSale = new Color[2];
        //        btnBulkSale[0] = Color.FromArgb(255, 255, 255, 255);
        //        btnBulkSale[1] = Color.FromArgb(255, 52, 66, 82);
        //        User32.SetFocus(app.MainWindowHandle);
        //        pixel1 = Screen.GetColor(rect, 71.09375, 94.02777777777778, out clickPoint);
        //        pixel2 = Screen.GetColor(rect, 72.421875, 91.80555555555556);
        //        if (Screen.CompareColor(pixel1, btnBulkSale[0], 2) & Screen.CompareColor(pixel2, btnBulkSale[1], 2))
        //        {
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            Thread.Sleep(100);
        //        }

        //        //'Sale' button.green
        //        Color[] btnSellGreen = new Color[2];
        //        btnSellGreen[0] = Color.FromArgb(255, 251, 248, 237);
        //        btnSellGreen[1] = Color.FromArgb(255, 80, 130, 84);
        //        User32.SetFocus(app.MainWindowHandle);
        //        pixel1 = Screen.GetColor(rect, 91.5625, 93.75, out clickPoint);
        //        pixel2 = Screen.GetColor(rect, 90.078125, 93.61111111111111);
        //        if (Screen.CompareColor(pixel1, btnSellGreen[0], 2) & Screen.CompareColor(pixel2, btnSellGreen[1], 2))
        //        {
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            Thread.Sleep(100);
        //        }

        //        //'Sale' button.blue
        //        Color[] btnSellBlue = new Color[2];
        //        btnSellBlue[0] = Color.FromArgb(255, 255, 255, 255);
        //        btnSellBlue[1] = Color.FromArgb(255, 49, 85, 127);
        //        User32.SetFocus(app.MainWindowHandle);
        //        pixel1 = Screen.GetColor(rect, 58.125, 68.19444444444444, out clickPoint);
        //        pixel2 = Screen.GetColor(rect, 57.109375, 68.19444444444444);
        //        if (Screen.CompareColor(pixel1, btnSellBlue[0], 2) & Screen.CompareColor(pixel2, btnSellBlue[1], 2))
        //        {
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            Thread.Sleep(100);
        //        }

        //        //'Ok' button
        //        Color[] btnOk = new Color[2];
        //        btnOk[0] = Color.FromArgb(255, 255, 255, 255);
        //        btnOk[1] = Color.FromArgb(255, 49, 85, 127);
        //        User32.SetFocus(app.MainWindowHandle);
        //        pixel1 = Screen.GetColor(rect, 48.90625, 66.94444444444444, out clickPoint);
        //        pixel2 = Screen.GetColor(rect, 47.96875, 66.25);
        //        if (Screen.CompareColor(pixel1, btnOk[0], 2) & Screen.CompareColor(pixel2, btnOk[1], 2))
        //        {
        //            Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //            Thread.Sleep(2000);
        //            Color[] btnClose = new Color[2];
        //            btnClose[0] = Color.FromArgb(255, 28, 31, 40);
        //            btnClose[1] = Color.FromArgb(255, 216, 216, 218);
        //            User32.SetFocus(app.MainWindowHandle);
        //            pixel1 = Screen.GetColor(rect, 96.71875, 2.777777777777778, out clickPoint);
        //            pixel2 = Screen.GetColor(rect, 96.640625, 4.027777777777778);
        //            if (Screen.CompareColor(pixel1, btnClose[0], 2) & Screen.CompareColor(pixel2, btnClose[1], 2))
        //            {
        //                Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //                Thread.Sleep(1000);
        //                saleComplete = true;
        //                return true;
        //            }
        //        }

        //        //close button; top right, looks like a door with an arrow



        //    }
        //    return false;
        //}//WIP
        ///// <summary>
        ///// Keeps quests going to navigating through the popup menus and driving the quest
        ///// </summary>
        ///// <param name="app">the process object for Nox Player/Android emulator</param>
        ///// <param name="input"></param>

        //public static void PopUpKiller(Process app)//WIP
        //{
        //    Mouse mouse = new Mouse();
        //    Rectangle rect = Screen.GetRect(app);
        //    Color pixel1;
        //    Color pixel2;

        //    //Template
        //    Color[] btnTemp = new Color[2];
        //    btnTemp[0] = Color.FromArgb(255, 255, 255, 255);
        //    btnTemp[1] = Color.FromArgb(255, 0, 0, 0);
        //    pixel1 = Screen.GetColor(rect, 90.78125, 69.86111111111111, out Point clickPoint);
        //    pixel2 = Screen.GetColor(rect, 97.109375, 82.5);
        //    if (btnTemp[0] == pixel1 & btnTemp[1] == pixel2)
        //    {
        //        Thread.Sleep(300);
        //        User32.SetFocus(app.MainWindowHandle);
        //        Mouse.LeftMouseClick(clickPoint.X, clickPoint.Y);
        //    }

        //}

    }
}
