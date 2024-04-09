
using System.Collections.Generic;

/// <summary>
/// 有限状态机
/// </summary>

public class FSM : UnityMono
{
    protected virtual string TAG { get; set; } = "";

    /// <summary>
    /// 当前状态
    /// </summary>
    public FSMState curState;

    /// <summary>
    /// 所有状态
    /// </summary>
    public Dictionary<int, FSMState> stateDic = new Dictionary<int, FSMState>();

    protected virtual void Update()
    {
        if (curState != null)
        {
            curState.Update();
        }
    }

    private FSMState GetStateById(int stateId)
    {
        if (stateDic.ContainsKey(stateId))
        {
            return stateDic[stateId];
        }
        else
        {
            Logger.LogError(TAG + "not have state id: " + stateId);
            return null;
        }
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    public void ChangeState(ushort stateId)
    {
        if (curState != null && curState.stateId == stateId)
        {
            return;
        }
        if (curState != null)
        {
            //当前状态先退出
            curState.ExitState();
        }
        FSMState changeState = GetStateById(stateId);
        //进入新状态
        changeState?.EnterState();
        curState = changeState;
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    protected void AddState(FSMState fsmState)
    {
        if (stateDic.ContainsKey(fsmState.stateId))
        {
            Logger.LogError(TAG + "Have has State can not add again:" + fsmState.stateId);
        }
        else
        {
            stateDic.Add(fsmState.stateId, fsmState);
        }
    }
}
