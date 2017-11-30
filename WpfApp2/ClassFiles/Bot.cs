using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using L2RBot.Common;

namespace L2RBot
{
    class Bot
    {

        static Pixel WifiLogo = new Pixel
        {
            Color = Colors.WifiLogo,

            Point = new Point(8, 708)
        };
        /// <summary>
        /// Checks to see if the green wifi logo is visible indicating that the combat Screen is displayed.
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>true: once wifi logo is detected</returns>
        public static Boolean IsCombatScreenUp(Process proc)
        {
            Rectangle _screen = Screen.GetRect(proc);

            return (WifiLogo.IsPresent(_screen, 4)) ? true : false;
        }

        //Refactored

        ///// <summary>
        ///// returns array of open proccesses with matching
        ///// </summary>
        ///// <param name="ProcName"></param>
        ///// <returns></returns>
        //public static Process[] GetOpenProcess(string ProcName)
        //{
        //    Process[] noxPlayers;
        //    Process[] AllWindowsProcesses = Process.GetProcesses();
        //    int i = 0;

        //    foreach (Process pro in AllWindowsProcesses)
        //    {
        //        if (ProcName.Equals(pro.ProcessName))
        //        {
        //            i++;
        //        }
        //    }
        //    noxPlayers = new Process[i];
        //    i = 0;
        //    foreach (Process pro in AllWindowsProcesses)
        //    {
        //        if (ProcName.Equals(pro.ProcessName))
        //        {
        //            noxPlayers[i] = Process.GetProcessById(pro.Id);
        //            MainWindow.main.UpdateLog = pro.MainWindowTitle + " detected!";
        //            i++;
        //        }
        //    }
        //    return noxPlayers;
        //}
        /// <summary>
        /// returns array of open proccesses with matching
        /// </summary>
        /// <param name="ProcName"></param>
        /// <returns></returns>
        public static Process[] GetOpenProcess(string ProcName, string t = "")
        {
            Process[] AllWindowsProcesses = Process.GetProcesses();

            List<Process> noxPlayerList = new List<Process>();

            foreach (Process pro in AllWindowsProcesses)
            {
                if (ProcName.Equals(pro.ProcessName))
                {
                    noxPlayerList.Add(pro);
                }
            }

            Process[] noxPlayerArray = new Process[noxPlayerList.Count];

            for (int i = 0; i < noxPlayerList.Count; i++)
            {
                noxPlayerArray[i] = noxPlayerList[i];
            }

            return noxPlayerArray;
        }
    }
}
