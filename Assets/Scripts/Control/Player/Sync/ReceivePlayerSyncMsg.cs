
using UnityEngine;

/// <summary>
/// 接收人物同步数据
/// </summary>
public class ReceivePlayerSyncMsg : MonoBehaviour
{
    private PlayerCtr curPlayer;

    /// <summary>
    /// 同步位置和旋转逻辑
    /// </summary>
    private SyncPosRotate syncPosRotateLogic;

    PlayerAnim playerAnim;

    private void Awake()
    {
        syncPosRotateLogic = gameObject.AddComponent<SyncPosRotate>();
        curPlayer = GetComponent<PlayerCtr>();
        playerAnim = GetComponent<PlayerAnim>();
    }

    /// <summary>
    /// 接收人物同步消息
    /// </summary>
    public void OnReceivePlayerSyncModel(PlayerSyncModel model)
    {
        if (model.IsProtect)
        {
            //防护
            curPlayer.DoProtect();
        }
        else
        {
            curPlayer.UnDoProtect();
        }

        //状态变了 直接赋值位置旋转
        if (model.IsInCar != curPlayer.IsInCar)
        {
            if (model.IsInCar)
            {
                curPlayer.InCarState();
            }
            else
            {
                curPlayer.OutCarState();
            }
            curPlayer.SetPosition(model.Pos.ToVector3());
            curPlayer.SetRotation(model.Rotate.ToQuaternion());
            syncPosRotateLogic.syncData = null;
        }
        else
        {
            //位置旋转同步数据
            syncPosRotateLogic.syncData = new SyncPosRotateData()
            {
                Pos = model.Pos.ToVector3(),
                Rotate = model.Rotate.ToQuaternion(),
                startSyncTime = Time.time,
                startPos = transform.position,
                startRotate = transform.rotation,
            };
        }
        //动画同步
        playerAnim.ReceiveAnimSyncModel(model.AnimParam);
    }
}
