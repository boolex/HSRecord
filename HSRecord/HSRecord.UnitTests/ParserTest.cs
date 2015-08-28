using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HSRecord.UnitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using HSRecord.Core.Entities;
	using HSRecord.Core.Parsing;
	using HSRecord.Core.Enum;
	using HSRecord.Core.Acts;
	[TestClass]
	public class ParserTest
	{
		private const string Grim1FilePath =
            @"C:\prog\HSRecord\data\Grim1.txt";
		private const string Grim2FilePath =
            @"C:\prog\HSRecord\data\Grim2.txt";

		[TestMethod]
		public void TotalNumberOfTurnsInGrim2ShouldBe19()
		{
			HSParser parser = new HSParser();
			Game game = parser.ParseGame(Grim2FilePath);
			Assert.AreEqual(9 + 10, game.Turns.Count);
		}

		[TestMethod]
		public void Step3DrawInGrim2ShouldExist()
		{
			HSParser parser = new HSParser();
			Game game = parser.ParseGame(Grim2FilePath);
			var turn = game.Turns.Skip(2).First();
			Assert.AreNotEqual(turn, null);
		}

		[TestMethod]
		public void Step1DrawInGrim2ShouldBeWarSong()
		{
			HSParser parser = new HSParser();
			Game game = parser.ParseGame(Grim2FilePath);
			var turn = game.Turns.Single(x =>x.TurnIndex == 2);
			var draw = (DrawCardAct)(turn.Acts.FirstOrDefault(x => x.Type == ActType.Draw));
			Assert.AreEqual("Командир песни войны", draw.Card.Name);
		}


    }
}
