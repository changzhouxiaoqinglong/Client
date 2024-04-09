
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车上人物管理
/// </summary>

public class CarPlayerMgr : MonoBehaviour
{
    /// <summary>
    /// 车上人物
    /// </summary>
    private List<PlayerCtr> playerCtrs = new List<PlayerCtr>();

    /// <summary>
    /// 座位
    /// </summary>
    [Tooltip("座位")]
    public List<Transform> seatList;

    /// <summary>
    /// 下车点
    /// </summary>
    [Tooltip("下车点")]
    public List<Transform> outPosList;

    /// <summary>
    /// 车
    /// </summary>
    [HideInInspector]
    public CarBase Car
    {
        get; set;
    }

    [Tooltip("下车点范围")]
    public float OutPosCheckRange = 0.5f;

    [Tooltip("每个点尝试取点次数")]
    public int GetOutPosTime = 10;

    /// <summary>
    /// 更新车上参加训练的人
    /// </summary>
    public void UpdatePlayerDatas(List<TrainSeatVarData> trainSeatDatas)
    {
        foreach (var seatData in trainSeatDatas)
        {
            //是设备管理软件 不是人
            if (seatData.SeatId == SeatType.DEVICE)
            {
                continue;
            }
            PlayerCtr player = GetPlayerCtrBySeatId(seatData.SeatId);
            //还没有生成对应的人物
            if (player == null)
            {
                player = CreatePlayer(seatData);
            }
            player.TrainSeatData = seatData;
        }
    }

    public PlayerCtr GetPlayerCtrBySeatId(int seatId)
    {
        foreach (var player in playerCtrs)
        {
            if (player.TrainSeatData.SeatId == seatId)
            {
                return player;
            }
        }
        return null;
    }

    /// <summary>
    /// 获得车上人物 新版修改
    /// </summary>
    public List<PlayerCtr> GetPlayerCtr()
    {      
        return playerCtrs;
    }

    /// <summary>
    /// 生成人物
    /// </summary>
    private PlayerCtr CreatePlayer(TrainSeatVarData seatData)
    {
        if (seatData.SeatId == SeatType.DEVICE)
        {
            Logger.LogError("device seat  can not create player");
            return null;
        }
        GameObject playerPrefab = Resources.Load<GameObject>(AssetPath.SOLDIER);
        Transform seat = seatList[seatData.SeatId - 1];
        GameObject playerObj = Instantiate(playerPrefab, seat);
        PlayerCtr playerCtr = playerObj.GetComponent<PlayerCtr>();
        playerCtr.SetLocalPosition(Vector3.zero);
        playerCtr.TrainSeatData = seatData;
        playerCtr.SeatRoot = seat;
        playerCtr.Car = Car;
        playerCtrs.Add(playerCtr);
        this.InvokeByYield(() =>
        {
            //初始 是在车里
            playerCtr.InCar();
        }, new WaitForEndOfFrame());
        return playerCtr;
    }

    /// <summary>
    /// 下车
    /// </summary>
    public bool OutCar(int seatId)
    {
        PlayerCtr player = GetPlayerCtrBySeatId(seatId);
        if (player != null)
        {
            Vector3 outPos;
            if (GetRandomOutPos(player, out outPos))
            {
                //是自己的车  且是侦查员1要下车 就可以操作人物
                bool isControl = Car.IsSelfCar() && seatId == SeatType.INVEST1;
                player.OutCar(isControl, outPos);
                return true;
            }
            else
            {
                //没找到合适的下车点
                UIMgr.GetInstance().ShowToast("该位置无法下车，请换个位置重新尝试！");
            }
        }
        else
        {
            Logger.LogWarning("have no seat can not out car: " + seatId);
        }
        return false;
    }

    /// <summary>
    /// 获得下车位置
    /// </summary>
    private bool GetRandomOutPos(PlayerCtr playerCtr, out Vector3 pos)
    {
        //是否找到
        bool find = false;
        //位置对应的 下车点索引
        int posIndex = playerCtr.TrainSeatData.SeatId - 1;
        //先找对应的下车点
        if (GetOutPosByRoot(outPosList[posIndex], playerCtr, out pos))
        {
            find = true;
        }
        else
        {
            //对应的下车点找不到  就找其他位置的下车点下车
            for (int i = 0; i < outPosList.Count; i++)
            {
                //该位置在前面已经找过了
                if (i == posIndex)
                {
                    continue;
                }
                if (GetOutPosByRoot(outPosList[i], playerCtr, out pos))
                {
                    find = true;
                    break;
                }
            }
        }
        return find;
    }

    private bool GetOutPosByRoot(Transform outPosRoot, PlayerCtr playerCtr, out Vector3 pos)
    {
        pos = Vector3.zero;
        Vector3 rootPos = outPosRoot.position;
        //随机位置
        Vector3 randomPos;
        for (int i = 0; i < GetOutPosTime; i++)
        {
            //下车点 球形区域随机
            randomPos = rootPos + Random.insideUnitSphere * Random.Range(0, OutPosCheckRange);
            //该位置下车  不会有碰撞问题
            if (!playerCtr.IsColliderPoint(randomPos))
            {
                pos = randomPos;
                return true;
            }
        }
        //没找到合适的点
        return false;
    }

    /// <summary>
    /// 上车
    /// </summary>
    public void InCar(int seatId)
    {
        PlayerCtr player = GetPlayerCtrBySeatId(seatId);
        if (player != null)
        {
            player.InCar();
        }
        else
        {
            Logger.LogWarning("have no seat can not in car: " + seatId);
        }
    }

    /// <summary>
    /// 获得车里 人物的同步数据
    /// </summary>
    public List<PlayerSyncModel> GetPlayerSyncModels()
    {
        List<PlayerSyncModel> res = new List<PlayerSyncModel>();
        foreach (var item in playerCtrs)
        {
            res.Add(item.GetPlayerSyncModel());
        }
        return res;
    }

    /// <summary>
    /// 收到人物同步数据
    /// </summary>
    public void ReceivePlayerSyncModels(List<PlayerSyncModel> models)
    {
        foreach (var item in models)
        {
            PlayerCtr player = GetPlayerCtrBySeatId(item.SeatId);
            if (player)
            {
                player.ReceiveSyncModel(item);
            }
        }
    }
}
