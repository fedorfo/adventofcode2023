namespace adventofcode2023;

using helpers;

public class Day10 : PuzzleBase
{
    private static readonly Dictionary<char, List<V2>> Directions = new()
    {
        { '|', new List<V2> { new(-1, 0), new(1, 0) } },
        { 'L', new List<V2> { new(-1, 0), new(0, 1) } },
        { 'J', new List<V2> { new(-1, 0), new(0, -1) } },
        { '-', new List<V2> { new(0, -1), new(0, 1) } },
        { '7', new List<V2> { new(1, 0), new(0, -1) } },
        { 'F', new List<V2> { new(1, 0), new(0, 1) } },
        { '.', new List<V2>() }
    };

    private static readonly Dictionary<V2, Func<List<List<char>>, V2, bool>> IsMoveAllowed = new()
    {
        { new V2(-1, 0), (map, v) => !"FL-".Contains(map[v.X - 1][v.Y - 1]) },
        { new V2(1, 0), (map, v) => !"7J-".Contains(map[v.X][v.Y]) },
        { new V2(0, -1), (map, v) => !"F7|".Contains(map[v.X - 1][v.Y - 1]) },
        { new V2(0, 1), (map, v) => !"JL|".Contains(map[v.X][v.Y]) }
    };

    public override void Solve()
    {
        var map = ReadLines().Select(x => x.ToList()).ToList();
        var animal = map.Select((x, i) => new V2(i, x.IndexOf('S'))).Single(x => x.Y != -1);
        var neighbourPipes = animal.GetNeighbours4()
            .Where(x => Directions[map[x.X][x.Y]].Select(y => y + x).Contains(animal))
            .Select(x => x - animal)
            .ToArray();
        var animalChar = Directions.Keys.Single(x => neighbourPipes.All(y => Directions[x].Contains(y)));
        map[animal.X][animal.Y] = animalChar;
        var distances = new Dictionary<V2, int>();
        Bfs(animal, map, distances);
        var result1 = distances.Select(x => x.Value).Max();
        Console.WriteLine(result1);

        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[0].Count; j++)
            {
                if (!distances.ContainsKey(new V2(i, j)))
                {
                    map[i][j] = '.';
                }
            }
        }

        map = new[] { Enumerable.Range(0, map[0].Count + 2).Select(_ => '.').ToList() }
            .Concat(map.Select(x => new[] { '.' }.Concat(x).Concat(new[] { '.' }).ToList()))
            .Concat(new[] { Enumerable.Range(0, map[0].Count + 2).Select(_ => '.').ToList() })
            .ToList();

        var distances2 = new Dictionary<V2, int>();
        Bfs2(new V2(1, 1), map, distances2);
        var result2 = 0;
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[0].Count; j++)
            {
                if (
                    !distances2.ContainsKey(new V2(i, j)) &&
                    !distances2.ContainsKey(new V2(i + 1, j)) &&
                    !distances2.ContainsKey(new V2(i, j + 1)) &&
                    !distances2.ContainsKey(new V2(i + 1, j + 1)) &&
                    map[i][j] == '.'
                )
                {
                    result2++;
                }
            }
        }

        Console.WriteLine(result2);
    }

    private static void Bfs(V2 start, List<List<char>> map, Dictionary<V2, int> distances)
    {
        var queue = new Queue<V2>();
        distances[start] = 0;
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            foreach (var delta in Directions[map[v.X][v.Y]])
            {
                var u = v + delta;
                if (!distances.ContainsKey(u))
                {
                    queue.Enqueue(u);
                    distances[u] = distances[v] + 1;
                }
            }
        }
    }

    private static void Bfs2(V2 start, List<List<char>> map, Dictionary<V2, int> distances)
    {
        var queue = new Queue<V2>();
        distances[start] = 0;
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            foreach (var u in v.GetNeighbours4().Where(x => x > V2.Zero && x < new V2(map.Count, map[0].Count)))
            {
                var delta = u - v;
                if (IsMoveAllowed[delta](map, v))
                {
                    if (!distances.ContainsKey(u) || distances[u] > distances[v] + 1)
                    {
                        queue.Enqueue(u);
                        distances[u] = distances[v] + 1;
                    }
                }
            }
        }
    }
}
