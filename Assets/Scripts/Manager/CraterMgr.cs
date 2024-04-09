
using System.Collections.Generic;
using UnityEngine;

public class CraterMgr
{
    //弹坑 资源地址
    private const string CRATER_RES_PATH = "Prefabs/Craters/Crater";

    /// <summary>
    /// 弹坑
    /// </summary>
    public List<CraterBase> craterList = new List<CraterBase>();

    /// <summary>
    /// 毒剂地面检测 弹坑范围
    /// </summary>
    private const float GROUND_CHECK_DRUG_DIS = 15;

    /// <summary>
    /// 弹坑节点
    /// </summary>
    private Transform craterRoot;
    private Transform CraterRoot
    {
        get
        {
            if (craterRoot == null)
            {
                GameObject vehicleGo = GameObject.Find("Craters");
                if (vehicleGo)
                {
                    craterRoot = vehicleGo.transform;
                }
                else
                {
                    craterRoot = new GameObject("Craters").transform;
                }
            }
            return craterRoot;
        }
    }

    public Train3DSceneCtrBase CurScene
    {
        get; set;
    }

    /// <summary>
    /// 初始化弹坑
    /// </summary>
    public void InitCraters()
    {
        foreach (var item in NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.CraterDatas)
        {
            CreateCrater(item);
        }
    }

    /// <summary>
    /// 生成弹坑 毒剂类型：1无毒2沙林3梭曼4芥子气5VX6DMMP

    /// </summary>
    private void CreateCrater(CraterVarData craterData)
    {
        string tempCraterPath = CRATER_RES_PATH + craterData.Type.ToString();
        GameObject craterPrefab = Resources.Load<GameObject>(tempCraterPath);
        GameObject craterObj = Object.Instantiate(craterPrefab, CraterRoot);
        CraterBase crater = craterObj.GetComponent<CraterBase>();
        crater.VarData = craterData;
        Vector3 vector3 = CurScene.terrainChangeMgr.GetTerrainPosByGis(craterData.Pos.ToVector2());
        vector3 += new Vector3(0,0.1f,0);
        crater.SetPosition(vector3);
        crater.SetRotation(craterData.GetRotation());
        craterList.Add(crater);
        //Debug.Log("弹坑生成位置：" + vector3);
        //Debug.Log("弹坑位置：" + crater.transform.position);
    }

    public CraterBase GetQuestionCrater(Vector3 carPos)
    {
        return GetNearCrater(carPos, 20);
    }

    /// <summary>
    /// 地面检测毒剂  弹坑数据
    /// </summary>
    public CraterBase GetGroundCheckCrater(Vector3 pos)
    {
        return GetNearCrater(pos, GROUND_CHECK_DRUG_DIS);
    }

    /// <summary>
    /// 获取附近的弹坑
    /// </summary>    
    private CraterBase GetNearCrater(Vector3 pos, float distance)
    {
        foreach (var item in craterList)
        {
            if (item.CraterDistanceCar(pos, distance))
            {
                return item;
            }
        }
        return null;
    }
}
