namespace HSRecord.Core.Entities
{
    using System.Collections.Generic;
    public class GameAction
    {
        public Entity Entity { get; set; }
        public string SubType { get; set; }
        public int? Index { get; set; }
        public Entity Target { get; set; }
        public List<Tag> Tags { get; set; }
        public bool IsConsistent { get; set; }

        public GameAction()
        {
            Tags = new List<Tag>();
            Entity=new Entity();
            Target=new Entity();
        }
    }
}
