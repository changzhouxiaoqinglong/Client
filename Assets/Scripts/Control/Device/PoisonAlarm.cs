
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 毒剂报警器
/// </summary>
public class PoisonAlarm : DeviceBase
{
    /// <summary>
    /// 计时器
    /// </summary>
    private float checkTimer = 0;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //化学训练才起作用
        if (TaskMgr.GetInstance().curTaskData.CheckType != HarmAreaType.DRUG)
        {
            return;
        }
        if (!UIMgr.GetInstance().IsOpenView(ViewType.DrugDgreeTest))
        {
            CountDrugData();
        }
        if (Keyboard.current.ctrlKey.isPressed)
        {
            if (Keyboard.current.tKey.isPressed)
            {
                UIMgr.GetInstance().OpenView(ViewType.DrugDgreeTest);
            }
        }
    }

    /// <summary>
    /// 计算上报化学区域信息
    /// </summary>
    private void CountDrugData()
    {
        if (car.IsSelfCar())
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= AppConstant.DRUG_CHECK_OFFTIME)
            {
                checkTimer = 0;
                ReportCurDrugData();
            }
        }
    }

    /// <summary>
    /// 上报当前化学信息
    /// </summary>
    protected virtual void ReportCurDrugData()
    {
        //print("上报当前化学信息");
        //浓度
        float dentity = HarmAreaMgr.GetPosDrugDentity(car.GetPosition());
        DrugVarData drugVarData = HarmAreaMgr.GetPosDrugData(car.GetPosition());
        //毒数据
        ExPoisonData exPoisonData = null;
        if (drugVarData != null)
        {
            exPoisonData = ExPoisonDataMgr.GetInstance().GetDataById(drugVarData.Type);
        }
        ReportDrugDataModel model = new ReportDrugDataModel()
        {
            Id = drugVarData != null ? drugVarData.Id : 0,
            Type = drugVarData != null ? drugVarData.Type : PoisonType.NO_POISON,
            Dentity = dentity,
            Degree = exPoisonData != null ? exPoisonData.GetdDegreeByDentity(dentity) : DrugDegree.NONE,
            DType = exPoisonData != null ? exPoisonData.DType : DrugDType.NONE,
        };
        //发给设备管理软件
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.SEND_DRUG_DATA, NetManager.GetInstance().SameMachineAllSeats);
    }
}
