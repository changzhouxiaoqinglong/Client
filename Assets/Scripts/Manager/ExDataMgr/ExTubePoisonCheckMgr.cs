using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExTubePoisonCheckMgr : ExDataMgrBase<ExTubePoisonCheckMgr, ExTubePoisonCheck>
{
    protected override string FILE_NAME { get; set; } = "TubePoisonCheck";

    public string GetTubePoisonCheck(int tubeId,int poisonId,int degreeLow, int checkType)
    {
        foreach (var item in dataList)
        {
            if (item.CheckScene == checkType)
            {
                if (item.TubeId == tubeId && item.PoisonId == poisonId && item.DegreeLow == degreeLow)
                {
                    return item.SpritePath;
                }
            }
        }
        foreach (var item in dataList)
        {
            if (item.CheckScene == checkType)
            {
                if (item.TubeId == tubeId && item.PoisonId == PoisonType.NO_POISON && item.DegreeLow == DrugDegree.NONE)
                {
                    return item.SpritePath;
                }
            }
        }
        Logger.LogError("GetTubePoisonCheck：No SpritePath");
        return null;

    }
}
