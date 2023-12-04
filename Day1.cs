namespace adventofcode2023;

public class Day1 : PuzzleBase
{
    private static readonly string[] Digits = {
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine"
    };

    public override void Solve()
    {
        var lines = ReadLines();
        var result = lines.Sum(x => FirstDigit(x) * 10 + FirstDigit(x.Reverse()));
        Console.WriteLine(result);

        result = lines.Sum(x =>
        {
            var indices = Enumerable.Range(0, x.Length).Where(idx => Digit(x, idx) is not null).ToArray();
            return Digit(x, indices.First())!.Value * 10 + Digit(x, indices.Last())!.Value;
        });
        Console.WriteLine(result);
    }

    private int FirstDigit(IEnumerable<char> line)
    {
        return line.Where(char.IsDigit).Select(x => x - '0').First();
    }

    private int? Digit(string line, int position)
    {
        if (char.IsDigit(line[position]))
            return line[position] - '0';
        for (var i = 1; i <= 9; i++)
            if (
                line.Length >= position + Digits[i - 1].Length &&
                line.Substring(position, Digits[i - 1].Length) == Digits[i - 1]
            )
                return i;
        return null;
    }
}