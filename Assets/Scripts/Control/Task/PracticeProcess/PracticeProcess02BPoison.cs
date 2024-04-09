
using UnityEngine;
/// <summary>
/// 02b毒剂报警器 训练流程
/// </summary>
public class PracticeProcess02BPoison : PracticeProcessBase
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
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP, OnGetPoisonAlarmOpMsg);
    }

    /// <summary>
    /// 操作毒剂报警器
    /// </summary>
    private void OnGetPoisonAlarmOpMsg(IEventParam param)
    {
        //已完成所有步骤
        if (IsFinish())
        {
            return;
        }
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            PoisonAlarmOpModel model = JsonTool.ToObject<PoisonAlarmOpModel>(tcpReceiveEvParam.netData.Msg);
            switch (model.Type)
            {
                case PoisonAlarmOpType.Intake:
                    Debug.LogWarning("Intake");
                    DoProcess(model.Operate == OperateDevice.OPEN ? ProcessId.POISON_ALARM_OP_INTAKE_02B : ProcessId.POISON_ALARM_CLOSE_INTAKE_02B);
                    break;
                case PoisonAlarmOpType.OpenClose:
                    Debug.LogWarning("OpenClose");
                    DoProcess(model.Operate == OperateDevice.OPEN ? ProcessId.POISON_ALARM_OPEN_02B : ProcessId.POISON_ALARM_CLOSE_02B);
                    break;
                case PoisonAlarmOpType.Check:
                    Debug.LogWarning("Check");
                    DoProcess(model.Operate == OperateDevice.OPEN ? ProcessId.POISON_ALARM_CHECK_02B : ProcessId.POISON_ALARM_CHECK_END_02B);
                    break;
                case PoisonAlarmOpType.JinYang:
                    Debug.LogWarning("JinYang");
                    //开始进样
                    if (model.Operate == OperateDevice.OPEN)
                    {
                        //记下进样时间
                        startJinYangTime = Time.realtimeSinceStartup;
                        DoProcess(ProcessId.POISON_ALARM_JINYANG_02B);
                    }
                    else
                    {
                        //结束进样
                        int jinIndex = GetProcessIndex(ProcessId.POISON_ALARM_JINYANG_02B);
                        //当前步骤在开始进样之后，才判断
                        if (curIndex >= jinIndex)
                        {
                            //计算进样时间
                            float time = Time.realtimeSinceStartup - startJinYangTime;
                            //时间充足
                            if (time > jinYangMinTime)
                            {
                                //进样结束
                                DoProcess(ProcessId.POISON_ALARM_END_JINYANG);
                            }
                            else
                            {
                                //提示进样不足
                                EventDispatcher.GetInstance().DispatchEvent(EventNameList.PRACTICE_PROCESS_ERROR_TIP, new StringEvParam("进样时间不足！"));
                            }
                        }
                    }
                    break;
                case PoisonAlarmOpType.Alarm:
                    Debug.LogWarning("Alarm");
                    if (model.Operate == OperateDevice.OPEN)
                    {
                        DoProcess(ProcessId.POISON_ALARM_02B);
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
                case ProcessId.POISON_ALARM_OPEN_02B:
                    //设置状态为可开机
                    SendPoisonAlarmStatCtr(PoisonAlarmStat.OPEN_CTR);
                    break;
                case ProcessId.POISON_ALARM_JINYANG_02B:
                    //设置状态为进样可控
                    SendPoisonAlarmStatCtr(PoisonAlarmStat.JINYANG_CTR);
                    break;
                case ProcessId.POISON_ALARM_CLOSE_02B:
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
        PoisonAlarmStatCtr02BModel model = new PoisonAlarmStatCtr02BModel()
        {
            Operate = statCtr,
        };
        //发给设备
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.POISON_ALARM_STAT_CTR, NetManager.GetInstance().CurDeviceForward);
    }

    public override void End()
    {
        base.End();
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.POISON_ALARM_OP, OnGetPoisonAlarmOpMsg);
    }
}
