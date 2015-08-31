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
        protected string GetPlayedCardId(int turnIndex, int playIndex = 1)
        {
            var turn = game.Turns.Single(x => x.TurnIndex == turnIndex);
            var draws = turn.Acts.Where(x => x.Type == ActType.Play).ToList();

            if (draws.Count < playIndex)
                return null;
            return ((PlayCardAct)draws[playIndex - 1]).Card.Id;
        }
    }
}
