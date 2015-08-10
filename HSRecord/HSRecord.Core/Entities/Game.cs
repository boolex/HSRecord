using System.Linq;

namespace HSRecord.Core.Entities
{
    using System.Collections.Generic;

    public class Game
    {
        public List<Turn> Turns { get; set; }
        public object Context { get; set; }

        public Game()
        {
            Turns = new List<Turn>();
        }
        public Card Draw { get; set; }
        
    }
}
