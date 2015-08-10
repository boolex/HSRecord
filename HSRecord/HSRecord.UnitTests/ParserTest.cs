using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HSRecord.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HSRecord.Core.Entities;
    using HSRecord.Core.Parsing;

    [TestClass]
    public class ParserTest
    {
        private const string Grim1FilePath =
            @"C:\Projects\HSRecord\HSRecord\data\Grim1.txt";
        private const string Grim2FilePath =
            @"C:\Projects\HSRecord\HSRecord\data\Grim2.txt";

        [TestMethod]
        public void TotalNumberOfTurnsInGrim1ShouldBe16()
        {
            HSParser parser = new HSParser();
            Game game = parser.ParseGame(Grim1FilePath);
            Assert.AreEqual(16, game.Turns.Count);
        }
        [TestMethod]
        public void TotalNumberOfTurnsInGrim2ShouldBe19()
        {
            HSParser parser = new HSParser();
            Game game = parser.ParseGame(Grim2FilePath);
            Assert.AreEqual(9 + 10, game.Turns.Count);
        }
        [TestMethod]
        public void Step1DrawInGrim2ShouldBeWarSong()
        {
            HSParser parser = new HSParser();
            Game game = parser.ParseGame(Grim2FilePath);
            var turn = game.Turns.SingleOrDefault(x => x.TurnIndex == 3);

            Assert.AreEqual("Командир песни войны", turn.Draw.Name);
        }


    }
}
