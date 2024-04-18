using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeProcess106Poison : PracticeProcessBase
{

    /// <summary>
    /// 记录开始进样时间
    /// </summary>
    private float startJinYangTime = 0;

    /// <summary>
    /// 进样最少时间
    /// </summary>
    private float jinYangMinTime = 5;
    public override void Init(int taskId)
    {
        base.Init(taskId);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_106, OnGetPoisonAlarmOpMsg);
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_SetReliefThreshold, OnGetRadioDoseThresholdMsg);
    }

    private void OnGetRadioDoseThresholdMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish()) return;

        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            SetReliefThreshold model = JsonTool.ToObject<SetReliefThreshold>(tcpReceiveEvParam.netData.Msg);



            if (model.ReliefThreshold>0.35f&& model.ReliefThreshold < 0.45f)
			{
                DoProcess(Poison106Id.SetReliefThreshold_106);
               // Debug.Log("设置减压阀数据0.35~0.45MPa");
            }
            else if (model.ReliefThreshold == 0)
            {
                DoProcess(Poison106Id.SetReliefThreshold_0_106);
               // Debug.Log("设置减压阀数据归零");
            }
           
        }

    }


    /// <summary>
    /// 操作毒剂报警器
    /// </summary>
    private void OnGetPoisonAlarmOpMsg(IEventParam param)
    {
       // Logger.LogError("dsc");
        //已完成所有步骤
        if (IsFinish())
        {
            return;
        }
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PoisonAlarmOp106Model model = JsonTool.ToObject<PoisonAlarmOp106Model>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PoisonAlarmOp106Type.kaiguanji:
                    //Debug.LogWarning("106kaiguanji");
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison106Id.POISON_ALARM_OPEN_106 : Poison106Id.POISON_ALARM_CLOSE_106);
                    break;
                case PoisonAlarmOp106Type.jinqi:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison106Id.POISON_ALARM_OPEN_PROTECT_106 : Poison106Id.POISON_ALARM_ClOSE_PROTECT_106);
                    break;
                case PoisonAlarmOp106Type.lingqi:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison106Id.POISON_ALARM_OPEN_LINGQI_106 : Poison106Id.POISON_ALARM_CLOSE_LINGQI_106);
                    break;
               
                case PoisonAlarmOp106Type.danqi:
                    DoProcess(model.Operate == OperateDevice.OPEN ? Poison106Id.POISON_ALARM_OPEN_DANQI_106 : Poison106Id.POISON_ALARM_CLOSE_DANQI_106);
                    break;
                case PoisonAlarmOp106Type.yure:    
                    if (model.Operate == OperateDevice.OPEN)
                    {
                        DoProcess(Poison106Id.POISON_ALARM_YURE_106);
                    }
                    break;
                case PoisonAlarmOp106Type.jinyang:

                    if (model.Operate == OperateDevice.OPEN)
                    {
                        //记下进样时间
                        startJinYangTime = Time.realtimeSinceStartup;
                        DoProcess(Poison106Id.POISON_ALARM_JINYANG_106);
                    }
                    else
                    {
                        //结束进样
                        int jinIndex = GetProcessIndex(Poison106Id.POISON_ALARM_JINYANG_106);
                        //当前步骤在开始进样之后，才判断
                        if (curIndex >= jinIndex)
                        {
                            //计算进样时间
                            float time = Time.realtimeSinceStartup - startJinYangTime;
                            //时间充足
                            if (time > jinYangMinTime)
                            {
                                //进样结束
                                DoProcess(Poison106Id.POISON_ALARM_END_JINYANG_106);
                            }
                            else
                            {
                                //提示进样不足
                                EventDispatcher.GetInstance().DispatchEvent(EventNameList.PRACTICE_PROCESS_ERROR_TIP, new StringEvParam("进样超过5秒结束！"));
                            }
                        }
                    }

                    break;
                case PoisonAlarmOp106Type.alarm:
                    if (model.Operate == OperateDevice.OPEN)
                    {
                        DoProcess(Poison106Id.POISON_ALARM_106);
                    }
                    break;
               
            }
        }
    }

    protected override void JumpToNext()
    {
        base.JumpToNext();
        if (curIndex >= 0 && curIndex < processList.Count)
        {
            //当前步骤id
            int processId = processList[curIndex].Id;
            switch (processId)
            {
                case Poison106Id.POISON_ALARM_OPEN_106:
                    //设置状态为开机可控
                    SendPoisonAlarmStatCtr(PoisonAlarmStat.OPEN_CTR);
                    break;
                case Poison106Id.POISON_ALARM_JINYANG_106:
                    //设置状态为进样可控
                    SendPoisonAlarmStatCtr(PoisonAlarmStat.JINYANG_CTR);
                    break;
                case Poison106Id.POISON_ALARM_CLOSE_106:
                    //设置状态为关机可控
                    SendPoisonAlarmStatCtr(PoisonAlarmStat.CLOSE_CTR);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 发送毒剂报警器状态可控消息
    /// </summary>
    private void SendPoisonAlarmStatCtr(int statCtr)
    {
        PoisonAlarmStatCtr106Model model = new PoisonAlarmStatCtr106Model()
        {
            Operate = statCtr,
        };
        //发给设备
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.POISON_ALARM_STAT_CTR_106, NetManager.GetInstance().CurDeviceForward);
    }

    public override void End()
    {
        base.End();       
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP_106, OnGetPoisonAlarmOpMsg);
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.SET_SetReliefThreshold, OnGetRadioDoseThresholdMsg);
    }
}
