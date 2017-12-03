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
        private static Screen _screenObj;

        static Pixel WifiLogo = new Pixel
        {
            Color = Colors.WifiLogo,

            Point = new Point(8, 708)
        };

        /// <summary>
        /// Checks to see if the green wifi logo is visible indicating that the combat Screen is displayed.
        /// </summary>
        /// <param name="App"></param>
        /// <returns>true: once wifi logo is detected</returns>
        public static Boolean IsCombatScreenUp(Process App)
        {
            _screenObj = new Screen();

            Rectangle _screen = _screenObj.GetRect(App);

            return (WifiLogo.IsPresent(_screen, 4)) ? true : false;
        }

        /// <summary>
        /// Returns array of open proccesses with matching ProcessName
        /// </summary>
        /// <param name="ProcName">Main ProcessName</param>
        /// <param name="PName">Secondary ProcessName</param>
        /// <returns></returns>
        public static Process[] GetOpenProcess(string ProcName, string PName = "")
        {

            Process[] AllWindowsProcesses = Process.GetProcesses();

            List<Process> noxPlayerList = new List<Process>();

            foreach (Process pro in AllWindowsProcesses)
            {
                if (ProcName.Equals(pro.ProcessName))
                {
                    if(pro.MainWindowTitle != "")
                    {
                        noxPlayerList.Add(pro);
                    }
                    
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
