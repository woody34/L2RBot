using System.Diagnostics;
using System.Drawing;
using L2RBot.Common.Enum;
using L2RBot.Common;

namespace L2RBot
{
    /// <summary>
    /// Used to perform and assist in completion of Lineage 2 Revolution quests. QuestHelper performs repetative tasks that are similar over all QuestTypes
    /// </summary>
    public class QuestHelper
    {
        private Process app; //game process object(nox player)
        private Rectangle Screen; //game window screen object(nox players screen location and demensions)
        public QuestType Quest { get; set; }//quest type is to be set during instantiation using object initializer {Quest = QuestType.Main}

        //Pixel objects
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

        /// <summary>
        /// Constructs QuestHelper data objects.
        /// </summary>
        /// <param name="App">Android Emulator Process object</param>
        public QuestHelper(Process App)
        {
            app = Process.GetProcessById(App.Id);
            Screen = L2RBot.Screen.GetRect(App);

            //QuestHelper Pixel objects initialized
            _mainQuest = new Pixel //the pixel on btn to start/stop the main quest, method isPresent will not function as desired on this pixel as it changes frequently
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(125, 250)
            };

            //SkipQuestDialog button
            _skipDialog = new Pixel[2];
            _skipDialog[0] = new Pixel
            {
                Color = Colors.White,
                Point = new Point(1165, 507)//White S on 'Skip' button.
            };
            _skipDialog[1] = new Pixel
            {
                Color = Colors.Black,
                Point = new Point(1249, 600)//Black BG of the '>' (skip arrow).
            };

            //SkipQuestDialog button
            _skipCutScene = new Pixel[3];
            _skipCutScene[0] = new Pixel
            {
                Color = Color.FromArgb(255, 232, 232, 232),
                Point = new Point(1137, 46)
            };
            _skipCutScene[1] = new Pixel
            {
                Color = Color.White,
                Point = new Point(1149, 10)
            };
            _skipCutScene[2] = new Pixel
            {
                Color = Color.FromArgb(255, 232, 232, 232),
                Point = new Point(39, 16)
            };

            //AcceptQuest button
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

            //ClaimReward button
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

            //Continue button
            _continue = new Pixel[2];
            _continue[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(1055, 633)
            };
            _continue[1] = new Pixel
            {
                Color = Color.FromArgb(255, 51, 88, 130),
                Point = new Point(1012, 623)
            };

            //SkipTutorial button
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

            //Ok button, Skip Quest
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

            //Complete button, Weekly Quest
            _questComplete = new Pixel[2];
            _questComplete[0] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 85, 127),
                Point = new Point(841, 494)
            };
            _questComplete[1] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(854, 500)
            };

            //Start Quest button, Weekly Quest
            _questStart = new Pixel[2];
            _questStart[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(871, 491)
            };
            _questStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 41, 56, 76),
                Point = new Point(926, 496)
            };

            //Go Now button, Weekly Quest
            _questGoNow = new Pixel[2];
            _questGoNow[0] = new Pixel
            {
                Color = Color.FromArgb(255, 41, 55, 76),
                Point = new Point(911, 496)
            };
            _questGoNow[1] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(925, 493)
            };

            //Walk button, Weekly Quest
            _questWalk = new Pixel[2];
            _questWalk[0] = new Pixel
            {
                Color = Color.FromArgb(255, 120, 130, 140),
                Point = new Point(477, 514)
            };
            _questWalk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 25, 38, 56),
                Point = new Point(492, 537)
            };

            //Walk button, Weekly Quest
            _subQuestStart = new Pixel[2];
            _subQuestStart[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(712, 603)
            };
            _subQuestStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 85, 127),
                Point = new Point(701, 607)
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

        }

        /// <summary>
        /// A click method for the Quest object to complete and monitor click events.
        /// </summary>
        /// <param name="ClickHere"></param>
        private void Click(Point ClickHere)
        {
            ClickHere = L2RBot.Screen.PointToScreenPoint(Screen, ClickHere.X, ClickHere.Y);
            Mouse.LeftMouseClick(ClickHere.X, ClickHere.Y);
        }

        /// <summary>
        /// Starts helping with quest.
        /// </summary>
        public void Start()
        {
            SkipQuestDialog();
            SkipCutScene();
            AcceptQuest();
            ClaimReward();
            Continue();
            SkipTutorial();
            SkipTutorialOK();
            QuestComplete();
            QuestStart();
            QuestGoNow();
            Walk();
            SubQuestStart();
        }

        //common quest actions
        /// <summary>
        /// Detects and Click() the Skip dialog box that appears while questing.
        /// </summary>
        private void SkipQuestDialog()
        {
            if (_skipDialog[0].IsPresent(Screen, 2) && _skipDialog[1].IsPresent(Screen, 2))
            {
                Click(_skipDialog[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Skip cutscene box that appears while questing.
        /// </summary>
        private void SkipCutScene()
        {
            if (_skipCutScene[0].IsPresent(Screen, 2) && !_skipCutScene[1].IsPresent(Screen, 2) && !_skipCutScene[2].IsPresent(Screen, 2))
            {
                Click(_skipCutScene[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Accept Quest button that appears while questing.
        /// </summary>
        private void AcceptQuest()
        {
            if (_acceptQuest[0].IsPresent(Screen, 2) && _acceptQuest[1].IsPresent(Screen, 2))
            {
                Click(_acceptQuest[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Claim Rewards button that appears while questing. Performs aditional tasks based on QuestType present in the Quest object.
        /// </summary>
        private void ClaimReward()
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
        private void Continue()
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
        private void SkipTutorial()
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
        private void SkipTutorialOK()
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
        private void QuestComplete()
        {

            if (_questComplete[0].IsPresent(Screen, 2) & _questComplete[1].IsPresent(Screen, 2))
            {
                Click(_questComplete[1].Point);
            }
        }

        /// <summary>
        /// Detects and calls Click() at the Quest Start Button present in the Weekly Quest menu.
        /// </summary>
        private void QuestStart()
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
        private void QuestGoNow()
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
        private void Walk()
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
        private void SubQuestStart()
        {
            if (_subQuestStart[0].IsPresent(Screen, 2) & _subQuestStart[1].IsPresent(Screen, 2))
            {
                Click(_subQuestStart[0].Point);
            }
        }

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

    }
}
