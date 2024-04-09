
using System.Diagnostics;

public class Logger
{
    public static void Log(string msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    [Conditional("UNITY_EDITOR")]
    [Conditional("DEBUG")]
    public static void LogDebug(string msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    public static void LogWarning(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }

    public static void LogError(string msg)
    {
        UnityEngine.Debug.LogError(msg);
    }
}
