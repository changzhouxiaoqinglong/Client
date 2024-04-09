
using UnityEngine;

public enum PlayerAnimType
{
    /// <summary>
    /// 移动混合树
    /// </summary>
    BlendMove = 0,

    /// <summary>
    /// 坐下
    /// </summary>
    Sit,

    /// <summary>
    /// 下蹲
    /// </summary>
    Crouch,

    /// <summary>
    /// 起立
    /// </summary>
    StandUp,

    /// <summary>
    /// 扫描
    /// </summary>
    Scan
}

public class PlayerAnim : FSM
{
    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public PlayerCtr playerCtr;

    [HideInInspector]
    private ReceivePlayerSyncMsg receiveSyncMsg;

    public ReceivePlayerSyncMsg ReceiveSyncMsg
    {
        get
        {
            if (receiveSyncMsg == null)
            {
                receiveSyncMsg = GetComponent<ReceivePlayerSyncMsg>();
            }
            return receiveSyncMsg;
        }
    }
    #region 动画参数
    /// <summary>
    /// 速度
    /// </summary>
    public static AnimProperty Speed = new AnimProperty("Speed");

    /// <summary>
    /// 坐下
    /// </summary>
    public static AnimProperty Sit = new AnimProperty("Sit");

    /// <summary>
    /// 下蹲
    /// </summary>
    public static AnimProperty Crouch = new AnimProperty("Crouch");

    /// <summary>
    /// 起立
    /// </summary>
    public static AnimProperty StandUp = new AnimProperty("StandUp");

    /// <summary>
    /// 扫描
    /// </summary>
    public static AnimProperty Scan = new AnimProperty("Scan");


    #endregion

    protected override void Awake()
    {
        base.Awake();
        animator = transform.Find("playerModel").GetComponent<Animator>();
        playerCtr = GetComponent<PlayerCtr>();
        Init();
    }

    protected override void Start()
    {
        base.Start();
        //默认状态
        ChangeState((ushort)PlayerAnimType.BlendMove);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        PlayerAnimBlendMove blendMove = new PlayerAnimBlendMove(this, (ushort)PlayerAnimType.BlendMove, PlayerAnimType.BlendMove.ToString());
        AddState(blendMove);
        PlayerAnimSit sit = new PlayerAnimSit(this, (ushort)PlayerAnimType.Sit, PlayerAnimType.Sit.ToString());
        AddState(sit);
        PlayerAnimCrouch crouch = new PlayerAnimCrouch(this, (ushort)PlayerAnimType.Crouch, PlayerAnimType.Crouch.ToString());
        AddState(crouch);
        PlayerAnimStandUp standUp = new PlayerAnimStandUp(this, (ushort)PlayerAnimType.StandUp, PlayerAnimType.StandUp.ToString());
        AddState(standUp);
        PlayerAnimScan scan = new PlayerAnimScan(this, (ushort)PlayerAnimType.Scan, PlayerAnimType.Scan.ToString());
        AddState(scan);
    }

    public float GetFloat(int id)
    {
        return animator.GetFloat(id);
    }

    public float GetAnimatorTimer(string name)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if(clip.name.Equals(name))
            { return clip.length; }
        }
        return -1;
    }

    /// <summary>
    /// 获得动画同步数据
    /// </summary>
    public PlayerAnimSyncParam GetAnimSysParam()
    {
        return new PlayerAnimSyncParam()
        {
            AnimState = curState.stateId,
            Speed = GetFloat(Speed.Value),
        };
    }

    /// <summary>
    /// 接收到动画同步数据
    /// </summary>
    public void ReceiveAnimSyncModel(PlayerAnimSyncParam param)
    {
        //切换动画状态
        ChangeState(param.AnimState);
        animator.SetFloat(Speed.Value, param.Speed);
    }
}

public class PlayerAnimName
{
    /// <summary>
    /// 正常走路动画名字
    /// </summary>
    public const string BLENDMOVE = "BlendMove";

    /// <summary>
    /// 坐下动画名字
    /// </summary>
    public const string SIT = "Sit";

    /// <summary>
    /// 蹲下动画名字
    /// </summary>
    public const string CROUCH = "Crouch";

    /// <summary>
    /// 站立动画名字
    /// </summary>
    public const string STANDUP = "StandUp";

    /// <summary>
    /// 测量动画名字
    /// </summary>
    public const string SCAN = "Scan";
}
