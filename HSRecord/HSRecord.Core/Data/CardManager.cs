using System;
using System.Reflection;

namespace HSRecord.Core.Data
{
    using System.Collections.Generic;
    using Entities;
    using System.IO;
    using Newtonsoft.Json.Linq;

    public static class CardManager
    {
        private static Dictionary<string, Card> Cards;
        static CardManager()
        {
            Cards = new Dictionary<string, Card>();
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            var stream = new StreamReader(assembly.GetManifestResourceStream("HSRecord.Core.Data.AllSets.json"));

            var cardsFileContent = stream.ReadToEnd();
            stream.Close();
            var cardsRaw = JObject.Parse(cardsFileContent);
            foreach (dynamic item in cardsRaw)
            {
                var cardArray = item.Value;
                foreach (dynamic card in cardArray)
                {
                    var c = new HSRecord.Core.Entities.Card()
                    {
                        Id = card.id.Value,
                        CardGlobalId = card.id.Value,
                        Name = card.name.Value,
                        Text = (card.text == null) ? string.Empty : card.text.Value
                    };
                    Cards.Add(card.id.Value, c);
                }
            }
        }
        private static void LoadCards()
        {
            Cards = new Dictionary<string, Card>();
            var assembly = Assembly.GetExecutingAssembly();
            var stream = new StreamReader(assembly.GetManifestResourceStream("AllSets.json"));

            var cardsFileContent = stream.ReadToEnd();
            stream.Close();
            var cardsRaw = JArray.Parse(cardsFileContent);
        }
        public static Card Get(string code)
        {
            if (Cards.ContainsKey(code))
                return Cards[code];

            return null;
        }
    }
}
