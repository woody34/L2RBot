using System;
using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    /// <summary>
    /// Used to perform and complete the Lineage 2 Revolution Main Quest.
    /// </summary>
    class MainQuest
    {
        private Process app; //game process object(nox player)
        private Rectangle screen; //game window screen object(nox players screen location and demensions)
        private static Stopwatch timer; //timer object used to raising events
        private bool initialClick; //for tracking the very first click to start the quest
        private int timeoutInMilliS; //used to drive the clicking of mainquest if it has not clicked it in a while. 
        private QuestHelper questHelper;

        //Pixel objects
        private Pixel mainQuest; //the pixel on btn to start/stop the main quest, method isPresent will not function as desired on this pixel as it changes frequently
        private Pixel movePixel;//used in conjunction with timer to detect non-movement.
        private Pixel[] blueArrow; //the pixels the blue arrow graphic detection
        private Pixel questDone; //the pixel value for the quest done graphic detection
        private Pixel[] questBubble; //the pixel value for the quest chat bubble

        /// <summary>
        /// Constructs MainQuest data objects.
        /// </summary>
        /// <param name="App">Android Emulator Process object</param>
        public MainQuest(Process App)
        {
            app = Process.GetProcessById(App.Id);
            screen = Screen.GetRect(App);
            timer = new Stopwatch();
            timer.Start();
            initialClick = false;
            timeoutInMilliS = 60000; //1 minute
            questHelper = new QuestHelper(App) { Quest = QuestType.Main};

            //Pixel objects
            mainQuest = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(125, 250));
            movePixel = new Pixel(Screen.GetColor(screen, 230, 490), new Point(230, 490));
            blueArrow = new Pixel[2];
            blueArrow[0] = new Pixel(Color.FromArgb(255, 54, 103, 123), new Point(282, 243));
            blueArrow[1] = new Pixel(Color.FromArgb(255, 46, 91, 108), new Point(282, 249));
            questDone = new Pixel(Color.FromArgb(255, 232, 227, 224), new Point(259, 272));
            questBubble = new Pixel[2];
            questBubble[1] = new Pixel(Color.FromArgb(255, 226, 226, 226), new Point(691, 248));
            questBubble[0] = new Pixel(Color.FromArgb(255, 63, 63, 63), new Point(691, 262));
            

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

            InitialClick();
            QuestDone();
            QuestBubble();
            //QuestDone()
            IdleCheck();
            //BlueArrowPresent();

            questHelper.Start();

        }

        /// <summary>
        /// Detects the Blue Arrow animation that overlays the MainQuest at lower levels.
        /// </summary>
        private void BlueArrowPresent()
        {
            if (blueArrow[0].IsPresent(screen, 5) & blueArrow[1].IsPresent(screen, 5))
            {
                Click(mainQuest.Point);
            }
        }

        /// <summary>
        /// Detects movement using Click(), calls Click if idle is detected.
        /// </summary>
        private void IdleCheck()
        {
            if (timer.ElapsedMilliseconds > timeoutInMilliS)
            {
                if( movePixel.IsPresent(screen, 2))
                {
                    movePixel.UpdateColor(screen);
                    Click(mainQuest.Point);
                }
                timer.Stop();
                timer.Reset();
                timer.Start();
            }
        }

        /// <summary>
        /// detects the QuestBubble containing an elipsis(...), calls Click() upon detection
        /// </summary>
        private void QuestBubble()
        {
            if (questBubble[0].IsPresent(screen, 2) & questBubble[1].IsPresent(screen, 2))
            {
                Click(mainQuest.Point);
            }
            Bot.PopUpKiller(app);
        }

        /// <summary>
        /// Performs the initial Click() upon starting the quest using the initialClick bool, setting true upon initial click
        /// </summary>
        private void InitialClick()
        {
            if (initialClick.Equals(false))
            {
                Click(mainQuest.Point);
                initialClick = true;
            }
        }

        /// <summary>
        /// Detected the Quest Done graphic that overlays the main quest upon completion, calls Click() of mainQuest.Point
        /// </summary>
        private void QuestDone()
        {
            if (questDone.IsPresent(screen, 2))
            {
                Click(mainQuest.Point);
            }
        }
    }
}
