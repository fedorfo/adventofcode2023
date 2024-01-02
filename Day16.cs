namespace adventofcode2023;

using helpers;

public class Day16 : PuzzleBase
{
    //public override string InputFileName => "sample.txt";

    private static readonly Dictionary<char, char> RotateFieldCellCcw = new()
    {
        { '.', '.' },
        { '-', '|' },
        { '|', '-' },
        { '/', '\\' },
        { '\\', '/' }
    };

    private static readonly Dictionary<char, List<V2>> NewDirections = new()
    {
        { '.', new List<V2> { new(0, 1) } },
        { '-', new List<V2> { new(0, 1) } },
        { '|', new List<V2> { new(1, 0), new(-1, 0) } },
        { '/', new List<V2> { new(-1, 0) } },
        { '\\', new List<V2> { new(1, 0) } }
    };

    public override void Solve()
    {
        var map = ReadLines().Select(x => x.ToList()).ToList();
        var result1 = CountEnergizedTiles(map, new V2(0, 0), new V2(0, 1));
        Console.WriteLine(result1);

        var candidates = new List<Tuple<V2, V2>>().Concat(
            Enumerable.Range(0, map[0].Count).Select(y => Tuple.Create(new V2(0, y), new V2(1, 0)))
        ).Concat(
            Enumerable.Range(0, map[0].Count).Select(y => Tuple.Create(new V2(map.Count - 1, y), new V2(-1, 0)))
        ).Concat(
            Enumerable.Range(0, map.Count).Select(x => Tuple.Create(new V2(x, 0), new V2(0, 1)))
        ).Concat(
            Enumerable.Range(0, map.Count).Select(x => Tuple.Create(new V2(x, map[0].Count-1), new V2(0, -1)))
        ).ToList();
        var result2 = candidates.Select(x => CountEnergizedTiles(map, x.Item1, x.Item2)).Max();
        Console.WriteLine(result2);
    }

    private static int CountEnergizedTiles(List<List<char>> map, V2 start, V2 direction)
    {
        var visited = new HashSet<Tuple<V2, V2>>();
        Bfs(start, direction, map, visited);
        return visited.Select(x=>x.Item1).Distinct().Count();
    }

    private static void Bfs(V2 start, V2 direction, List<List<char>> map, HashSet<Tuple<V2, V2>> visited)
    {
        visited.Add(Tuple.Create(start, direction));
        var queue = new Queue<Tuple<V2, V2>>();
        queue.Enqueue(Tuple.Create(start, direction));

        while (queue.Count > 0)
        {
            var (v, d) = queue.Dequeue();
            var cnt = 0;
            var fieldValue = map[v.X][v.Y];
            while (d != new V2(0, 1))
            {
                d = RotateCcw(d);
                fieldValue = RotateCcw(fieldValue);
                cnt++;
            }

            var newDirections = NewDirections[fieldValue];
            while (cnt < 4)
            {
                newDirections = newDirections.Select(RotateCcw).ToList();
                cnt++;
            }

            var nextVertices = newDirections
                .Select(x => Tuple.Create(v + x, x))
                .Where(x => x.Item1 >= new V2(0, 0) && x.Item1 < new V2(map.Count, map[0].Count))
                .ToList();

            foreach (var x in nextVertices)
            {
                if (!visited.Contains(x))
                {
                    queue.Enqueue(x);
                    visited.Add(x);
                }
            }
        }
    }

    private static V2 RotateCcw(V2 v) => new(v.Y, -v.X);
    private static char RotateCcw(char c) => RotateFieldCellCcw[c];
}
