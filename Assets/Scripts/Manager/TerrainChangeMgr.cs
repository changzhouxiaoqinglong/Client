
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainChangeMgr
{
    /// <summary>
    /// 当前地形
    /// </summary>
    public Terrain curTerrain;

    [SerializeField]
    /// <summary>
    /// 左上角经纬度
    /// </summary>
    public CustVect2 TopLeftGis;

    /// <summary>
    /// 右下角经纬度
    /// </summary>
    public CustVect2 BottomRightGis;

    /// <summary>
    /// 经纬度坐标转换
    /// </summary>
    public CarGisPointMgr gisPointMgr;


    // Start is called before the first frame update
    public void Start()
    {
        gisPointMgr = new CarGisPointMgr(TopLeftGis.ToVector2(), BottomRightGis.ToVector2(), GetCurTerrainSize());
    }

    /// <summary>
    /// 地形下降
    /// </summary>
    /// <param name="pos">指定位置</param>
    /// <param name="radius">下降范围半径</param>
    /// <param name="opacity">下降深度</param>
    public void TerrainHeightDown(Vector3 pos, float radius, float opacity)
    {
        TerrainChange.TerrainModule.ChangeHeight(pos, radius, opacity, false, true);
        Debug.Log("TeainDownCount:" + TerrainChange.downCount);
    }

    public void TerrainHeightDown(List<CraterBase> craters, float radius, float opacity)
    {
        foreach (CraterBase crater in craters)
        {
            TerrainChange.TerrainModule.ChangeHeight(crater.transform.position, radius, opacity, false, true);
            Debug.Log("弹坑下陷位置：" + crater.transform.position);
            TerrainChange.downCount++;
        }
    }

    /// <summary>
    /// 获取当前terrain大小
    /// </summary>
    public Vector3 GetCurTerrainSize()
    {
        return curTerrain.GetComponent<Collider>().bounds.size;
    }

    /// <summary>
    /// 通过经纬度获得地图上对应的位置
    /// </summary>
    public Vector3 GetTerrainPosByGis(Vector2 gis)
    {
        Vector3 vector3 = gisPointMgr.GetTerrainPos(gis);
        return GetTerrainPosByPos(vector3);
    }

    /// <summary>
    /// 通过untiy位置获得地形上对应的位置
    /// </summary>
    public Vector3 GetTerrainPosByPos(Vector3 pos)
    {
        Vector3 origin = new Vector3(pos.x, 10000, pos.z);
        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit[] hitinfo = Physics.RaycastAll(ray, 20000, 1<<LayerMask.NameToLayer("Terrain"));
        if (hitinfo.Length > 0)
        {
            return hitinfo[0].point;
        }
        else
        {
            Logger.LogWarning("GetWorldPosByGis: no raycast terrian!!!");
            return pos;
        }
    }

    /// <summary>
    /// 获得对应位置高程
    /// </summary>
    /// <param name="pos"></param>
    public float GetEvelationByPos(Vector3 pos)
    {
        float terrianEvelat = SceneMgr.GetInstance().CurSceneData.Altitude;
        return pos.y - curTerrain.transform.position.y + terrianEvelat;
    }
}
