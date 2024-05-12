
public class PoisonAlarm102 : PoisonAlarm
{
    protected override void ReportCurDrugData()
    {
       
        //浓度
        float dentity = HarmAreaMgr.GetPosDrugDentity(car.GetPosition());
        DrugVarData drugVarData = HarmAreaMgr.GetPosDrugData(car.GetPosition());
        print("毒区域数据:   "+(drugVarData == null));
        DefenseReportDrugDataModel model = new DefenseReportDrugDataModel()
        {
            Type = drugVarData != null ? drugVarData.Type : PoisonType.NO_POISON,
            Dentity = dentity,
        };
        //发给设备管理软件
        NetManager.GetInstance().SendMsg(ServerType.GuideServer, JsonTool.ToJson(model), NetProtocolCode.DEFENSE_SEND_DRUG_DATA, NetManager.GetInstance().CurDeviceForward);
    }
}
