

using UnityEngine;
/// <summary>
/// 辐射仪
/// </summary>
public class Radiometer : DeviceBase
{
    /// <summary>
    /// 计时器
    /// </summary>
    private float checkTimer = 0;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //核辐射训练才起作用
        if (TaskMgr.GetInstance().curTaskData.CheckType != HarmAreaType.NUCLEAR)
        {
            return;
        }
        RadiomCount();
    }

    /// <summary>
    /// 计算辐射剂量率
    /// </summary>
    private void RadiomCount()
    {
        if (car.IsSelfCar())
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= AppConstant.RADIOM_CHECK_OFFTIME)
            {
                checkTimer = 0;
                //剂量率
                float radiomRate = HarmAreaMgr.GetPosRadiomRate(car.GetPosition());
                //发送剂量率
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.SEND_RADIOM_RATE, new FloatEvParam(radiomRate));
            }
        }
    }
}
