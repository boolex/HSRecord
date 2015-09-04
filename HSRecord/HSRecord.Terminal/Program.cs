namespace HSRecord.Terminal
{
	using System.IO;	
	using Newtonsoft.Json;
	
	using HSRecord.Core.Data;
	using HSRecord.Core.Parsing;
	
	class Program
	{
		static void Main(string[] args)
		{
           
            var filePath = @"C:\prog\HSRecord\data\Game32506171.txt";
			var parser = new HSParser();
		    var card = CardManager.Get("EX1_392");
			//parser.Parse(@"C:\Projects\HSRecord\HSRecord\data\Grim1.txt");
			var game = parser.ParseGame(filePath);

			var outputFilePath = @"C:\prog\HSRecord\game.js";
			File.WriteAllText (outputFilePath, string.Format("var game = {0};",  JsonConvert.SerializeObject(game)));
        }
    }
}
