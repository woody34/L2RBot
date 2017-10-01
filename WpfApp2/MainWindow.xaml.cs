using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace L2RBot
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Process[] Emulators;
        public int EmulatorCount = 0;
        Thread t;
        internal static MainWindow main;
        internal string UpdateLog
        {
            get { return txtLog.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { txtLog.Text += Environment.NewLine + value; })); }
            //usage
            //MainWindow.main.UpdateLog = "string data here";
        }
        public void ClearLog( Object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(new Action(() => { txtLog.Text = ""; }));
        }
        public IntPtr MainWindowHandle { get; private set; }
        public void priWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (t.ThreadState.Equals(System.Threading.ThreadState.Running))
            {
                t.Join();
            }
        }

        public MainWindow()
        {
            main = this;
            InitializeComponent();
            
        }
        public void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if(t.IsAlive == true)
            {
                //t.Interrupt();
                t.Abort();
                
            }
            btnStopBot.IsEnabled = false;
            btnProcessGrab.IsEnabled = true;
            //initialize variables
            t = null;
            EmulatorCount = 0;


        }
        public delegate void UpdateLogCallback(string text);
        
        public void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int)this.Height, (int)this.Width, 1);
            t  = new Thread(MainBot);
            t.Start();
            
        }
        public void MainBot()
        {

            //MainQuest[] bots = new MainQuest[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                //bots[ind] = new MainQuest(Emulators[ind]);
                Rectangle screen = Screen.GetRect(Emulators[ind]);
                User32.SetWindowPos(Emulators[ind].MainWindowHandle, 0, 0, 0, screen.Height, screen.Width, 1);//moves each screen to 0,0 point
            }


            while (true)//replace with start stop button states
            {

                for (int ind = EmulatorCount - 1; ind >= 0; ind--)
                {
                    if (Emulators[ind].HasExited == true)
                    {
                        MainWindow.main.UpdateLog = Emulators[ind].MainWindowTitle + " has terminated. Please stop bot.";
                        return;
                    }
                    //bots[ind].Start();
                }
            }
        }

        public void BtnWeekly_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int)this.Height, (int)this.Width, 1);
            t = new Thread(WeeklyBot);
            t.Start();
        }
        public void WeeklyBot()
        {
            Weekly[] bots = new Weekly[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Weekly(Emulators[ind]);
                Rectangle screen = Screen.GetRect(Emulators[ind]);
                User32.SetWindowPos(Emulators[ind].MainWindowHandle, 0, 0, 0, screen.Width, screen.Width, 1);
            }


            while (true)//replace with start stop button states
            {
                for (int ind = EmulatorCount - 1; ind >= 0; ind--)
                {
                    if (Emulators[ind].HasExited == true)
                    {
                        MainWindow.main.UpdateLog = Emulators[ind].MainWindowTitle + " has terminated. Please stop bot.";
                        return;
                    }
                    //need to present users with an option for item slot
                    //if (bots[ind].ScollItemNumber == 0)
                    //{
                    //    bots[ind].ScollItemNumber = InputBox();
                    //}
                    bots[ind].Start();
                }
            }
        }

        private void btnScroll_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int)this.Height, (int)this.Width, 1);
            t = new Thread(ScrollBot);
            t.Start();
        }
        public void ScrollBot()
        {
            Scroll[] bots = new Scroll[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Scroll(Emulators[ind]);
                Rectangle screen = Screen.GetRect(Emulators[ind]);
                User32.SetWindowPos(Emulators[ind].MainWindowHandle, 0, 0, 0, screen.Width, screen.Width, 1);
            }


            while (true)//replace with start stop button states
            {
                for (int ind = EmulatorCount - 1; ind >= 0; ind--)
                {
                    if (Emulators[ind].HasExited == true)
                    {
                        MainWindow.main.UpdateLog = Emulators[ind].MainWindowTitle + " has terminated. Please stop bot.";
                        return;
                    }
                    bots[ind].Start();
                }
            }
        }

        private void btnDaily_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(DailyBot);
            t.Start();
        }
        public void DailyBot()
        {
            DailyDungeon[] bots = new DailyDungeon[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new DailyDungeon(Emulators[ind]);
                Rectangle screen = Screen.GetRect(Emulators[ind]);
                User32.SetWindowPos(Emulators[ind].MainWindowHandle, 0, 0, 0, screen.Width, screen.Width, 1);
            }


            while (true)//replace with start stop button states
            {
                for (int ind = EmulatorCount - 1; ind >= 0; ind--)
                {
                    if (Emulators[ind].HasExited == true)
                    {
                        MainWindow.main.UpdateLog = Emulators[ind].MainWindowTitle + " has terminated. Please stop bot.";
                        return;
                    }
                    bots[ind].Start();
                }
            }
        }

        public void DisableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = false;
            btnWeekly.IsEnabled = false;
            btnScroll.IsEnabled = false;
            btnProcessGrab.IsEnabled = false;
            btnDaily.IsEnabled = false;

            //enables stop button
            btnStopBot.IsEnabled = true;
        }
        public void EnableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = true;
            btnWeekly.IsEnabled = true;
            btnScroll.IsEnabled = true;
            btnProcessGrab.IsEnabled = true;
            btnDaily.IsEnabled = true;

            //enables stop button
            btnStopBot.IsEnabled = false;
        }

        private void BtnProcessGrab_Click(object sender, RoutedEventArgs e)
        {
            listProcessList.Items.Clear();
            Process[] NoxPlayers = Bot.BindNoxPlayer();
            if (NoxPlayers == null)//value check
            {

                MainWindow.main.UpdateLog = "null process value ProcessGrabber_Click";
                return;
            }
            if (NoxPlayers != null)//enbale buttons for quest if we bind to the Nox player process
            {
                EnableButtons();
                listProcessList.IsEnabled = true;
                listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
            }
            foreach (Process pro in NoxPlayers)
            {
                if (pro == null)
                {
                    MainWindow.main.UpdateLog = "null process value ProcessGrabber_Click";
                    return;
                }

                ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };
                listProcessList.Items.Add(itm);
                EmulatorCount++;
            }

            Emulators =  NoxPlayers;
        }

    }
    public static class ResizeArray
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }
}
