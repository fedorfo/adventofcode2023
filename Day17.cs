namespace adventofcode2023;

using helpers;

public class Day17 : PuzzleBase
{
    public override void Solve()
    {
        var map = ReadLines().Select(x => x.Select(y => y - '0').ToList()).ToList();
        var graph1 = new Graph1(map);
        var (distance1, _) = GraphAlgo.Dijikstra(graph1, graph1.GetVertex(V2.Zero, new V2(0, 1), 0));
        var result1 = graph1
            .GetVertices()
            .Where(x => x.Position == new V2(map.Count - 1, map[0].Count - 1))
            .Select(x => distance1.GetValueOrDefault(x, long.MaxValue))
            .Min();

        Console.WriteLine(result1);

        var graph2 = new Graph2(map);
        var (distance2, _) = GraphAlgo.Dijikstra(
            graph2,
            graph2.GetVertex(V2.Zero, new V2(0, 1), 0),
            graph2.GetVertex(V2.Zero, new V2(1, 0), 0)
        );
        var result2 = graph2
            .GetVertices()
            .Where(x => x.Position == new V2(map.Count - 1, map[0].Count - 1) && x.Distance >= 4)
            .Select(x => distance2.GetValueOrDefault(x, long.MaxValue))
            .Min();

        Console.WriteLine(result2);
    }

    private sealed class Vertex
    {
        public Vertex(V2 position, V2 direction, int distance)
        {
            this.Position = position;
            this.Direction = direction;
            this.Distance = distance;
        }

        public V2 Position { get; }
        public V2 Direction { get; }
        public int Distance { get; }

        public override string ToString() => $"{this.Position}, {this.Direction}({this.Distance})";
    }

    private sealed class Graph1 : IGraph<Vertex>
    {
        private readonly List<List<int>> map;
        private readonly Dictionary<(V2 Postion, V2 Direction, int Distance), Vertex> vertices;

        public Graph1(List<List<int>> map)
        {
            this.map = map;
            this.vertices = new Dictionary<(V2 Postion, V2 Direction, int), Vertex>();
            foreach (var position in V2.EnumerateRange(V2.Zero, new V2(map.Count, map[0].Count)))
            {
                foreach (var direction in V2.Zero.GetNeighbours4())
                {
                    for (var distance = 0; distance <= 3; distance++)
                    {
                        this.vertices[(position, direction, distance)] = new Vertex(position, direction, distance);
                    }
                }
            }
        }

        public IEnumerable<Vertex> GetVertices() => this.vertices.Values;

        public IEnumerable<(Vertex Vertex, long Distance)> GetEdges(Vertex v)
        {
            foreach (var direction in V2.Zero.GetNeighbours4())
            {
                if (direction + v.Direction == V2.Zero)
                {
                    continue;
                }

                var position = v.Position + direction;
                var distance = v.Direction == direction ? v.Distance + 1 : 1;
                if (this.vertices.TryGetValue((position, direction, distance), out var u))
                {
                    yield return (u, this.map[u.Position.X][u.Position.Y]);
                }
            }
        }

        internal Vertex GetVertex(V2 position, V2 direction, int distance) =>
            this.vertices[(position, direction, distance)];
    }

    private sealed class Graph2 : IGraph<Vertex>
    {
        private readonly List<List<int>> map;
        private readonly Dictionary<(V2 Postion, V2 Direction, int Distance), Vertex> vertices;

        public Graph2(List<List<int>> map)
        {
            this.map = map;
            this.vertices = new Dictionary<(V2 Postion, V2 Direction, int), Vertex>();
            foreach (var position in V2.EnumerateRange(V2.Zero, new V2(map.Count, map[0].Count)))
            {
                foreach (var direction in V2.Zero.GetNeighbours4())
                {
                    for (var distance = 0; distance <= 10; distance++)
                    {
                        this.vertices[(position, direction, distance)] = new Vertex(position, direction, distance);
                    }
                }
            }
        }

        public IEnumerable<(Vertex Vertex, long Distance)> GetEdges(Vertex v)
        {
            foreach (var direction in V2.Zero.GetNeighbours4())
            {
                if (direction + v.Direction == V2.Zero)
                {
                    continue;
                }

                if (v.Distance < 4 && direction != v.Direction)
                {
                    continue;
                }

                var position = v.Position + direction;
                var distance = v.Direction == direction ? v.Distance + 1 : 1;
                if (this.vertices.TryGetValue((position, direction, distance), out var u))
                {
                    yield return (u, this.map[u.Position.X][u.Position.Y]);
                }
            }
        }

        public IEnumerable<Vertex> GetVertices() => this.vertices.Values;

        internal Vertex GetVertex(V2 position, V2 direction, int distance) =>
            this.vertices[(position, direction, distance)];
    }
}
