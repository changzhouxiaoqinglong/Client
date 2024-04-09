using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point3dControl : MonoBehaviour
{
    public static Point3dControl Instance;

    private GameObject pointModel;

    private List<CustVect3> pointGisList = new List<CustVect3>();

    private void Awake()
    {
        Instance = this;
        pointModel = transform.GetChild(0).gameObject;
    }


    /// <summary>
    /// 创建点
    /// </summary>
    public void CreatePoint(Transform[] point,Vector2 radio,Vector3 carPos)
    {
        float posY = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.curTerrain.transform.position.y + 380;
        Vector3 carTerrainPos = new Vector3(radio.x * carPos.x, 70, radio.y * carPos.y);
        carTerrainPos = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.gisPointMgr.GetGisPos(carTerrainPos);
        pointGisList.Add(carTerrainPos.ToCustVect3());
        for (int i = 1; i < point.Length; i++)
        {
            GameObject cube = Instantiate(pointModel, transform);
            Vector2 pointPos = point[i].GetComponent<RectTransform>().anchoredPosition;
            Vector3 temp = new Vector3(radio.x * pointPos.x, posY, radio.y * pointPos.y);
            cube.transform.localPosition = temp;
            temp = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.gisPointMgr.GetGisPos(temp);
            pointGisList.Add(temp.ToCustVect3());
        }
        SituationSyncLogic.SyncRoute(pointGisList);
        pointGisList.Clear();
    }

    /// <summary>
    /// 删除点
    /// </summary>
    public void DestroyPoint()
    {
        for(int i = 1;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
