/// <summary>
/// 网络协议号
/// </summary>
public class NetProtocolCode
{
    #region tcp

    /// <summary>
    /// 心跳包
    /// </summary>
    public const int HEART_BEAT = 0;

    /// <summary>
    /// 初始数据
    /// </summary>
    public const int INIT_MSG = 1;

    /// <summary>
    /// 登录
    /// </summary>
    public const int LOGIN = 2;

    /// <summary>
    /// 下发任务环境
    /// </summary>
    public const int TASK_ENV = 4;

    /// <summary>
    /// 训练开始
    /// </summary>
    public const int TRAIN_START = 5;

    /// <summary>
    /// 操作毒剂报警器
    /// </summary>
    public const int POISON_ALARM_OP = 2011;

    /// <summary>
    /// 操作毒剂报警器训练流程  可控状态设置
    /// </summary>
    public const int POISON_ALARM_STAT_CTR = 2012;

    /// <summary>
    /// 毒剂报警器 进样情况
    /// </summary>
    public const int POISON_IN_STATUS = 7;

    /// <summary>
    /// 温湿度风向风速下发
    /// </summary>
    public const int METEOR_ENV = 104;

    /// <summary>
    /// 结束训练
    /// </summary>
    public const int END = 9;

    /// <summary>
    /// 北斗数据  经纬度 时间
    /// </summary>
    public const int BEIDOU_DATA = 103;

    /// <summary>
    /// 操作辐射仪
    /// </summary>
    public const int RADIOME_OP = 2021;

    /// <summary>
    /// 设置辐射剂量率阈值
    /// </summary>
    public const int SET_RADIOM_RATE_THRESHOLD = 2022;

    /// <summary>
    /// 设置辐射 累计剂量率阈值
    /// </summary>
    public const int SET_TT_RADIOM_RATE_THRESHOLD = 2023;

    /// <summary>
    /// 发送辐射剂量率
    /// </summary>
    public const int SEND_RADIOM_RATE = 101;

    /// <summary>
    /// 发送化学信息
    /// </summary>
    public const int SEND_DRUG_DATA = 102;

    /// <summary>
    /// 发送化学信息  三防毒报用
    /// </summary>
    public const int DEFENSE_SEND_DRUG_DATA = 105;

    /// <summary>
    /// 操作电源
    /// </summary>
    public const int POWER_OP = 2041;

    /// <summary>
    /// 操作电台
    /// </summary>
    public const int RadioStation_OP = 2071;

    /// <summary>
    /// 操作北斗
    /// </summary>
    public const int BEIDOU_OP = 2051;

    /// <summary>
    /// 操作气象器件
    /// </summary>
    public const int METEOR_OP = 2052;

    /// <summary>
    /// 插旗子 通知导控
    /// </summary>
    public const int FLAG = 17;

    /// <summary>
    /// 上报侦察结果
    /// </summary>
    public const int REPORT_DETECT_RES = 18;

    /// <summary>
    /// 操作车载侦毒器
    /// </summary>
    public const int OP_CAR_DETECT_POISON = 2032;

    /// <summary>
    /// 下车
    /// </summary>
    public const int OUT_CAR = 19;

    /// <summary>
    /// 下车结果
    /// </summary>
    public const int OUT_CAR_RES = 20;

    /// <summary>
    /// 上车
    /// </summary>
    public const int IN_CAR = 21;

    /// <summary>
    /// 上车结果
    /// </summary>
    public const int IN_CAR_RES = 22;

    /// <summary>
    /// 防护
    /// </summary>
    public const int PROTECT = 23;

    /// <summary>
    /// 开门
    /// </summary>
    public const int OpenDoor = 41;//新版修改

    /// <summary>
    /// 关门
    /// </summary>
    public const int CloseDoor = 42;//新版修改

    /// <summary>
    /// 插旗子 通知驾驶位 3D场景内插旗
    /// </summary>
    public const int FLAG_TO_DRIVER = 24;

    /// <summary>
    /// 设置抽气时间
    /// </summary>
    public const int SET_CAR_POIS_GAS_TIME = 2031;

    /// <summary>
    /// 成绩下发
    /// </summary>
    public const int GET_SCORE = 26;

    /// <summary>
    /// 车长命令
    /// </summary>
    public const int MASTER_INSTRUCT = 27;


    /// <summary>
    /// 请求答题
    /// </summary>
    public const int REQUEST_QUESTION = 28;

    /// <summary>
    /// 请求答题结果
    /// </summary>
    public const int RESULT_QUESTION = 29;

    /// <summary>
    /// 答题上报
    /// </summary>
    public const int QUESTION_REPORT = 30;


    /// <summary>
    /// 接近测量点 车速上报
    /// </summary>
    public const int CHECK_POINT_SPEED = 31;

    /// <summary>
    /// 把侦察结果给指定人
    /// </summary>
    public const int SEND_DETCT_RES_TO_SEAT = 32;

    /// <summary>
    /// 导控控制训练进程
    /// </summary>
    public const int GUIDE_PROCESS_CTR = 33;

    /// <summary>
    /// 车辆同步
    /// </summary>
    public const int CAR_SYNC = 34;

    /// <summary>
    /// 态势同步
    /// </summary>
    public const int SITUATION_SYNC = 35;

    /// <summary>
    /// 随机导调
    /// </summary>
    public const int RANDOM_GUIDE = 36;

    /// <summary>
    /// 侦查员下车测量
    /// </summary>
    public const int OUT_CAR_MEASURE = 37;

    #region 102车协议

    /// <summary>
    /// 操作车载辐射仪
    /// </summary>
    public const int CAR_RADIOM_OP_102 = 10211;

    /// <summary>
    /// 设置辐射剂量率阈值 102
    /// </summary>
    public const int SET_RADIOM_RATE_THRESHOLD_102 = 10212;

    /// <summary>
    /// 设置辐射累计剂量率阈值 102
    /// </summary>
    public const int SET_TT_RADIOM_RATE_THRESHOLD_102 = 10213;

    /// <summary>
    /// 三防装置毒报
    /// </summary>
    public const int POIS_ALARM_102 = 10221;

    /// <summary>
    /// 三防装置差压计
    /// </summary>
    public const int DIFF_PRESSURE_102 = 10222;

    /// <summary>
    /// 三防装置辐射仪
    /// </summary>
    public const int PREVENT_DEVICE_RADIOM_102 = 10224;

    /// <summary>
    /// 车载质谱仪
    /// </summary>
    public const int CAR_MASS_SPECT_102 = 10231;

    /// <summary>
    /// 车载质谱仪设置
    /// </summary>
    public const int CAR_MASS_SPECT_SET102 = 10232;

    /// <summary>
    /// 车载质谱仪压力
    /// </summary>
    public const int CAR_MASS_SPECT_PRESS_102 = 10233;

    /// <summary>
    /// 红外遥测模拟器
    /// </summary>
    public const int INFARED_TELEMETRY_102 = 10241;

    /// <summary>
    /// 红外遥测模拟器 参数
    /// </summary>
    public const int INFARED_TELEMETRY_PARAM_102 = 10242;

    /// <summary>
    /// 红外遥测模拟器毒剂信息
    /// </summary>
    public const int INFARED_TELEMETRY_DRUG_DATA_102 = 10243;

    /// <summary>
    /// 电源模拟器
    /// </summary>
    public const int POWER_102 = 10251;

    /// <summary>
    /// 操作电台
    /// </summary>
    public const int RadioStation_OP_102 = 10271;

    /// <summary>
    /// 气象器件
    /// </summary>
    public const int METEOR_102 = 10261;

    /// <summary>
    /// 车内温湿度
    /// </summary>
    public const int CAR_WET_TEM = 10262;
    #endregion

    #region 384车协议
    /// <summary>
    /// 操作DFH辐射仪
    /// </summary>
    public const int RADIOME_OP_384 = 38411;

    /// <summary>
    /// 设置辐射剂量率阈值
    /// </summary>
    public const int SET_RADIOM_RATE_THRESHOLD_384 = 38412;

    /// <summary>
    /// 设置辐射 累计剂量率阈值
    /// </summary>
    public const int SET_TT_RADIOM_RATE_THRESHOLD_384 = 38413;

    /// <summary>
    /// 操作毒剂报警器384
    /// </summary>
    public const int POISON_ALARM_OP_384 = 38421;

    /// <summary>
    /// 车载毒报选择工作模式
    /// </summary>
    public const int POISON_ALARM_WORK_TYPE_384 = 38422;

    /// <summary>
    /// 操作电源
    /// </summary>
    public const int POWER_OP_384 = 38431;

   

    /// <summary>
    /// 操作电台
    /// </summary>
    public const int RadioStation_OP_384 = 38471;
    #endregion

    #region 106车协议
    /// <summary>
    /// 操作电源106
    /// </summary>
    public const int POWER_OP_106 = 10641;

    /// <summary>
    /// 操作DFH辐射仪
    /// </summary>
    public const int RADIOME_OP_106 = 10621;

    /// <summary>
    /// 设置辐射剂量率阈值
    /// </summary>
    public const int SET_RADIOM_RATE_THRESHOLD_106 = 10622;

    /// <summary>
    /// 设置辐射 累计剂量率阈值
    /// </summary>
    public const int SET_TT_RADIOM_RATE_THRESHOLD_106 = 10623;

    public const int POISON_ALARM_OP_106 = 10611;

    public const int Biology_OP_106 = 10631;

    /// <summary>
    /// 106毒剂报警器训练流程  可控状态设置
    /// </summary>
    public const int POISON_ALARM_STAT_CTR_106= 10612;

    /// <summary>
    /// 设置生物模拟器数据
    /// </summary>
    public const int SET_Biology_RATE_THRESHOLD_106 = 10632;

    /// <summary>
    /// 操作电台
    /// </summary>
    public const int RadioStation_OP_106 = 10671;

    /// <summary>
    /// 设置减压阀数据
    /// </summary>
    public const int SET_SetReliefThreshold = 10681;
    #endregion

    #endregion
}
