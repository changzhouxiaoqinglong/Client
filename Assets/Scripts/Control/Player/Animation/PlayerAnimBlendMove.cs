
using UnityEngine;

public class PlayerAnimBlendMove : FSMState
{
    /// <summary>
    /// 玩家动画控制
    /// </summary>
    private PlayerAnim curPlayerAnim;

    public PlayerAnimBlendMove(FSM fsm, ushort stateId, string stateName) : base(fsm, stateId, stateName)
    {
        curPlayerAnim = fsm as PlayerAnim;
    }

    public override void Update()
    {
        base.Update();
        //同步状态下这里不设置动画参数
        if (curPlayerAnim.ReceiveSyncMsg == null)
        {
            curPlayerAnim.animator.SetFloat(PlayerAnim.Speed.Value, curPlayerAnim.playerCtr.GetAnimSpeedValue());
        }
    }

    /// <summary>
    /// 离开状态
    /// </summary>
    public override void ExitState()
    {
        base.ExitState();
        curPlayerAnim.animator.SetFloat(PlayerAnim.Speed.Value, 0);
    }
}
