namespace HSRecord.Terminal
{
	using HSRecord.Core.Parsing;
	class Program
	{
		static void Main(string[] args)
		{
			var filePath = @"C:\prog\HSRecord\data\Grim2.txt";
			var parser = new HSParser();
			//parser.Parse(@"C:\Projects\HSRecord\HSRecord\data\Grim1.txt");
			var game = parser.ParseGame(filePath);

			foreach(var turn in game.Turns)
			{
				foreach(var act in turn.Acts)
				{
					System.Console.WriteLine(act.Type);
				}				
			}
			
        }
    }
}
