
/// <summary>
/// 毒类型
/// </summary>
public class PoisonType
{
    /// <summary>
    /// 无毒
    /// </summary>
    public const int NO_POISON = 1;

    public const int VX_POISON = 5;
}

/// <summary>
/// 毒数据
/// </summary>
public class ExPoisonData : ExDataBase
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 类别
    /// </summary>
    public int DType;

    /// <summary>
    /// 地面检测图片路径
    /// </summary>
    public string GroundPath;

    /// <summary>
    /// 空气检测图片路径
    /// </summary>
    public string AirPath;

    /// <summary>
    /// 程度低
    /// </summary>
    public float DegreeLow;

    /// <summary>
    /// 程度高
    /// </summary>
    public float DegreeHigh;

    /// <summary>
    /// 根据检测类型获得图片
    /// </summary>
    public string GetPathByCheckType(int checkType)
    {
        switch (checkType)
        {
            case QstPoisonCheckType.GROUND_CHECK_TYPE:
                return GroundPath;
            case QstPoisonCheckType.AIR_CHECK_TYPE:
                return AirPath;
            default:
                Logger.LogError("Not Cantain CheckType");
                return string.Empty;
        }
    }

    /// <summary>
    /// 根据浓度 返回对应的程度
    /// </summary>
    public int GetdDegreeByDentity(float dentity)
    {
        //无毒
        if (Id == PoisonType.NO_POISON)
        {
            return DrugDegree.NONE;
        }

        if (dentity <= DegreeLow)
        {
            return DrugDegree.LOW;
        }
        else if (dentity <= DegreeHigh)
        {
            return DrugDegree.MIDDLE;
        }
        else
        {
            return DrugDegree.HIGH;
        }
    }
}
