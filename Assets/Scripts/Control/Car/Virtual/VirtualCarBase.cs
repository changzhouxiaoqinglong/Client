
using System.Collections.Generic;
/// <summary>
/// 虚拟车（因为只有驾驶员有3d场景，其他没有，所以没办法使用3d真实车对象和脚本，
/// 但是又需要相关对象管理， 所以这里加入新的虚拟车辆逻辑  来处理一些特殊的，每个席位都要处理的车相关逻辑）
/// </summary>
public class VirtualCarBase
{
    /// <summary>
    /// 虚拟设备
    /// </summary>
    protected List<VirtualDeviceBase> virtualDevices = new List<VirtualDeviceBase>();

    protected QstDrugPoisonLog qstDrugPoisonLog;


    public VirtualCarBase()
    {
        //初始化虚拟设备
        InitVirtualDevices();
    }

    protected virtual void InitVirtualDevices()
    {

    }

    /// <summary>
    /// 设置qstRequestLog数据
    /// </summary>
    /// <param name="tempLog"></param>
    public virtual void SetQstDrugPoisonLog(QstDrugPoisonLog tempLog)
    {
        qstDrugPoisonLog = tempLog;
    }

    public virtual QstDrugPoisonLog GetQstDrugPoisonLog()
    {
        return qstDrugPoisonLog;
    }

    public virtual void TubeCountAdd()
    {
        if(qstDrugPoisonLog != null)
            qstDrugPoisonLog.TubeCount++;
    }

    public virtual void SetQstDrugPoison(int poison)
    {
        if (qstDrugPoisonLog != null)
            qstDrugPoisonLog.UserSelectPoison = poison;
    }

    public T GetDevice<T>() where T : VirtualDeviceBase
    {
        foreach (var device in virtualDevices)
        {
            if (device is T)
            {
                return (T)device;
            }
        }
        return null;
    }

    public virtual void OnDestroy()
    {
        foreach (VirtualDeviceBase device in virtualDevices)
        {
            device.OnDestory();
        }
    }
}
