
using System;
using UnityEngine;

/// <summary>
/// 训练时间管理
/// </summary>
public class TrainDateMgr
{
    /// <summary>
    /// 当前时间
    /// </summary>
    private DateTime curDate = DateTime.Now;

    //记录训练时间
    public float timer = 0;

    //是否开始记录，训练时间
    public bool IsStartTimer {
        get;set;
    }

    public TrainDateMgr()
    {
        //初始化为导控下发时间
        curDate = TimeTool.TransTimeYYYYToDate(NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Time);
    }

    public string GetCurDateStr()
    {
        return curDate.ToString();
    }

    /// <summary>
    /// yyyyMMddHHmmss格式时间
    /// </summary>
    public string GetCurDateYYYYStr()
    {
        return TimeTool.TransDateToYYYY(curDate);
    }

    public void Update()
    {
        curDate = curDate.AddSeconds(Time.deltaTime);
        if (IsStartTimer) {
            timer += Time.deltaTime;
        }
    }
}
