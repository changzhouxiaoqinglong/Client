using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.InputSystem;
/// <summary>
/// 红外遥测102
/// </summary>
public class InfaredTelemetry102 : DeviceBase
{
    #region 属性
    //遥测父物体
    public GameObject telemetryMain;
    //遥测本体
    private Transform telemerty;
    //遥测盖板
    private Transform coverPlate;
    //连接杆
    private Transform shank;
    //方向角物体
    private Transform fxObj;
    //俯仰角物体
    private Transform fyObj;
    //射线模拟物体
    private GameObject rayObj;

    //是否上升完成 30为正在上升，31位上升到顶 0为无状态
    private int isUpComplete = 0;
    //是否下降完成 20位正在下降，21位下降到底 0位无状态
    private int isDownComplete = 21;
    //(弃用)是否侦测到毒
    //(弃用)private bool isCheckDrug = false;
    //遥测上升和下降时间
    private const float Speed = 77f;
    //计时
    private float sendMeteorTimer = 0;

    //是否已经上传了侦察结果，不考虑有多个毒剂云团的情况
    bool isSendDetctRes = false;

    public GameObject FM_SendYaoCeScreen;//102遥测窗口画面

    #endregion
    #region 方法
    protected override void Awake()
    {
        telemerty = telemetryMain.transform.Find("Telemetry");
        coverPlate = telemetryMain.transform.Find("CoverPlate");
        shank = coverPlate.transform.Find("Shank");
        fxObj = telemerty.transform.Find("Top");
        fyObj = fxObj.transform.Find("FyObj");

        rayObj = fyObj.transform.Find("Cube").gameObject;
        rayObj.SetActive(false);
        rayObj.GetComponent<MeshRenderer>().material.color = Color.green;
        base.Awake();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_102, GetInfared_Telemerty);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_PARAM_102, Infared_Telemetry_PARAM);
    }
    [ContextMenu("StartUp")]
    /// <summary>
    /// 控制遥控上升
    /// </summary>
    public void StartUP()
    {
        isDownComplete = 0;
        isUpComplete = 30;
        coverPlate.DOLocalRotate(new Vector3(57.2f, 0f, 0f), Speed);
        shank.DOLocalRotate(new Vector3(23f, 0f, 0f), Speed);
        telemerty.DOLocalMove(new Vector3(-0.117f, 0.75f, -0.255f), Speed).OnComplete(() =>
        {
            isUpComplete = 31;
            Debug.Log("上升完成");
            FM_SendYaoCeScreen.gameObject.SetActive(true);
        });

    }
    [ContextMenu("StartDown")]
    /// <summary>
    /// 控制遥控下降
    /// </summary>
    public void StartDown()
    {
        FM_SendYaoCeScreen.gameObject.SetActive(false);
        isUpComplete = 0;
        isDownComplete = 20;
        coverPlate.DOLocalRotate(new Vector3(-180f, 0, -0), Speed);
        shank.DOLocalRotate(new Vector3(-58.33f, 0, -0), Speed);
        rayObj.SetActive(false);
        telemerty.DOLocalMove(new Vector3(-0.117f, 0.7301981f, -0.924666f), Speed).OnComplete(() =>
        {
            isDownComplete = 21;
            Debug.Log("下降完成");
            
        });
    }
    /// <summary>
    /// 设置方向角
    /// </summary>
    public void SetFxValue(float value)
    {
        float fxSpeed = Mathf.Abs(fxObj.localRotation.y - value) / 9;
        fxObj.DOLocalRotate(new Vector3(0f, value, 0f), fxSpeed);
    }
    /// <summary>
    /// 设置俯仰角
    /// </summary>
    public void SetFyValue(float value)
    {
        float fySpeed = Mathf.Abs(fxObj.localRotation.z - value) / 9;
        fyObj.DOLocalRotate(new Vector3(-value, 0f, 0f), fySpeed);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (isUpComplete == 31)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                rayObj.SetActive(!rayObj.activeSelf);
                Debug.Log("RayObjState:" + rayObj.activeSelf);
            }
            Ray ray = new Ray(fyObj.position, fyObj.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5000f))
            {
                Debug.DrawLine(fyObj.position, hitInfo.point, Color.red);
                Debug.Log(hitInfo.collider.name);
                if (hitInfo.collider.name == "DrugCloud" /*&& isCheckDrug == false*/)
                {
                    sendMeteorTimer += Time.deltaTime;
                    if (sendMeteorTimer >= 1f)
                    {
                        sendMeteorTimer = 0;
                        //isCheckDrug = true;
                        Logger.Log("云内检测到毒，发送信息");
                        rayObj.GetComponent<MeshRenderer>().material.color = Color.red;
                        InfaredTelemetryDrug model = new InfaredTelemetryDrug()
                        {
                            Type = 2
                        };
                        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.INFARED_TELEMETRY_DRUG_DATA_102, NetManager.GetInstance().CurDeviceForward);

                        //把侦察结果数据给2号侦察员，后面上报时用
                        if (!isSendDetctRes)
                        {
                            isSendDetctRes = true;

                            if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase scene3D)
                            {
                                Vector3 lation = scene3D.terrainChangeMgr.gisPointMgr.GetGisPos(hitInfo.collider.transform.position);
                                string res = "发现沙林毒剂云团,经纬度为:" + lation.x.ToString() + "," + lation.y.ToString();
                                DetectResParam detectModel = new DetectResParam(DetectResType.Flag, res);
                                //发给侦查员2
                                List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                                    .Append(AppConfig.MACHINE_ID, SeatType.INVEST2)
                                    .Build();
                                NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(detectModel), NetProtocolCode.SEND_DETCT_RES_TO_SEAT, forwardModels);
                            }
                        }
                    }
                }
                else
                {
                    //isCheckDrug = false;
                    rayObj.GetComponent<MeshRenderer>().material.color = Color.green;
                    return;
                }
            }
            else
            {
                rayObj.GetComponent<MeshRenderer>().material.color = Color.green;
                return;
            }
        }

    }


    private void GetInfared_Telemerty(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            InfaredTelemetryOp102Model infaredTelemetry = JsonTool.ToObject<InfaredTelemetryOp102Model>(tcpReceiveEvParam.netData.Msg);
            string str = infaredTelemetry.Type.ToString() + infaredTelemetry.Operate.ToString();
            Logger.Log("监听到遥测信息,type为" + str);
            if (str == "30")
            {
                Debug.Log("开始上升");
                StartUP();
            }
            else if (str == "20")
            {
                Debug.Log("开始下降");
                StartDown();
            }
        }
    }
    private void Infared_Telemetry_PARAM(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            InfaredTelemetryParamModel infaredTelemetryParam = JsonTool.ToObject<InfaredTelemetryParamModel>(tcpReceiveEvParam.netData.Msg);
            Logger.Log("监听到遥测信息，方向角和俯仰角为" + infaredTelemetryParam.Fxvalue + "," + infaredTelemetryParam.Fyvalue);
            SetFyValue(infaredTelemetryParam.Fyvalue);
            SetFxValue(infaredTelemetryParam.Fxvalue);
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_102, GetInfared_Telemerty);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.INFARED_TELEMETRY_PARAM_102, Infared_Telemetry_PARAM);
    }
    #endregion
}