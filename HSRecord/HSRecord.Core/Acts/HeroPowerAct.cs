namespace HSRecord.Core.Acts
{
    using HSRecord.Core.Entities;
    using HSRecord.Core.Enum;
    public class HeroPowerAct : Act
    {
        public override ActType Type
        {
            get { return ActType.HeroPower; }
        }
    }
}
