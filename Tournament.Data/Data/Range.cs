namespace Tournament.Data.Data;

internal class Range<T>(T min, T max) where T : IComparable
{
    public bool Intersects(Range<T> other) =>
        Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;

    public T Min { get => min; }
    public T Max { get => max; }
}
