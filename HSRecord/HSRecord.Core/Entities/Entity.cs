using HSRecord.Core.Data;

namespace HSRecord.Core.Entities
{
    public class Entity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Zone { get; set; }
        public int ZonePos { get; set; }
        public string CardId { get; set; }
        public int Player { get; set; }
        public string Type { get; set; }

        public Card Card
        {
            get { return CardManager.Get(CardId); }
        }
    }
}
