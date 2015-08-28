namespace HSRecord.Terminal
{
	using System.IO;
	using HSRecord.Core.Parsing;
	using Newtonsoft.Json;
	
	class Program
	{
		static void Main(string[] args)
		{
			var filePath = @"C:\prog\HSRecord\data\Grim2.txt";
			var parser = new HSParser();
			//parser.Parse(@"C:\Projects\HSRecord\HSRecord\data\Grim1.txt");
			var game = parser.ParseGame(filePath);

			var outputFilePath = @"C:\prog\HSRecord\game.js";
			File.WriteAllText (outputFilePath, string.Format("var game = {0};",  JsonConvert.SerializeObject(game)));
        }
    }
}
