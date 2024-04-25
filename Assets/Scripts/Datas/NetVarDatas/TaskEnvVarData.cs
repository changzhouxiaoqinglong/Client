using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下发的任务环境信息
/// </summary>
public class TaskEnvVarData
{
    /// <summary>
    /// 任务编号
    /// </summary>
    public int TaskId;

    public string TrainUid;

    /// <summary>
    /// 模式 0.单机  1.考核  2.训练
    /// </summary>
    public int CheckType;

    /// <summary>
    /// 训练类型1.操作2.协同3.战术
    /// </summary>
    public int TaskType;

    /// <summary>
    /// 任务描述
    /// </summary>
    public string TaskDesc;

    /// <summary>
    /// 场景
    /// </summary>
    public int Scene;

    /// <summary>
    /// 车型编号
    /// </summary>
    public int CarId;

    /// <summary>
    /// 有害元素信息
    /// </summary>
    public List<HarmData> HarmDatas = new List<HarmData>();

    /// <summary>
    /// 弹坑数据
    /// </summary>
    public List<CraterVarData> CraterDatas = new List<CraterVarData>();

    /// <summary>
    /// 天气
    /// </summary>
    public Wearth Wearth;

    /// <summary>
    /// 时间
    /// </summary>
    public string Time;

    /// <summary>
    /// 计划导调
    /// </summary>
    public List<PlanGuideData> PlanGuides = new List<PlanGuideData>();

    private ExTaskData exTaskData;

    public ExTaskData ExTaskData
    {
        get
        {
            if (exTaskData == null)
            {
                exTaskData = TaskExDataMgr.GetInstance().GetDataById(TaskId);
            }
            return exTaskData;
        }
    }
}

/// <summary>
/// 有害区域的类型
/// </summary>
public class HarmAreaType
{
    /// <summary>
    /// 化学 毒
    /// </summary>
    public const int DRUG = 1;

    /// <summary>
    /// 核
    /// </summary>
    public const int NUCLEAR = 2;

    /// <summary>
    /// 生物
    /// </summary>
    public const int BIOLOGY = 3;

    public static string GetTypeStr(int type)
    {
        switch (type)
        {
            case DRUG:
                return "化学";
            case NUCLEAR:
                return "核";
            case BIOLOGY:
                return "生物";
            default:
                return "";
        }
    }
}

/// <summary>
/// 有害区域信息
/// </summary>
public class HarmData
{

    /// <summary>
    /// 类型
    /// </summary>
    public int HarmType;

    /// <summary>
    /// 具体内容
    /// </summary>
    public string Content;
}

/// <summary>
/// 下发化学毒区域数据
/// </summary>
public class DrugVarData
{
    public int Id;

    /// <summary>
    /// 毒剂类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 范围
    /// </summary>
    public int Range;

    /// <summary>
    /// 浓度
    /// </summary>
    public float Dentity;

    /// <summary>
    /// 袭击方式
    /// </summary>
    public int AttackType;

    /// <summary>
    /// 释放速度
    /// </summary>
    public int Speed;
}

/// <summary>
/// 下发辐射区域数据
/// </summary>
public class RadiatVarData
{
    public int Id;

    /// <summary>
    /// 位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 范围
    /// </summary>
    public int Range;

    /// <summary>
    /// 剂量率
    /// </summary>
    public int DoseRate;

    /// <summary>
    /// 比高
    /// </summary>
    public float BiGao;

    /// <summary>
    /// 当量
    /// </summary>
    public int DangLiang;
}

/// <summary>
/// 下发弹坑数据
/// </summary>
public class CraterVarData
{
    public int Id;

    /// <summary>
    /// 位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 毒剂类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 浓度
    /// </summary>
    public float Dentity;

    /// <summary>
    /// 角度
    /// </summary>
    public string Rotate;

    public Quaternion GetRotation()
    {
        if (Rotate.IsNullOrEmpty())
        {
            return Quaternion.identity;
        }
        else
        {
            string[] rotateStrs = Rotate.Split(',');
            return Quaternion.Euler(rotateStrs[0].ToFloat(), rotateStrs[1].ToFloat(), rotateStrs[2].ToFloat());
        }
    }
}


/// <summary>
/// 天气数据
/// </summary>
public class Wearth
{
    /// <summary>
    /// 天气类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 风向
    /// </summary>
    public int WindDir;

    public float GetWindDir()
    {
        return WindDir * 45;
    }

    /// <summary>
    /// 风速
    /// </summary>
    public int WindSp;

    public float GetWindSp()
    {
        switch (WindSp)
        {
            //无
            case 0:
                return 0;
            //低
            case 1:
                return 3.5f;
            //中
            case 2:
                return 7.2f;
            //高
            case 3:
                return 10.1f;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 温度
    /// </summary>
    public float Temperate;

    /// <summary>
    /// 湿度
    /// </summary>
    public float Humidity;

    public string GetDes()
	{
        switch (Type)
        {
           
            case 0:
                return "阴";
           
            case 1:
                return "晴";
           
            case 3:
                return "雨";

            case 5:
                return "雪";
            case 9:
                return "雾";
            case 11:
                return "沙尘暴";
            default:
                return "天气信息异常";

        }
    }
}

/// <summary>
/// 计划导调
/// </summary>
public class PlanGuideData {
    //事件开始时间
    public float Time;
    //事件类型
    public int Type;
}