namespace HSRecord.UnitTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Core.Acts;
    using Core.Entities;
    using Core.Enum;
    using Core.Parsing;

    [TestClass]
    public class Game_32506171_Tests : BaseTest
    {
        protected static string GameFilePath
        {
            get { return @"C:\Projects\HSRecord\HSRecord\data\Game32506171.txt"; }
        }

        [ClassInitialize]
        public static void InitGame(TestContext context)
        {
            var parser = new HSParser();
            game = parser.ParseGame(GameFilePath);
        }

        [TestMethod]
        public void TotalNumberOfTurnsShouldBe19()
        {
            Assert.AreEqual(9 + 10, game.Turns.Count);
        }
        #region Draw
        [TestMethod]
        public void Step1Draw()
        {
            //warsong
            Assert.AreEqual("EX1_084", GetDrawnCardId(1));
        }
        [TestMethod]
        public void Step3Draw()
        {
            //Inner rage
            Assert.AreEqual("EX1_607", GetDrawnCardId(3));
        }
        [TestMethod]
        public void Step5Draw()
        {
            //Gnomish inventor
            Assert.AreEqual("CS2_147", GetDrawnCardId(5));
        }
        [TestMethod]
        public void Step7Draw()
        {
            //Death's bite
            Assert.AreEqual("FP1_021", GetDrawnCardId(7));
        }
        [TestMethod]
        public void Step9Draw()
        {
            //Unstable Ghoul
            Assert.AreEqual("FP1_024", GetDrawnCardId(9));
        }
        [TestMethod]
        public void Step9Draw2()
        {
            //Grommash Hellscream
            Assert.AreEqual("EX1_414", GetDrawnCardId(9, 2));
        }
        [TestMethod]
        public void Step11Draw()
        {
            //Death's bite
            Assert.AreEqual("FP1_021", GetDrawnCardId(11));
        }
        [TestMethod]
        public void Step11Draw2()
        {
            //War axe
            Assert.AreEqual("CS2_106", GetDrawnCardId(11, 2));
        }
        [TestMethod]
        public void Step11Draw3()
        {
            //Emperor Thaurissan
            Assert.AreEqual("BRM_028", GetDrawnCardId(11, 3));
        }
        [TestMethod]
        public void Step11Draw4()
        {
            //Cruel Taskmaster
            Assert.AreEqual("EX1_603", GetDrawnCardId(11, 4));
        }
        [TestMethod]
        public void Step11Draw5()
        {
            //Execute
            Assert.AreEqual("CS2_108", GetDrawnCardId(11, 5));
        }
        [TestMethod]
        public void Step13Draw()
        {
            //Acolyte of pain
            Assert.AreEqual("EX1_007", GetDrawnCardId(13));
        }
        [TestMethod]
        public void Step15Draw()
        {
            //Unstable ghoul
            Assert.AreEqual("FP1_024", GetDrawnCardId(15));
        }
        [TestMethod]
        public void Step17Draw()
        {
            //War axe
            Assert.AreEqual("CS2_106", GetDrawnCardId(17));
        }
        [TestMethod]
        public void Step19Draw()
        {
            //Battle rage
            Assert.AreEqual("EX1_392", GetDrawnCardId(19));
        }
        #endregion Draw
        #region Play
        [TestMethod]
        public void Step2Play()
        {
            //Warbot
            Assert.AreEqual("GVG_051", GetPlayedCardId(2));
        }
        [TestMethod]
        public void Step4Play()
        {
            //War axe
            Assert.AreEqual("CS2_106", GetPlayedCardId(4));
        }
        [TestMethod]
        public void Step5Play()
        {
            //Acolyte
            Assert.AreEqual("EX1_007", GetPlayedCardId(5));
        }
        [TestMethod]
        public void Step5Play2()
        {
            //Inner rage
            Assert.AreEqual("EX1_607", GetPlayedCardId(5, 2));
        }
        [TestMethod]
        public void Step6Play1()
        {
            //Execue
            Assert.AreEqual("CS2_108", GetPlayedCardId(6));
        }
        [TestMethod]
        public void Step6Play2()
        {
            //Coin
            Assert.AreEqual("GAME_005", GetPlayedCardId(6, 2));
        } 
        [TestMethod]
        public void Step6Play3()
        {
            //Warsong
            Assert.AreEqual("EX1_084", GetPlayedCardId(6, 3));
        }
        [TestMethod]
        public void Step7Play()
        {
            //Death's bite
            Assert.AreEqual("FP1_021", GetPlayedCardId(7));
        }
        [TestMethod]
        public void Step8Play()
        {
            //Arathi
            Assert.AreEqual("EX1_398", GetPlayedCardId(8));
        }
        [TestMethod]
        public void Step9Play()
        {
            //Gnomish inventor
            Assert.AreEqual("CS2_147", GetPlayedCardId(9));
        }
        [TestMethod]
        public void Step10Play()
        {
            //Axe Slinger
            Assert.AreEqual("BRM_016", GetPlayedCardId(10));
        }
        [TestMethod]
        public void Step11Play()
        {
            //Ace Slinger
            Assert.AreEqual("BRM_016", GetPlayedCardId(10));
        }
        #endregion
    }
}
