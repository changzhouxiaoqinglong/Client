
using System.Collections.Generic;
using UnityEngine;

public interface IHarmArea
{
    /// <summary>
    /// 是否在范围内
    /// </summary>
    bool IsInRange(Vector3 pos);    
}

/// <summary>
/// 有害区域基类
/// </summary>
public class HarmAreaBase : UnityMono
{
    public TaskEnvVarData taskEnvVarData = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData;

    /// <summary>
    /// 扇形区域范围
    /// </summary>
    public List<Vector3> pointList = new List<Vector3>();

    /// <summary>
    /// 风向角
    /// </summary>
    protected float windDir = 0;

    /// <summary>
    /// 风速
    /// </summary>
    protected float windSp = 0;

    /// <summary>
    /// 是否在范围内
    /// </summary>
    public virtual bool IsInRange(Vector3 pos)
    {
        return false;
    }

    /// <summary>
    /// 获取目标点类型的图片路径
    /// </summary>

    public virtual string GetImageSpritePath()
    {
        return "";
    }


    public virtual float GetHarmRange()
    {
        return -1;
    }

    public virtual void SetPosition()
    {

    }
}

public class HarmAreaBaseConstant
{
    /// <summary>
    /// 移动比值
    /// </summary>
    public const float SPEED_RADIO = 0.1f;

    /// <summary>
    /// 大小比值
    /// </summary>
    public const float SIZE_RADIO = 1f;
}
