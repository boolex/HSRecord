namespace HSRecord.Core.Acts
{
    using HSRecord.Core.Enum;
    using HSRecord.Core.Entities;
    public class PlayCardAct : Act
    {
        public override ActType Type
        {
            get { return ActType.Card; }
        }

        public Card Card { get; set; }

        public PlayCardAct(Card card)
        {
            Card = card;
        }

    }
}
