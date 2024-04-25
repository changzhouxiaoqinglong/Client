
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下发参与训练的车人数据
/// </summary>

public class TrainMachineVarData
{
    /// <summary>
    /// 机号
    /// </summary>
    public int MachineId;

    /// <summary>
    /// 车型
    /// </summary>
    public int CarId;

    /// <summary>
    /// 车辆初始位置
    /// </summary>
    public CustVect3 InitPos;

    /// <summary>
    /// 车辆旋转
    /// </summary>
    public string Rotate;

    /// <summary>
    /// 该车参加训练的人的数据
    /// </summary>
    public List<TrainSeatVarData> TrainUserDatas;
    //TrainSeatDatas


    public bool IsSelf()
    {
        return MachineId == AppConfig.MACHINE_ID;
    }
}
