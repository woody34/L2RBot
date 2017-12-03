using System.Diagnostics;
using System.Drawing;
using L2RBot.Common.Enum;
using L2RBot.Common;
using System.Threading;

namespace L2RBot
{
    /// <summary>
    /// Used to perform and assist in completion of Lineage 2 Revolution quests. QuestHelper performs repetative tasks that are similar over all QuestTypes
    /// </summary>
    public class QuestHelper : Quest
    {
        //Globals.
        private Process app; //game process object(nox player)

        public QuestType Quest { get; set; }//quest type is to be set during instantiation using object initializer {Quest = QuestType.Main}

        private Pixel[] _skipDialog;

        private Pixel[] _skipCutScene;

        private Pixel[] _acceptQuest;

        private Pixel[] _claimReward;

        private Pixel[] _continue;

        private Pixel[] _skipTutorial;

        private Pixel[] _skipTutorialOk;

        private Pixel[] _questComplete;

        private Pixel[] _questStart;

        private Pixel[] _questGoNow;

        private Pixel[] _questWalk;

        private Pixel[] _subQuestStart;

        private Pixel _mainQuest;

        private Pixel[] _stuckLoading;

        private Pixel[] _connectNow;

        private Pixel[] _loginPopUp;

        private Pixel[] _specialSalePopUp;

        private Pixel[] _skipDialogTop;

        private Pixel[] _equipPopUp;

        private Pixel[] _okPopUp;

        //Properties.
        public Pixel[] SkipDialog
        {
            get
            {
                return _skipDialog;
            }
            set
            {
                _skipDialog = value;
            }
        }

        public Pixel[] SkipCutScene
        {
            get
            {
                return _skipCutScene;
            }
            set
            {
                _skipCutScene = value;
            }
        }

        public Pixel[] AcceptQuest
        {
            get
            {
                return _acceptQuest;
            }
            set
            {
                _acceptQuest = value;
            }
        }

        public Pixel[] ClaimReward
        {
            get
            {
                return _claimReward;
            }
            set
            {
                _claimReward = value;
            }
        }

        public Pixel[] Continue
        {
            get
            {
                return _continue;
            }
            set
            {
                _continue = value;
            }
        }

        public Pixel[] SkipTutorial
        {
            get
            {
                return _skipTutorial;
            }
            set
            {
                _skipTutorial = value;
            }
        }

        public Pixel[] SkipTutorialOk
        {

            get
            {
                return _skipTutorialOk;
            }
            set
            {
                _skipTutorialOk = value;
            }
        }

        public Pixel[] QuestComplete
        {
            get
            {
                return _questComplete;
            }
            set
            {
                _questComplete = value;
            }
        }

        public Pixel[] QuestStart
        {
            get
            {
                return _questStart;
            }
            set
            {
                _questStart = value;
            }
        }

        public Pixel[] QuestGoNow
        {
            get
            {
                return _questGoNow;
            }
            set
            {
                _questGoNow = value;
            }
        }

        public Pixel[] QuestWalk
        {
            get
            {
                return _questWalk;
            }
            set
            {
                _questWalk = value;
            }
        }

        public Pixel[] SubQuestStart
        {
            get
            {
                return _subQuestStart;
            }
            set
            {
                _subQuestStart = value;
            }
        }

        public Pixel MainQuest
        {
            get
            {
                return _mainQuest;
            }
            set
            {
                _mainQuest = value;
            }
        }

        public Pixel[] StuckLoading
        {
            get
            {
                return _stuckLoading;
            }
            set
            {
                _stuckLoading = value;
            }
        }

        public Pixel[] ConnectNow
        {
            get
            {
                return _connectNow;
            }
            set
            {
                _connectNow = value;
            }
        }

        public Pixel[] LoginPopUp
        {
            get
            {
                return _loginPopUp;
            }
            set
            {
                _loginPopUp = value;
            }
        }

        public Pixel[] SpecialSalePopUp
        {
            get
            {
                return _specialSalePopUp;
            }
            set
            {
                _specialSalePopUp = value;
            }
        }

        public Pixel[] SkipDialogTop
        {
            get
            {
                return _skipDialogTop;
            }
            set
            {
                _skipDialogTop = value;
            }
        }

        public Pixel[] EquipPopUp
        {
            get
            {
                return _equipPopUp;
            }
            set
            {
                _equipPopUp = value;
            }
        }

        public Pixel[] OkPopUp
        {
            get
            {
                return _okPopUp;
            }
            set
            {
                _okPopUp = value;
            }
        }

        /// <summary>
        /// Constructs QuestHelper data objects.
        /// </summary>
        /// <param name="App">Android Emulator Process object</param>
        public QuestHelper(Process App) : base(App)
        {
            app = Process.GetProcessById(App.Id);

            UpdateScreen();

            BuildPixels();
        }

        private void BuildPixels()
        {
            _mainQuest = new Pixel //The pixel on btn to start/stop the main quest, method isPresent will not function as desired on this pixel as it changes frequently
            {
                Color = Color.FromArgb(255, 255, 255, 255),

                Point = new Point(125, 250)
            };

            _skipDialog = new Pixel[2];

            _skipDialog[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(1178, 509)//White S on 'Skip' button.
            };

            _skipDialog[1] = new Pixel
            {
                Color = Colors.White,//!White

                Point = new Point(1249, 600)//Black BG of the '>' (skip arrow).
            };

            _skipCutScene = new Pixel[3];

            _skipCutScene[0] = new Pixel
            {
                Color = Color.FromArgb(255, 232, 232, 232),

                Point = new Point(1168, 48)//S on the 'Skip' button.
            };

            _skipCutScene[1] = new Pixel
            {
                Color = Color.White,

                Point = new Point(1149, 10)//Spot to check if whole screen is white.
            };

            _skipCutScene[2] = new Pixel
            {
                Color = Color.FromArgb(255, 232, 232, 232),

                Point = new Point(39, 16)//The navigation back logo, to make sure menu is up.
            };

            _acceptQuest = new Pixel[2];

            _acceptQuest[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(702, 617)//White A on 'Accept Quest' button.
            };

            _acceptQuest[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(732, 592)//Blue 'Accept Quest' button's background.
            };

            _claimReward = new Pixel[2];

            _claimReward[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(563, 611)//White C on 'Claim Reward' button.
            };

            _claimReward[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(626, 592)//Blue BG of 'Claim Reward' button.
            };

            _continue = new Pixel[2];

            _continue[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),

                Point = new Point(1055, 633)//White C on 'Continue' button.
            };

            _continue[1] = new Pixel
            {
                Color = Color.FromArgb(255, 51, 88, 130),

                Point = new Point(1012, 623)//Blue BG of 'Continue' button.
            };

            _skipTutorial = new Pixel[3];

            _skipTutorial[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(1169, 50)//White S on the 'Skip' button.
            };

            _skipTutorial[1] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(1154, 54)//Point that is not white.
            };

            _skipTutorialOk = new Pixel[2];

            _skipTutorialOk[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(749, 472)//White O on 'Ok' button.
            };

            _skipTutorialOk[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(735, 457)//Blue bg of 'Ok' button.
            };

            //Start Quest button, Weekly Quest
            _questStart = new Pixel[2];

            _questStart[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(864, 491)//The White S on the 'Start Quest' button.
            };

            _questStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 63, 83),

                Point = new Point(887, 479)//The blue BG on the 'Start Quest' button.
            };

            _questGoNow = new Pixel[2];

            _questGoNow[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(888, 494)//The G on the "Go Now' button.
            };

            _questGoNow[1] = new Pixel
            {
                Color = Color.FromArgb(255, 45, 59, 78),

                Point = new Point(897, 490)//The blue between the head and tale of the g on the 'Go Now' button.
            };

            _questComplete = new Pixel[2];

            _questComplete[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(838, 493)//Front of the Q on the ' Quest Complete' button.
            };

            _questComplete[1] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(843, 492)//Center of Q on 'Quest Complete' button.
            };

            //Walk button, Weekly Quest.
            _questWalk = new Pixel[2];

            _questWalk[0] = new Pixel
            {
                Color = Color.FromArgb(255, 120, 130, 140),

                Point = new Point(468, 518)//W on the 'Walk" button.
            };

            _questWalk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 54, 90, 131),

                Point = new Point(666, 508)//The blue BG of the 'Teleport' button(none opaque.)
            };

            _subQuestStart = new Pixel[2];

            _subQuestStart[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),

                Point = new Point(712, 603)//White S on the 'Start' button.
            };

            _subQuestStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 85, 127),

                Point = new Point(701, 607)//BG of button.
            };

            _stuckLoading = new Pixel[4];

            _stuckLoading[0] = new Pixel
            {
                Color = Colors.StuckLoading,

                Point = new Point(611, 339)//Top left quadrant.
            };

            _stuckLoading[1] = new Pixel
            {
                Color = Colors.StuckLoading,

                Point = new Point(664, 333)//Top right quadrant.
            };

            _stuckLoading[2] = new Pixel
            {
                Color = Colors.StuckLoading,

                Point = new Point(617, 387)//Bottom left quadrant.
            };

            _stuckLoading[3] = new Pixel
            {
                Color = Colors.StuckLoading,

                Point = new Point(670, 377)//Bottom Right quadrant
            };

            _connectNow = new Pixel[2];

            _connectNow[0] = new Pixel
            {
                Color = Colors.White,

                Point = new Point(716, 521)//C on 'Connect Now' button.
            };

            _connectNow[1] = new Pixel
            {
                Color = Colors.CompleteOk,

                Point = new Point(749, 501)//Blue button BG on 'Connect Now' button.
            };
            _loginPopUp = new Pixel[2];

            _loginPopUp[0] = new Pixel()
            {
                Color = Colors.White,

                Point = new Point(1244, 36)//X of the close button.
            };

            _loginPopUp[1] = new Pixel()
            {
                Color = Colors.Black,

                Point = new Point(1230, 36)//BG of the close button.
            };

            _specialSalePopUp = new Pixel[2];

            _specialSalePopUp[0] = new Pixel()
            {
                Color = Color.FromArgb(255, 168, 168, 171),

                Point = new Point(1104, 151)//X of the close button.
            };

            _specialSalePopUp[1] = new Pixel()
            {
                Color = Color.FromArgb(255, 168, 168, 171),

                Point = new Point(1088, 152)//!BG of the close button.
            };

            _skipDialogTop = new Pixel[2];

            _skipDialogTop[0] = new Pixel()
            {
                Color = Colors.White,

                Point = new Point(960, 55)//S of the 'Skip' button.
            };

            _skipDialogTop[1] = new Pixel()
            {
                Color = Color.White,

                Point = new Point(953, 56)//!BG of the 'Skip' button.
            };

            _equipPopUp = new Pixel[2];

            _equipPopUp[0] = new Pixel()
            {
                Color = Color.FromArgb(255, 160, 162, 165),

                Point = new Point(831, 465)//X of close buton.
            };

            _equipPopUp[1] = new Pixel()
            {
                Color = Colors.CompleteOk,

                Point = new Point(645, 536)//BG of the 'Equip' button.
            };

            _okPopUp = new Pixel[2];

            _okPopUp[0] = new Pixel()
            {
                Color = Colors.White,

                Point = new Point(626, 475)//O of 'Ok' buton.
            };

            _okPopUp[1] = new Pixel()
            {
                Color = Colors.CompleteOk,

                Point = new Point(627, 456)//BG of the 'Ok' button.
            };
        }

        /// <summary>
        /// Starts helping with quest.
        /// </summary>
        public void Start()
        {
            ClosePopUps();

            SkipDialogs();

            SkipCutScenes();

            AcceptQuests();

            ClaimRewards();

            Continues();

            SkipTutorials();

            SkipTutorialOks();

            QuestCompletes();

            QuestStarts();

            QuestGoNows();

            Walks();

            SubQuestStarts();

            ConnectNows();

            ClosePopUps();
        }

        //common quest actions
        /// <summary>
        /// Detects and Click() the Skip dialog box that appears while questing.
        /// </summary>
        private void SkipDialogs()
        {
            if (_skipDialog[0].IsPresent(Screen, 2) && !_skipDialog[1].IsPresent(Screen, 2))
            {
                Thread.Sleep(100);
                Click(_skipDialog[0].Point);

            }

            if (_skipDialogTop[0].IsPresent(Screen, 2) && !_skipDialogTop[1].IsPresent(Screen, 2))
            {
                Thread.Sleep(100);
                Click(_skipDialogTop[0].Point);

            }
        }

        /// <summary>
        /// Detects and Click() the Skip cutscene box that appears while questing.
        /// </summary>
        private void SkipCutScenes()
        {
            if (_skipCutScene[0].IsPresent(Screen, 2) && !_skipCutScene[1].IsPresent(Screen, 2) && !_skipCutScene[2].IsPresent(Screen, 2))
            {
                Click(_skipCutScene[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Accept Quest button that appears while questing.
        /// </summary>
        private void AcceptQuests()
        {
            if (_acceptQuest[0].IsPresent(Screen, 2) && _acceptQuest[1].IsPresent(Screen, 2))
            {
                Click(_acceptQuest[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Claim Rewards button that appears while questing. Performs aditional tasks based on QuestType present in the Quest object.
        /// </summary>
        private void ClaimRewards()
        {
            if (_claimReward[0].IsPresent(Screen, 2) && _claimReward[1].IsPresent(Screen, 2))
            {
                Click(_claimReward[0].Point);
                if (Quest.Equals(QuestType.Main))
                {
                    System.Threading.Thread.Sleep(4000);//Sleep 4 seconds and then click click MainQuest
                    Click(_mainQuest.Point);

                }
            }
        }

        /// <summary>
        /// Detects and Click() the Continue button that appears while questing after completing a quest segment. Performs aditional tasks based on QuestType present in the Quest object.
        /// </summary>
        private void Continues()
        {
            if (_continue[0].IsPresent(Screen, 2) & _continue[1].IsPresent(Screen, 2))
            {
                Click(_continue[0].Point);
                if (Quest.Equals(QuestType.Main))
                {
                    System.Threading.Thread.Sleep(7000);//sleep 7 seconds and click main quest
                    Click(_mainQuest.Point);
                }
            }
        }

        /// <summary>
        /// Detects the Skip Tutorial button present during a quest tutorial and calls Click() on the button.
        /// </summary>
        private void SkipTutorials()
        {
            if (!Quest.Equals(QuestType.Weekly) && !Quest.Equals(QuestType.AltarOfMadness))//This is to quickly resolve an issue caused during the weekly quest
            {
                if (_skipTutorial[0].IsPresent(Screen, 2) & !_skipTutorial[1].IsPresent(Screen, 2))
                {
                    Click(_skipTutorial[0].Point);
                }
            }
        }

        /// <summary>
        /// Detects the OK validation pop up that is present after you press Skip Tutorial and calls Click() on the Ok button.
        /// </summary>
        private void SkipTutorialOks()
        {
            if (_skipTutorialOk[0].IsPresent(Screen, 2) & _skipTutorialOk[1].IsPresent(Screen, 2) && !Quest.Equals(QuestType.AltarOfMadness))
            {
                Click(_skipTutorialOk[0].Point);
            }
        }

        //weekly quest actions that apply to other quest types as well
        /// <summary>
        /// Detects and calls Click() at the Quest Complete button.
        /// </summary>
        private void QuestCompletes()
        {

            if (_questComplete[0].IsPresent(Screen, 2) & !_questComplete[1].IsPresent(Screen, 2))
            {
                Click(_questComplete[1].Point);
            }
        }

        /// <summary>
        /// Detects and calls Click() at the Quest Start Button present in the Weekly Quest menu.
        /// </summary>
        private void QuestStarts()
        {
            if (_questStart[0].IsPresent(Screen, 2) & _questStart[1].IsPresent(Screen, 2))
            {
                Click(_questStart[0].Point);
            }
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Detects and calls Click() at the Go Now button present on the Weekly Quest menu, after the Quest Start button is pushed.
        /// </summary>
        private void QuestGoNows()
        {
            if (_questGoNow[0].IsPresent(Screen, 2) & _questGoNow[0].IsPresent(Screen, 2))
            {
                Click(_questGoNow[1].Point);
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Detects and calls Click() at the Walk button that presents itself after you press Go Now on the Weekly Quest, when the Quest is located far away(ammong other times).
        /// </summary>
        private void Walks()
        {
            if (_questWalk[0].IsPresent(Screen, 2) & _questWalk[1].IsPresent(Screen, 2))
            {
                Click(_questWalk[0].Point);
                System.Threading.Thread.Sleep(100);
            }
        }

        //scroll quest actions that apply to other quest types.
        /// <summary>
        /// Detects and calls Click at the Start Quest button during a Scroll Quest. The graphics have small variations.
        /// </summary>
        private void SubQuestStarts()
        {
            if (_subQuestStart[0].IsPresent(Screen, 2) & _subQuestStart[1].IsPresent(Screen, 2))
            {
                Click(_subQuestStart[0].Point);
            }
        }

        /// <summary>
        /// Looks for the Loading graphic.
        /// </summary>
        /// <returns></returns>
        public bool IsStuckLoading()
        {
            foreach (Pixel p in _stuckLoading)
            {
                if (p.IsPresent(Screen, 5))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Detects and Click()'s the Connect Now button that appears during a loss of game session.
        /// </summary>
        private void ConnectNows()
        {
            if (_connectNow[0].IsPresent(Screen, 2) & _connectNow[1].IsPresent(Screen, 2))
            {
                Click(_connectNow[0].Point);
            }
        }

        /// <summary>
        /// Closes the annoying pop-ups.
        /// </summary>
        public void ClosePopUps()
        {
            if(_loginPopUp[0].IsPresent(Screen, 2) && !_loginPopUp[1].IsPresent(Screen, 2))
            {
                Click(_loginPopUp[0].Point);
            }

            if (_specialSalePopUp[0].IsPresent(Screen, 2) && !_specialSalePopUp[1].IsPresent(Screen, 2))
            {
                Click(_specialSalePopUp[0].Point);
            }

            if (_equipPopUp[0].IsPresent(Screen, 10) && _equipPopUp[1].IsPresent(Screen, 2))
            {
                Click(_equipPopUp[0].Point);
            }

            if (_okPopUp[0].IsPresent(Screen, 10) && _okPopUp[1].IsPresent(Screen, 2))
            {
                Click(_okPopUp[0].Point);
            }
        }
    }
}
