//using System.Diagnostics;
//using System.Drawing;

//namespace L2RBot
//{
//    class ScrollQuest
//    {
//        private Process app; //game process object(nox player)
//        private Rectangle screen; //game window screen object(nox players screen location and demensions)
//        private static Stopwatch timer; //timer object used to raising events
//        private bool initialClick; //for tracking the very first click to start the quest
//        private int timeoutInMilliS; //used to drive the clicking of scrollquest if object hasn't clicked it in a while. 
//        private QuestHelper questHelper; //QuestHelper object used for navigating quests
//        private Point scrollQuets;
//        public ItemSlot itemSlot { get; set; }
//        private Point bag;
//        private Point subBagWeapons;
//        private Point subBagArmor;
//        private Point subBagJewels;
//        private Point subBagSoulCrytals;
//        private Point subBagPetItems;
//        private Point subBagPotions;
//        private Point subBagScrolls;
//        private Point[] itemNumber;
//        public bool paidReset { get; set; }
//        private Point resetOne;
//        private Point resetTwo;
//        private Point fulfillRequest;
//        private Point ok;

//        //Pixel objects
//        private Pixel[] scrollQuest; //the pixel on btn to start/stop the weekly quest, method isPresent will not function as desired on this pixel as it changes frequently
//        private Pixel[] scrollDone;
//        private Pixel btnRedZero;

//        public ScrollQuest(Process App)
//        {
//            app = Process.GetProcessById(App.Id);
//            screen = Screen.GetRect(App);
//            timer = new Stopwatch();
//            timer.Start();
//            initialClick = false;
//            timeoutInMilliS = 60000; //1 minute
//            questHelper = new QuestHelper(App) { Quest = QuestType.Scroll };
//            scrollQuets = new Point(925, 87);
//            //these variables will be using values passed during initialization but for now i am assigning them for testing purposes
//            itemSlot = ItemSlot.Slot3;
//            paidReset = true;

//            //Point Objects
//            itemNumber = new Point[10];
//            itemNumber[0] = Screen.PointToScreenPoint(screen, 731, 225);
//            itemNumber[1] = Screen.PointToScreenPoint(screen, 838, 225);
//            itemNumber[2] = Screen.PointToScreenPoint(screen, 955, 225);
//            itemNumber[3] = Screen.PointToScreenPoint(screen, 1074, 225);
//            itemNumber[4] = Screen.PointToScreenPoint(screen, 1196, 225);
//            itemNumber[5] = Screen.PointToScreenPoint(screen, 731, 350);
//            itemNumber[6] = Screen.PointToScreenPoint(screen, 838, 350);
//            itemNumber[7] = Screen.PointToScreenPoint(screen, 955, 350);
//            itemNumber[8] = Screen.PointToScreenPoint(screen, 1074, 350);
//            itemNumber[9] = Screen.PointToScreenPoint(screen, 1196, 350);
//            //Bag Points
//            bag = Screen.PointToScreenPoint(screen, 986, 26);

//            subBagWeapons = Screen.PointToScreenPoint(screen, 678, 120);
//            subBagArmor = Screen.PointToScreenPoint(screen, 763, 120);
//            subBagJewels = Screen.PointToScreenPoint(screen, 859, 120);
//            subBagSoulCrytals = Screen.PointToScreenPoint(screen, 945, 120);
//            subBagPetItems = Screen.PointToScreenPoint(screen, 1033, 120);
//            subBagPotions = Screen.PointToScreenPoint(screen, 1123, 120);
//            subBagScrolls = Screen.PointToScreenPoint(screen, 1208, 120);

//            resetOne = Screen.PointToScreenPoint(screen, 730, 400);
//            resetTwo = Screen.PointToScreenPoint(screen, 760, 500);

//            fulfillRequest = Screen.PointToScreenPoint(screen, 1154, 668);
//            ok = Screen.PointToScreenPoint(screen, 758, 492);

//            //Pixel objects
//            scrollQuest = new Pixel[2];
//            scrollQuest[0] = new Pixel(Color.FromArgb(255, 255, 174, 0), new Point(13, 306));
//            scrollQuest[1] = new Pixel(Color.FromArgb(255, 255, 174, 0), new Point(13, 319));

//            scrollDone = new Pixel[2];
//            scrollDone[0] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(243, 333));
//            scrollDone[1] = new Pixel(Color.FromArgb(255, 255, 255, 255), new Point(245, 334));

//            btnRedZero = new Pixel(Color.FromArgb(255, 33, 43, 57), new Point(1284, 752));
//        }
//        private void Click(Point ClickHere)
//        {
//            ClickHere = Screen.PointToScreenPoint(screen, ClickHere.X, ClickHere.Y);
//            Mouse.LeftMouseClick(ClickHere.X, ClickHere.Y);
//            timer.Stop();
//            timer.Reset();
//            timer.Start();
//        }
//        public void Start()
//        {
//            User32.SetForegroundWindow(app.MainWindowHandle);
//            System.Threading.Thread.Sleep(100);

//            //InitialClick();
//            CheckScrollQuest();
//            questHelper.Start();

//        }
//        private void InitialClick()
//        {
//            if (initialClick.Equals(false))
//            {
//                Click(scrollQuest[0].Point);
//                initialClick = true;
//            }
//        }

//        /// <summary>
//        /// Detects Scroll Sub Quest and Calls QuestHelper.Start() if found. It opens an Item scroll if not.
//        /// </summary>
//        private void CheckScrollQuest()
//        {
//            if (Bot.IsCombatScreenUp(app))
//            {
//                if (!scrollQuest[1].IsPresent(screen, 2))
//                {
//                    OpenScrollQuestBag();
//                    ClickScrollQuest();
//                    ResetScroll();
//                    FulfillRequest();
//                }
//            }
//        }
//        private void OpenScrollQuestBag()
//        {
//            Click(bag);
//            System.Threading.Thread.Sleep(5000);//wait for bag to load
//            Click(subBagPotions);
//            System.Threading.Thread.Sleep(2000);//wait for sub bag to load

//        }
//        private void ClickScrollQuest()
//        {


//            //this iterates through ItemSlots and looks for the value assigned to itemSlot
//            int item = 0;
//            foreach (ItemSlot slot in System.Enum.GetValues(typeof(ItemSlot)))
//            {
//                if (slot.Equals(itemSlot))
//                {
//                    Click(itemNumber[item]);//once it finds a match it clicks the corispoding itemNumber[] point, being the scroll quest item
//                }
//                item++;
//            }
//            System.Threading.Thread.Sleep(1000);//wait for the item to load1 


//        }
//        private void ResetScroll()
//        {
//            if (paidReset.Equals(true))
//            {

//                if (btnRedZero.IsPresent(screen, 2))
//                {
//                    Click(resetOne);
//                    System.Threading.Thread.Sleep(1000);
//                    Click(resetTwo);
//                    System.Threading.Thread.Sleep(1000);

//                }
//            }
//        }
//        private void FulfillRequest()
//        {
//            Click(fulfillRequest);
//            System.Threading.Thread.Sleep(1000);
//            Click(ok);
//            System.Threading.Thread.Sleep(1000);
//        }

//        //delete this method after refactoring is complete
//        public void SacrollQuest(Process app)
//        {
//            Pixel btnRedZero = new Pixel(Color.FromArgb(255, 255, 152, 176), new Point(1011, 224));
//            //Color[] btnRedZero = new Color[1];
//            //btnRedZero[0] = Color.FromArgb(255, 255, 152, 176);
//            //Color pixel1 = Screen.GetColor(rect, 1011, 224);
//            //if (Screen.CompareColor(pixel1, btnRedZero[0], 2))
//            //{
//            //    point = //first reset button
//            //    Mouse.LeftMouseClick(point.X, point.Y);//click first reset button
//            //    Thread.Sleep(1000);//wait for second reset button to load

//            //    point = Screen.PointToScreenPoint(rect, 755, 494);//second reset button
//            //    Mouse.LeftMouseClick(point.X, point.Y);//click second reset button
//            //    Thread.Sleep(1000);//wait for second reset button to close


//            //}

//            //point = Screen.PointToScreenPoint(rect, 1154, 668);//fulfill request location
//            //Mouse.LeftMouseClick(point.X, point.Y);//click fulfill request
//            //Thread.Sleep(1000);//wait for ok button to pop up

//            //point = Screen.PointToScreenPoint(rect, 758, 492);//Ok button location
//            //Mouse.LeftMouseClick(point.X, point.Y);//click Ok
//            //Thread.Sleep(1000);//wait for Start Quest to pop up

//        }
//    }
//    public enum ItemSlot
//    {
//        Slot1,
//        Slot2,
//        Slot3,
//        Slot4,
//        Slot5,
//        Slot6,
//        Slot7,
//        SLot8,
//        Slot9,
//        Slot10
//    }
//}
