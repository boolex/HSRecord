namespace HSRecord.Core.Acts
{
	using HSRecord.Core.Entities;
	using HSRecord.Core.Enum;

	public class DeathAct : Act
	{
		public Card Card { get; set; }
		
		public override ActType Type
        {
            get { return ActType.Death; }
        }
    }
}
