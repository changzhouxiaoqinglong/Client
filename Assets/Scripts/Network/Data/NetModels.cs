
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 通用数据传输模板
/// </summary>
public class NetData
{
    /// <summary>
    /// 协议号
    /// </summary>
    public int ProtocolCode;

    /// <summary>
    /// 机号
    /// </summary>
    public int MachineId;

    /// <summary>
    /// 席位号
    /// </summary>
    public int SeatId;

    /// <summary>
    /// 车型
    /// </summary>
    public int CarType;

    /// <summary>
    /// 消息转发
    /// </summary>
    public List<ForwardModel> Forwards = new List<ForwardModel>();

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Msg;

    public NetData(int ProtocolCode, string Msg)
    {
        this.ProtocolCode = ProtocolCode;
        this.Msg = Msg;
        MachineId = AppConfig.MACHINE_ID;
        SeatId = AppConfig.SEAT_ID;
        CarType = AppConfig.CAR_ID;
    }
}

/// <summary>
/// 转发
/// </summary>
public class ForwardModel
{
    /// <summary>
    /// 转发的机号
    /// </summary>
    public int MachineId;

    /// <summary>
    /// 转发 的席位号
    /// </summary>
    public int SeatId;
}

/// <summary>
/// 转发数据组构建
/// </summary>
public class ForwardModelsBuilder
{
    private List<ForwardModel> models = new List<ForwardModel>();

    public ForwardModelsBuilder Append(int machineId, int seatId)
    {
        models.Add(new ForwardModel()
        {
            MachineId = machineId,
            SeatId = seatId,
        });
        return this;
    }

    public List<ForwardModel> Build()
    {
        return models;
    }
}

/// <summary>
/// 客户端初始数据
/// </summary>
public class InitModel
{
    /// <summary>
    /// 软件类型 1.三维软件 2.设别管理软件
    /// </summary>
    public int EquipType = 1;
}

public class LoginModel
{
    public string UserName;

    public string Password;

    public int CarId;
}

public class LoginRes : ResBase
{
    public string UserName;
}


/// <summary>
/// 任务日志接口
/// </summary>
public interface ITaskLogModel
{
    string GetTaskLog(int seatId);
}

/// <summary>
/// 设备操作
/// </summary>
public class OperateDevice
{
    /// <summary>
    /// 关闭
    /// </summary>
    public const int CLOSE = 0;

    /// <summary>
    /// 打开
    /// </summary>
    public const int OPEN = 1;
}

#region 操作毒剂报警器
/// <summary>
/// 毒剂报警器操作类型
/// </summary>
public class PoisonAlarmOpType
{
    /// <summary>
    /// 进气帽
    /// </summary>
    public const int Intake = 1;

    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 2;

    /// <summary>
    /// 进样
    /// </summary>
    public const int JinYang = 3;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 4;

    /// <summary>
    /// 上电
    /// </summary>
    public const int EleOn = 5;

    /// <summary>
    /// 自检
    /// </summary>
    public const int Check = 6;
}

/// <summary>
/// 操作毒剂报警器
/// </summary>
public class PoisonAlarmOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PoisonAlarmOpType.Intake:
                return "毒剂报警器 进气帽：" + (Operate == OperateDevice.OPEN ? "打开" : "关闭");
            case PoisonAlarmOpType.OpenClose:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case PoisonAlarmOpType.JinYang:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开始进样" : "停止进样");
            case PoisonAlarmOpType.Alarm:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            case PoisonAlarmOpType.EleOn:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case PoisonAlarmOpType.Check:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "自检开始" : "自检结束");
            default:
                return "";
        }
    }
}

/// <summary>
/// 可控状态
/// </summary>
public class PoisonAlarmStat
{
    /// <summary>
    /// 开机
    /// </summary>
    public const int OPEN_CTR = 1;

    /// <summary>
    /// 进样
    /// </summary>
    public const int JINYANG_CTR = 2;

    /// <summary>
    /// 关机
    /// </summary>
    public const int CLOSE_CTR = 3;
}

/// <summary>
/// 毒剂报警器可控状态设置
/// </summary>
public class PoisonAlarmStatCtr02BModel
{
    public int Operate;
}
#endregion

#region 02b操作辐射仪

/// <summary>
/// 辐射仪操作类型
/// </summary>
public class RadiomOpType
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RateAlarm = 2;

    /// <summary>
    /// 累计剂量率报警
    /// </summary>
    public const int TotalRateAlarm = 3;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 4;

    /// <summary>
    /// 自检
    /// </summary>
    public const int Check = 5;
}

/// <summary>
/// 操作辐射仪
/// </summary>
public class RadiomeOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadiomOpType.OpenClose:
                return "辐射仪：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case RadiomOpType.RateAlarm:
                return "辐射仪：" + (Operate == OperateDevice.OPEN ? "剂量率报警" : "停止剂量率报警");
            case RadiomOpType.TotalRateAlarm:
                return "辐射仪：" + (Operate == OperateDevice.OPEN ? "累积剂量报警" : "停止累积剂量报警");
            case RadiomOpType.Elec:
                return "辐射仪：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case RadiomOpType.Check:
                return "辐射仪：" + (Operate == OperateDevice.OPEN ? "自检开始" : "自检结束");
            default:
                return "";
        }
    }
}
#endregion

#region 操作电源

/// <summary>
/// 电源操作类型
/// </summary>
public class PowerOpType
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 2;

    /// <summary>
    /// 输出
    /// </summary>
    public const int OutPut = 3;
}

/// <summary>
/// 操作电源
/// </summary>
public class PowerOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PowerOpType.OpenClose:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开" : "关");
            case PowerOpType.Elec:
                return "电源：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case PowerOpType.OutPut:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开启输出" : "关闭输出");
            default:
                return "";
        }
    }
}




#endregion

#region 操作北斗

/// <summary>
/// 北斗操作类型
/// </summary>
public class BeiDouOpType
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 2;
}

/// <summary>
/// 操作北斗
/// </summary>
public class BeiDouOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case BeiDouOpType.OpenClose:
                return "北斗：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case BeiDouOpType.Elec:
                return "北斗：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            default:
                return "";
        }
    }
}
#endregion

#region 操作气象器件
public class MeteorOpType
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 2;
}

/// <summary>
/// 操作气象器件
/// </summary>
public class MeteorOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case MeteorOpType.OpenClose:
                return "气象：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case MeteorOpType.Elec:
                return "气象：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            default:
                return "";
        }
    }
}
#endregion

#region 车载侦毒器
public class CarDetectPoisonOpType
{
    /// <summary>
    /// 泵开关
    /// </summary>
    public const int Pump = 2;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 3;

    /// <summary>
    /// 加热开关
    /// </summary>
    public const int Heat = 1;
}

/// <summary>
/// 操作车载侦毒器
/// </summary>
public class CarDetectPoisonOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case CarDetectPoisonOpType.Pump:
                return "车载侦毒器：" + (Operate == OperateDevice.OPEN ? "泵打开" : "泵关闭");
            case CarDetectPoisonOpType.Elec:
                return "车载侦毒器：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case CarDetectPoisonOpType.Heat:
                return "车载侦毒器：" + (Operate == OperateDevice.OPEN ? "加热开始" : "加热结束");
            default:
                return "";
        }
    }
}

/// <summary>
/// 设置车载侦毒器抽气时间
/// </summary>
public class SetCarPoisonGasTime
{
    /// <summary>
    /// 抽气时间
    /// </summary>
    public float Time;
}

#endregion

#region 电台
public class RadioStationOpType
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;
}


/// <summary>
/// 操作电台
/// </summary>
public class RadioStationOpModel : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadioStationOpType.OpenClose:
                return "电台：" + (Operate == OperateDevice.OPEN ? "开" : "关");

            default:
                return "";
        }
    }
}
#endregion

public class PoinsonInStatus
{
    /// <summary>
    /// 不足
    /// </summary>
    public const int NOT_ENOUGH = 0;

    /// <summary>
    /// 充足
    /// </summary>
    public const int ENOUGH = 1;

}

/// <summary>
/// 毒剂报警器 进样情况
/// </summary>
public class PoisonInStatusModel : ITaskLogModel
{
    /// <summary>
    /// 状态 0不足  1充足
    /// </summary>
    public int Status;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Status)
        {
            case PoinsonInStatus.NOT_ENOUGH:
                return "毒剂报警器 进样不足";
            case PoinsonInStatus.ENOUGH:
                return "毒剂报警器 进样充足";
            default:
                return "";
        }
    }
}

/// <summary>
/// 结束训练
/// </summary>
public class EndModel
{
    /// <summary>
    /// 任务id
    /// </summary>
    public int TaskId;

    public EndModel(int taskId)
    {
        TaskId = taskId;
    }
}

/// <summary>
/// 设置剂量率
/// </summary>
public class SetDoseRateModel
{
    /// <summary>
    /// 剂量率
    /// </summary>
    public float DoseRate;
}

/// <summary>
/// 通知驾驶员插旗
/// </summary>
public class FlagToDriveModel
{
    /// <summary>
    /// 旗子类型
    /// </summary>
    public int FlagType;

    /// <summary>
    /// 信息
    /// </summary>
    public string Info;
}

/// <summary>
/// 插旗
/// </summary>
public class FlagModel
{
    /// <summary>
    /// 旗子类型
    /// </summary>
    public int FlagType;

    /// <summary>
    /// 插旗位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 旋转信息
    /// </summary>
    public CustVect3 Rotate;

    /// <summary>
    /// 经度
    /// </summary>
    public float Longicude;
    /// <summary>
    /// 纬度
    /// </summary>
    public float Latitude;
}

/// <summary>
/// 开始训练 数据
/// </summary>
public class TrainStartModel
{
    /// <summary>
    /// 任务唯一id  由导控生成
    /// </summary>
    public string TrainID;

    /// <summary>
    /// 科目描述(导控用)
    /// </summary>
    public string SubjectDes;

    public List<TrainMachineVarData> TrainMachineDatas;
}

/// <summary>
/// 上报侦察结果
/// </summary>
public class DetectResModel : ITaskLogModel
{
    public string Result;

    public string GetTaskLog(int seatId)
    {
        switch (seatId)
        {
            case SeatType.DRIVE:
                return "驾驶员：上报侦察结果";
            case SeatType.MASTER:
                return "车长：上报侦察结果";
            case SeatType.INVEST1:
                return "1号侦察员：上报侦察结果";
            case SeatType.INVEST2:
                return "2号侦察员：上报侦察结果";
            default:
                return "";
        }
    }
}

/// <summary>
/// 获得成绩
/// </summary>
public class GetScoreModel
{
    /// <summary>
    /// 得分
    /// </summary>
    public float Score;

    /// <summary>
    /// 扣分项
    /// </summary>
    public List<DeductItem> DeductItems;

    /// <summary>
    /// 获得成绩描述
    /// </summary>
    public string GetDesc()
    {
        StringBuilder sb = "得分：".AppendLine(Score.ToString())
            .AppendLine("扣分项：");
        foreach (var item in DeductItems)
        {
            sb.Append(item.Desc).Append("   ").Append(item.DeductScore).Append("\n");
        }
        return sb.ToString();
    }
}

/// <summary>
/// 扣分项
/// </summary>
public class DeductItem
{
    /// <summary>
    /// 扣分项描述
    /// </summary>
    public string Desc;

    /// <summary>
    /// 扣分数
    /// </summary>
    public string DeductScore;
}

/// <summary>
/// 车长命令
/// </summary>
public class MasterInstructModel : ITaskLogModel
{
    /// <summary>
    /// 命令编号
    /// </summary>
    public int Id;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        ExInstructData exInstructData = ExInstructDataMgr.GetInstance().GetDataById(Id);
        string res = $"<color={ ColorConstant.RED_VALUE }>收到车长指令：{exInstructData.Name}</color>";
        return res;
    }
}

/// <summary>
/// 气象信息上报数据
/// </summary>
public class MeteorEnvModel
{
    /// <summary>
    /// 温度
    /// </summary>
    public float Temperate;

    /// <summary>
    /// 湿度
    /// </summary>
    public float Humidity;

    /// <summary>
    /// 风向
    /// </summary>
    public float WinDir;

    /// <summary>
    /// 风速
    /// </summary>
    public float WinSp;
}

/// <summary>
/// 北斗信息上报
/// </summary>
public class BeiDouModel
{
    /// <summary>
    /// 经度
    /// </summary>
    public float Longicude;

    /// <summary>
    /// 纬度
    /// </summary>
    public float Latitude;

    /// <summary>
    /// 高程
    /// </summary>
    public float Elevation;

    /// <summary>
    /// 时间
    /// </summary>
    public string Date;
}

/// <summary>
/// 接近测量点车速上报
/// </summary>
public class CheckSpeedModel
{
    public float CurSpeed;
}

/// <summary>
/// 请求答题
/// </summary>
public class QstRequest
{
    /// <summary>
    /// 题目触发类型
    /// </summary>
    public int TriggerType;


    /// <summary>
    /// 指定答题人员
    /// </summary>
    public int SeatId;
}


/// <summary>
/// 请求答题结果
/// </summary>
public class QstRequestResult
{
    /// <summary>
    /// 是否可以答题
    /// </summary>
    public bool IsOk;

    /// <summary>
    /// 不能答题时的提示
    /// </summary>
    public string Tip;

    /// <summary>
    /// 触发类型
    /// </summary>
    public int TriggerType;

    /// <summary>
    /// 题目答案配置表目标Id
    /// </summary>
    public int TargetId;

    /// <summary>
    /// 附加参数
    /// </summary>
    public string Param;

}

public class QstPoisonDrugType
{
    /// <summary>
    /// 车内侦毒
    /// </summary>
    public const int IN_CAR_DRUG = 1;

    /// <summary>
    /// 下车侦毒
    /// </summary>
    public const int OUT_CAR_DRUG = 2;
}

public class QstPoisonCheckType
{
    /// <summary>
    /// 地面检测
    /// </summary>
    public const int GROUND_CHECK_TYPE = 1;

    /// <summary>
    /// 空气检测
    /// </summary>
    public const int AIR_CHECK_TYPE = 2;


    public static string GetQstPoisonCheckType(int type)
    {
        switch (type)
        {
            case GROUND_CHECK_TYPE:
                return "弹坑";
            case AIR_CHECK_TYPE:
                return "空气";
            default:
                return "";
        }
    }
}


public class QstPoisonParam
{
    /// <summary>
    /// 检测类型(地面1，空气2)
    /// </summary>
    public int CheckType;

    /// <summary>
    /// 程度(低1，中2，高3）
    /// </summary>
    public int DegreeLow;

    /// <summary>
    /// 坐标
    /// </summary>
    public CustVect2 pos;

    /// <summary>
    /// 侦毒类型（车上侦1,下车侦2）
    /// </summary>
    public int DrugType;
}

public class QstReport : ITaskLogModel
{
    /// <summary>
    /// 题目id
    /// </summary>
    public int QuestionId;

    /// <summary>
    /// 是否正确
    /// </summary>
    public bool IsCorrect;

    public string GetTaskLog(int seatId)
    {
        string title = ExQuestionDataMgr.GetInstance().GetQuestionItem(QuestionId).Title;
        return title + ":" + (IsCorrect ? "正确" : "错误");
    }
}

/// <summary>
/// 导控训练进程
/// </summary>
public enum GuideProcessType
{
    Pause = 1,
    Continue,
    End,
}

/// <summary>
/// 导控控制训练进程
/// </summary>
public class GuideProcessCtrModel
{
    public GuideProcessType Operate;
}

/// <summary>
/// 毒 浓度程度
/// </summary>
public class DrugDegree
{
    /// <summary>
    /// 无
    /// </summary>
    public const int NONE = 0;

    /// <summary>
    /// 低
    /// </summary>
    public const int LOW = 1;

    /// <summary>
    /// 中
    /// </summary>
    public const int MIDDLE = 2;

    /// <summary>
    /// 高
    /// </summary>
    public const int HIGH = 3;

    public static string GetDesc(int degree)
    {
        switch (degree)
        {
            case NONE:
                return "无";
            case LOW:
                return "低";
            case MIDDLE:
                return "中";
            case HIGH:
                return "高";
            default:
                return string.Empty;
        }
    }
}

public class DrugDType
{
    /// <summary>
    /// 无
    /// </summary>
    public const int NONE = 0;

    /// <summary>
    /// 一类
    /// </summary>
    public const int DTYPE1 = 1;

    /// <summary>
    /// 一类
    /// </summary>
    public const int DTYPE2 = 2;

    public static string GetDesc(int type)
    {
        switch (type)
        {
            case NONE:
                return "无毒";
            case DTYPE1:
                return "一类";
            case DTYPE2:
                return "二类";
            default:
                return string.Empty;
        }
    }
}

/// <summary>
/// 毒来源
/// </summary>
public class PoisonOrigin
{
    /// <summary>
    /// 空气
    /// </summary>
    public const int AIR = 1;

    /// <summary>
    /// 弹坑
    /// </summary>
    public const int CRATER = 2;
}

/// <summary>
/// 实时上报化学信息
/// </summary>
public class ReportDrugDataModel
{
    public int Id;

    /// <summary>
    /// 毒剂类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 浓度
    /// </summary>
    public float Dentity;

    /// <summary>
    /// 浓度 程度  低中高
    /// </summary>
    public int Degree = DrugDegree.NONE;

    /// <summary>
    /// 毒类  一类 二类
    /// </summary>
    public int DType = DrugDType.NONE;
}

/// <summary>
/// 实时上报化学信息(三防毒报)
/// </summary>
public class DefenseReportDrugDataModel
{
    /// <summary>
    /// 毒剂类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 浓度
    /// </summary>
    public float Dentity;
}

/// <summary>
/// 随机导调 类型
/// </summary>
public class RandomGuideType
{
    /// <summary>
    /// 暴雨
    /// </summary>
    public const int RAIN_STORM = 1;

    /// <summary>
    /// 敌火袭击
    /// </summary>
    public const int ENEMY_ATTACK = 2;

    /// <summary>
    /// 敌空中侦察
    /// </summary>
    public const int ENEMY_AIR_DETECT = 3;
}

/// <summary>
/// 随机导调
/// </summary>
public class RandomGuideModel
{
    /// <summary>
    /// 类型
    /// </summary>
    public int Type;
}

/// <summary>
/// 设置辐射仪剂量率阈值 02b
/// </summary>
public class SetRadiomThreHold02b : ITaskLogModel
{
    public float DoseThreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "辐射仪：当前剂量率阈值为 " + DoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

/// <summary>
/// 设置辐射仪累计剂量率阈值 02b
/// </summary>
public class SetTotalRadiomThreHold02b : ITaskLogModel
{
    public float TotalDoseThreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "辐射仪：当前累积剂量阈值为 " + TotalDoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

/// <summary>
/// 防护
/// </summary>
public class ProtectModel : ITaskLogModel
{
    public bool IsProtect;
    public string GetTaskLog(int seatId)
    {
        switch (seatId)
        {
            case SeatType.DRIVE:
                return IsProtect ? "驾驶员：防护" : "驾驶员：解除防护";
            case SeatType.MASTER:
                return IsProtect ? "车长：防护" : "车长：解除防护";
            case SeatType.INVEST1:
                return IsProtect ? "1号侦察员：防护" : "1号侦察员：解除防护";
            case SeatType.INVEST2:
                return IsProtect ? "2号侦察员：防护" : "2号侦察员：解除防护";
            default:
                return "";
        }
    }
}

/// <summary>
/// 车辆同步
/// </summary>
public class CarSyncModel
{
    /// <summary>
    /// 车位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 车旋转
    /// </summary>
    public CustVect3 Rotate;

    /// <summary>
    /// 人同步数据
    /// </summary>
    public List<PlayerSyncModel> PlayerSyncDatas;
}

/// <summary>
/// 人同步
/// </summary>
public class PlayerSyncModel
{
    /// <summary>
    /// 席位号
    /// </summary>
    public int SeatId;

    /// <summary>
    /// 防护
    /// </summary>
    public bool IsProtect;

    /// <summary>
    /// 是否在车里
    /// </summary>
    public bool IsInCar;

    /// <summary>
    /// 位置
    /// </summary>
    public CustVect3 Pos;

    /// <summary>
    /// 旋转
    /// </summary>
    public CustVect3 Rotate;

    /// <summary>
    /// 动画参数
    /// </summary>
    public PlayerAnimSyncParam AnimParam;
}

/// <summary>
/// 人物动画同步参数
/// </summary>
public class PlayerAnimSyncParam
{
    /// <summary>
    /// 当前动画状态
    /// </summary>
    public ushort AnimState;

    //速度参数值
    public float Speed;
}

/// <summary>
/// 态势同步
/// </summary>
public class SituationSyncModel
{
    /// <summary>
    /// 同步类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 训练id
    /// </summary>
    public string ExerciseId;

    /// <summary>
    /// 本机时间
    /// </summary>
    public string SysTemTime;

    /// <summary>
    /// 作战时间
    /// </summary>
    public string SimulateTime;

    /// <summary>
    /// 机号
    /// </summary>
    public int MachineNumber = AppConfig.MACHINE_ID;

    /// <summary>
    /// 位置
    /// </summary>
    public List<CustVect3> PosList = new List<CustVect3>();

    /// <summary>
    /// 标志旗类型
    /// </summary>
    public int SignType;

    /// <summary>
    /// 旗子信息
    /// </summary>
    public string FlagInfo;

    /// <summary>
    /// 毒区或辐射区域的编号
    /// </summary>
    public int EnvironId;
}

public class QstDrugPoisonLog
{
    /// <summary>
    /// 经纬度坐标
    /// </summary>
    public CustVect2 GisPos;

    /// <summary>
    /// 侦测类型(1.地面，2.空气)
    /// </summary>
    public int CheckType;

    /// <summary>
    /// 管子数量
    /// </summary>
    public int TubeCount;

    /// <summary>
    /// 毒剂类型
    /// </summary>
    public int PoisonType;

    /// <summary>
    /// 用户选择的毒剂类型
    /// </summary>
    public int UserSelectPoison;

    /// <summary>
    /// 上报字符串
    /// </summary>
    /// <returns></returns>
    public string ReportQstLogStr(int checkType, int poisonType, int selectPoisonType)
    {
        string checkTypeStr = QstPoisonCheckType.GetQstPoisonCheckType(checkType);
        string poisonTypeStr = HarmType.GetTypeStr(poisonType);
        string selectPoisonTypeStr = QstPoisonType.GetPoisonType(selectPoisonType);
        return "位置:" + GisPos + "，发现染毒" + checkTypeStr + "，用管" + TubeCount
            + "根，用户选择毒剂类型为:" + selectPoisonTypeStr + "，" + checkTypeStr + "毒剂类型为：" + poisonTypeStr + "。";
    }
}


#region 102车载辐射仪
/// <summary>
/// 102剂量率报警 特殊操作Operate
/// </summary>
public class RadiomOpAlarmOperate102
{
    /// <summary>
    /// 都不报警
    /// </summary>
    public const int NONE = 0;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RADIOM_ALARM = 1;

    /// <summary>
    /// 累计剂量率报警
    /// </summary>
    public const int TT_RADIOM_ALARM = 2;

    /// <summary>
    /// 都报警
    /// </summary>
    public const int BOTH = 3;
}

/// <summary>
/// 辐射仪操作类型
/// </summary>
public class RadiomOpType102
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RateAlarm = 2;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 4;
}

/// <summary>
/// 操作辐射仪102
/// </summary>
public class RadiomeOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadiomOpType102.OpenClose:
                return "车载辐射仪：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case RadiomOpType102.Elec:
                return "车载辐射仪：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            default:
                return "";
        }
    }
}

#endregion

#region 102三防毒报
/// <summary>
/// 操作类型
/// </summary>
public class PoisAlarmOpType102
{
    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 1;

    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 2;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 3;
}

/// <summary>
/// 操作三防装置毒报
/// </summary>
public class PoisAlarm102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PoisAlarmOpType102.Elec:
                return "三防毒报装置：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case PoisAlarmOpType102.OpenClose:
                switch (Operate)
                {
                    case 1:
                        return "三防毒报装置：开机";
                    case 2:
                        return "三防毒报装置：关机";
                    case 3:
                        return "三防毒报装置：自检";
                    default:
                        return "";
                }
            case PoisAlarmOpType102.Alarm:
                return "三防毒报装置：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            default:
                return "";
        }
    }
}
#endregion

#region 102三防差压计
/// <summary>
/// 操作类型
/// </summary>
public class DiffPressureOpType102
{
    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 1;

    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 2;

    /// <summary>
    /// 差压舱门
    /// </summary>
    public const int Gate = 3;
}

/// <summary>
/// 操作三防装置差压计
/// </summary>
public class DiffPressureOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case DiffPressureOpType102.Elec:
                return "三防差压计：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case DiffPressureOpType102.OpenClose:
                return "三防差压计：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case DiffPressureOpType102.Gate:
                return "三防差压计：" + (Operate == OperateDevice.OPEN ? "差压舱门开启" : "差压舱门关闭");
            default:
                return "";
        }
    }
}
#endregion

#region 102三防辐射仪
/// <summary>
/// 操作类型
/// </summary>
public class Prre3RadiomOpType102
{
    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 1;

    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 2;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 3;
}

/// <summary>
/// 操作三防装置辐射仪
/// </summary>
public class PreRadiomOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case Prre3RadiomOpType102.Elec:
                return "三防辐射仪器：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case Prre3RadiomOpType102.OpenClose:
                return "三防辐射仪器：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case Prre3RadiomOpType102.Alarm:
                return "三防辐射仪器：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            default:
                return "";
        }
    }
}
#endregion


#region 102车载质谱仪
/// <summary>
/// 操作类型
/// </summary>
public class CarMasssSpectOpType102
{
    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 1;

    /// <summary>
    /// 氮气瓶总阀
    /// </summary>
    public const int NitrogenTap = 2;

    /// <summary>
    /// 电源
    /// </summary>
    public const int Power = 3;

    /// <summary>
    /// ZPY软件
    /// </summary>
    public const int ZPY = 4;

    /// <summary>
    /// 进样探杆密封盖
    /// </summary>
    public const int SampPoleCap = 5;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 6;

    /// <summary>
    /// 错误类型1
    /// </summary>
    public const int ErrorOne = 7;

    /// <summary>
    /// 错误类型2
    /// </summary>
    public const int ErrorTwo = 8;

    /// <summary>
    /// 错误类型3
    /// </summary>
    public const int ErrorThree = 9;
}

/// <summary>
/// 操作车载质谱仪
/// </summary>
public class CarMassSpectOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case CarMasssSpectOpType102.Elec:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case CarMasssSpectOpType102.NitrogenTap:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "氮气瓶总阀打开" : "氮气瓶总阀关闭");
            case CarMasssSpectOpType102.Power:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "电源打开" : "电源关闭");
            case CarMasssSpectOpType102.ZPY:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "ZPY打开" : "ZPY关闭");
            case CarMasssSpectOpType102.SampPoleCap:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "进样探杆密封盖打开" : "进样探杆密封盖关闭");
            case CarMasssSpectOpType102.Alarm:
                return "车载质谱仪：" + (Operate == OperateDevice.OPEN ? "开始报警" : "报警结束");
            default:
                return "";
        }
    }
}
#endregion

#region 102红外遥测

/// <summary>
/// 操作类型
/// </summary>
public class InfaredTelemetryOpType102
{
    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 1;

    /// <summary>
    /// 升
    /// </summary>
    public const int Rise = 2;

    /// <summary>
    /// 降
    /// </summary>
    public const int Drop = 3;


    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 4;

    /// <summary>
    /// 自检
    /// </summary>
    public const int Check = 5;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 6;

    /// <summary>
    /// 错误类型1
    /// </summary>
    public const int ErrorOne = 7;
}

/// <summary>
/// 红外遥测
/// </summary>
public class InfaredTelemetryOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case InfaredTelemetryOpType102.Elec:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case InfaredTelemetryOpType102.Rise:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "升到位" : "开始下降");
            case InfaredTelemetryOpType102.Drop:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "降到位" : "开始上升");
            case InfaredTelemetryOpType102.OpenClose:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case InfaredTelemetryOpType102.Check:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "开始自检" : "自检结束");
            case InfaredTelemetryOpType102.Alarm:
                return "红外遥测：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            default:
                return "";
        }
    }
}

public class InfaredTelemetryParamOpType
{
    /// <summary>
    /// 定点
    /// </summary>
    public const int FIEXD_POINT = 1;

    /// <summary>
    /// 扇区
    /// </summary>
    public const int A_SECTROR = 2;

    /// <summary>
    /// 警戒
    /// </summary>
    public const int ALERT = 3;

    /// <summary>
    /// 自检
    /// </summary>
    public const int CHECK = 4;

    /// <summary>
    /// 停止
    /// </summary>
    public const int STOP = 5;
}




/// <summary>
/// 红外遥测参数  102
/// </summary>
public class InfaredTelemetryParamModel : ITaskLogModel
{
    /// <summary>
    /// 训练模式
    /// </summary>
    public int Tmode;

    /// <summary>
    /// 方向角
    /// </summary>
    public float Fxvalue;

    /// <summary>
    /// 俯仰角
    /// </summary>
    public float Fyvalue;

    public string GetTaskLog(int seatId)
    {
        return "训练模式： " + Tmode + "   \n方向角：" + Fxvalue + "    \n俯仰角：" + Fyvalue;
    }
}
/// <summary>
/// 检测到的毒种类
/// </summary>
public class InfaredTelemetryDrug
{
    public int Type;
}
#endregion
#region 102电源
/// <summary>
/// 操作类型
/// </summary>
public class PowerOpType102
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;
}

/// <summary>
/// 电源
/// </summary>
public class PowerOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PowerOpType102.OpenClose:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            default:
                return "";
        }
    }
}
#endregion

#region 102气象
/// <summary>
/// 操作类型
/// </summary>
public class MeteorOpType102
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 2;
}

/// <summary>
/// 气象设备
/// </summary>
public class MeteorOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case MeteorOpType102.OpenClose:
                return "气象：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case MeteorOpType102.Elec:
                return "气象：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            default:
                return "";
        }
    }
}
#endregion

#region 102电台
public class RadioStationOpType102
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;
}


/// <summary>
/// 操作电台
/// </summary>
public class RadioStationOp102Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadioStationOpType102.OpenClose:
                return "102电台：" + (Operate == OperateDevice.OPEN ? "开" : "关");

            default:
                return "";
        }
    }
}
#endregion

/// <summary>
/// 设置辐射率阈值102
/// </summary>
public class SetRadiomThreShold102Model : ITaskLogModel
{
    public float DoseThreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "辐射仪：当前剂量率阈值为 " + DoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

/// <summary>
/// 设置累计辐射率阈值102
/// </summary>
public class SetTTRadiomThreShold102Model : ITaskLogModel
{
    public float TotalDoseTreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "辐射仪：当前累积剂量阈值为 " + TotalDoseTreshold + " " + AppConstant.RADIOM_UNIT;
    }
}






#region 384车载辐射仪
/// <summary>
/// 384剂量率报警 特殊操作Operate
/// </summary>
public class RadiomOpAlarmOperate384
{
    /// <summary>
    /// 都不报警
    /// </summary>
    public const int NONE = 0;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RADIOM_ALARM = 1;

    /// <summary>
    /// 累计剂量率报警
    /// </summary>
    public const int TT_RADIOM_ALARM = 2;

    /// <summary>
    /// 都报警
    /// </summary>
    public const int BOTH = 3;
}

/// <summary>
/// 辐射仪操作类型
/// </summary>
public class RadiomOpType384
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RateAlarm = 2;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 4;
}

/// <summary>
/// 操作辐射仪384
/// </summary>
public class RadiomeOp384Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadiomOpType384.OpenClose:
                return "DFH辐射仪：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case RadiomOpType384.Elec:
                return "DFH辐射仪：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            default:
                return "";
        }
    }
}

#endregion

/// <summary>
/// 设置辐射计量率阈值384
/// </summary>
public class SetRadiomThreShold384Model : ITaskLogModel
{
    public float DoseThreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "DFH辐射仪：当前剂量率阈值为 " + DoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

/// <summary>
/// 设置辐射仪累计剂量率阈值 384
/// </summary>
public class SetTotalRadiomThreHold384 : ITaskLogModel
{
    public float TotalDoseTreshold;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "DFH辐射仪：当前累积剂量阈值为 " + TotalDoseTreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

#region 操作毒剂报警器 384
/// <summary>
/// 毒剂报警器操作类型384
/// </summary>
public class PoisonAlarmOp384Type
{
    /// <summary>
    /// 上电
    /// </summary>
   // public const int EleOn = 1;

    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenStatus = 1;

    /// <summary>
    /// 空气进样
    /// </summary>
    public const int AirJinYang = 2;

    /// <summary>
    /// 地面进样
    /// </summary>
    public const int GroundJinYang = 3;

    /// <summary>
    /// 空气探头加热
    /// </summary>
    public const int AirProbHeat = 4;

    /// <summary>
    /// 地面探头加热
    /// </summary>
    public const int GroundProbHeat = 5;

    /// <summary>
    /// 空气探头打开
    /// </summary>
    public const int AirProbOpen = 6;

    /// <summary>
    /// 地面探头打开
    /// </summary>
    public const int GroundProbOpen = 7;

    /// <summary>
    /// 故障
    /// </summary>
    public const int Error = 8;

    /// <summary>
    /// 报警
    /// </summary>
    public const int Alarm = 9;

}

/// <summary>
/// 操作毒剂报警器384
/// </summary>
public class PoisonAlarmOp384Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
           // case PoisonAlarmOp384Type.EleOn:
           //     return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case PoisonAlarmOp384Type.AirJinYang:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "空气进样开始" : "空气进样结束");
            case PoisonAlarmOp384Type.GroundJinYang:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "地面进样开始" : "地面进样结束");
            case PoisonAlarmOp384Type.AirProbHeat:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "空气探头开始加热" : "空气探头加热结束");
            case PoisonAlarmOp384Type.GroundProbHeat:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "地面探头开始加热" : "地面探头加热结束");
            case PoisonAlarmOp384Type.Error:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "故障（空气探头没开）" : "故障排除");

            case PoisonAlarmOp384Type.AirProbOpen:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "空气探头打开" : "空气探头关闭");
            case PoisonAlarmOp384Type.GroundProbOpen:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "地面探头打开" : "地面探头关闭");

            //这个状态要特殊处理
            case PoisonAlarmOp384Type.OpenStatus:
                string res = "毒剂报警器：";
                switch (Operate)
                {
                    case 0:
                        res += "关闭";
                        break;
                    case 1:
                        res += "开始预热";
                        break;
                    case 2:
                        res += "启动";
                        break;
                    default:
                        break;
                }
                return res;
            case PoisonAlarmOp384Type.Alarm:
                return "毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            default:
                return "";
        }
    }
}

public class PoisonAlarmWorkType
{
    /// <summary>
    /// 空气检测
    /// </summary>
    public const int AIRE_CHECK = 1;

    /// <summary>
    /// 空气清洁
    /// </summary>
    public const int AIRE_CLEAN = 2;

    /// <summary>
    /// 地面检测
    /// </summary>
    public const int ROUND_CHECK = 3;

    /// <summary>
    /// 地面清洁
    /// </summary>
    public const int ROUND_CLEAN = 4;

    /// <summary>
    /// 除污
    /// </summary>
    public const int CLEAN = 5;

    /// <summary>
    /// 修改侦检模式
    /// </summary>
    public const int UPDATE_MODEL = 6;
}

/// <summary>
/// 毒剂报警器选择工作模式384
/// </summary>
public class PoisonAlarmWorkType384Model : ITaskLogModel
{
    /// <summary>
    /// 工作模式
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PoisonAlarmWorkType.AIRE_CHECK:
                return "毒剂报警器工作模式：空气检测";
            case PoisonAlarmWorkType.AIRE_CLEAN:
                return "毒剂报警器工作模式：空气清洁";
            case PoisonAlarmWorkType.ROUND_CHECK:
                return "毒剂报警器工作模式：地面检测";
            case PoisonAlarmWorkType.ROUND_CLEAN:
                return "毒剂报警器工作模式：地面清洁";
            case PoisonAlarmWorkType.CLEAN:
                return "毒剂报警器工作模式：除污";
            default:
                return "";
        }
    }
}

public class InfaredTelemetry102DrugModel
{
    /// <summary>
    /// 毒剂种类
    /// </summary>
    public int Type;
}
#endregion

#region 操作电源 384

/// <summary>
/// 电源操作类型
/// </summary>
public class PowerOp384Type
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 输出
    /// </summary>
    public const int OutPut = 2;
}

/// <summary>
/// 操作电源
/// </summary>
public class PowerOp384Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PowerOp384Type.OpenClose:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case PowerOp384Type.OutPut:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开启输出" : "关闭输出");
            default:
                return "";
        }
    }
}

#endregion

#region 384电台
public class RadioStationOpType384
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;
}


/// <summary>
/// 操作电台
/// </summary>
public class RadioStationOp384Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadioStationOpType384.OpenClose:
                return "384电台：" + (Operate == OperateDevice.OPEN ? "开" : "关");

            default:
                return "";
        }
    }
}
#endregion



#region 操作电源 106

/// <summary>
/// 电源操作类型
/// </summary>
public class PowerOp106Type
{
    /// <summary>
    /// 开关机
    /// </summary>
    public const int OpenClose = 1;

    
}

/// <summary>
/// 操作电源
/// </summary>
public class PowerOp106Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PowerOp106Type.OpenClose:
                return "电源：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
                
            default:
                return "";
        }
    }
}

#endregion

#region 操作毒剂报警器 106
/// <summary>
/// 毒剂报警器操作类型106
/// </summary>
public class PoisonAlarmOp106Type
{
    public const int kaiguanji = 1;

    public const int jinqi = 2;//进气口保护罩开关

    public const int lingqi = 3;

    public const int danqi = 4;

    public const int jinyang = 5;

    public const int alarm = 6;

    

    /// <summary>
    /// yure
    /// </summary>
    public const int yure = 7;
}

/// <summary>
/// 操作毒剂报警器106
/// </summary>
public class PoisonAlarmOp106Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case PoisonAlarmOp106Type.kaiguanji:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case PoisonAlarmOp106Type.jinqi:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "进气口保护罩打开" : "进气口保护罩关闭");
            case PoisonAlarmOp106Type.lingqi:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "零气打开" : "零气关闭");
            case PoisonAlarmOp106Type.danqi:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "氮气打开" : "氮气关闭");
            case PoisonAlarmOp106Type.jinyang:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开始进样" : "停止进样");
            case PoisonAlarmOp106Type.alarm:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "开始报警" : "停止报警");
            
            case PoisonAlarmOp106Type.yure:
                return "106毒剂报警器：" + (Operate == OperateDevice.OPEN ? "预热开始" : "预热结束");
            default:
                return "";
        }
    }
}


/// <summary>
/// 毒剂报警器可控状态设置
/// </summary>
public class PoisonAlarmStatCtr106Model
{
    public int Operate;
}

/// <summary>
/// 设置减压阀数据
/// </summary>
public class SetReliefThreshold
{
    /// <summary>
    /// 减压阀数据
    /// </summary>
    public float ReliefThreshold;
}

#endregion

#region 106操作辐射仪
public class RadiomOpType106
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;

    /// <summary>
    /// 剂量率报警
    /// </summary>
    public const int RateAlarm = 2;

    /// <summary>
    /// 累计剂量率报警
    /// </summary>
    public const int TotalRateAlarm = 3;

    /// <summary>
    /// 上电
    /// </summary>
    public const int Elec = 4;

    /// <summary>
    /// 自检
    /// </summary>
    public const int Check = 5;
}


/// <summary>
/// 操作辐射仪106
/// </summary>
public class RadiomeOp106Model
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadiomOpType106.OpenClose:
                return "106辐射仪：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case RadiomOpType106.RateAlarm:
                return "106辐射仪：" + (Operate == OperateDevice.OPEN ? "剂量率报警" : "停止剂量率报警");
            case RadiomOpType106.TotalRateAlarm:
                return "106辐射仪：" + (Operate == OperateDevice.OPEN ? "累积剂量报警" : "停止累积剂量报警");
            case RadiomOpType106.Elec:
                return "106辐射仪：" + (Operate == OperateDevice.OPEN ? "上电" : "离线");
            case RadiomOpType106.Check:
                return "106辐射仪：" + (Operate == OperateDevice.OPEN ? "自检开始" : "自检结束");
            default:
                return "";
        }
    }
}

/// <summary>
/// 设置辐射计量率阈值106
/// </summary>
public class SetRadiomThreShold106 : ITaskLogModel
{
    public float DoseThreshold;
    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "106辐射仪：当前剂量率阈值为 " + DoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

/// <summary>
/// 设置辐射仪累计剂量率阈值 106
/// </summary>
public class SetTotalRadiomThreHold106 : ITaskLogModel
{
    public float TotalDoseThreshold;
    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "辐射仪：当前累积剂量阈值为 " + TotalDoseThreshold + " " + AppConstant.RADIOM_UNIT;
    }
}

#endregion

#region 操作生物报警器106
public class BiologyOp106Type
{

    public const int kaiguanji = 1;


    public const int alarm = 2;

}

public class BiologyOp106Model
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case BiologyOp106Type.kaiguanji:
                return "106生物模拟器：" + (Operate == OperateDevice.OPEN ? "开机" : "关机");
            case BiologyOp106Type.alarm:
                return "106生物模拟器：" + (Operate == OperateDevice.OPEN ? "生物报警" : "停止生物报警");
            
            default:
                return "";
        }
    }
}

/// <summary>
/// 生物模拟器数据监测
/// </summary>
public class SetBiologyThreShold106Model : ITaskLogModel
{
    public float BiologicalData;
    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        return "生物模拟器数据监测" + BiologicalData + " " + AppConstant.RADIOM_UNIT;
    }
}
#endregion

#region 106电台
public class RadioStationOpType106
{
    /// <summary>
    /// 开关
    /// </summary>
    public const int OpenClose = 1;
}


/// <summary>
/// 操作电台
/// </summary>
public class RadioStationOp106Model : ITaskLogModel
{
    /// <summary>
    /// 操作 0关  1开
    /// </summary>
    public int Operate;

    /// <summary>
    /// 操作类型
    /// </summary>
    public int Type;

    /// <summary>
    /// 获得日志
    /// </summary>
    public string GetTaskLog(int seatId)
    {
        switch (Type)
        {
            case RadioStationOpType106.OpenClose:
                return "106电台：" + (Operate == OperateDevice.OPEN ? "开" : "关");

            default:
                return "";
        }
    }
}
#endregion