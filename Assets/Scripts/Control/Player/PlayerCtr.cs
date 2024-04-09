
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtr : InputCtrBase
{
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minimumY = -60f;
    public float maximumY = 60f;

    private bool grounded = false;
    private Rigidbody rb;

    [HideInInspector]
    public float inputX;

    [HideInInspector]
    public float inputY;

    [Tooltip("面具")]
    public GameObject mask;

    /// <summary>
    /// 跟随相机
    /// </summary>
    public FollowPlayerCamera followCamera;

    /// <summary>
    /// 席位数据
    /// </summary>
    public TrainSeatVarData TrainSeatData
    {
        get; set;
    }

    /// <summary>
    /// 动画控制
    /// </summary>
    [HideInInspector]
    public PlayerAnim playerAnim;

    /// <summary>
    /// 碰撞体
    /// </summary>
    private CapsuleCollider curCollider;

    /// <summary>
    /// 席位节点
    /// </summary>
    public Transform SeatRoot
    {
        get; set;
    }

    /// <summary>
    /// 下车节点
    /// </summary>
    private Transform outCarRoot;
    private Transform OutCarRoot
    {
        get
        {
            if (outCarRoot == null)
            {
                GameObject outCar = GameObject.Find("OutCarRoot");
                if (outCar)
                {
                    outCarRoot = outCar.transform;
                }
                else
                {
                    outCarRoot = new GameObject("OutCarRoot").transform;
                }
            }
            return outCarRoot;
        }
    }

    /// <summary>
    /// 所在车
    /// </summary>
    [HideInInspector]
    public CarBase Car
    {
        get; set;
    }

    /// <summary>
    /// 有害区域管理
    /// </summary>
    protected HarmAreaMgr harmAreaMgr;

    /// <summary>
    /// 辐射剂量率检测 计时器
    /// </summary>
    private float checkTimer = 0;

    /// <summary>
    /// 插旗射线起始高度
    /// </summary>
    public float flagFromHeight = 1f;

    /// <summary>
    /// 插旗射线结束方向位置距离
    /// </summary>
    public float flagToZDis = 0.5f;

    /// <summary>
    /// 是否在车里
    /// </summary>
    public bool IsInCar = false;

    /// <summary>
    /// 是否在操作
    /// </summary>
    private bool IsOperate = false;


    /// <summary>
    /// 接收同步数据
    /// </summary>
    private ReceivePlayerSyncMsg receiveSyncMsg;


    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnim>();
        curCollider = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        harmAreaMgr = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).harmAreaMgr;
    }

    private void Start()
    {
        if (TrainSeatData.MachineId != AppConfig.MACHINE_ID || AppConfig.SEAT_ID != SeatType.DRIVE)
        {
            //接收同步消息
            receiveSyncMsg = gameObject.AddComponent<ReceivePlayerSyncMsg>();
        }
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR_MEASURE, OnGetMeasureMsg);
        NetManager.GetInstance().AddQstRequstMsgEvent(ExTriggerType.SamplingPoison, OnGetSamplingMsg);

    }

    private void Update()
    {

    }


    void FixedUpdate()
    {
        // We apply gravity manually for more tuning control
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

        //控制
        ControlLogic();

        //相机跟随
        if (followCamera.gameObject.activeSelf)
        {
            followCamera.FollowTarget();
        }
    }

    /// <summary>
    /// 控制逻辑
    /// </summary>
    private void ControlLogic()
    {
        //未启用控制
        if (!IsEnabled)
        {
            //还原输入值
            inputX = 0;
            inputY = 0;
            return;
        }
        //if (grounded)
        {
            // Calculate how fast we should be moving
            inputX = CustomInput.Horizontal;
            inputY = CustomInput.Vertical;

            Vector3 targetVelocity = new Vector3(inputX, 0, inputY);
            targetVelocity = followCamera.transform.TransformDirection(targetVelocity);
            //移动方向y要为0
            targetVelocity = new Vector3(targetVelocity.x, 0, targetVelocity.z).normalized;
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            //移动的时候 y不变化
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            //旋转
            if (inputX != 0 || inputY != 0)
            {
                transform.forward = new Vector3(targetVelocity.x, 0, targetVelocity.z);
            }

            //// Jump
            //if (canJump && Input.GetKeyDown(KeyCode.Space))
            //{
            //    rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            //}
        }
        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    /// <summary>
    /// 获得动画参数speed应设置的值  取x  y输入最大值
    /// </summary>
    public float GetAnimSpeedValue()
    {
        return Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(inputY));
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetLocalRotation(Quaternion quaternion)
    {
        transform.localRotation = quaternion;
    }

    public void SetRotation(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }

    public Vector3 GetPosition()
    {
        Debug.Log(transform.position);
        return transform.position;
    }

    /// <summary>
    /// 进车
    /// </summary>
    public void InCar()
    {
        playerAnim.ChangeState((ushort)PlayerAnimType.Sit);
        if (isEnabled)
        {
            //启用车辆相机
            Car.cameraChanger.SetEnable();
            //启用车辆控制
            VehicleInputMgr.GetInstance().SetEnable();
            if (Car.IsSelfCar() && TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.NUCLEAR)
            {
                //隐藏展示 徒步侦察的 辐射剂量率
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.HIDE_OUT_RADIOM_RATE);
            }
        }
        InCarState();
        SetLocalPosition(Vector3.zero);
        SetLocalRotation(Quaternion.identity);
    }

    public void InCarState()
    {
        curCollider.enabled = false;
        rb.isKinematic = true;
        IsInCar = true;
        transform.SetParent(SeatRoot);
    }

    /// <summary>
    /// 下车
    /// </summary>
    /// <param name="control">是否进行控制</param>
    /// <param name="outPos">下车位置</param>
    public void OutCar(bool control, Vector3 outPos)
    {
        playerAnim.ChangeState((ushort)PlayerAnimType.BlendMove);
        if (control)
        {
            //开启控制
            SetEnable();
            //相机跟随
            followCamera.SetEnable();
        }
        OutCarState();
        transform.position = outPos;
    }

    public void OutCarState()
    {
        curCollider.enabled = true;
        //下车 如果是同步人物 就不受力影响  否则就设置为受力影响
        rb.isKinematic = receiveSyncMsg != null;
        IsInCar = false;
        transform.SetParent(OutCarRoot);
    }

    /// <summary>
    /// 人物在对应位置是否会产生碰撞
    /// </summary>
    public bool IsColliderPoint(Vector3 pos)
    {
        //位置偏差
        Vector3 offSet = pos - transform.position;
        //胶囊startPoint
        Vector3 startPoint = curCollider.transform.position + curCollider.center - Vector3.up * curCollider.height / 2f + Vector3.up * curCollider.radius;
        startPoint = startPoint + offSet;
        //胶囊endPoint
        Vector3 endPoint = curCollider.transform.position + curCollider.center + Vector3.up * curCollider.height / 2f - Vector3.up * curCollider.radius;
        endPoint = endPoint + offSet;
        Collider[] colliders = Physics.OverlapCapsule(startPoint, endPoint, curCollider.radius);
        return colliders.Length > 0;
    }

    /// <summary>
    /// 防护
    /// </summary>
    public void DoProtect()
    {
        mask.SetActive(true);
    }

    /// <summary>
    /// 取消防护
    /// </summary>
    public void UnDoProtect()
    {
        mask.SetActive(false);
    }

    /// <summary>
    /// 检测辐射率
    /// </summary>
    private void DoRadiance()
    {
        if (Car.IsSelfCar() && TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.NUCLEAR)
        {
            //检测剂量率
            checkTimer += Time.deltaTime;
            if (checkTimer >= AppConstant.RADIOM_CHECK_OFFTIME)
            {
                checkTimer = 0;
                //剂量率
                float radiomRate = harmAreaMgr.GetPosRadiomRate(transform.position);
                //下车 检测到剂量率
                EventDispatcher.GetInstance().DispatchEvent(EventNameList.OUTCAR_RADIOMRATE, new FloatEvParam(radiomRate));
            }
        }
    }

    /// <summary>
    /// 插旗
    /// </summary>
    /// <param name="flagType">旗子类型</param>
    public void DoFlag(int flagType, string info)
    {
        //射线初始位置
        Vector3 fromPos = transform.position + Vector3.up * flagFromHeight;
        //射线方向位置
        Vector3 toPos = transform.position + transform.forward * flagToZDis;
        Vector3 dir = toPos - fromPos;
        Ray ray = new Ray(fromPos, dir);
#if UNITY_EDITOR
        Debug.DrawRay(fromPos, dir, Color.red);
#endif
        RaycastHit[] hits = Physics.RaycastAll(ray, 2);
        bool insert = false;
        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (item.transform.CompareTag(TagConstant.TERRAIN))
                {
                    //插旗子
                    if (SceneMgr.GetInstance().curScene is Train3DSceneCtrBase sceneCtr)
                    {
                        sceneCtr.flagMgr.InsertFlagLogic(flagType, item.point, info);
                    }
                    else
                    {
                        Logger.LogWarning("scene is not 3dscene  can not insert flag");
                    }
                    insert = true;
                }
                break;
            }
        }
        if (!insert)
        {
            UIMgr.GetInstance().ShowToast("当前位置无法放置旗子");
        }
    }

    /// <summary>
    /// 获得当前同步数据
    /// </summary>
    public PlayerSyncModel GetPlayerSyncModel()
    {
        PlayerSyncModel model = new PlayerSyncModel()
        {
            SeatId = TrainSeatData.SeatId,
            IsProtect = mask.activeSelf,
            IsInCar = IsInCar,
            Pos = transform.position.ToCustVect3(),
            Rotate = transform.eulerAngles.ToCustVect3(),
            AnimParam = playerAnim.GetAnimSysParam(),
        };
        return model;
    }

    /// <summary>
    /// 收到同步数据
    /// </summary>
    public void ReceiveSyncModel(PlayerSyncModel model)
    {
        ReceivePlayerSyncMsg receiveMsg = GetComponent<ReceivePlayerSyncMsg>();
        if (receiveMsg)
        {
            receiveMsg.OnReceivePlayerSyncModel(model);
        }
    }

    /// <summary>
    /// 测量携程
    /// </summary>
    IEnumerator IMeasureTime()
    {
        //playerAnim.ChangeState((ushort)PlayerAnimType.Scan);
        player1.SetActive(false);
        player3.SetActive(true);
        SetDisable();
        float nowTime = 0.0f;
        while ((nowTime += Time.deltaTime) <= 10f)
        {
            yield return null;
            DoRadiance();
        }
        //playerAnim.ChangeState((ushort)PlayerAnimType.BlendMove);
        player1.SetActive(true);
        player3.SetActive(false);
        yield return null;
        SetEnable();
        EventDispatcher.GetInstance().DispatchEvent(EventNameList.HIDE_OUT_RADIOM_RATE);
        IsOperate = false;
    }

    /// <summary>
    /// 采样携程
    /// </summary>
    IEnumerator ISamplingTime(QstRequestResult model, List<ForwardModel> forwardModels)
    {
        //playerAnim.ChangeState((ushort)PlayerAnimType.Crouch);
        player1.SetActive(false);
        player2.SetActive(true);
        SetDisable();
        Debug.Log("CROUCH" + playerAnim.GetAnimatorTimer(PlayerAnimName.CROUCH));
        yield return new WaitForSeconds(playerAnim.GetAnimatorTimer(PlayerAnimName.CROUCH) * 2);
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.RESULT_QUESTION, forwardModels);
        //float waitTime = 2.0f;
        //yield return new WaitForSeconds(waitTime);
        //playerAnim.ChangeState((ushort)PlayerAnimType.StandUp);
        yield return new WaitForSeconds(4);
        //playerAnim.ChangeState((ushort)PlayerAnimType.BlendMove);
        player1.SetActive(true);
        player2.SetActive(false);
        IsOperate = false;
        SetEnable();
    }


    /// <summary>
    /// 监听测量事件
    /// </summary>
    private void OnGetMeasureMsg(IEventParam param)
    {
        if (transform.parent != outCarRoot) return;
        if(IsOperate)
        {
            UIMgr.GetInstance().ShowToast("当前正在执行操作，请结束后再试。");
            return;
        }
        IsOperate = true;
        StartCoroutine(IMeasureTime());
    }


    private void OnGetSamplingMsg(IEventParam param)
    {
        if (transform.parent != outCarRoot) return;
        if (IsOperate)
        {
            UIMgr.GetInstance().ShowToast("当前正在执行操作，请结束后再试。");
            return;
        }
        IsOperate = true;
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            QstRequest qst = JsonTool.ToObject<QstRequest>(tcpReceiveEvParam.netData.Msg);
            QstRequestResult qstRequestResult = new QstRequestResult();
            List<ForwardModel> forwardModels = new ForwardModelsBuilder()
                .Append(AppConfig.MACHINE_ID, qst.SeatId)
                .Build();
            qstRequestResult.TriggerType = qst.TriggerType;
            qstRequestResult.TargetId = PoisonType.NO_POISON;
            qstRequestResult.IsOk = true;
            StartCoroutine(ISamplingTime(qstRequestResult, forwardModels));
        }
    }

    private void OnDestroy()
    {
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OUT_CAR_MEASURE, OnGetMeasureMsg);
        NetManager.GetInstance().RemoveQstRequstMsgEvent(ExTriggerType.SamplingPoison, OnGetSamplingMsg);
    }
}
