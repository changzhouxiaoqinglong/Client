
using UnityEngine;

/// <summary>
/// 报站效果相机
/// </summary>
public class GrenadeCamera : MainCameraItemBase
{
    private Camera curCamera;

    private void Awake()
    {
        curCamera = GetComponent<Camera>();
    }

    public override Camera GetCamera()
    {
        return curCamera;
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public override void SetDisable()
    {
        base.SetDisable();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 启用
    /// </summary>
    public override void SetEnable()
    {
        gameObject.SetActive(true);
        base.SetEnable();
    }

    /// <summary>
    /// 不要记录相机 此相机只用于查看爆炸，之后会销毁
    /// </summary>
    public override bool NeedRecordLastMainCamera()
    {
        return false;
    }
}
