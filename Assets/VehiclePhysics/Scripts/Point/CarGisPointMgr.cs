using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGisPointMgr 
{    

    /// <summary>
    /// 左上角经纬度
    /// </summary>
    private Vector2 topLeftGis;

    /// <summary>
    /// 右下角经纬度
    /// </summary>
    private Vector2 bottomRightGis;

    /// <summary>
    /// 左上角地图坐标
    /// </summary>
    private Vector3 topLeftPos;

    /// <summary>
    /// 右下角地图坐标
    /// </summary>
    private Vector3 bottomRightPos;

    /// <summary>
    /// 三维地图与二维地图的比值
    /// </summary>
    private Vector2 radio;


    /// <summary>
    /// 经纬度转unity坐标
    /// </summary>
    public Vector3 GetTerrainPos(Vector2 gisPos)
    {
        GetGisAndTerrainRadio();
        Vector3 tempVector3;
        tempVector3 = new Vector3((gisPos.x - topLeftGis.x) * radio.x, 50, topLeftPos.z + ((gisPos.y - topLeftGis.y) * radio.y));
        return tempVector3;
    }


    /// <summary>
    /// unity坐标转经纬度坐标
    /// </summary>
    public Vector2 GetGisPos(Vector3 terrainPos)
    {
        GetGisAndTerrainRadio();
        Vector2 tempVector2;
        tempVector2 = new Vector2(topLeftGis.x+(terrainPos.x - topLeftPos.x) / radio.x, topLeftGis.y + (terrainPos.z - topLeftPos.z) / radio.y);
        return tempVector2;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public CarGisPointMgr(Vector2 startGis, Vector2 endGis, Vector3 terrainSize)
    {
        topLeftGis = startGis;
        bottomRightGis = endGis;
        topLeftPos = new Vector3(0, 0, terrainSize.z);
        bottomRightPos = new Vector3(terrainSize.x, 0, 0);
    }

    /// <summary>
    /// 获取二维地图和三维地图的比值
    /// </summary>
    private void GetGisAndTerrainRadio()
    {
        float gisxDistance = bottomRightGis.x - topLeftGis.x;
        float gisyDistance = bottomRightGis.y - topLeftGis.y;
        float terrainxDistance = bottomRightPos.x - topLeftPos.x;
        float terrainyDistance = bottomRightPos.z - topLeftPos.z;
        radio = new Vector2(terrainxDistance/gisxDistance, terrainyDistance/gisyDistance);



    }

}
