namespace HSRecord.Core.Entities
{
    using System.Collections.Generic;

    public class Turn
    {
        public string Player { get; set; }
        public List<Act> Acts { get; set; }
        public byte ManaCrystals { get; set; }
        public int TurnIndex { get; set; }
        public Card Draw { get; set; }

        public Turn()
        {
            Acts=new List<Act>();
        }
    }
}
