
using System.Collections;
using UnityEngine;
/// <summary>
/// 毒区
/// </summary>
public class DrugArea : HarmAreaBase
{
    /// <summary>
    /// 毒区路径
    /// </summary>
    private const string CREATE_DRUG_PATH = "Prefabs/Sprite/MaxMapIcon/DrugArea";

    private const int TempRadio = 2;
    /// <summary>
    /// 毒数据
    /// </summary>
    public DrugVarData DrugVarData
    {
        get; set;
    }

    protected override void Start()
    {
        base.Start();
        //初始化大小 范围要*2
        //transform.localScale = Vector3.one * DrugVarData.Range * 2;
        //位置
        transform.position = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.GetTerrainPosByGis(DrugVarData.Pos.ToVector2());
        windDir = taskEnvVarData.Wearth.GetWindDir();
        windSp = taskEnvVarData.Wearth.WindSp;
        CreatePointRange(transform.position, windDir);
        StartCoroutine(ISetDrugAreaPoison());
    }

    /// <summary>
    /// 是否在范围内
    /// </summary>
    public override bool IsInRange(Vector3 pos)
    {
        return MathTool.IsPointInPolygon(pos, pointList);
    }

    /*    /// <summary>
        /// 获得对应位置的浓度
        /// </summary>
        public float GetDrugDentity(Vector3 pos)
        {
            //距离
            float dis = Vector3.Distance(transform.position, pos);
            if (dis > DrugVarData.Range)
            {
                //在范围之外
                return 0;
            }
            else
            {
                return (DrugVarData.Range - dis) / DrugVarData.Range * DrugVarData.Dentity;
            }
        }*/

    /// <summary>
    /// 获得对应位置的浓度
    /// </summary>
    public float GetDrugDentity(Vector3 pos)
    {
        if (MathTool.IsPointInPolygon(pos, pointList))
        {
            float dentity = DrugAreaConstanst.DRUG_DENTITY * GetHarmRange();
            float dis = Vector3.Distance(transform.position, pos);
            float drugSize;
            if (NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Scene == SceneConstant.HILLS)
                drugSize = DrugAreaConstanst.DRUG_SIZE * GetHarmRange();
            else
                drugSize = DrugAreaConstanst.DRUG_SIZE * GetHarmRange() / TempRadio;
            if (dis <= drugSize * 0.1)
            {
                return ((drugSize - dis) / drugSize) * dentity;
            }
            if(dis <= drugSize * 0.8)
            {
                return ((drugSize - dis) / drugSize) * 0.03f * dentity; 
            }
            return ((drugSize - dis) / drugSize) * 0.000001f * dentity;
        }
        return 0;
    }


    /// <summary>
    /// 设置毒类型的图片和坐标
    /// </summary>
    public override string GetImageSpritePath()
    {
        return CREATE_DRUG_PATH;
    }

/*  /// <summary>
    /// 返回毒区的直径范围
    /// </summary>
    public override float GetHarmRange()
    {
        return DrugVarData.Range * 2;
    }*/


    /// <summary>
    /// 返回毒区的直径范围
    /// </summary>
    public override float GetHarmRange()
    {
        return (DrugVarData.Speed / (float)DrugAreaConstanst.DRUG_SIZE) * HarmAreaBaseConstant.SIZE_RADIO / 2;
    }

    /// <summary>
    /// 更新坐标
    /// </summary>
    public override void SetPosition()
    {
        //transform.position = MathsMgr.PointDistance(windDir, 0,  transform.position);
        transform.position = MathsMgr.PointDistance(windDir, windSp * HarmAreaBaseConstant.SPEED_RADIO ,  transform.position);
        CreatePointRange(transform.position, windDir);
    }

    IEnumerator ISetDrugAreaPoison()
    {
        int count = 0;
        while(true)
        {
            yield return new WaitForSeconds(DrugAreaConstanst.DRUGAREA_UPDATEPOS_TIME);
            count++;
            SetPosition();
            if (count >= NetConfig.SITUATION_SYNC_POS_OFF_TIME)
            {
                SituationSyncLogic.SyncHarm(transform.position.ToCustVect3());
                count = 0;
            }

        }

    }

    /// <summary>
    /// 找到扇形所有区域点
    /// </summary>
    private void CreatePointRange(Vector3 startPos, float angle)
    {
        if (NetVarDataMgr.GetInstance()._NetVarData._TaskEnvVarData.Scene == SceneConstant.HILLS)
        {
            pointList.Clear();
            pointList.Add(MathsMgr.PointDistance(angle - 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MIN_DISTANCE, startPos));
            pointList.Add(MathsMgr.PointDistance(angle + 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MIN_DISTANCE, startPos));
            Vector3 endPos = MathsMgr.PointDistance(angle, (DrugAreaConstanst.DRUG_SIZE * GetHarmRange()) * 0.91f, startPos);
            pointList.Add(MathsMgr.PointDistance(angle - 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MAX_DISTANCE, endPos));
            pointList.Add(MathsMgr.PointDistance(angle + 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MAX_DISTANCE, endPos));
        }
        else
        {
            pointList.Clear();
            pointList.Add(MathsMgr.PointDistance(angle - 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MIN_DISTANCE / TempRadio, startPos));
            pointList.Add(MathsMgr.PointDistance(angle + 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MIN_DISTANCE / TempRadio, startPos));
            Vector3 endPos = MathsMgr.PointDistance(angle, (DrugAreaConstanst.DRUG_SIZE * GetHarmRange()) * 0.91f / TempRadio, startPos);
            pointList.Add(MathsMgr.PointDistance(angle - 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MAX_DISTANCE / TempRadio, endPos));
            pointList.Add(MathsMgr.PointDistance(angle + 90, DrugAreaConstanst.DRUG_SIZE * GetHarmRange() * DrugAreaConstanst.MAX_DISTANCE / TempRadio, endPos));
        }
        
    
    
    }

}


public class DrugAreaConstanst
{
   /// <summary>
   /// 毒区中心浓度
   /// </summary>
    public const float DRUG_DENTITY = 12000;

    /// <summary>
    /// 毒区大小 默认500位基数1
    /// </summary>
    public const int DRUG_SIZE = 500;

    /// <summary>
    /// 中心两端的延长距离
    /// </summary>
    public const float MIN_DISTANCE = 0.12f;

    /// <summary>
    /// 底部两端的延长距离
    /// </summary>
    public const float MAX_DISTANCE = 0.44f;

    /// <summary>
    /// 毒区坐标更新间隔时间
    /// </summary>
    public const int DRUGAREA_UPDATEPOS_TIME = 1;

}