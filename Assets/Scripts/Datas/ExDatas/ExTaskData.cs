
using System.Collections.Generic;
/// <summary>
/// 训练任务类型
/// </summary>
public enum TaskType
{
    /// <summary>
    /// 基本训练
    /// </summary>
    Base = 1,

    /// <summary>
    /// 车组协同
    /// </summary>
    Coord,

    /// <summary>
    /// 战术训练
    /// </summary>
    Tactic,
}

public class ExTaskId
{
    #region 02B
    /// <summary>
    /// 02B毒剂报警器 基本操作训练
    /// </summary>
    public const int BASE_POISON_ALARM_02B = 2011;

    /// <summary>
    /// 02B辐射仪 基本操作训练
    /// </summary>
    public const int BASE_RADIOMETE_02B = 2013;

    /// <summary>
    /// 02B车载侦毒器 基本操作训练
    /// </summary>
    public const int BASE_CAR_POISON_DETECT_02B = 2012;

    public const int BASE_CAR_Power_02B = 2014;

    public const int BASE_CAR_RadioStation_02B = 2016;

    #endregion

    #region 384
    /// <summary>
    /// 384辐射仪 基本操作训练
    /// </summary>
    public const int BASE_RADIOMETE_384 = 38411;

    /// <summary>
    /// 384毒剂报警器 基本操作训练
    /// </summary>
    public const int BASE_POISON_384 = 38413;


    /// <summary>
    /// 384电源
    /// </summary>
    public const int BASE_Power_384 = 38414;

    /// <summary>
    /// 384电台
    /// </summary>
    public const int BASE_RadioStation_384 = 38416;

    #endregion

    #region 102
    /// <summary>
    /// 102辐射仪 基本操作训练
    /// </summary>
    public const int BASE_RADIOMETE_102 = 10212;

    /// <summary>
    /// 102电源
    /// </summary>
    public const int BASE_Power_102 = 10218;

    /// <summary>
    /// 102电台
    /// </summary>
    public const int BASE_RadioStation_102 = 102110;

    /// <summary>
    /// 102三防
    /// </summary>
    public const int BASE_PREVENT_102 = 10211;

    /// <summary>
    /// 102毒剂报警器 基本操作训练
    /// </summary>
    public const int BASE_POISON_102 = 10214;

    /// <summary>
    /// 102质谱仪 基本操作训练
    /// </summary>
    public const int BASE_MESS_SPECT_102 = 10215;

    /// <summary>
    /// 102红外遥测 基本操作训练
    /// </summary>
    public const int BASE_INFARE_102 = 10216;
    #endregion

    #region 106
    /// <summary>
    /// 106毒剂报警器 基本操作训练
    /// </summary>
    public const int BASE_POISON_ALARM_106 = 10601;

    public const int BASE_RADIOMETE_106 = 10602;

    public const int BASE_Biology_106 = 10603;

    /// <summary>
    /// 106电源
    /// </summary>
    public const int BASE_Power_106 = 10607;

    /// <summary>
    /// 106电台
    /// </summary>
    public const int BASE_RadioStation_106 = 10608;

    #endregion
}

public class CheckTypeConst
{
    /// <summary>
    /// 单机
    /// </summary>
    public const int SINGLE =  0;

    /// <summary>
    /// 考核
    /// </summary>
    public const int EXAMINE = 1;

    /// <summary>
    /// 训练
    /// </summary>
    public const int PRACTICE = 2;
}

/// <summary>
/// 任务表数据
/// </summary>
public class ExTaskData : ExDataBase
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType Type;

    /// <summary>
    /// 检测类型
    /// </summary>
    public int CheckType;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc;

    /// <summary>
    /// 车长指令
    /// </summary>
    public string Instructs;

    /// <summary>
    /// 获得指令数据
    /// </summary>
    public List<ExInstructData> GetInstructList()
    {
        List<ExInstructData> list = new List<ExInstructData>();
        string[] instructIds = Instructs.Split(',');
        foreach (string instructId in instructIds)
        {
            ExInstructData instruct = ExInstructDataMgr.GetInstance().GetDataById(instructId.ToInt());
            list.Add(instruct);
        }
        return list;
    }

    /// <summary>
    /// 是否要进基础操作场景训练，基本训练或者车组协同训练才进入基础操作场景训练
    /// </summary>
    /// <returns></returns>
    public bool IsNeedInBaseTranScene()
    {
        return Type == TaskType.Base || Type == TaskType.Coord;
    }
}
