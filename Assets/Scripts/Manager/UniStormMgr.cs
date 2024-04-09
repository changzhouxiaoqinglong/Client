using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniStorm;
using System;

public class UniStormMgr
{
    /// <summary>
    ///修改天气。根据传入的天气序号切换天气。
    /// </summary>
    /// <param name="weatherCount"></param>

    private TrainDateMgr trainDateMgr = new TrainDateMgr();
    private DateTime time;
    public void ChangeWeather(int weatherCount)
    {
        Debug.Log("获取到的天气代码为:" + weatherCount + "  天气为：" + UniStormSystem.Instance.AllWeatherTypes[weatherCount]);
        if (weatherCount > UniStorm.UniStormSystem.Instance.AllWeatherTypes.Count)
        {
            Logger.LogError("传入的天气代码超出当前天气数组长度");
            return;
        }
        WeatherType weatherType = UniStormSystem.Instance.AllWeatherTypes[weatherCount];
        UniStormSystem.Instance.ChangeWeather(weatherType);

    }
    public void Update()
    {
        trainDateMgr.Update();
        time = TimeTool.TransTimeYYYYToDate(trainDateMgr.GetCurDateYYYYStr());

        ChangeDate(time.Year, time.Month, time.Day);
        ChangeTime(time.Hour, time.Minute);
    }
    /// <summary>
    /// 修改天气系统内的时间。
    /// </summary>
    /// <param name="hour">小时</param>
    /// <param name="minute">分钟</param>
    public void ChangeTime(int hour,int minute)
    {
        UniStormManager.Instance.SetTime(hour,minute);
    }

    /// <summary>
    /// 修改天气系统内的日期
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    public void ChangeDate(int year, int month, int day)
    {
        UniStormManager.Instance.SetDate(month, day, year);
    }
}
