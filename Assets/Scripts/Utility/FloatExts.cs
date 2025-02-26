using UnityEngine;

public static class FloatExts
{
    public static bool IsZero(this float value, float tolerance = 0.0001f)
    {
        return value < tolerance && value > -tolerance;
    }

    public static bool IsGreaterThanZero(this float value, float tolerance = 0.0001f)
    {
        return value > tolerance;
    }

    public static bool IsLessThanZero(this float value, float tolerance = 0.0001f)
    {
        return value < -tolerance;
    }

    public static bool IsEqual(this float value, float other, float tolerance = 0.0001f)
    {
        return IsZero(value - other, tolerance);
    }
}