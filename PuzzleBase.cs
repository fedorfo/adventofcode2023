namespace adventofcode2023;

public abstract class PuzzleBase: IPuzzle
{
    public abstract void Solve();
    public int Day => int.Parse(GetType().Name.Replace("Day", ""));

    protected List<string> ReadLines()
    {
        var result = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (line is null)
                return result;
            result.Add(line);
        }
    }

    public virtual string InputFileName => $"input{Day}.txt";
}