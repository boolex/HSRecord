namespace HSRecord.Core.Acts
{
    using HSRecord.Core.Entities;
    public class DrawCardAct :Act
    {
        private Card card;
        public DrawCardAct(Card card)
        {
            this.card = card;
        }
    }
}
