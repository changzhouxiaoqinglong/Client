
using System;
using System.Linq;
/// <summary>
/// 席位类型
/// </summary>
public class SeatType
{
    /// <summary>
    /// 驾驶位
    /// </summary>
    public const int DRIVE = 1;

    /// <summary>
    /// 车长
    /// </summary>
    public const int MASTER = 2;

    /// <summary>
    /// 侦查员1
    /// </summary>
    public const int INVEST1 = 3;

    /// <summary>
    /// 侦查员2
    /// </summary>
    public const int INVEST2 = 4;

    /// <summary>
    /// 设备管理软件
    /// </summary>
    public const int DEVICE = 5;
}

/// <summary>
/// id
/// </summary>
public class CarIdConstant
{
    public const int ID_02B = 1;

    public const int ID_102 = 2;

    public const int ID_384C = 3;
}

/// <summary>
/// 设备类型
/// </summary>
public class DeviceType
{
    /// <summary>
    /// 车载毒剂报警器
    /// </summary>
    public const int CAR_POISON_ALARM = 1;

    /// <summary>
    /// 车载辐射仪
    /// </summary>
    public const int CAR_RADIOMETER = 2;

    /// <summary>
    /// 车载侦毒器
    /// </summary>
    public const int CAR_POISON_CHECK = 3;

    /// <summary>
    /// 便携式辐射仪
    /// </summary>
    public const int EASY_RADIOMETER = 4;
}

public class ExCarData : ExDataBase
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 资源路径
    /// </summary>
    public string Res;

    /// <summary>
    /// 设备
    /// </summary>
    public string Devices = "";

    private int[] deviceTypes;

    /// <summary>
    /// 获得当前车有的设备类型
    /// </summary>
    private int[] GetDeviceTypes()
    {
        if (deviceTypes == null || deviceTypes.Length <= 0)
        {
            string[] typeStrs = Devices.Split(',');
            if (typeStrs.Length > 0)
            {
                deviceTypes = typeStrs.Select(s => s.ToInt()).ToArray();
            }
        }
        return deviceTypes;
    }

    /// <summary>
    /// 是否有对应的设备
    /// </summary>
    public bool IsHaveDevice(int deviceType)
    {
        return GetDeviceTypes().Contains(deviceType);
    }
}