
using UnityEngine;

/// <summary>
/// 态势同步车位置
/// </summary>
public class SituationSyncCarPos
{
    /// <summary>
    /// 车
    /// </summary>
    private Transform carTrans;

    private Train3DSceneCtrBase curScene;

    /// <summary>
    /// 记录上次发送真实时间
    /// </summary>
    private float lastSendRealTime = 0;

    public SituationSyncCarPos(Transform carTrans, Train3DSceneCtrBase curScene)
    {
        this.carTrans = carTrans;
        this.curScene = curScene;
        lastSendRealTime = Time.realtimeSinceStartup;
    }

    public void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastSendRealTime;
        if (deltaTime >= NetConfig.SITUATION_SYNC_POS_OFF_TIME)
        {
            SendCarPosSituation();
            lastSendRealTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// 上报位置态势
    /// </summary>
    private void SendCarPosSituation()
    {
        //经纬度
        Vector3 lation = curScene.terrainChangeMgr.gisPointMgr.GetGisPos(carTrans.position);
        SituationSyncLogic.SyncCarPos(lation.ToCustVect3());
    }
}
