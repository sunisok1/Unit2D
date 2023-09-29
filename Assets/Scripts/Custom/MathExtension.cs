using System;

public static class MathExtension
{
    public static bool Between<T>(this T i, T start, T end) where T : IComparable<T>
    {
        return i.CompareTo(start) >= 0 && i.CompareTo(end) <= 0;
    }
    public static bool Between<T>(this T i, (T, T) range) where T : IComparable<T>
    {
        return i.CompareTo(range.Item1) >= 0 && i.CompareTo(range.Item2) <= 0;
    }
}