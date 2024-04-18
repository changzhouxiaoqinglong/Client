
using System;
using System.Collections.Generic;
/// <summary>
/// 训练流程控制
/// </summary>
public class PracticeProcessCtr
{
    /// <summary>
    /// 训练和对应类映射
    /// </summary>
    private Dictionary<int, Type> processConfigs = new Dictionary<int, Type>()
    {
        {ExTaskId.BASE_POISON_ALARM_02B, typeof(PracticeProcess02BPoison)},
        {ExTaskId.BASE_RADIOMETE_02B, typeof(PracticeProcess02BRadio)},
        {ExTaskId.BASE_CAR_POISON_DETECT_02B,typeof(PracticeProcess02BDrug) },
        {ExTaskId.BASE_CAR_Power_02B,typeof(PracticeProcess02BPower) },     
        {ExTaskId.BASE_CAR_RadioStation_02B,typeof(PracticeProcess02BRadioStation) },

        {ExTaskId.BASE_RADIOMETE_384,typeof(PracticeProcess384Radio) },
        {ExTaskId.BASE_POISON_384,typeof(PracticeProcess384Poison) },
        {ExTaskId.BASE_Power_384,typeof(PracticeProcess384Power) },
        {ExTaskId.BASE_RadioStation_384,typeof(PracticeProcess384RadioStation) },

        {ExTaskId.BASE_RADIOMETE_102,typeof(PracticeProcess102Radio) },
        {ExTaskId.BASE_MESS_SPECT_102,typeof(PracticeProcess102MessSpect) },
        {ExTaskId.BASE_PREVENT_102,typeof(PracticeProcess102Prevent) },
        {ExTaskId.BASE_INFARE_102,typeof(PracticeProcess102Infare) },
         {ExTaskId.BASE_Power_102,typeof(PracticeProcess102Power) },
          {ExTaskId.BASE_RadioStation_102,typeof(PracticeProcess102RadioStation) },

         {ExTaskId.BASE_POISON_ALARM_106, typeof(PracticeProcess106Poison)},
         {ExTaskId.BASE_RADIOMETE_106, typeof(PracticeProcess106Radio)},
          {ExTaskId.BASE_Biology_106, typeof(PraticeProcess106Biology)},
           {ExTaskId.BASE_Power_106,typeof(PracticeProcess106Power) },
          {ExTaskId.BASE_RadioStation_106,typeof(PracticeProcess106RadioStation) },
    };

    /// <summary>
    /// 当前训练流程
    /// </summary>
    public PracticeProcessBase curProcess;

    /// <summary>
    /// 初始化当前训练流程
    /// </summary>
    public void InitProcess(int taskId)
    {
        if (processConfigs.ContainsKey(taskId) &&
            NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CheckType == CheckTypeConst.PRACTICE)
        {
            curProcess = (PracticeProcessBase)Activator.CreateInstance(processConfigs[taskId]);
        }
        else
        {
            curProcess = new PracticeProcessBase();
        }
        curProcess.Init(taskId);
    }

    /// <summary>
    /// 当前训练是否有流程
    /// </summary>
    public bool IsHaveProcess()
    {
        return curProcess.processList.Count > 0;
    }

    /// <summary>
    /// 结束
    /// </summary>
    public void End()
    {
        curProcess.End();
    }
}
