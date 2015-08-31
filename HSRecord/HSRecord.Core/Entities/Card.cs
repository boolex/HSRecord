namespace HSRecord.Core.Entities
{
    using HSRecord.Core.Enum;
    public class Card
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int CardIdInGame { get; set; }
        public string CardGlobalId { get; set; }
        public string Name { get; set; }
        public Team Team { get; set; }
    }
}
