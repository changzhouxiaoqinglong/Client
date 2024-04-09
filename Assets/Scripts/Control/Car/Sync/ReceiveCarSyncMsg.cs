
using UnityEngine;


/// <summary>
/// 接收车辆同步消息
/// </summary>
public class ReceiveCarSyncMsg : MonoBehaviour
{
    private CarBase car;

    /// <summary>
    /// 是否是首次同步位置(首次同步不用插值，直接设置位置旋转)
    /// </summary>
    private bool firstSyncPos = true;

    /// <summary>
    /// 同步位置和旋转逻辑
    /// </summary>
    private SyncPosRotate syncPosRotateLogic;

    private void Awake()
    {
        car = GetComponent<CarBase>();
        NetManager.GetInstance().AddNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_SYNC, OnGetCarSyncMsg);
        syncPosRotateLogic = gameObject.AddComponent<SyncPosRotate>();
    }

    private void OnGetCarSyncMsg(IEventParam param)
    {
        if (param is TcpReceiveEvParam tcpReceiveEvParam)
        {
            if (tcpReceiveEvParam.netData.MachineId == car.MachineId)
            {
                CarSyncModel model = JsonTool.ToObject<CarSyncModel>(tcpReceiveEvParam.netData.Msg);
                #region 同步车
                if (firstSyncPos)
                {
                    firstSyncPos = false;
                    //首次同步 直接赋值
                    transform.position = model.Pos.ToVector3();
                    transform.rotation = model.Rotate.ToQuaternion();
                    syncPosRotateLogic.syncData = null;
                }
                else
                {
                    //需要同步 才同步
                    if (model.Pos.ToVector3() != transform.position || model.Rotate.ToQuaternion() != transform.rotation)
                    {
                        syncPosRotateLogic.syncData = new SyncPosRotateData()
                        {
                            Pos = model.Pos.ToVector3(),
                            Rotate = model.Rotate.ToQuaternion(),
                            startSyncTime = Time.time,
                            startPos = transform.position,
                            startRotate = transform.rotation,
                        };
                    }
                }
                #endregion

                #region 同步人
                car.playerMgr.ReceivePlayerSyncModels(model.PlayerSyncDatas);
                #endregion
            }
        }
    }

    private void OnDestroy()
    {
        NetManager.GetInstance().RemoveNetMsgEventListener(ServerType.GuideServer, NetProtocolCode.CAR_SYNC, OnGetCarSyncMsg);
    }
}
