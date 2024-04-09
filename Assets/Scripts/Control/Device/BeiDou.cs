
using UnityEngine;

public class BeiDou : DeviceBase
{
    /// <summary>
    /// 计时
    /// </summary>
    private float sendMeteorTimer = 0;

    /// <summary>
    /// 间隔上报经纬度高程时间
    /// </summary>
    private void OffSendData()
    {
        sendMeteorTimer += Time.deltaTime;
        if (sendMeteorTimer >= AppConstant.BEIDOU_SEND_OFFTIME)
        {
            sendMeteorTimer = 0;
            SendData();
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!car.IsSelfCar())
        {
            return;
        }
        //间隔时间 上报数据
        OffSendData();
    }

    /// <summary>
    /// 上报数据
    /// </summary>
    private void SendData()
    {
        if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
        {
            //经纬度
            Vector3 lation = scene3D.terrainChangeMgr.gisPointMgr.GetGisPos(transform.position);
            //高程
            float elevat = scene3D.terrainChangeMgr.GetEvelationByPos(transform.position);
            BeiDouModel model = new BeiDouModel()
            {
                Longicude = lation.x,
                Latitude = lation.y,
                Elevation = elevat,
                Date = TaskMgr.GetInstance().curTaskCtr.trainDateMgr.GetCurDateStr(),
            };
            //发给设备
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.BEIDOU_DATA, NetManager.GetInstance().CurDeviceForward);
        }
    }
}
