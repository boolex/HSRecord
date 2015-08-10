namespace HSRecord.Core.Acts
{
	using HSRecord.Core.Entities;
	using HSRecord.Core.Enum;
	
	public class DrawCardAct :Act
	{
		public Card Card;
		public DrawCardAct(Card card)
		{
			this.Card = card;
		}
		 public override ActType Type
        {
            get { return ActType.Draw; }
        }
    }
}
