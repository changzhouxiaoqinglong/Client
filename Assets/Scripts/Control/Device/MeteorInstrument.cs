
using UnityEngine;

/// <summary>
/// 气象仪
/// </summary>
public class MeteorInstrument : DeviceBase
{
    /// <summary>
    /// 气象数据管理
    /// </summary>
    public MeteorDataMgr meteorDataMgr = new MeteorDataMgr();

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!car.IsSelfCar())
        {
            return;
        }
        meteorDataMgr.Update();
    }
}
