using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    class ScrollQuest
    {
        private Process app; //game process object(nox player)
        private Rectangle screen; //game window screen object(nox players screen location and demensions)
        private static Stopwatch timer; //timer object used to raising events
        private bool initialClick; //for tracking the very first click to start the quest
        private int timeoutInMilliS; //used to drive the clicking of scrollquest if object hasn't clicked it in a while. 
        private QuestHelper questHelper; //QuestHelper object used for navigating quests
        private Point scrollQuets;
        public ItemSlot itemSlot { get; set; }

        //Pixel objects
        private Pixel[] scrollQuest; //the pixel on btn to start/stop the weekly quest, method isPresent will not function as desired on this pixel as it changes frequently
        private Pixel[] scrollDone;

        public ScrollQuest(Process App)
        {
            app = Process.GetProcessById(App.Id);
            screen = Screen.GetRect(App);
            timer = new Stopwatch();
            timer.Start();
            initialClick = false;
            timeoutInMilliS = 60000; //1 minute
            questHelper = new QuestHelper(App) { Quest = QuestType.Scroll };
            scrollQuets = new Point(925, 87);
            

            //Pixel objects
            scrollQuest = new Pixel[2];
            scrollQuest[0] = new Pixel(Color.FromArgb(255, 255, 255, 174), new Point(19, 309));
            scrollQuest[1] = new Pixel(Color.FromArgb(255, 255, 255, 174), new Point(20, 316));

            scrollDone = new Pixel[2];
            scrollDone[0] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(243, 333));
            scrollDone[1] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(245, 334));
        }
        private void Click(Point ClickHere)
        {
            ClickHere = Screen.PointToScreenPoint(screen, ClickHere.X, ClickHere.Y);
            Mouse.LeftMouseClick(ClickHere.X, ClickHere.Y);
            timer.Stop();
            timer.Reset();
            timer.Start();
        }
        public void Start()
        {
            User32.SetForegroundWindow(app.MainWindowHandle);
            System.Threading.Thread.Sleep(100);

            InitialClick();

            questHelper.Start();

        }
        private void InitialClick()
        {
            if (initialClick.Equals(false))
            {
                Click(scrollQuest[0].Point);
                initialClick = true;
            }
        }

        /// <summary>
        /// Detects Scroll Sub Quest and Calls QuestHelper.Start() if found.
        /// </summary>
        private void CheckScrollQuest()
        {
            if (Bot.IsCombatScreenUp(app) & scrollQuest[0].IsPresent(screen, 2) && scrollQuest[1].IsPresent(screen, 2) && scrollQuest[0].B.Equals(0) && scrollQuest[1].B.Equals(0))
            {
                questHelper.Start();
            }
            else
            {
                StartScrollQuest();
            }
        }
        private void StartScrollQuest()
        {
            //code here
        }

        //delete this method after refactoring is complete
        public void SacrollQuest(Process app)
        {
                    //Thread.Sleep(1000);
                    //Point point = Screen.PointToScreenPoint(rect, 986, 26);//bag point
                    //Mouse.LeftMouseClick(point.X, point.Y);//click bag
                    //Thread.Sleep(5000);//wait for bag to load

                    //point = Screen.PointToScreenPoint(rect, 1112, 123);//potions tab
                    //Mouse.LeftMouseClick(point.X, point.Y);//click bag
                    //Thread.Sleep(1000);//wait for tab to load

                    //point = Screen.PointToScreenPoint(rect, 955, 225);//location of third item
                    //Mouse.LeftMouseClick(point.X, point.Y);//click item in third position
                    //Thread.Sleep(1000);//wait for scroll quest to open

                    //Color[] btnRedZero = new Color[1];
                    //btnRedZero[0] = Color.FromArgb(255, 255, 152, 176);
                    //Color pixel1 = Screen.GetColor(rect, 1011, 224);
                    //if (Screen.CompareColor(pixel1, btnRedZero[0], 2))
                    //{
                    //    point = Screen.PointToScreenPoint(rect, 729, 408);//first reset button
                    //    Mouse.LeftMouseClick(point.X, point.Y);//click first reset button
                    //    Thread.Sleep(1000);//wait for second reset button to load

                    //    point = Screen.PointToScreenPoint(rect, 755, 494);//second reset button
                    //    Mouse.LeftMouseClick(point.X, point.Y);//click second reset button
                    //    Thread.Sleep(1000);//wait for second reset button to close


                    //}

                    //point = Screen.PointToScreenPoint(rect, 1154, 668);//fulfill request location
                    //Mouse.LeftMouseClick(point.X, point.Y);//click fulfill request
                    //Thread.Sleep(1000);//wait for ok button to pop up

                    //point = Screen.PointToScreenPoint(rect, 758, 492);//Ok button location
                    //Mouse.LeftMouseClick(point.X, point.Y);//click Ok
                    //Thread.Sleep(1000);//wait for Start Quest to pop up

        }
    }
    public enum ItemSlot
    {
        Slot1,
        Slot2,
        Slot3,
        Slot4,
        Slot5,
        Slot6,
        Slot7,
        SLot8,
        Slot9,
        Slot10
    }
}
