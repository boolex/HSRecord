namespace HSRecord.Core.Acts
{
    using HSRecord.Core.Entities;
    using HSRecord.Core.Enum;
    public class AttackAct : Act
    {
        public override ActType Type
        {
            get { return ActType.Attack; }
        }
        public Card Entity { get; set; }
        public Card Target { get; set; } 

    }
}
