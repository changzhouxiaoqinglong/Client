
using System;
using UnityEngine;

public class TimeTool
{

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void UnPause()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// 时间戳起始时间
    /// </summary>
    private static DateTime DateStampBegin = new DateTime(1970, 1, 1);

    /// <summary>
    /// 时间戳转日期
    /// </summary>
    /// <returns></returns>
    public static DateTime TransTimeStampToDate(long timeStamp)
    {
        TimeSpan ts = TimeSpan.FromMilliseconds(timeStamp);
        return DateStampBegin + ts;
    }

    /// <summary>
    /// yyyyMMddHHmmss时间转日期
    /// </summary>
    public static DateTime TransTimeYYYYToDate(string time)
    {
     //   Debug.Log(time);
        DateTime res;
        if (!DateTime.TryParseExact(time, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out res))
        {
            Logger.LogError("TransTimeError: " + time);
        }
        return res;
    }

    /// <summary>
    /// 日期转成 yyyyMMddHHmmss格式
    /// </summary>
    public static string TransDateToYYYY(DateTime date)
    {
        return date.ToString("yyyyMMddHHmmss");
    }
}
