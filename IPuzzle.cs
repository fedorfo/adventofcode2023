namespace adventofcode2023;

public interface IPuzzle
{
    int Day { get; }
    string InputFileName { get; }
    void Solve();
}
