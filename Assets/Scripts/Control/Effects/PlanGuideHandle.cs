
using System.Collections.Generic;

public class PlanGuideHandle
{
    public GuideMgr _GuideMgr {
        get;set;
    }

    //当前计划导调索引
    private int curGuideIndex = 0;

    //计划导调数据
    private List<PlanGuideData> planGuideDatas;

    public PlanGuideHandle() {
        planGuideDatas = NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.PlanGuides;
    }

    public void Update() {
        if (planGuideDatas != null && planGuideDatas.Count > 0 && curGuideIndex < planGuideDatas.Count) {
            if (TaskMgr.GetInstance().curTaskCtr.trainDateMgr.timer >= planGuideDatas[curGuideIndex].Time) {
                //到达触发时间
                _GuideMgr.TriggerGuideEv(planGuideDatas[curGuideIndex].Type);
                curGuideIndex++;
            }
        }
    }

}
