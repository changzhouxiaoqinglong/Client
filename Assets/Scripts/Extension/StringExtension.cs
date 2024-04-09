
using System.Text;

public static class StringExtension
{
    public static StringBuilder Append(this string self, string appendStr)
    {
        return new StringBuilder(self).Append(appendStr);
    }

    public static StringBuilder AppendLine(this string self, string appendStr)
    {
        return new StringBuilder(self).AppendLine(appendStr);
    }

    public static bool IsNullOrEmpty(this string self)
    {
        return self == null || self == string.Empty;
    }

    public static int ToInt(this string selfStr, int defaultValue = 0)
    {
        int resValue;
        return int.TryParse(selfStr, out resValue) ? resValue : defaultValue;
    }

    public static float ToFloat(this string selfStr, float defaultValue = 0)
    {
        float resValue;
        return float.TryParse(selfStr, out resValue) ? resValue : defaultValue;
    }

    public static long ToLong(this string selfStr, long defaultValue = 0)
    {
        long resValue;
        return long.TryParse(selfStr, out resValue) ? resValue : defaultValue;
    }
}
