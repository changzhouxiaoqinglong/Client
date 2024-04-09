
using UnityEngine;
/// <summary>
/// 气象数据管理
/// </summary>
public class MeteorDataMgr
{
    /// <summary>
    /// 风向浮动值
    /// </summary>
    private float WindDirOff = 5;

    /// <summary>
    /// 风速浮动值
    /// </summary>
    private float WindSpOff = 0.3f;

    /// <summary>
    /// 天气信息
    /// </summary>
    private Wearth weather;

    /// <summary>
    /// 计时
    /// </summary>
    private float sendMeteorTimer = 0;

    public MeteorDataMgr()
    {
        weather = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Wearth;
    }

    /// <summary>
    /// 间隔上报风向风速温湿度等信息
    /// </summary>
    private void OffSendMeteorData()
    {
        sendMeteorTimer += Time.deltaTime;
        if (sendMeteorTimer >= AppConstant.SEND_METEOR_OFF_TIME)
        {
            sendMeteorTimer = 0;
            SendMeteorData();
        }
    }

    public void Update()
    {
        //间隔上报气象数据
        OffSendMeteorData();
    }

    /// <summary>
    /// 上报气象数据
    /// </summary>
    private void SendMeteorData()
    {
        //风向浮动
        float windDir = weather.GetWindDir();
        windDir += WindDirOff * Random.Range(-1f, 1f);
        windDir = Mathf.Clamp(windDir, 0, 359);

        //风速浮动
        float windSp = weather.GetWindSp();
        windSp += WindSpOff * Random.Range(-1f, 1f);
        windSp = windSp < 0 ? 0 : windSp;

        MeteorEnvModel meteorEnvModel = new MeteorEnvModel()
        {
            Temperate = weather.Temperate,
            Humidity = weather.Humidity,
            WinDir = windDir,
            WinSp = windSp,
        };
        //发给设备
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(meteorEnvModel), NetProtocolCode.METEOR_ENV, NetManager.GetInstance().CurDeviceForward);
    }
}