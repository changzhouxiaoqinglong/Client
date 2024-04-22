
using DG.Tweening;
using NWH.VehiclePhysics;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车辆基类
/// </summary>
public class CarBase : UnityMono
{
    /// <summary>
    /// 机号
    /// </summary>
    public int MachineId
    {
        get; set;
    }

    /// <summary>
    /// 插旗检测 射线点
    /// </summary>
    public Transform flagRayCastNode;

    /// <summary>
    /// 插旗检测方向点
    /// </summary>
    public Transform flagRayCatDir;

    /// <summary>
    /// 插旗检测射线长度
    /// </summary>
    [Tooltip("插旗检测射线长度")]
    public float flagRayDis = 1.5f;

    [Tooltip("插旗射线检测随机角度范围")]
    public float flagRayAngleRange = 60;

    /// <summary>
    /// 相机切换控制
    /// </summary>
    public CameraChanger cameraChanger;

    /// <summary>
    /// 车门
    /// </summary>
    public Transform carDoor;

    /// <summary>
    /// 开门角度
    /// </summary>
    public Vector3 openDoorAngle;

    /// <summary>
    /// 后视镜相机节点
    /// </summary>
    public List<GameObject> RearMirrorCameraNode;

    /// <summary>
    /// 车位置态势同步
    /// </summary>
    private SituationSyncCarPos situationSyncCarPos;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// 车辆控制逻辑
    /// </summary>
    [HideInInspector]
    public VehicleController vehicleCtr;

    /// <summary>
    /// 人物管理
    /// </summary>
    [HideInInspector]
    public CarPlayerMgr playerMgr;

    private Train3DSceneCtrBase curScene;

    protected override void Awake()
    {
        base.Awake();
        vehicleCtr = GetComponent<VehicleController>();
        playerMgr = GetComponent<CarPlayerMgr>();
        playerMgr.Car = this;
        InitDevices();
        curScene = SceneMgr.GetInstance().curScene as Train3DSceneCtrBase;
        situationSyncCarPos = new SituationSyncCarPos(transform, curScene);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR, PlayerOutCar);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.IN_CAR, PlayerInCar);

        #region 新版修改
        // EventDispatcher.GetInstance().AddEventListener(EventNameList.DO_SELF_PROTECT, DoSelfProtect);
        // EventDispatcher.GetInstance().AddEventListener(EventNameList.OPEN_CAR_DOOR, OpenCarDoor);
        // EventDispatcher.GetInstance().AddEventListener(EventNameList.CLOSE_CAR_DOOR, CLoseCarDoor);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OpenDoor, OpenCarDoor);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CloseDoor, CLoseCarDoor);
        #endregion

        NetManager.GetInstance().AddQstRequstMsgEvent(ExTriggerType.InitJudgePoison, OnGetQstRequstMsg);
    }

    protected override void Start()
    {
        base.Start();
        InitMinimap();
        //初始 第一帧使车不受力，防止车子刚生成出现的翻车问题
        vehicleCtr.SetisKinematic(true);
        this.InvokeByYield(() =>
        {
            //驾驶员的车才受力控制
            if (IsSelfCar() && AppConfig.SEAT_ID == SeatType.DRIVE)
            {
                vehicleCtr.SetisKinematic(false);
                //发送同步消息
                gameObject.AddComponent<SendCarSyncMsg>();
            }
            else
            {
                //接收同步消息
                gameObject.AddComponent<ReceiveCarSyncMsg>();
            }
        }, new WaitForSeconds(0));
        if (!IsSelfCar() || AppConfig.SEAT_ID != SeatType.DRIVE)
        {
            //不是自己的车 或者不是驾驶员  就禁用驾驶脚本（只同步位置）
            vehicleCtr.Active = false;
        }
        InitRearMirror();
    }

    protected virtual void Update()
    {
        if (IsSelfCar()) {
            situationSyncCarPos?.Update();
        }
    }

    /// <summary>
    /// 初始化后视镜
    /// </summary>
    protected virtual void InitRearMirror()
    {
        if (RearMirrorCameraNode != null && RearMirrorCameraNode.Count > 0)
        {
            foreach (var item in RearMirrorCameraNode)
            {
                //不是自己的车  就隐藏后视镜相机  避免性能消耗
                item.SetActive(IsSelfCar());
            }
        }
    }

    /// <summary>
    /// 初始设置minimap相机跟随
    /// </summary>
    private void InitMinimap()
    {
        if (IsSelfCar())
        {
            curScene.miniMapMgr.MiniMapCamera.SetTarget(transform);
        }
    }

    /// <summary>
    /// 初始化设备
    /// </summary>
    private void InitDevices()
    {
        Transform deviceRoot = transform.Find("Devices");
        DeviceBase[] devices = deviceRoot.GetComponents<DeviceBase>();
        foreach (DeviceBase device in devices)
        {
            device.SetCurCar(this);
        }
    }

    /// <summary>
    /// 是否是自己的车
    /// </summary>
    public bool IsSelfCar()
    {
        return MachineId == AppConfig.MACHINE_ID;
    }

    /// <summary>
    /// 插旗
    /// </summary>
    public void DoFlag(int flagType, string info)
    {
        //随机角度
        float angle = Random.Range(-flagRayAngleRange, flagRayAngleRange);
        flagRayCastNode.localRotation = Quaternion.Euler(0, 0, angle);
        //射线方向
        Vector3 rayDir = flagRayCatDir.position - flagRayCastNode.position;
        Ray ray = new Ray(flagRayCastNode.position, rayDir);
        RaycastHit[] hits = Physics.RaycastAll(ray, flagRayDis);
#if UNITY_EDITOR
        Debug.DrawRay(flagRayCastNode.position, (flagRayCatDir.position - flagRayCastNode.position).normalized * flagRayDis);
#endif
        bool insert = false;
        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (item.transform.CompareTag(TagConstant.TERRAIN))
                {
                    insert = true;
                    //插旗子
                    if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase sceneCtr)
                    {
                        sceneCtr.flagMgr.InsertFlagLogic(flagType, item.point, info);
                    }
                    else
                    {
                        Logger.LogWarning("scene is not 3dscene  can not insert flag");
                    }
                    break;
                }
            }
        }
        if (!insert)
        {
            UIMgr.GetInstance().ShowToast("当前位置无法放置旗子");
        }
    }

    /// <summary>
    /// 下车
    /// </summary>
    private void PlayerOutCar(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            if (tcpParam.netData.MachineId == MachineId)
            {
                //下车
                bool outRes = playerMgr.OutCar(tcpParam.netData.SeatId);
                //回复下车结果
                ResBase res = new ResBase()
                {
                    Code = outRes ? NetResCode.RES_SUCCESS : NetResCode.RES_FAILED,
                };
                //发给侦查员1
                List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                    .Append(AppConfig.MACHINE_ID, SeatType.INVEST1)
                    .Build();
                NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(res), NetProtocolCode.OUT_CAR_RES, forwardModels);
            }
        }
    }

    /// <summary>
    /// 上车
    /// </summary>
    private void PlayerInCar(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            if (tcpParam.netData.MachineId == MachineId)
            {
                //上车
                playerMgr.InCar(tcpParam.netData.SeatId);
                //回复上车结果
                ResBase res = new ResBase()
                {
                    Code = NetResCode.RES_SUCCESS,
                };

                //发给侦查员1
                List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                    .Append(AppConfig.MACHINE_ID, SeatType.INVEST1)
                    .Build();
                NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(res), NetProtocolCode.IN_CAR_RES, forwardModels);
            }
        }
    }

    /// <summary>
    /// 自己做防护
    /// </summary>
    private void DoSelfProtect(IEventParam param)
    {
        if (IsSelfCar())
        {
            //当前人物
            PlayerCtr player = playerMgr.GetPlayerCtrBySeatId(AppConfig.SEAT_ID);
            player.DoProtect();
        }
    }

    /// <summary>
    /// 获取请求答题结果
    /// </summary>
    /// <param name="param"></param>
    private void OnGetQstRequstMsg(IEventParam param)
    {
        if (!IsSelfCar()) return;
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            QstRequest qst = JsonTool.ToObject<QstRequest>(tcpReceiveEvParam.netData.Msg);
            List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                .Append(AppConfig.MACHINE_ID, qst.SeatId)
                .Build();
            CraterBase crater = curScene.craterMgr.GetQuestionCrater(transform.position);// 获取附近的弹坑
            DrugVarData drugArea = curScene.harmAreaMgr.GetPosDrugData(transform.position);//获得对应位置的 毒区
            QstPoisonParam qstPoisonParam = new QstPoisonParam();
            qstPoisonParam.pos = curScene.terrainChangeMgr.gisPointMgr.GetGisPos(GetPosition()).ToCustVect2();
            qstPoisonParam.DrugType = QstPoisonDrugType.IN_CAR_DRUG;
            //如果是人在操作
            if (InputCtrMgr.GetInstance().curInputCtr is PlayerCtr playerCtr)
            {
                crater = curScene.craterMgr.GetQuestionCrater(playerCtr.GetPosition());
                drugArea = curScene.harmAreaMgr.GetPosDrugData(playerCtr.GetPosition());
                qstPoisonParam.pos = curScene.terrainChangeMgr.gisPointMgr.GetGisPos(playerCtr.GetPosition()).ToCustVect2();
                qstPoisonParam.DrugType = QstPoisonDrugType.OUT_CAR_DRUG;
            }
            float dentity = -1;
            //HarmAreaBase
            QstRequestResult model = new QstRequestResult()
            {
                IsOk = true,
                Tip = "发送成功",
                TriggerType = qst.TriggerType,
            };
            if (crater != null)
            {
               
                model.TargetId = crater.VarData.Type;
                print("xxx弹坑" + model.TargetId);
                dentity = crater.VarData.Dentity;
                qstPoisonParam.CheckType = QstPoisonCheckType.GROUND_CHECK_TYPE;
            }
            else if (drugArea != null)
            {
                
                model.TargetId = drugArea.Type;
                print("xxxdrugArea" + model.TargetId);
                dentity = curScene.harmAreaMgr.GetPosDrugDentity(transform.position);
                qstPoisonParam.CheckType = QstPoisonCheckType.AIR_CHECK_TYPE;
            }
            else
            {
                model.TargetId = PoisonType.NO_POISON;
                model.IsOk = false;
                print("当前位置无可侦捡对象" + model.TargetId);
                model.Tip = "未到达袭击区域！";
                UIMgr.GetInstance().ShowToast(model.Tip);
            }
            ExPoisonData exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(model.TargetId);
            qstPoisonParam.DegreeLow = exPoisonData.GetdDegreeByDentity(dentity);
            model.Param = JsonTool.ToJson(qstPoisonParam);
            NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.RESULT_QUESTION, forwardModels);
        }
    }


    /// <summary>
    /// 开车门
    /// </summary>
    /// <param name="param"></param>
    private void OpenCarDoor(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            if (tcpParam.netData.MachineId == MachineId)
            {
                carDoor.DOLocalRotate(openDoorAngle, 1.0f);
            }
        }
    }


    /// <summary>
    /// 关车门
    /// </summary>
    /// <param name="param"></param>
    private void CLoseCarDoor(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpParam)
        {
            if (tcpParam.netData.MachineId == MachineId)
            {
                carDoor.DOLocalRotate(new Vector3(carDoor.localEulerAngles.x,0,0), 1.0f);
            }
        }
    }

    /*
	#region 
	/// <summary>
	/// 开车门
	/// </summary>
	/// <param name="param"></param>
	private void OpenCarDoor(IEventParam param)
    {
        if (IsSelfCar())
        {
            carDoor.DOLocalRotate(openDoorAngle, 1.0f);
        }
    }


    /// <summary>
    /// 关车门
    /// </summary>
    /// <param name="param"></param>
    private void CLoseCarDoor(IEventParam param)
    {
        if (IsSelfCar())
        {
            carDoor.DOLocalRotate(Vector3.zero, 1.0f);
        }
    }

	#endregion
    */

	/// <summary>
	/// 获得车附近随机位置
	/// </summary>
	/// <param name="minDis">最小距离</param>
	/// <param name="maxDis">最大距离</param>
	/// <returns></returns>
	public Vector3 GetRandomNearPos(float minDis, float maxDis)
    {
        float randomX = Random.Range(minDis, maxDis);
        randomX = randomX * (Random.Range(-1.0f, 1.0f) > 0 ? 1 : -1);
        float randomZ = Random.Range(minDis, maxDis);
        randomZ = randomZ * (Random.Range(-1.0f, 1.0f) > 0 ? 1 : -1);
        return transform.position + new Vector3(randomX, 0, randomZ);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR, PlayerOutCar);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.IN_CAR, PlayerInCar);

        #region 新版修改
        // EventDispatcher.GetInstance().RemoveEventListener(EventNameList.DO_SELF_PROTECT, DoSelfProtect);
        //  EventDispatcher.GetInstance().RemoveEventListener(EventNameList.OPEN_CAR_DOOR, OpenCarDoor);
        //  EventDispatcher.GetInstance().RemoveEventListener(EventNameList.CLOSE_CAR_DOOR, CLoseCarDoor);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OpenDoor, OpenCarDoor);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CloseDoor, CLoseCarDoor);
        #endregion


        NetManager.GetInstance().RemoveQstRequstMsgEvent(ExTriggerType.InitJudgePoison, OnGetQstRequstMsg);
        

    }
}
