namespace HSRecord.Core.TagConverter
{
    using HSRecord.Core.Acts;
    using HSRecord.Core.Entities;
    public class ZoneTagConverter
    {
        public Act Convert(Tag tag)
        {
            var act = new PlayMinionCardAct(tag.Entity.Card);
            act.Card.Name = tag.Entity.Name;
            return act;
        }
    }
}
