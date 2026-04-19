using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumExtensions
{
    public static IEnumerable<Enum> GetFlags(Enum input)
    {
        foreach (Enum value in Enum.GetValues(input.GetType()))
            if (input.HasFlag(value))
                yield return value;
    }

    /// <summary>
    /// Get all values in the enum as an array
    /// </summary>
    public static T[] GetValues<T>()
    {
        return (T[])Enum.GetValues(typeof(T));
    }

    /// <summary>
    /// Check if any of the member of the source matches any of other.
    /// </summary>
    public static bool Any<T>(this T source, T other) where T : Enum
    {
        return (source.GetHashCode() & other.GetHashCode()) > 0;
    }

    /// <summary>
    /// Check if the flags in other are all set in source.
    /// </summary>
    public static bool Contains<T>(this T source, T other) where T : Enum
    {
        if ((source.GetHashCode() & other.GetHashCode()) != other.GetHashCode())
            return false;
        //If there are less flags in the source, it cannot contain the other
        return source.CountFlags() >= other.CountFlags();
    }

    /// <summary>
    /// Warning : 50 times slower than manual implemetation (source | other).
    /// </summary>
    public static T AddFlags<T>(this T source, T other) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), (source.GetHashCode() | other.GetHashCode()));
    }

    /// <summary>
    /// Warning : 50 times slower than manual implemetation (source & ~other).
    /// </summary>
    public static T RemoveFlags<T>(this T source, T other) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), (source.GetHashCode() & ~other.GetHashCode()));
    }

    /// <summary>
    /// Count how many flags are on.
    /// </summary>
    public static int CountFlags<T>(this T source) where T : Enum
    {
        int iCount = 0;
        int value = source.GetHashCode();

        //Loop the value while there are still bits
        while (value != 0)
        {
            //Remove the end bit
            value = value & (value - 1);

            //Increment the count
            iCount++;
        }

        //Return the count
        return iCount;
    }

    /// <summary>
    /// Gives one of the matching flags if any
    /// UNTESTED
    /// </summary>
    public static bool TryGetOneMatchingFlag<T>(this T source, T other, out T match) where T : Enum
    {
        int intersect = source.GetHashCode() & other.GetHashCode();
        match = source;
        if (intersect == 0)
            return false;

        //n & (n - 1) is a technique to remove the rightmost set bit.
        //So, n - (n & n - 1) will return a number with only the rightmost set bit.
        int rightMostBit = intersect - (intersect & intersect - 1);
        match = (T)Enum.ToObject(typeof(T), rightMostBit);

        return true;
    }
}
