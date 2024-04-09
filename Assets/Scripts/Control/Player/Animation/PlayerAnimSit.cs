
/// <summary>
/// 坐下动画
/// </summary>
public class PlayerAnimSit : FSMState
{
    /// <summary>
    /// 玩家动画控制
    /// </summary>
    private PlayerAnim curPlayerAnim;

    public PlayerAnimSit(FSM fsm, ushort stateId, string stateName) : base(fsm, stateId, stateName)
    {
        curPlayerAnim = fsm as PlayerAnim;
    }

    public override void EnterState()
    {
        base.EnterState();
        curPlayerAnim.animator.SetBool(PlayerAnim.Sit.Value, true);
    }

    /// <summary>
    /// 离开状态
    /// </summary>
    public override void ExitState()
    {
        base.ExitState();
        curPlayerAnim.animator.SetBool(PlayerAnim.Sit.Value, false);
    }
}
