public class ProcessId
{
    /// <summary>
    /// 02b打开进气帽
    /// </summary>
    public const int POISON_ALARM_OP_INTAKE_02B = 1;

    /// <summary>
    /// 02b毒剂报警器开机
    /// </summary>
    public const int POISON_ALARM_OPEN_02B = 2;

    /// <summary>
    /// 02b毒剂报警器 自检
    /// </summary>
    public const int POISON_ALARM_CHECK_02B = 3;

    /// <summary>
    /// 02b毒剂报警器 等待自检结束
    /// </summary>
    public const int POISON_ALARM_CHECK_END_02B = 4;

    /// <summary>
    /// 02b进样
    /// </summary>
    public const int POISON_ALARM_JINYANG_02B = 5;

    /// <summary>
    /// 02b毒剂报警器 报警
    /// </summary>
    public const int POISON_ALARM_02B = 6;

    /// <summary>
    /// 02b进样超过5秒后结束
    /// </summary>
    public const int POISON_ALARM_END_JINYANG = 7;

    /// <summary>
    /// 02b毒剂报警器 关机
    /// </summary>
    public const int POISON_ALARM_CLOSE_02B = 8;

    /// <summary>
    /// 02b关闭进气帽
    /// </summary>
    public const int POISON_ALARM_CLOSE_INTAKE_02B = 9;

    
}

public class DrugId
{
    /// <summary>
    /// 02b车载侦毒器设置抽气时间
    /// </summary>
    public const int DRUG_DET_BLEED_TIME = 10;

    /// <summary>
    /// 02b车载侦毒器打开加热开关
    /// </summary>
    public const int DRUG_HEAT_OPEN = 11;

    /// <summary>
    /// 02b车载侦毒器打开泵开关
    /// </summary>
    public const int DRUG_PUMP_OPEN = 12;

    /// <summary>
    /// 02b车载侦毒器关闭加热开关
    /// </summary>
    public const int DRUG_HEAT_CLOSE = 13;

    /// <summary>
    /// 02b车载侦毒器关闭泵开关
    /// </summary>
    public const int DRUG_PUMP_CLOSE = 14;
}

public class RadioId
{

    /// <summary>
    /// 02b车在辐射仪不操作索引
    /// </summary>
    public const int RADIO_NO_OPERATE = -1;

    /// <summary>
    /// 02b车载辐射仪开机
    /// </summary>
    public const int RADIO_OPEN_02B = 15;

    /// <summary>
    /// 02b车载辐射仪自检
    /// </summary>
    public const int RADIO_CHECK_02B = 16;

    /// <summary>
    /// 02b车载辐射仪设置剂量率
    /// </summary>
    public const int RADIOM_RATE_THRESHOLD = 17;

    /// <summary>
    /// 02b车载辐射仪设置累计剂量率
    /// </summary>
    public const int TT_RADIOM_RATE_THRESHOLD = 18;

    /// <summary>
    /// 02b车载辐射仪关机
    /// </summary>
    public const int RADIO_CLOSE_02B = 19;

}

public class PowerId
{

   
    /// <summary>
    /// 02b电源开机
    /// </summary>
    public const int Power_OPEN_02B = 466;



    /// <summary>
    /// 02b电源关机
    /// </summary>
    public const int Power_CLOSE_02B = 467;

}

public class RadioStationId
{


    /// <summary>
    /// 02b电台开机
    /// </summary>
    public const int RadioStation_OPEN_02B = 468;



    /// <summary>
    /// 02b电台关机
    /// </summary>
    public const int RadioStation_CLOSE_02B = 469;

}


public class Power102Id
{


    /// <summary>
    /// 102电源开机
    /// </summary>
    public const int Power_OPEN_102 = 470;



    /// <summary>
    /// 102电源关机
    /// </summary>
    public const int Power_CLOSE_102 = 471;
}

public class RadioStation102Id
{


    /// <summary>
    /// 102电台开机
    /// </summary>
    public const int RadioStation_OPEN_102 = 472;



    /// <summary>
    /// 102电台关机
    /// </summary>
    public const int RadioStation_CLOSE_102 = 473;

}

public class Prevent102Id
{
    /// <summary>
    /// 102三防毒报开机
    /// </summary>
    public const int PREVENT_DRUG_OPEN_102 = 137;

    /// <summary>
    /// 102三防辐射仪开机
    /// </summary>
    public const int PREVENT_RADIOM_OPEN_102 = 138;

    /// <summary>
    /// 102三防舱门开机
    /// </summary>
    public const int PREVENT_PRESSURE_OPEN_102 = 139;

    /// <summary>
    /// 102三防毒报关机
    /// </summary>
    public const int PREVENT_DRUG_CLOSE_102 = 140;

    /// <summary>
    /// 102三防辐射仪关机
    /// </summary>
    public const int PREVENT_RADIOM_CLOSE_102 = 141;

    /// <summary>
    /// 102三防舱门关机
    /// </summary>
    public const int PREVENT_PRESSURE_CLOSE_102 = 142;


}


public class Radiom102Id
{
    /// <summary>
    /// 102车载侦毒器开机
    /// </summary>
    public const int RADIOM_OPEN_102 = 143;

    /// <summary>
    /// 102车载侦毒器设置剂量率
    /// </summary>
    public const int RADIOM_RATE_THRESHOLD_102 = 144;

    /// <summary>
    /// 102车载侦毒器设置累计剂量率
    /// </summary>
    public const int TT_RADIOM_RATE_THRESHOLD_102 = 145;

    /// <summary>
    /// 102车载侦毒器关机
    /// </summary>
    public const int RADIOM_CLOSE_102 = 146;
}

public class MessSpect102Id
{
    /// <summary>
    /// 102质谱仪打开氮气瓶总阀
    /// </summary>
    public const int MESS_SPECT_NITRO_GENTIP_OPEN_102 = 147;

    /// <summary>
    /// 102质谱仪打开电源
    /// </summary>
    public const int MESS_SPECT_POWER_OPEN_102 = 148;

    /// <summary>
    /// 102质谱仪打开软件
    /// </summary>
    public const int MESS_SPECT_ZPY_OPEN_102 = 149;

    /// <summary>
    /// 102质谱仪打开进项探杆
    /// </summary>
    public const int MESS_SPECT_SAMP_POLE_CAP_OPEN_102 = 150;

    /// <summary>
    /// 102质谱仪关闭软件
    /// </summary>
    public const int MESS_SPECT_ZPY_CLOSE_102 = 151;

    /// <summary>
    /// 102质谱仪关闭电源
    /// </summary>
    public const int MESS_SPECT_POWER_CLOSE_102 = 152;

    /// <summary>
    /// 102质谱仪关闭氮气瓶总阀
    /// </summary>
    public const int MESS_SPECT_NITRO_GENTIP_CLOSE_102 = 153;

    /// <summary>
    /// 102质谱仪关闭进项探杆
    /// </summary>
    public const int MESS_SPECT_SAMP_POLE_CAP_CLOSE_102 = 154;
}

public class Infare102Id
{

    /// <summary>
    /// 红外遥测无操作
    /// </summary>
    public const int INFARE_NO_OPERATE = -1;
    
    /// <summary>
    /// 102红外遥测上升
    /// </summary>
    public const int INFARE_UP_MODEL_102 = 155;

    /// <summary>
    /// 102红外遥测开机
    /// </summary>
    public const int INFARE_OPEN_102 = 156;

    /// <summary>
    /// 102红外遥测设置模式
    /// </summary>
    public const int INFARE_SET_MODEL_102 = 157;

    /// <summary>
    /// 102红外遥测开机
    /// </summary>
    public const int INFARE_CLOSE_102 = 158;

    /// <summary>
    /// 102红外遥测下降
    /// </summary>
    public const int INFARE_DOWN_MODEL_102 = 159;
}
public class Poison384Id
{

    /// <summary>
    /// 384车载侦毒器打开探头
    /// </summary>
    public const int POISON384_HEAD_OPEN = 279;


    /// <summary>
    /// 384车载侦毒器开机
    /// </summary>
    public const int POISON384_OPEN = 284;

    /// <summary>
    /// 384车载侦毒器DFH开启
    /// </summary>
    //public const int POISON384_DFH_OPEN = 285;

    /// <summary>
    /// 384模拟进样
    /// </summary>
    public const int POISON384_JINYANG = 285;

    /// <summary>
    /// 384模拟进样结束
    /// </summary>
    public const int POISON384_JINYANG_End = 286;

    /// <summary>
    /// 384毒剂报警器报警
    /// </summary>
    public const int POISON384_ALARM = 287;

    /// <summary>
    /// 384车载侦毒器设置工作模式
    /// </summary>
    //public const int POISON384_SET_WORK_MODEL = 287;

    /// <summary>
    /// 384车载侦毒器关机
    /// </summary>
    public const int POISON384_CLOSE = 288;

    /// <summary>
    /// 384车载侦毒器关闭探头
    /// </summary>
    public const int POISON384_HEAD_CLOSE = 289;

    
}


public class Radiom384Id
{

    /// <summary>
    /// 384车载辐射仪开机
    /// </summary>
    public const int RADIOM384_OPEN = 280;

    /// <summary>
    /// 384设置剂量率阈值
    /// </summary>
    public const int RADIOM384_RATE_THRESHOLD = 281;

    /// <summary>
    /// 384设置累计剂量率阈值
    /// </summary>
    public const int TT_RADIOM384_RATE_THRESHOLD = 282;

    /// <summary>
    /// 384车载辐射仪关机
    /// </summary>
    public const int RADIOM384_CLOSE = 283;
}

public class Power384Id
{


    /// <summary>
    /// 384电源开机
    /// </summary>
    public const int Power_OPEN_384 = 474;



    /// <summary>
    /// 384电源关机
    /// </summary>
    public const int Power_CLOSE_384 = 475;
}

public class RadioStation384Id
{


    /// <summary>
    /// 384电台开机
    /// </summary>
    public const int RadioStation_OPEN_384 = 476;



    /// <summary>
    /// 384电台关机
    /// </summary>
    public const int RadioStation_CLOSE_384 = 477;

}

public class Poison106Id
{
    

    /// <summary>
    /// 进气口保护罩打开
    /// </summary>
    public const int POISON_ALARM_OPEN_PROTECT_106 = 407;

    /// <summary>
    /// 零气开
    /// </summary>
    public const int POISON_ALARM_OPEN_LINGQI_106 = 408;


    /// <summary>
    /// 氮气开
    /// </summary>
    public const int POISON_ALARM_OPEN_DANQI_106 = 409;

    /// <summary>
    /// 106设置减压阀数据
    /// </summary>
    public const int SetReliefThreshold_106 = 410;

    /// <summary>
    /// 106毒剂报警器开机
    /// </summary>
    public const int POISON_ALARM_OPEN_106 = 411;

    //106毒剂报警器预热
    public const int POISON_ALARM_YURE_106 = 412;

   

    /// <summary>
    /// 106进样
    /// </summary>
    public const int POISON_ALARM_JINYANG_106 = 413;

    /// <summary>
    /// 进样超过5秒后结束
    /// </summary>
    public const int POISON_ALARM_END_JINYANG_106 = 414;

    /// <summary>
    /// 毒剂报警器 报警
    /// </summary>
    public const int POISON_ALARM_106 = 415;

    


    /// <summary>
    /// 进气口保护罩关闭
    /// </summary>
    public const int POISON_ALARM_ClOSE_PROTECT_106 = 416;



    /// <summary>
    /// 零气关
    /// </summary>
    public const int POISON_ALARM_CLOSE_LINGQI_106 = 417;



    /// <summary>
    /// 氮气关
    /// </summary>
    public const int POISON_ALARM_CLOSE_DANQI_106 = 418;

    /// <summary>
    /// 106减压阀数据归零
    /// </summary>
    public const int SetReliefThreshold_0_106 = 419;

    /// <summary>
    /// 毒剂报警器关机
    /// </summary>
    public const int POISON_ALARM_CLOSE_106 = 420;





}

public class RadioId106
{

    /// <summary>
    /// 106车在辐射仪不操作索引
    /// </summary>
    public const int RADIO_NO_OPERATE = 421;

    /// <summary>
    /// 106车载辐射仪开机
    /// </summary>
    public const int RADIO_OPEN_106 = 422;

    /// <summary>
    /// 106车载辐射仪自检
    /// </summary>
    public const int RADIO_CHECK_106 = 423;

    /// <summary>
    /// 106车载辐射仪设置剂量率
    /// </summary>
    public const int RADIOM_RATE_THRESHOLD_106 = 424;

    /// <summary>
    /// 106车载辐射仪设置累计剂量率
    /// </summary>
    public const int TT_RADIOM_RATE_THRESHOLD_106 = 425;

    /// <summary>
    /// 106车载辐射仪关机
    /// </summary>
    public const int RADIO_CLOSE_106 = 426;

}

public class BiologyId106
{

    /// <summary>
    /// 106生物模拟器开机
    /// </summary>
    public const int Biology_OPEN_106 = 427;

    /// <summary>
    /// 106生物模拟器报警
    /// </summary>
    public const int Biology_ALARM_OPEN_106 = 428;

    /// <summary>
    /// 106生物模拟器数据监测
    /// </summary>
    public const int Biology_RATE_THRESHOLD_106 = 429;

    /// <summary>
    /// 106生物模拟器关机
    /// </summary>
    public const int Biology_CLOSE_106 = 430;

    /// <summary>
    /// 106生物模拟器报警
    /// </summary>
    public const int Biology_ALARM_CLOSE_106 = 431;




}


public class Power106Id
{


    /// <summary>
    /// 106电源开机
    /// </summary>
    public const int Power_OPEN_106 = 478;



    /// <summary>
    /// 106电源关机
    /// </summary>
    public const int Power_CLOSE_106 = 479;
}

public class RadioStation106Id
{


    /// <summary>
    /// 106电台开机
    /// </summary>
    public const int RadioStation_OPEN_106 = 480;



    /// <summary>
    /// 106电台关机
    /// </summary>
    public const int RadioStation_CLOSE_106 = 481;

}

/// <summary>
/// 训练流程数据
/// </summary>
public class ExPracticeProcess : ExDataBase
{
    /// <summary>
    /// 对应训练id
    /// </summary>
    public int TaskId;

    /// <summary>
    /// 提示语
    /// </summary>
    public string Tip;

    /// <summary>
    /// 错误提示
    /// </summary>
    public string ErrorTip;

    /// <summary>
    /// 获得错误提示语
    /// </summary>
    public string GetErrorTip()
    {
        if (ErrorTip.IsNullOrEmpty())
        {
            return string.Empty;
        }
        else
        {
            return "错误操作：" + ErrorTip;
        }
    }
}
