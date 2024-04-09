
using UnityEngine;

/// <summary>
/// 无人机 相机
/// </summary>
public class UavFollowCamera : MainCameraItemBase
{
    private Camera curCamera;

    /// <summary>
    /// 目标
    /// </summary>
    public Transform lookTarget;

    private void Awake()
    {
        curCamera = GetComponent<Camera>();
    }

    public override Camera GetCamera()
    {
        return curCamera;
    }

    private void Update()
    {
        transform.LookAt(lookTarget);
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

    /// 不要记录相机 此相机只用于查看飞机，之后会销毁
    public override bool NeedRecordLastMainCamera()
    {
        return false;
    }
}
