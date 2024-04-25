
/// <summary>
/// 下发参加训练的 席位信息
/// </summary>

public class TrainSeatVarData
{
    /// <summary>
    /// 机号
    /// </summary>
    public int MachineId;

    /// <summary>
    /// 席位号
    /// </summary>
    public int SeatId;

    /// <summary>
    /// 该席位转发信息
    /// </summary>
    private ForwardModel forwardModel;
    public ForwardModel ForwardModel
    {
        get
        {
            if (forwardModel == null)
            {
                forwardModel = new ForwardModel()
                {
                    MachineId = this.MachineId,
                    SeatId = this.SeatId,
                };
            }
            return forwardModel;
        }
    }
}
