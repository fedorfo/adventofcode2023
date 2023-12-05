namespace adventofcode2023;

using System.Globalization;

public abstract class PuzzleBase : IPuzzle
{
    public abstract void Solve();
    public int Day => int.Parse(this.GetType().Name.Replace("Day", ""), CultureInfo.InvariantCulture);

    public virtual string InputFileName => $"input{this.Day}.txt";

    protected static List<string> ReadLines()
    {
        var result = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (line is null)
            {
                return result;
            }

            result.Add(line);
        }
    }
}
