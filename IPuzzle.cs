namespace adventofcode2023;

public interface IPuzzle
{
    void Solve();
    int Day { get; }
    string InputFileName { get; }
}