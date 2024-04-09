
using UnityEngine;

/// <summary>
/// 同步数据
/// </summary>
public class SyncPosRotateData
{
    /// <summary>
    /// 目标位置
    /// </summary>
    public Vector3 Pos;

    /// <summary>
    /// 目标旋转
    /// </summary>
    public Quaternion Rotate;

    /// <summary>
    /// 开始同步时间
    /// </summary>
    public float startSyncTime = 0;

    /// <summary>
    /// 开始同步时的位置
    /// </summary>
    public Vector3 startPos;

    /// <summary>
    /// 开始同步时的旋转
    /// </summary>
    public Quaternion startRotate;
}


/// <summary>
/// 同步位置旋转逻辑
/// </summary>
public class SyncPosRotate : MonoBehaviour
{
    /// <summary>
    /// 同步时间间隔
    /// </summary>
    private float syncOffTime = 0;

    /// <summary>
    /// 同步数据
    /// </summary>
    public SyncPosRotateData syncData = null;

    private void Awake()
    {
        syncOffTime = 1.0f / NetConfig.SYNC_SECOND_TIMES;
    }

    private void FixedUpdate()
    {
        //同步
        if (syncData != null)
        {
            float offTime = Time.time - syncData.startSyncTime;
            if (offTime < syncOffTime)
            {
                float ratio = offTime / syncOffTime;
                ratio = Mathf.Clamp01(ratio);
                transform.position = Vector3.Lerp(syncData.startPos, syncData.Pos, ratio);
                transform.rotation = Quaternion.Lerp(syncData.startRotate, syncData.Rotate, ratio);
            }
            else
            {
                transform.position = syncData.Pos;
                transform.rotation = syncData.Rotate;
            }
        }
    }
}
