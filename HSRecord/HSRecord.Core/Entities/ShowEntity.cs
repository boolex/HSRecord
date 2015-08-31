namespace HSRecord.Core.Entities
{
    using System.Collections.Generic;

    public class ShowEntity
    {
        public Entity Entity { get; set; }
        public List<Tag> Tags { get; set; }

        public ShowEntity()
        {
            Entity = new Entity();
            Tags=new List<Tag>();
        }
    }
}
