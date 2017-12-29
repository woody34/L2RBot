using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
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
using static L2RBot.User32;
using Managed.Adb;

namespace L2RBot
{

    public partial class MainWindow : Window
    {
        //Globals
        private Process[] Emulators = null;

        private L2RDevice[] L2RDevices = null;

        private Process NullEmulator = null;

        private L2RDevice NullL2RDev = null;

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

            if (CbItemMEmu != null)
            {
                if (CbItemMEmu.IsSelected)
                {
                    btnProcessGrab.Content = "Find MEmu(Ctl+Alt+F) ";
                }
            }

            if (CbItemADB != null)
            {
                if (CbItemADB.IsSelected)
                {
                    btnProcessGrab.Content = "Find ADB(Ctl+Alt+F) ";
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



            //Turned them off until I fix them.
            //btnExp.IsEnabled = true;

            btnDaily.IsEnabled = true;

            btnTower.IsEnabled = true;



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

                        if (pro.MainWindowTitle != "")
                        {
                            ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                            listProcessList.Items.Add(itm);

                            EmulatorCount++;
                        }
                    }

                    Emulators = EmulatorProcess;
                }

                if (CbItemMEmu.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("MEmu");

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

                if (CbItemADB.IsSelected)
                {
                    AndroidDebugBridge.Initialize(true);

                    //ADB devices.
                    List<Device> Devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

                    L2RDevices = new L2RDevice[Devices.Count];

                    //Initializes the L2RDevice array.
                    for (int i = 0; i < Devices.Count; i++)
                    {
                        L2RDevices[i] = new L2RDevice(Devices[i]);

                        EmulatorCount++;
                    }

                    //Enbale buttons for quest if we find ADB devices.
                    if (Devices != null)
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (L2RDevice Device in L2RDevices)
                    {
                        if (Device.CharacterName == null)
                        {
                            UpdateLog = "Unable to find Character name";
                        }

                        ListBoxItem itm = new ListBoxItem() { Content = Device.CharacterName };

                        listProcessList.Items.Add(itm);
                    }
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

                        if (pro.MainWindowTitle != "")
                        {
                            ListBoxItem itm = new ListBoxItem() { Content = pro.MainWindowTitle.ToString() };

                            listProcessList.Items.Add(itm);

                            EmulatorCount++;
                        }
                    }

                    Emulators = EmulatorProcess;
                }

                if (CbItemMEmu.IsSelected)
                {
                    EmulatorProcess = Bot.GetOpenProcess("MEmu");

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

                if (CbItemADB.IsSelected)
                {
                    AndroidDebugBridge.Initialize(true);

                    //ADB devices.
                    List<Device> Devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

                    L2RDevices = new L2RDevice[Devices.Count];

                    //Initializes the L2RDevice array.
                    for (int i = 0; i < Devices.Count; i++)
                    {
                        L2RDevices[i] = new L2RDevice(Devices[i]);

                        EmulatorCount++;
                    }

                    //Enbale buttons for quest if we find ADB devices.
                    if (Devices != null)
                    {
                        EnableButtons();
                        listProcessList.IsEnabled = true;
                        listProcessList.Background = System.Windows.Media.Brushes.LightGreen;
                    }

                    foreach (L2RDevice Device in L2RDevices)
                    {
                        if (Device.CharacterName == null)
                        {
                            UpdateLog = "Unable to find Character name";
                        }

                        ListBoxItem itm = new ListBoxItem() { Content = Device.CharacterName };

                        listProcessList.Items.Add(itm);
                    }
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

                Emulators = null;

                L2RDevices = null;

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
            //Build Bot[]
            Weekly[] bots = new Weekly[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new Weekly[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Weekly(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new Weekly[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Weekly(Emulators[i], NullL2RDev);
                }
            }

            foreach (Weekly bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void MainBot()
        {
            //Build Bot[]
            Main[] bots = new Main[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new Main[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Main(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new Main[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Main(Emulators[i], NullL2RDev);
                }
            }

            foreach (Main bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void ScrollBot()
        {
            //Build Bot[]
            Scroll[] bots = new Scroll[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new Scroll[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Scroll(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new Scroll[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new Scroll(Emulators[i], NullL2RDev);
                }
            }

            foreach (Scroll bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {

                    #region Scroll_CheckBox_Logic
                    //Reset CheckBox
                    bool? isResetChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isResetChecked = ScrollReset.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isResetChecked)
                    {
                        bots[i].Reset = true;
                    }

                    //ScrollS CheckBox
                    bool? isScrollSChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollSChecked = ScrollS.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollSChecked)
                    {
                        bots[i].Preference.Add(Grade.S);
                    }

                    //ScrollA Checkbox
                    bool? isScrollAChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollAChecked = ScrollA.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollAChecked)
                    {
                        bots[i].Preference.Add(Grade.A);
                    }

                    //ScrollB CheckBox
                    bool? isScrollBChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollBChecked = ScrollB.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollBChecked)
                    {
                        bots[i].Preference.Add(Grade.B);
                    }

                    //ScrollC CheckBox
                    bool? isScrollCChecked = null;

                    ScrollReset.Dispatcher.Invoke(new Action(() =>
                    {
                        isScrollCChecked = ScrollC.IsChecked;
                    }), DispatcherPriority.Normal);

                    if ((bool) isScrollCChecked)
                    {
                        bots[i].Preference.Add(Grade.C);
                    }
                    #endregion

                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void DailyBot()
        {
            //Build Bot[]
            DailyDungeon[] bots = new DailyDungeon[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new DailyDungeon[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new DailyDungeon(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new DailyDungeon[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new DailyDungeon(Emulators[i], NullL2RDev);
                }
            }

            foreach (DailyDungeon bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void TOIBot()
        {
            //Build Bot[]
            TowerOfInsolence[] bots = new TowerOfInsolence[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new TowerOfInsolence[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new TowerOfInsolence(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new TowerOfInsolence[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new TowerOfInsolence(Emulators[i], NullL2RDev);
                }
            }

            foreach (TowerOfInsolence bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void ExpBot()
        {
            //Build Bot[]
            ExpDungeon[] bots = new ExpDungeon[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new ExpDungeon[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new ExpDungeon(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new ExpDungeon[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new ExpDungeon(Emulators[i], NullL2RDev);
                }
            }

            foreach (ExpDungeon bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void AoMBot()
        {
            //Build Bot[]
            AltarOfMadness[] bots = new AltarOfMadness[0];

            //Builds Bot[] to for ADB clients
            if (L2RDevices != null)
            {
                bots = new AltarOfMadness[L2RDevices.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new AltarOfMadness(NullEmulator, L2RDevices[i]);
                }
            }

            //Builds Bot[] to for ADB clients
            if (Emulators != null)
            {
                bots = new AltarOfMadness[Emulators.Length];

                for (int i = 0; i < bots.Length; i++)
                {
                    bots[i] = new AltarOfMadness(Emulators[i], NullL2RDev);
                }
            }

            foreach (AltarOfMadness bot in bots)
            {
                //Checks if user deselected process to exclude from automation.
                foreach (ListBoxItem item in listProcessList.Items)
                {
                    bool isSelected = false;

                    string itemContent = "";

                    item.Dispatcher.Invoke(new Action(() => isSelected = item.IsSelected));

                    item.Dispatcher.Invoke(new Action(() => itemContent = item.Content.ToString()));

                    if (isSelected && itemContent == bot.BotName)
                    {
                        bot.Complete = true;
                    }
                }

                BotBuilder(bot);
            }


            while (true)//replace with start stop button states
            {
                for (int i = 0; i < bots.Length; i++)
                {
                    if (bots[i].Complete == false)
                    {
                        bots[i].Start();
                    }
                }
            }
        }

        public void BotBuilder(Quest bot)
        {
            //Settings
            bool BlueStacks = false;

            bool Nox = false;

            bool MEmu = false;

            bool Respwn = false;

            uint DeathCnt = 0;

            bool Cycle = false;

            bool Home = false;

            bool CloseTV = false;

            bool ADB = false;

            MainWindow.main.Dispatcher.Invoke(new Action(() => BlueStacks = CbItemBS.IsSelected));

            MainWindow.main.Dispatcher.Invoke(new Action(() => Nox = CbItemNox.IsSelected));

            MainWindow.main.Dispatcher.Invoke(new Action(() => MEmu = CbItemMEmu.IsSelected));

            MainWindow.main.Dispatcher.Invoke(new Action(() => Respwn = (bool) Respawn.IsChecked));

            MainWindow.main.Dispatcher.Invoke(new Action(() => DeathCnt = uint.Parse(DeathCount.Text)));

            MainWindow.main.Dispatcher.Invoke(new Action(() => Cycle = (bool) BringWindowToFront.IsChecked));

            MainWindow.main.Dispatcher.Invoke(new Action(() => Home = (bool) HomeWindows.IsChecked));

            MainWindow.main.Dispatcher.Invoke(new Action(() => CloseTV = (bool) TVOk.IsChecked));

            MainWindow.main.Dispatcher.Invoke(new Action(() => ADB = CbItemADB.IsSelected));

            bot.Respawn = Respwn;

            bot.Deathcount = DeathCnt;

            if (ADB == true)
            {
                bot.BringToFront = false;
                bot.SleepTime = 0;
            }
            else
            {
                bot.BringToFront = Cycle;
            }

            bot.HomePosition = Home;

            bot.CloseTVPopup = CloseTV;

            if (bot.HomePosition == true)
            {
                bot.UpdateScreen();

                User32.SetWindowPos(bot.BotWindowHandle, 0, 0, 0, bot.Screen.Height, bot.Screen.Width, 1);//Moves each screen to 0,0 point.
            }

            if (BlueStacks)
            {
                WindowPlacement Placement = new WindowPlacement
                {
                    ShowCmd = Input.ShowWindowEnum.ShowNormal
                };

                GetWindowPlacement(bot.BotWindowHandle, out Placement);

                User32.MoveWindow(bot.BotWindowHandle, Placement.NormalPosition.Top, Placement.NormalPosition.Left, 1331, 814, true);//Resizes window to 1280x720 + all borders.
            }

        }
        #endregion
    }
}
