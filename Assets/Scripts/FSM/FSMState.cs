
/// <summary>
/// 状态
/// </summary>
public class FSMState
{
    /// <summary>
    /// 状态id
    /// </summary>
    public ushort stateId;

    public string stateName;

    public FSM curFSM;

    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void EnterState()
    {
        if (LogConfig.LOG_FSM)
        {
            Logger.LogDebug("FSM_Enter State:" + stateName);
        }
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void ExitState()
    {
        if (LogConfig.LOG_FSM)
        {
            Logger.LogDebug("FSM_Exit State:" + stateName);
        }
    }

    public virtual void Update()
    {

    }

    public FSMState(FSM fsm, ushort stateId, string stateName)
    {
        curFSM = fsm;
        this.stateId = stateId;
        this.stateName = stateName;
    }
}
