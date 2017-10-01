using System.Diagnostics;
using System.Drawing;

namespace L2RBot
{
    /// <summary>
    /// Used to perform and assist in completion of Lineage 2 Revolution quests. QuestHelper performs repetative tasks that are similar over all QuestTypes
    /// </summary>
    class QuestHelper
    {
        private Process app; //game process object(nox player)
        private Rectangle screen; //game window screen object(nox players screen location and demensions)
        public QuestType Quest { get; set; }//quest type is to be set during instantiation using object initializer {Quest = QuestType.Main}

        //Pixel objects
        private Pixel[] btnSkipDialog;
        private Pixel[] btnAcceptQuest;
        private Pixel[] btnClaimReward;
        private Pixel[] btnContinue;
        private Pixel[] btnSkipTutorial;
        private Pixel[] btnSkipOk;
        private Pixel[] btnQuestComplete;
        private Pixel[] btnQuestStart;
        private Pixel[] btnQuestGoNow;
        private Pixel[] btnQuestWalk;
        private Pixel[] btnSubQuestStart;
        private Pixel mainQuest;

        /// <summary>
        /// Constructs QuestHelper data objects.
        /// </summary>
        /// <param name="App">Android Emulator Process object</param>
        public QuestHelper(Process App)
        {
            app = Process.GetProcessById(App.Id);
            screen = Screen.GetRect(App);

            //QuestHelper Pixel objects initialized
            mainQuest = new Pixel //the pixel on btn to start/stop the main quest, method isPresent will not function as desired on this pixel as it changes frequently
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(125, 250)
            };

            //SkipQuestDialog button
            btnSkipDialog = new Pixel[2];
            btnSkipDialog[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(1162, 504)
            };
            btnSkipDialog[1] = new Pixel
            {
                Color = Color.FromArgb(255, 0, 0, 0),
                Point = new Point(1243, 594)
            };

            //AcceptQuest button
            btnAcceptQuest = new Pixel[2];
            btnAcceptQuest[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(772, 608)
            };
            btnAcceptQuest[1] = new Pixel
            {
                Color = Color.FromArgb(255, 55, 91, 133),
                Point = new Point(688, 595)
            };

            //ClaimReward button
            btnClaimReward = new Pixel[2];
            btnClaimReward[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(577, 609)
            };
            btnClaimReward[1] = new Pixel
            {
                Color = Color.FromArgb(255, 59, 95, 136),
                Point = new Point(609, 590)
            };

            //Continue button
            btnContinue = new Pixel[2];
            btnContinue[0] = new Pixel
            {
                Color = Color.FromArgb(255, 251, 251, 251),
                Point = new Point(1055, 633)
            };
            btnContinue[1] = new Pixel
            {
                Color = Color.FromArgb(255, 51, 88, 130),
                Point = new Point(1012, 623)
            };

            //SkipTutorial button
            btnSkipTutorial = new Pixel[3];
            btnSkipTutorial[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(1161, 43)
            };
            btnSkipTutorial[1] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(1236, 58)
            };
            btnSkipTutorial[2] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(1161, 39)
            };

            //Ok button, Skip Quest
            btnSkipOk = new Pixel[2];
            btnSkipOk[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(749, 480)
            };
            btnSkipOk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 55, 91, 133),
                Point = new Point(737, 468)
            };

            //Complete button, Weekly Quest
            btnQuestComplete = new Pixel[2];
            btnQuestComplete[0] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 85, 127),
                Point = new Point(841, 494)
            };
            btnQuestComplete[1] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(854, 500)
            };

            //Start Quest button, Weekly Quest
            btnQuestStart = new Pixel[2];
            btnQuestStart[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(871, 491)
            };
            btnQuestStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 41, 56, 76),
                Point = new Point(926, 496)
            };

            //Go Now button, Weekly Quest
            btnQuestGoNow = new Pixel[2];
            btnQuestGoNow[0] = new Pixel
            {
                Color = Color.FromArgb(255, 41, 55, 76),
                Point = new Point(911, 496)
            };
            btnQuestGoNow[1] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(925, 493)
            };

            //Walk button, Weekly Quest
            btnQuestWalk = new Pixel[2];
            btnQuestWalk[0] = new Pixel
            {
                Color = Color.FromArgb(255, 120, 130, 140),
                Point = new Point(477, 514)
            };
            btnQuestWalk[1] = new Pixel
            {
                Color = Color.FromArgb(255, 25, 38, 56),
                Point = new Point(492, 537)
            };

            //Walk button, Weekly Quest
            btnSubQuestStart = new Pixel[2];
            btnSubQuestStart[0] = new Pixel
            {
                Color = Color.FromArgb(255, 255, 255, 255),
                Point = new Point(712, 603)
            };
            btnSubQuestStart[1] = new Pixel
            {
                Color = Color.FromArgb(255, 49, 85, 127),
                Point = new Point(701, 607)
            };
        }

        /// <summary>
        /// A click method for the Quest object to complete and monitor click events.
        /// </summary>
        /// <param name="ClickHere"></param>
        private void Click(Point ClickHere)
        {
            ClickHere = Screen.PointToScreenPoint(screen, ClickHere.X, ClickHere.Y);
            Mouse.LeftMouseClick(ClickHere.X, ClickHere.Y);
        }

        /// <summary>
        /// Starts helping with quest.
        /// </summary>
        public void Start()
        {
            SkipQuestDialog();
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
            if (btnSkipDialog[0].IsPresent(screen, 2) && btnSkipDialog[1].IsPresent(screen, 2))
            {
                Click(btnSkipDialog[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Accept Quest button that appears while questing.
        /// </summary>
        private void AcceptQuest()
        {
            if (btnAcceptQuest[0].IsPresent(screen, 2) && btnAcceptQuest[1].IsPresent(screen, 2))
            {
                Click(btnAcceptQuest[0].Point);
            }
        }

        /// <summary>
        /// Detects and Click() the Claim Rewards button that appears while questing. Performs aditional tasks based on QuestType present in the Quest object.
        /// </summary>
        private void ClaimReward()
        {
            if (btnClaimReward[0].IsPresent(screen, 2) && btnClaimReward[1].IsPresent(screen, 2))
            {
                Click(btnClaimReward[0].Point);
                if (Quest.Equals(QuestType.Main))
                {
                    System.Threading.Thread.Sleep(4000);//Sleep 4 seconds and then click click MainQuest
                    Click(mainQuest.Point);

                }
            }
        }

        /// <summary>
        /// Detects and Click() the Continue button that appears while questing after completing a quest segment. Performs aditional tasks based on QuestType present in the Quest object.
        /// </summary>
        private void Continue()
        {
            if (btnContinue[0].IsPresent(screen, 2) & btnContinue[1].IsPresent(screen, 2))
            {
                Click(btnContinue[0].Point);
                if (Quest.Equals(QuestType.Main))
                {
                    System.Threading.Thread.Sleep(7000);//sleep 7 seconds and click main quest
                    Click(mainQuest.Point);
                }
            }
        }

        /// <summary>
        /// Detects the Skip Tutorial button present during a quest tutorial and calls Click() on the button.
        /// </summary>
        private void SkipTutorial()
        {
            if (!Quest.Equals(QuestType.Weekly))//This is to quickly resolve an issue caused during the weekly quest
            {
                if (btnSkipTutorial[0].IsPresent(screen, 2) & !btnSkipTutorial[1].IsPresent(screen, 2) & btnSkipTutorial[2].IsPresent(screen, 2))
                {
                    Click(btnSkipTutorial[0].Point);
                }
            }
        }

        /// <summary>
        /// Detects the OK validation pop up that is present after you press Skip Tutorial and calls Click() on the Ok button.
        /// </summary>
        private void SkipTutorialOK()
        {
            if (btnSkipOk[0].IsPresent(screen, 2) & btnSkipOk[1].IsPresent(screen, 2))
            {
                Click(btnSkipOk[0].Point);
            }
        }

        //weekly quest actions that apply to other quest types as well

        /// <summary>
        /// Detects and calls Click() at the Quest Complete button.
        /// </summary>
        private void QuestComplete()
        {

            if (btnQuestComplete[0].IsPresent(screen, 2) & btnQuestComplete[1].IsPresent(screen, 2))
            {
                Click(btnQuestComplete[1].Point);
            }
        }

        /// <summary>
        /// Detects and calls Click() at the Quest Start Button present in the Weekly Quest menu.
        /// </summary>
        private void QuestStart()
        {
            if (btnQuestStart[0].IsPresent(screen, 2) & btnQuestStart[1].IsPresent(screen, 2))
            {
                Click(btnQuestStart[0].Point);
            }
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// Detects and calls Click() at the Go Now button present on the Weekly Quest menu, after the Quest Start button is pushed.
        /// </summary>
        private void QuestGoNow()
        {
            if (btnQuestGoNow[0].IsPresent(screen, 2) & btnQuestGoNow[0].IsPresent(screen, 2))
            {
                Click(btnQuestGoNow[1].Point);
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Detects and calls Click() at the Walk button that presents itself after you press Go Now on the Weekly Quest, when the Quest is located far away(ammong other times)
        /// </summary>
        private void Walk()
        {
            if (btnQuestWalk[0].IsPresent(screen, 2) & btnQuestWalk[1].IsPresent(screen, 2))
            {
                Click(btnQuestWalk[0].Point);
                System.Threading.Thread.Sleep(100);
            }
        }
        //scroll quest actions that apply to other quest types

        /// <summary>
        /// Detects and calls Click at the Start Quest button during a Scroll Quest. The graphics have small variations
        /// </summary>
        private void SubQuestStart()
        {
            if (btnSubQuestStart[0].IsPresent(screen, 2) & btnSubQuestStart[1].IsPresent(screen, 2))
            {
                Click(btnSubQuestStart[0].Point);
            }
        }

    }

    /// <summary>
    /// an Enumeration for QuestHelper objects to enable extra functionality depending on which type of quest's it is being called by. 
    /// </summary>
    public enum QuestType
    {
        Main,
        Weekly,
        Scroll
    }
}
