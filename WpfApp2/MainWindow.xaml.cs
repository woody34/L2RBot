using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Drawing;

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
            set { Dispatcher.Invoke(new Action(() => { txtLog.Text += Environment.NewLine + value;  })); }
            //usage
            //MainWindow.main.UpdateLog = "string data here";
        }
        private void ClearLog ( object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { txtLog.Text = ""; }));
        }
        



        public IntPtr MainWindowHandle { get; private set; }

        public MainWindow()
        {
            main = this;
            InitializeComponent();
        }
        public void btnStopBot_Click(object sender, RoutedEventArgs e)
        {
            if(t.IsAlive == true)
            {
                //t.Interrupt();
                t.Abort();
                
            }
            btnStopBot.IsEnabled = false;
            //initialize variables
            t = null;
            EmulatorCount = 0;

        }
        public delegate void UpdateLogCallback(string text);
        
        public void btnMain_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int)this.Height, (int)this.Width, 1);
            t  = new Thread(mainBot);
            t.Start();
            
        }
        public void mainBot()
        {

            Bot[] bots = new Bot[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Bot();
                Rectangle screen = Screen.GetRect(Emulators[ind]);
                User32.SetWindowPos(Emulators[ind].MainWindowHandle, 0, 0, 0, screen.Height, screen.Width, 1);
            }
            

            while (true)//replace with start stop button states
            {
                
                for (int ind = EmulatorCount - 1; ind >= 0; ind--)
                {
                    if(Emulators[ind].HasExited == true)
                    {
                        MainWindow.main.UpdateLog = Emulators[ind].MainWindowTitle + " has terminated. Please stop bot.";
                        return;
                    }
                    bots[ind].MainQuest(Emulators[ind]);
                }
            }
        }

        public void btnWeekly_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1300, 0, (int)this.Height, (int)this.Width, 1);
            t = new Thread(weeklyBot);
            t.Start();
        }
        public void weeklyBot()
        {
            Bot[] bots = new Bot[EmulatorCount];
            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Bot();
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
                    bots[ind].WeeklyQuest(Emulators[ind]);
                }
            }
        }

        public void disableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = false;
            btnWeekly.IsEnabled = false;

            //enables stop button
            btnStopBot.IsEnabled = true;
        }
        public void enableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = true;
            btnWeekly.IsEnabled = true;

            //enables stop button
            btnStopBot.IsEnabled = false;
        }

        private void btnProcessGrab_Click(object sender, RoutedEventArgs e)
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
                enableButtons();
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

                ListBoxItem itm = new ListBoxItem();
                itm.Content = pro.MainWindowTitle.ToString();
                listProcessList.Items.Add(itm);
                EmulatorCount++;
            }

            Emulators =  NoxPlayers;
        }
    }
}
