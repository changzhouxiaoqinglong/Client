public class SceneConstant
{
    /// <summary>
    /// 登录场景id
    /// </summary>
    public const int ID_LOGIN_SCENE = 1001;

    /// <summary>
    /// 基本训练场景id
    /// </summary>
    public const int ID_BASE_SCENE = 1002;

    /// <summary>
    /// 无3D场景的训练场景
    /// </summary>
    public const int ID_NO3D_SCENE = 1003;

    /// <summary>
    /// 训练结束场景
    /// </summary>
    public const int END_SCENE = 1004;

    /// <summary>
    /// 丘陵场景id
    /// </summary>
    public const int HILLS = 3;
}

/// <summary>
/// 场景表数据
/// </summary>
public class ExSceneData : ExDataBase
{
    /// <summary>
    /// 场景名
    /// </summary>
    public string SceneName;

    public string Desc;

    /// <summary>
    /// 海拔
    /// </summary>
    public float Altitude;
}
