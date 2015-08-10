namespace HSRecord.Terminal
{
    using HSRecord.Core.Parsing;
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"C:\Projects\HSRecord\HSRecord\data\Grim2.txt";
           var parser = new HSParser();
            //parser.Parse(@"C:\Projects\HSRecord\HSRecord\data\Grim1.txt");
            var game = parser.ParseGame(filePath);
            //var actions = parser.ParseActionsJson();

        }
    }
}
