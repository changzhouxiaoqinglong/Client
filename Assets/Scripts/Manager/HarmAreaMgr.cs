
using System.Collections.Generic;
using UnityEngine;

public class HarmAreaMgr
{
    //毒区域 资源地址
    private const string DRUG_AREA_RES = "Prefabs/HarmAreas/DrugArea";

    //辐射区域 资源地址
    private const string RADIATE_AREA_RES = "Prefabs/HarmAreas/RadiomArea";

    /// <summary>
    /// 有害区域
    /// </summary>
    public List<HarmAreaBase> areaList = new List<HarmAreaBase>();

    /// <summary>
    /// 有害区域节点
    /// </summary>
    private Transform harmAreaRoot;
    private Transform HarmAreaRoot
    {
        get
        {
            if (harmAreaRoot == null)
            {
                GameObject vehicleGo = GameObject.Find("HarmAreas");
                if (vehicleGo)
                {
                    harmAreaRoot = vehicleGo.transform;
                }
                else
                {
                    harmAreaRoot = new GameObject("HarmAreas").transform;
                }
            }
            return harmAreaRoot;
        }
    }

    #region 随机剂量率
    /// <summary>
    /// 随机剂量率标准值(没进辐射区域的时候 也会有随机剂量率 不会为0)
    /// </summary>
    private float randomRadiomNormal = 0.075f;

    /// <summary>
    /// 随机剂量率浮动值
    /// </summary>
    private float randomRadiomOffValue = 0.025f;

    /// <summary>
    /// 当前随机剂量率
    /// </summary>
    private float curRandomRadiom = 0;

    /// <summary>
    /// 计时器
    /// </summary>
    private float randomRadiomTimer = 0;

    /// <summary>
    /// 随机时间间隔s
    /// </summary>
    private float randomOffTime = 1;
    #endregion


    /// <summary>
    /// 初始化有害区域
    /// </summary>
    public void InitHarmArea()
    {
        foreach (var item in NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.HarmDatas)
        {
            CreateHarmArea(item);
        }
    }

    /// <summary>
    /// 生成有害区域
    /// </summary>
    public void CreateHarmArea(HarmData harmData)
    {
        //毒
        if (harmData.HarmType == HarmAreaType.DRUG)
        {
            DrugVarData drug = JsonTool.ToObject<DrugVarData>(harmData.Content);
            GameObject res = Resources.Load<GameObject>(DRUG_AREA_RES);
            GameObject drugInst = Object.Instantiate(res, HarmAreaRoot);
            //区域
            DrugArea area = drugInst.GetComponent<DrugArea>();
            //设置数据
            area.DrugVarData = drug;
            areaList.Add(area);
        }
        //辐射
        else if (harmData.HarmType == HarmAreaType.NUCLEAR)
        {
            RadiatVarData radiate = JsonTool.ToObject<RadiatVarData>(harmData.Content);
            GameObject res = Resources.Load<GameObject>(RADIATE_AREA_RES);
            GameObject radiateInst = Object.Instantiate(res, HarmAreaRoot);
            //辐射区域
            RadiomArea area = radiateInst.GetComponent<RadiomArea>();
            //设置数据
            area.RadiatVarData = radiate;
            areaList.Add(area);
        }//生物
        else
        {

        }
    }

    public void OnUpdate()
    {
        CountRandomRadiom();
    }

    /// <summary>
    /// 获得对应位置的 辐射剂量率
    /// </summary>
    public float GetPosRadiomRate(Vector3 pos)
    {
        float res = 0;
        //遍历辐射区域 计算
        foreach (var item in areaList)
        {
            if (item is RadiomArea radiomArea && radiomArea.IsInRange(pos))
            {
                res += radiomArea.GetRadiomRate(pos);
            }
        }
        //要加上随机剂量率
        res += curRandomRadiom;
        return res;
    }

    /// <summary>
    /// 计算随机剂量率
    /// </summary>
    private void CountRandomRadiom()
    {
        //核任务
        if (TaskMgr.GetInstance().curTaskData.CheckType == HarmAreaType.NUCLEAR)
        {
            randomRadiomTimer += Time.deltaTime;
            if (randomRadiomTimer >= randomOffTime)
            {
                randomRadiomTimer = 0;
                curRandomRadiom = randomRadiomNormal + Random.Range(-randomRadiomOffValue, randomRadiomOffValue);
            }
        }
    }

    /// <summary>
    /// 获得对应位置的 毒浓度
    /// </summary>
    public float GetPosDrugDentity(Vector3 pos)
    {
        float res = 0;
        //遍历辐射区域 计算
        foreach (var item in areaList)
        {
            if (item is DrugArea drugArea && drugArea.IsInRange(pos))
            {
                res += drugArea.GetDrugDentity(pos);
            }
        }
        return res;
    }

    /// <summary>
    /// 获得对应位置的 毒区 毒数据
    /// </summary>
    public DrugVarData GetPosDrugData(Vector3 pos)
    {
        //遍历辐射区域 计算
        foreach (var item in areaList)
        {
            if (item is DrugArea drugArea && drugArea.IsInRange(pos))
            {
                return drugArea.DrugVarData;
            }
        }
        return null;
    }
}
