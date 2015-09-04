namespace HSRecord.UnitTests
{
    using System.Linq;
    using Core.Acts;
    using Core.Entities;
    using Core.Enum;

    public abstract class BaseTest
    {
        protected static Game game;

        protected string GetDrawnCardId(int turnIndex, int drawIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var draws = turn.Acts.Where(x => x.Type == ActType.Draw).ToList();

            if (draws.Count < drawIndex)
                return null;
            return ((DrawCardAct)draws[drawIndex - 1]).Card.Id;
        }
		protected int GetDrawnCardCount(int turnIndex)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            return turn.Acts.Where(x => x.Type == ActType.Draw).ToList().Count;
        }
        protected string GetPlayedCardId(int turnIndex, int playIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var draws = turn.Acts.Where(x => x.Type == ActType.Play).ToList();

            if (draws.Count < playIndex)
                return null;
            return ((PlayCardAct)draws[playIndex - 1]).Card.Id;
        }
		protected int GetPlayedCardCount(int turnIndex)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            return turn.Acts.Where(x => x.Type == ActType.Play).ToList().Count;
        }
		protected string GetAttackerCardId(int turnIndex, int playIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var draws = turn.Acts.Where(x => x.Type == ActType.Attack).ToList();

            if (draws.Count < playIndex)
                return null;
            return ((AttackAct)draws[playIndex - 1]).Entity.Id;
        }
		protected string GetAttackedCardId(int turnIndex, int playIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var draws = turn.Acts.Where(x => x.Type == ActType.Attack).ToList();

            if (draws.Count < playIndex)
                return null;
            return ((AttackAct)draws[playIndex - 1]).Target.Id;
        }
		protected int GetAttacksCount(int turnIndex)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            return turn.Acts.Where(x => x.Type == ActType.Attack).ToList().Count;
        }
		protected string GetDeathCardId(int turnIndex, int deathIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var deaths = turn.Acts.Where(x => x.Type == ActType.Death).ToList();

            if (deaths.Count < deathIndex)
                return null;
            return ((DeathAct)deaths[deathIndex - 1]).Card.Id;
        }
		protected int GetDeathCount(int turnIndex)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            return turn.Acts.Where(x => x.Type == ActType.Death).ToList().Count;
        }
    }
}
