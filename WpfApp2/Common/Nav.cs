using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace L2RBot.Common
{
    /// <summary>
    /// Contains the points of common menu buttons
    /// </summary>
    public class Nav
    {
        public static Point BtnHamburger = new Point(921, 34);

        #region Hamburger_SubNav
        //hamburger button.


        //hamburger button's child buttons
        //top row.
        public static Point BtnCharacter = new Point(240, 50);

        public static Point BtnItems = new Point(400, 50);

        public static Point BtnChallenges = new Point(560, 50);

        public static Point BtnEvents = new Point(720, 50);

        public static Point BtnShop = new Point(880, 50);

        public static Point BtnSettings = new Point(1040, 50);

        //bottom row.
        public static Point BtnDungeons = new Point(240, 650);

        public static Point BtnBattlefield = new Point(400, 650);

        public static Point BtnClan = new Point(560, 650);

        public static Point BtnFriends = new Point(720, 650);

        public static Point BtnTradingPost = new Point(880, 650);

        public static Point BtnRanking = new Point(1040, 650);
        //end of hamburger button's children

        //grandchildren points
        //all third button points are going to share common these Points.
        //top row
        private static Point _btnSubTop1 = new Point(240, 180);

        private static Point _btnSubTop2 = new Point(400, 180);

        private static Point _btnSubTop3 = new Point(560, 180);

        private static Point _btnSubTop4 = new Point(720, 180);

        private static Point _btnSubTop5 = new Point(880, 180);

        private static Point _btnSubTop6 = new Point(1040, 180);

        //bottom
        private static Point _btnSubBottom1 = new Point(240, 550);

        private static Point _btnSubBottom2 = new Point(400, 550);

        private static Point _btnSubBottom3 = new Point(560, 550);

        private static Point _btnSubBottom4 = new Point(720, 550);

        private static Point _btnSubBottom5 = new Point(880, 550);

        private static Point _btnSubBottom6 = new Point(1040, 550);
        //end of grandchildren points

        //Hamburger>Character's children Points.
        public static Point BtnSubCharacter = _btnSubTop1;

        public static Point BtnSubSkills = _btnSubTop2;

        public static Point BtnSubRunes = _btnSubTop3;

        public static Point BtnSubElixers = _btnSubTop4;

        public static Point BtnSubClass = _btnSubTop5;

        public static Point BtnSubPets = _btnSubTop6;

        //Hamburger>Items' children Points.
        public static Point BtnSubInventory = _btnSubTop1;

        public static Point BtnSubForge = _btnSubTop2;

        public static Point BtnSubSoulCrystals = _btnSubTop3;

        public static Point BtnSubWorkshop = _btnSubTop4;

        public static Point BtnSubCloak = _btnSubTop5;

        //Hamburger>Challenges' children Points.
        public static Point BtnSubQuest = _btnSubTop1;

        public static Point BtnSubDailyActivites = _btnSubTop2;

        public static Point BtnSubAchievements = _btnSubTop3;

        public static Point BtnSubMonsterCodex = _btnSubTop4;

        public static Point BtnSubTitle = _btnSubTop5;

        public static Point BtnSubMercenarysJourney = _btnSubTop6;

        //Hamburger>Events' children Points.
        public static Point BtnSubEvent = _btnSubTop1;

        public static Point BtnSubRecessReward = _btnSubTop2;

        public static Point BtnSubLoginReward = _btnSubTop3;

        public static Point BtnSubDraw = _btnSubTop4;

        //Hamburger>Dungeons' chidlren Points.
        public static Point BtnSubNormalDungeon = _btnSubBottom1;

        public static Point BtnSubTemporalRift = _btnSubBottom2;

        //Hamburger>Clans' chidlren Points.
        public static Point BtnSubClan = _btnSubBottom1;

        public static Point BtnSubClanDungeon = _btnSubBottom2;

        public static Point BtnSubClanHall = _btnSubBottom3;
        #endregion Hamburger_button

        public static Point BtnBag = new Point(983, 34);

        #region Bag_SubNav

        public static Point BagClose = new Point(1239, 38);

        public static Point WeaponBag = new Point(687, 123);

        public static Point ArmorBag = new Point(759, 123);

        public static Point JewelBag = new Point(858, 123);

        public static Point SoulCryBag = new Point(946, 123);

        public static Point PetBag = new Point(1037, 123);

        public static Point PotionBag = new Point(1123, 123);

        public static Point ScrollBag = new Point(1213, 123);

        #endregion


        public static Point QuestMenu = new Point(920, 88);

        #region QuestMenu_SubNav

        public static Point BtnWeekly = new Point(109, 285);

        #endregion      

        public static Point Map = new Point(1180, 100);

        public static Point MapClose = new Point(1243, 68);

        public static Point AutoCombat = new Point(935, 614);
    }
}
