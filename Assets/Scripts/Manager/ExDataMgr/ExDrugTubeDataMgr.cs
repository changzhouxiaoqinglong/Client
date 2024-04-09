using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExDrugTubeDataMgr : ExDataMgrBase<ExDrugTubeDataMgr, ExDrugTubeData>
{
    /// <summary>
    /// 表名
    /// </summary>
    protected override string FILE_NAME { get; set; } = "ExDrugTubeData";
}
