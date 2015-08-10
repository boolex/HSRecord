namespace HSRecord.Core.Entities
{
    using System.Collections.Generic;
    using HSRecord.Core.Enum;

    public class Act
    {
        public virtual ActType Type { get; set; }
        public byte ManaCrystalsAtStart { get; set; }
        public byte ManaCrystalsAtEnd { get; set; }
        public List<Act> AfterEffects { get; set; }

        public Act()
        {
            AfterEffects=new List<Act>();
        }
    }
}
