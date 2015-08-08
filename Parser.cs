using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Program
{
	public static void Main()
	{
		var parser = new HSParser();
		parser.Parse(@"C:\prog\HSRec\output_log.txt");
	}
}

public class HSParser
{
	private string fileName;
	private string[] lines;

	public void Parse(string fileName)
	{
		this.fileName = fileName;
		this.Read();
		Console.WriteLine("Total lines :{0}", this.lines.Length);
		for(var i = 0; i < this.lines.Length; i++)
			this.ParseLine(lines[i]);
	}

	private void Read()
	{
		this.lines = File.ReadAllLines(fileName);
	}

	private void ParseLine(string line)
	{
		if(line.Contains("SubType=ATTACK"))
			this.Attack(line);
	}

	private void Attack(string line)
	{
		var entityRegex = new Regex(@"Entity=\[name=([\w\s\d_-]+)\sid=(\d+)\szone=(\w+)\szonePos=(\d)\scardId=([\w\d_]+)\splayer=(\d)");
		var targetRegex = new Regex(@"Target=\[name=([\w\s\d_-]+)\sid=(\d+)\szone=(\w+)\szonePos=(\d)\scardId=([\w\d_]+)\splayer=(\d)");

		var entityMatch = entityRegex.Match(line);
		var targetMatch = targetRegex.Match(line);

		Console.WriteLine("Attacker: {0}, Defender: {1}", entityMatch.Groups[1].Captures[0].Value, targetMatch.Groups[1].Captures[0].Value);
	}
}



















