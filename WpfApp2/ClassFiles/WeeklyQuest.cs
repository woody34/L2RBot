using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    /// <summary>
    /// Used to perform and complete the Lineage 2 Revolution Weekly Quest.
    /// </summary>
    class WeeklyQuest
    {
        private Process app; //game process object(nox player)
        private Rectangle screen; //game window screen object(nox players screen location and demensions)
        private static Stopwatch timer; //timer object used to raising events
        private bool initialClick; //for tracking the very first click to start the quest
        private int timeoutInMilliS; //used to drive the clicking of weeklyquest if object hasn't clicked it in a while. 
        private QuestHelper questHelper;
        private Point questLog;
        private Point weeklyRow;

        //Pixel objects
        private Pixel[] weeklyQuest; //the pixel on btn to start/stop the weekly quest, method isPresent will not function as desired on this pixel as it changes frequently
        private Pixel[] weeklyDone;

        /// <summary>
        /// Constructs WeeklyQuest data objects.
        /// </summary>
        /// <param name="App">Android Emulator Process object</param>
        public WeeklyQuest(Process App)
        {
            //initialize WeekylyQuest data objects
            app = Process.GetProcessById(App.Id);
            screen = Screen.GetRect(App);
            timer = new Stopwatch();
            timer.Start();
            initialClick = false;
            timeoutInMilliS = 60000; //1 minute
            questHelper = new QuestHelper(App) { Quest = QuestType.Weekly };
            questLog = new Point(925, 87);
            weeklyRow = new Point(111, 291);
            //Pixel objects
            weeklyQuest = new Pixel[2];
            weeklyQuest[0] = new Pixel(Color.FromArgb(255, 75, 154, 255), new Point(12, 302));
            weeklyQuest[1] = new Pixel(Color.FromArgb(255, 75, 154, 255), new Point(79, 312));

            weeklyDone = new Pixel[2];
            weeklyDone[0] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(243, 333));
            weeklyDone[1] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(245, 334));
        }

        /// <summary>
        /// A click method for the Quest object to complete and monitor click events.
        /// </summary>
        /// <param name="ClickHere"></param>
        private void Click(Point ClickHere)
        {
            ClickHere = Screen.PointToScreenPoint(screen, ClickHere.X, ClickHere.Y);
            Mouse.LeftMouseClick(ClickHere.X, ClickHere.Y);
            timer.Stop();
            timer.Reset();
            timer.Start();
        }

        /// <summary>
        /// Starts quest.
        /// </summary>
        public void Start()
        {
            User32.SetForegroundWindow(app.MainWindowHandle);
            System.Threading.Thread.Sleep(100);
            Debug.WriteLine("InitialClick()");
            InitialClick();
            Debug.WriteLine("CheckForWeekly()");
            CheckForWeekly();
            Debug.WriteLine("CheckDone()");
            CheckDone();
            Debug.WriteLine("QuestHelper.Start()");
            questHelper.Start();

        }

        /// <summary>
        /// Performs the first click to start the quest chain.
        /// </summary>
        private void InitialClick()
        {
            if (initialClick.Equals(false))
            {
                Click(weeklyQuest[0].Point);
                initialClick = true;
            }
        }

        /// <summary>
        /// Check to see if the weekly quest has be started. If it has NOT, it will call OpenWeekly().
        /// </summary>
        private void CheckForWeekly()
        {
            //Weekly Quest present
            System.Threading.Thread.Sleep(100);
            if (!weeklyQuest[0].IsPresent(screen, 2) & !weeklyQuest[1].IsPresent(screen, 2) & Bot.IsCombatScreenUp(app))//if weekly quest is NOT present then compplete these actions
            {
                
                OpenWeekly();
            }

        }

        /// <summary>
        /// Opens the Quest log and chooses the weekly tab. QuestHelper will click complete subsequent navigation.
        /// </summary>
        private void OpenWeekly()
        {
            Click(questLog);
            System.Threading.Thread.Sleep(2000);

            Click(weeklyRow);
            System.Threading.Thread.Sleep(2000);

            questHelper.Start();
        }

        /// <summary>
        /// Checks to see if the Weekly Quest is in a done State, If Done it will call OpenWeekly()
        /// </summary>
        private void CheckDone()
        {
            if (Bot.IsCombatScreenUp(app))//if combat screen is up check done status
            {
                if(weeklyDone[0].IsPresent(screen, 2) & !weeklyDone[1].IsPresent(screen, 2))
                OpenWeekly();
            }
        }
    }
}
