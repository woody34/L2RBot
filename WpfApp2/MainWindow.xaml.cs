using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Drawing;
using L2RBot.Common.Enum;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Windows.Input;
using log4net;
using NHotkey.Wpf;
using NHotkey;
using System.Reflection;
using System.IO;
using static L2RBot.User32;

namespace L2RBot
{

    public partial class MainWindow : Window
    {
        //Globals
        private Process[] Emulators;

        private int EmulatorCount = 0;

        private Thread t;

        internal static MainWindow main;

        internal string UpdateLog
        {
            get { return txtLog.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { txtLog.Text = value + Environment.NewLine + txtLog.Text; })); }
            //usage
            //MainWindow.main.UpdateLog = "string data here";
        }

        public delegate void UpdateLogCallback(string text);

        //Properties
        public IntPtr MainWindowHandle { get; private set; }

        //Constructors
        public MainWindow()
        {

            main = this;

            InitializeComponent();

            BuildToolTips();

            HKeyBinding();
        }

        private void HKeyBinding()
        {
            HotkeyManager.Current.AddOrReplace("Stop", Key.S, ModifierKeys.Alt | ModifierKeys.Control, BtnStop_Click);

            HotkeyManager.Current.AddOrReplace("Exit", Key.E, ModifierKeys.Alt | ModifierKeys.Control, BtnExit_Click);

            HotkeyManager.Current.AddOrReplace("Find", Key.F, ModifierKeys.Alt | ModifierKeys.Control, BtnProcessGrab_Click);
        }

        //Methods
        private void PriWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Update_BtnProcessGrab(Object sender, RoutedEventArgs e)
        {
            if (CbItemNox != null)
            {
                if (CbItemNox.IsSelected)
                {
                    btnProcessGrab.Content = "Find Nox(Ctl+Alt+F) ";
                }
            }
            if (CbItemBS != null)
            {
                if (CbItemBS.IsSelected)
                {
                    btnProcessGrab.Content = "Find BS(Ctl+Alt+F) ";
                }
            }

        }

        #region Bot_Actions_Tab
        private void EnableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = true;

            btnWeekly.IsEnabled = true;

            btnScroll.IsEnabled = true;

            //turned them off until i fix them
            //btnDaily.IsEnabled = true;

            //btnTower.IsEnabled = true;

            //btnExp.IsEnabled = true;

            //btnAoM.IsEnabled = true;


            btnProcessGrab.IsEnabled = false;


            //enables stop button
            btnStopBot.IsEnabled = false;

            //Clears Log
            txtLog.Text = "";

            btnExitBot.IsEnabled = false;

            btnStopBot.IsEnabled = true;
        }

        private void DisableButtons()
        {
            //disable buttons after clicking to prevent multithread issues
            btnMain.IsEnabled = false;

            btnWeekly.IsEnabled = false;

            btnScroll.IsEnabled = false;

            btnDaily.IsEnabled = false;

            btnTower.IsEnabled = false;

            btnExp.IsEnabled = false;

            btnAoM.IsEnabled = false;

            btnProcessGrab.IsEnabled = false;


            //enables stop button
            btnStopBot.IsEnabled = true;
        }

        //Button Events
        private void ClearLog_Clicked(Object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { txtLog.Text = ""; }));
        }

        private void BtnProcessGrab_Click(object sender, RoutedEventArgs e)
        {
            //Do NOT forget to also change overload method below
            if (btnProcessGrab.IsEnabled)
            {
                listProcessList.Items.Clear();

                listProcessList.SelectionMode = SelectionMode.Multiple;

                Process[] EmulatorProcess;

                if (CbItemNox.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("Nox");

                    if (EmulatorProcess == null)//value check
                    {

                        UpdateLog = "null process value ProcessGrabber_Click";
                        return;
                    }

                    if (EmulatorProcess != null)//enbale buttons for quest if we bind to the Nox player process
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (Process pro in EmulatorProcess)
                    {
                        if (pro == null)
                        {
                            UpdateLog = "Null Process";

                            return;
                        }

                        ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                        listProcessList.Items.Add(itm);

                        EmulatorCount++;
                    }

                    Emulators = EmulatorProcess;
                }

                if (CbItemBS.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("Bluestacks");

                    if (EmulatorProcess == null)//value check
                    {

                        UpdateLog = "Null process value ProcessGrabber_Click";
                        return;
                    }

                    if (EmulatorProcess != null)//enbale buttons for quest if we bind to the Nox player process
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (Process pro in EmulatorProcess)
                    {
                        if (pro == null)
                        {
                            UpdateLog = "Null Process";

                            return;
                        }

                        if(pro.MainWindowTitle != "")
                        {
                            ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                            listProcessList.Items.Add(itm);

                            EmulatorCount++;
                        }
                    }

                    Emulators = EmulatorProcess;
                }

                if (listProcessList.HasItems)
                {
                    btnProcessGrab.IsEnabled = false;
                }

                UpdateLog = "Select any process that you would like the bot to ignore.";
            }
        }

        //HotKey Overload
        private void BtnProcessGrab_Click(object sender, HotkeyEventArgs e)
        {
            if (btnProcessGrab.IsEnabled)
            {
                listProcessList.Items.Clear();

                listProcessList.SelectionMode = SelectionMode.Multiple;

                Process[] EmulatorProcess;

                if (CbItemNox.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("Nox");

                    if (EmulatorProcess == null)//value check
                    {

                        UpdateLog = "null process value ProcessGrabber_Click";
                        return;
                    }

                    if (EmulatorProcess != null)//enbale buttons for quest if we bind to the Nox player process
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (Process pro in EmulatorProcess)
                    {
                        if (pro == null)
                        {
                            UpdateLog = "Null Process";

                            return;
                        }

                        ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                        listProcessList.Items.Add(itm);

                        EmulatorCount++;
                    }

                    Emulators = EmulatorProcess;
                }

                if (CbItemBS.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("Bluestacks");

                    if (EmulatorProcess == null)//value check
                    {

                        UpdateLog = "Null process value ProcessGrabber_Click";
                        return;
                    }

                    if (EmulatorProcess != null)//enbale buttons for quest if we bind to the Nox player process
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (Process pro in EmulatorProcess)
                    {
                        if (pro == null)
                        {
                            UpdateLog = "Null Process";

                            return;
                        }

                        if (pro.MainWindowTitle != "")
                        {
                            ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                            listProcessList.Items.Add(itm);

                            EmulatorCount++;
                        }
                    }

                    Emulators = EmulatorProcess;
                }

                if (listProcessList.HasItems)
                {
                    btnProcessGrab.IsEnabled = false;
                }

                UpdateLog = "Select any process that you would like the bot to ignore.";
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            //Do NOT forget to also change overload method below
            if (btnStopBot.IsEnabled)
            {
                if (t != null)
                {
                    t.Abort();
                }

                DisableButtons();

                btnExitBot.IsEnabled = true;

                btnStopBot.IsEnabled = false;

                btnProcessGrab.IsEnabled = true;

                //initialize variables
                t = null;

                EmulatorCount = 0;

                listProcessList.Items.Clear();

                listProcessList.Background = System.Windows.Media.Brushes.Red;
            }

        }

        //HotKey Overload
        private void BtnStop_Click(object sender, HotkeyEventArgs e)
        {
            if (btnStopBot.IsEnabled)
            {
                if (t != null)
                {
                    t.Abort();
                }

                DisableButtons();

                btnExitBot.IsEnabled = true;

                btnStopBot.IsEnabled = false;

                btnProcessGrab.IsEnabled = true;

                //initialize variables
                t = null;

                EmulatorCount = 0;

                listProcessList.Items.Clear();

                listProcessList.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //Do NOT forget to also change overload method below
            if (btnExitBot.IsEnabled)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        //HotKey Overload
        private void BtnExit_Click(object sender, HotkeyEventArgs e)
        {
            if (btnExitBot.IsEnabled)
            {
                System.Windows.Application.Current.Shutdown();
            }

        }

        private void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(MainBot);
            t.Start();

        }

        private void BtnWeekly_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(WeeklyBot);
            t.Start();
        }

        private void BtnScroll_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(ScrollBot);
            t.Start();
        }

        private void BtnDaily_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(DailyBot);
            t.Start();
        }

        private void BtnTOI_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(TOIBot);
            t.Start();
        }

        private void BtnExp_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(ExpBot);
            t.Start();
        }

        private void BtnAoM_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            User32.SetWindowPos(this.MainWindowHandle, 0, 1330, 0, (int) this.Height, (int) this.Width, 1);
            t = new Thread(AoMBot);
            t.Start();
        }
        #endregion

        #region Settings_Tab
        private void DeathCountShow(object sender, RoutedEventArgs e)
        {
            DeathCount.IsEnabled = true;
        }

        private void DeathCountHide(object sender, RoutedEventArgs e)
        {
            DeathCount.IsEnabled = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private const int RespawnCountTextDefault = 3;

        private void DeathCount_GotFocus(object sender, RoutedEventArgs e)
        {
            DeathCount.Text = DeathCount.Text == RespawnCountTextDefault.ToString() ? string.Empty : DeathCount.Text;
        }

        private void DeathCount_LostFocus(object sender, RoutedEventArgs e)
        {
            DeathCount.Text = DeathCount.Text == string.Empty ? RespawnCountTextDefault.ToString() : DeathCount.Text;
        }
        #endregion

        #region Bot_Scripts
        public void WeeklyBot()
        {
            Weekly[] bots = new Weekly[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Weekly(Emulators[ind]);
            }

            foreach (Weekly bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

                    if (bots[ind].Complete == false)
                    {
                        bots[ind].Start();
                    }
                }
            }
        }

        public void MainBot()
        {

            Main[] bots = new Main[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Main(Emulators[ind]);
            }

            foreach (Main bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

                    if (bots[ind].Complete == false)
                    {
                        bots[ind].Start();
                    }
                }
            }
        }

        public void ScrollBot()
        {
            Scroll[] bots = new Scroll[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new Scroll(Emulators[ind]);

                Rectangle screen = bots[ind].ScreenObj.GetRect(Emulators[ind]);
            }

            foreach (Scroll bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

                    #region Scroll_CheckBox_Logic
                    //Reset CheckBox
                    bool? isResetChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isResetChecked = ScrollReset.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isResetChecked)
                    {
                        bots[ind].Reset = true;
                    }

                    //ScrollS CheckBox
                    bool? isScrollSChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollSChecked = ScrollS.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollSChecked)
                    {
                        bots[ind].Preference.Add(Grade.S);
                    }

                    //ScrollA Checkbox
                    bool? isScrollAChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollAChecked = ScrollA.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollAChecked)
                    {
                        bots[ind].Preference.Add(Grade.A);
                    }

                    //ScrollB CheckBox
                    bool? isScrollBChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollBChecked = ScrollB.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollBChecked)
                    {
                        bots[ind].Preference.Add(Grade.B);
                    }

                    //ScrollC CheckBox
                    bool? isScrollCChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollCChecked = ScrollC.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollCChecked)
                    {
                        bots[ind].Preference.Add(Grade.C);
                    }
                    #endregion

                    if (bots[ind].Complete == false)
                    {
                        bots[ind].Start();
                    }
                }
            }
        }

        public void DailyBot()
        {
            DailyDungeon[] bots = new DailyDungeon[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new DailyDungeon(Emulators[ind]);

                Rectangle screen = bots[ind].ScreenObj.GetRect(Emulators[ind]);
            }

            foreach (DailyDungeon bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

        public void TOIBot()
        {
            TowerOfInsolence[] bots = new TowerOfInsolence[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new TowerOfInsolence(Emulators[ind]);

                Rectangle screen = bots[ind].ScreenObj.GetRect(Emulators[ind]);
            }

            foreach (TowerOfInsolence bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

        public void ExpBot()
        {
            ExpDungeon[] bots = new ExpDungeon[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new ExpDungeon(Emulators[ind]);

                Rectangle screen = bots[ind].ScreenObj.GetRect(Emulators[ind]);
            }

            foreach (ExpDungeon bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;

                    }
                }

                BotBuilder(bot);
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

        public void AoMBot()
        {
            AltarOfMadness[] bots = new AltarOfMadness[EmulatorCount];

            for (int ind = EmulatorCount - 1; ind >= 0; ind--)
            {
                bots[ind] = new AltarOfMadness(Emulators[ind]);

                Rectangle screen = bots[ind].ScreenObj.GetRect(Emulators[ind]);
            }

            foreach (AltarOfMadness bot in bots)
            {
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.App.MainWindowTitle.ToString())
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
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

        public void BotBuilder(Quest bot)
        {
            //Settings
            bool BlueStacks = false;

            bool Nox = false;

            MainWindow.main.Dispatcher.Invoke(new Action(() => BlueStacks = CbItemBS.IsSelected));

            MainWindow.main.Dispatcher.Invoke(new Action(() => Nox = CbItemNox.IsSelected));

            Respawn.Dispatcher.Invoke(new Action(() => bot.Respawn = (bool) Respawn.IsChecked));

            Respawn.Dispatcher.Invoke(new Action(() => bot.Deathcount = uint.Parse(DeathCount.Text)));

            Respawn.Dispatcher.Invoke(new Action(() => bot.BringToFront = (bool) BringWindowToFront.IsChecked));

            Respawn.Dispatcher.Invoke(new Action(() => bot.HomePosition = (bool) HomeWindows.IsChecked));

            if (bot.HomePosition == true)
            {
                bot.UpdateScreen();

                User32.SetWindowPos(bot.App.MainWindowHandle, 0, 0, 0, bot.Screen.Height, bot.Screen.Width, 1);//Moves each screen to 0,0 point.
            }

            if (BlueStacks)
            {
                WindowPlacement Placement = new WindowPlacement
                {
                    ShowCmd = Input.ShowWindowEnum.ShowNormal
                };

                GetWindowPlacement(bot.App.MainWindowHandle, out Placement);

                User32.MoveWindow(bot.App.MainWindowHandle, Placement.NormalPosition.Top, Placement.NormalPosition.Left, 1331, 814, true);//Resizes window to 1280x720 + all borders.
            }

        }
        #endregion

        #region ToolTip_Settings
        private void BuildToolTips()
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(UIElement),
            new FrameworkPropertyMetadata(20000));//Some magic I found on StackOverflow.

            ScrollGradeLabel.ToolTip = "Check all that apply. The bot will Select them in order of highest grade but it has no regard for level. Be sure to delete any lower level scrolls before using this feature. The bot looks through your first 10 items in your 'Potion Bag.'";

            ScrollResetLabel.ToolTip = "Warning: Uses 300 Pink Gems, ensure proper gem count before checking this box.";

            btnMain.ToolTip = "";

            btnWeekly.ToolTip = "";
        }
        #endregion
    }
}
