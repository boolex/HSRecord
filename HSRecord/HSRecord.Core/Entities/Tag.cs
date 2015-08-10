namespace HSRecord.Core.Entities
{
    public class Tag
    {
        public Entity Entity { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsConsistent { get; set; }
    }
}
