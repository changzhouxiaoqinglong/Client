using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetPoisonBleedView : ViewBase<DetPoisonBleedViewModel>
{
    public Action OnCloseCallBack { get; set; }


    private Transform detPoisonBody;

    /// <summary>
    /// 确定按钮
    /// </summary>
    private ButtonBase closeViewBtn;
    /// <summary>
    /// 分针
    /// </summary>
    private Transform minuteHand;

    /// <summary>
    /// 设置时间
    /// </summary>
    private Text setTime;

    /// <summary>
    /// 提交按钮
    /// </summary>
    private ButtonBase submitBtn;

    /// <summary>
    /// 时间计时
    /// </summary>
    private float bleedTime = -1;

    /// <summary>
    /// 时间是否在规定范围
    /// </summary>
    private float timeRange = -1;

    /// <summary>
    /// 时针角度
    /// </summary>
    private float minuteHandAngleZ;

    /// <summary>
    /// 虚拟设备
    /// </summary>
    private VirtualCarDrugPoison02B drugPoison02B;

    /// <summary>
    /// 问题参数
    /// </summary>
    public QstPoisonColorParam poisonColorParam;

    protected override void Awake()
    {
        base.Awake();
        detPoisonBody = transform.Find("Bleed/DetPoisonBody");
        minuteHand = detPoisonBody.Find("ShowTime/MinuteParent/MinuteHand");
        setTime = detPoisonBody.Find("SetTime/Text").GetComponent<Text>();
        closeViewBtn = transform.Find("Content/CloseViewBtn").GetComponent<ButtonBase>();
        closeViewBtn.RegistClick(OnClickCloseViewBtn);
        //submitBtn = detPoisonBody.Find("SubmitBtn").GetComponent<ButtonBase>();
        //submitBtn.RegistClick(OnClickSubmitBtn);
        drugPoison02B = (SceneMgr.GetInstance().curScene as TrainSceneCtrBase).virtualCar.GetDevice<VirtualCarDrugPoison02B>();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OpenQuesionView);


        //新版修改 抽气改为硬件发送
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_CAR_POIS_GAS_TIME, OnGetGasTimeMsg);
    }


    private void OnGetGasTimeMsg(IEventParam param)
    {
    

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            SetCarPoisonGasTime model = JsonTool.ToObject<SetCarPoisonGasTime>(tcpReceiveEvParam.netData.Msg);
            // model.Time=

            if (bleedTime != -1)
            {
                UIMgr.GetInstance().ShowToast("不要重复设值");
                return;
            }

            if (model.Time==0 || ((int)model.Time > BleedTimeConstant.MAXTIME && (int)model.Time < BleedTimeConstant.MINTIME))
            {
                UIMgr.GetInstance().ShowToast("请把时间正确设置在0~180秒之间");
                return;
            }
            setTime.text = model.Time.ToString();
            bleedTime = model.Time;
            timeRange = bleedTime;
            minuteHandAngleZ = -MathsMgr.TimeAngle(BleedTimeConstant.SECONDANGLE, bleedTime);
            minuteHand.DOLocalRotate(new Vector3(0, 0, minuteHandAngleZ), BleedTimeConstant.MOVETIME);


        }
    }

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 确定抽气时间按钮
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSubmitBtn(GameObject obj)
    {
        if(bleedTime != -1)
        {
            UIMgr.GetInstance().ShowToast("不要重复设值");
            return;
        }

        if(setTime.text.Equals("") || (int.Parse(setTime.text) > BleedTimeConstant.MAXTIME && int.Parse(setTime.text) < BleedTimeConstant.MINTIME))
        {
            UIMgr.GetInstance().ShowToast("请把时间正确设置在0~180秒之间");
            return;
        }
        bleedTime = int.Parse(setTime.text);
        timeRange = bleedTime;
        minuteHandAngleZ = -MathsMgr.TimeAngle(BleedTimeConstant.SECONDANGLE, bleedTime);
        minuteHand.DOLocalRotate(new Vector3(0,0, minuteHandAngleZ) , BleedTimeConstant.MOVETIME);
     //   EventDispatcher.GetInstance().DispatchEvent(EventNameList.SET_POIS_GAS_TIME, new FloatEvParam(timeRange));
    }

    private void FixedUpdate()
    {
        if (bleedTime == -1) return;
        if(drugPoison02B.curPumpState)//必须开机状态下
        {
            CountDownBleedTime();//抽气倒计时
        }
    }


    /// <summary>
    /// 时间
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTime()
    {
        while(bleedTime > BleedTimeConstant.MINTIME)
        {
            yield return new WaitForSeconds(BleedTimeConstant.MOVETIME);
            bleedTime--;
            minuteHandAngleZ = -MathsMgr.TimeAngle(BleedTimeConstant.SECONDANGLE, bleedTime);
            minuteHand.localEulerAngles = new Vector3(0, 0, minuteHandAngleZ);
        }
        closeViewBtn.gameObject.SetActive(true);
        yield return null;
    }
    
    private void CountDownBleedTime()
    {
        if(bleedTime <= 0) return;
        bleedTime -= Time.fixedDeltaTime;
        minuteHandAngleZ = -MathsMgr.TimeAngle(BleedTimeConstant.SECONDANGLE, bleedTime);
        minuteHand.localEulerAngles = new Vector3(0, 0, minuteHandAngleZ);
    }

    /// <summary>
    /// 点击确定关闭面板打开题目面板
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickCloseViewBtn(GameObject obj)
    {

    }

    private void OpenQuesionView(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            //操作数据
            CarDetectPoisonOpModel model = JsonTool.ToObject<CarDetectPoisonOpModel>(tcpReceiveEvParam.netData.Msg);
            if (model.Operate == OperateDevice.CLOSE && model.Type == CarDetectPoisonOpType.Pump)
            {
                UIMgr.GetInstance().CloseView(ViewType.DetPoisonBleedView);
                if (bleedTime <= 0 && timeRange >= BleedTimeConstant.CORRECTMINTIME && timeRange <= BleedTimeConstant.CORRECTMAXTIME)
                {
                    drugPoison02B.isOk = true;
                }
                else
                {
                    drugPoison02B.isOk = false;
                }
                QuestionView questionView = (UIMgr.GetInstance().OpenView(ViewType.QuestionView) as QuestionView);
                questionView.tubeType = poisonColorParam.tubeType;
                questionView.InitJumpId = QuestionConstant.JUDGEID;
                questionView.SetQstRequestResult(poisonColorParam.qstResult);
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.OP_CAR_DETECT_POISON, OpenQuesionView);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_CAR_POIS_GAS_TIME, OnGetGasTimeMsg);
        OnCloseCallBack?.Invoke();
    }
}
