
using UnityEngine;

/// <summary>
/// 发送车俩同步消息
/// </summary>
public class SendCarSyncMsg : MonoBehaviour
{
    /// <summary>
    /// 上次上报数据时间
    /// </summary>
    private float lastTime;

    /// <summary>
    /// 上报间隔时间
    /// </summary>
    private float syncOffTime = 0;

    private CarBase curCar;

    private void Awake()
    {
        lastTime = Time.time;
        syncOffTime = 1.0f / NetConfig.SYNC_SECOND_TIMES;
        curCar = GetComponent<CarBase>();
    }

    private void FixedUpdate()
    {
        CountSync();
    }

    /// <summary>
    /// 计时同步
    /// </summary>
    private void CountSync()
    {
        //距离上次上报的间隔
        float offTime = Time.time - lastTime;
        if (offTime >= syncOffTime)
        {
            //刷新上报时间
            lastTime = Time.time;
            //上报状态
            CarSyncModel model = new CarSyncModel()
            {
                Pos = transform.position.ToCustVect3(),
                Rotate = transform.eulerAngles.ToCustVect3(),
                PlayerSyncDatas = curCar.playerMgr.GetPlayerSyncModels(),
            };
            //发给同训练其他的驾驶位
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.CAR_SYNC, NetManager.GetInstance().SameTrainDriveSeatsExDevice);
        }
    }
}
