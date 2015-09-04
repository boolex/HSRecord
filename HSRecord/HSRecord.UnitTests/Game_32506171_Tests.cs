namespace HSRecord.UnitTests
{
	using System.Diagnostics;
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
            get { 
			//return @"C:\Projects\HSRecord\HSRecord\data\Game32506171.txt"; 
			return @"C:\prog\HSRecord\data\Game32506171.txt";
			}
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
		#region Draw Count
		[TestMethod]
        public void Step1Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(1));
        }
		[TestMethod]
        public void Step2Draws()
        {//
            Assert.AreEqual(0, GetDrawnCardCount(2));
        }
		[TestMethod]
        public void Step3Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(3));
        }
		[TestMethod]
        public void Step4Draws()
        {
            Assert.AreEqual(0, GetDrawnCardCount(4));
        }
		[TestMethod]
        public void Step5Draws()
        {
            Assert.AreEqual(2, GetDrawnCardCount(5));
        }
		[TestMethod]
        public void Step6Draws()
        {//
            Assert.AreEqual(0, GetDrawnCardCount(6));
        }
		[TestMethod]
        public void Step7Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(7));
        }
		[TestMethod]
        public void Step8Draws()
        {//
            Assert.AreEqual(0, GetDrawnCardCount(8));
        }
		[TestMethod]
        public void Step9Draws()
        {
            Assert.AreEqual(2, GetDrawnCardCount(9));
        }
		[TestMethod]
        public void Step10Draws()
        {///
            Assert.AreEqual(0, GetDrawnCardCount(10));
        }
		[TestMethod]
        public void Step11Draws()
        {
            Assert.AreEqual(5, GetDrawnCardCount(11));
        }
		[TestMethod]
        public void Step12Draws()
        {///
            Assert.AreEqual(0, GetDrawnCardCount(12));
        }
		[TestMethod]
        public void Step13Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(13));
        }
		[TestMethod]
        public void Step14Draws()
        {///
            Assert.AreEqual(0, GetDrawnCardCount(14));
        }
		[TestMethod]
        public void Step15Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(15));
        }
		[TestMethod]
        public void Step16Draws()
        {//
            Assert.AreEqual(0, GetDrawnCardCount(16));
        }
		[TestMethod]
        public void Step17Draws()
        {//
            Assert.AreEqual(2, GetDrawnCardCount(17));
        }
		[TestMethod]
        public void Step18Draws()
        {//
            Assert.AreEqual(2, GetDrawnCardCount(18));
        }
		[TestMethod]
        public void Step19Draws()
        {
            Assert.AreEqual(1, GetDrawnCardCount(19));
        }
		#endregion
        #region Draw
        [TestMethod]
        public void Step1Draw()
        {
            //Warsong
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
		#region Attack
		[TestMethod]
        public void Step7Attack1()
        {
            //Death's bite
            Assert.AreEqual("HERO_01", GetAttackerCardId(7));
        }
		[TestMethod]
        public void Step7Attacked1()
        {
            //Death's bite
            Assert.AreEqual("EX1_084", GetAttackedCardId(7));
        }
		[TestMethod]
        public void Step8Attack1()
        {
            //Warbot
            Assert.AreEqual("GVG_051", GetAttackerCardId(8));
		}
		[TestMethod]
        public void Step8Attacked1()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackedCardId(8));
		}
		[TestMethod]
        public void Step8Attack2()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackerCardId(8, 2));
		}
		[TestMethod]
        public void Step8Attacked2()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackedCardId(8, 2));
		}
		[TestMethod]
        public void Step10Attack1()
        {
			var card = GetAttackerCardId(10, 1);
			Trace.Listeners.Add(new ConsoleTraceListener());
			Trace.WriteLine(card);
            //Face
            Assert.AreEqual("GVG_051", card);
		}
		[TestMethod]
        public void Step10Attacked1()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackedCardId(10, 1));
		}
		[TestMethod]
        public void Step10Attack2()
        {
            //Warbot
            Assert.AreEqual("EX1_398", GetAttackerCardId(10,2));
		}
		[TestMethod]
        public void Step10Attacked2()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackedCardId(10, 2));
		}
		[TestMethod]
        public void Step10Attack3()
        {
            //Arathi
            Assert.AreEqual("HERO_01", GetAttackerCardId(10, 3));
		}
		[TestMethod]
        public void Step10Attacked3()
        {
            //Face
            Assert.AreEqual("HERO_01", GetAttackedCardId(10, 3));
		}
		[TestMethod]
        public void Step11Attack1()
        {
            //Gnomish inventor
            Assert.AreEqual("CS2_147", GetAttackerCardId(11, 1));
		}
		[TestMethod]
        public void Step11Attacked1()
        {
            //Warbot
            Assert.AreEqual("GVG_051", GetAttackedCardId(11, 1));
		}		
		[TestMethod]
        public void Step11Attack2()
        {
            //Grommash Death's bite
            Assert.AreEqual("HERO_01", GetAttackerCardId(11, 2));
		}
		[TestMethod]
        public void Step11Attacked2()
        {
            //Axe Slinger
            Assert.AreEqual("BRM_016", GetAttackedCardId(11, 2));
		}
		#endregion
		#region Deaths
		[TestMethod]
        public void Step11Death1()
        {
           //Death's bite
            Assert.AreEqual("FP1_021", GetDeathCardId(11, 1));
		}
		[TestMethod]
        public void Step11Death2()
        {
            //Warbot
            Assert.AreEqual("GVG_051", GetDeathCardId(11, 2));
		}
		[TestMethod]
        public void Step11Death3()
        {
            //Axe Slinger
            Assert.AreEqual("BRM_016", GetDeathCardId(11, 3));
		}
		#endregion
    }
}
