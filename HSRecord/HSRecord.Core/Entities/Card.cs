namespace HSRecord.Core.Entities
{
    using HSRecord.Core.Enum;
    public class Card
    {
        public int CardIdInGame { get; set; }
        private string CardGlobalId { get; set; }
        public string Name { get; set; }
        public Team Team { get; set; }
    }
}
