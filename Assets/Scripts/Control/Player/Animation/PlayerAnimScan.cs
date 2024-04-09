using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScan : FSMState
{
    /// <summary>
    /// 玩家动画控制
    /// </summary>
    private PlayerAnim curPlayerAnim;

    public PlayerAnimScan(FSM fsm, ushort stateId, string stateName) : base(fsm, stateId, stateName)
    {
        curPlayerAnim = fsm as PlayerAnim;
    }

    public override void EnterState()
    {
        base.EnterState();
        curPlayerAnim.animator.SetBool(PlayerAnim.Scan.Value, true);
    }

    /// <summary>
    /// 离开状态
    /// </summary>
    public override void ExitState()
    {
        base.ExitState();
        curPlayerAnim.animator.SetBool(PlayerAnim.Scan.Value, false);
    }
}
