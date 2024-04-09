using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsMgr
{
    /// <summary>
    /// 计算两点之间的线段长度
    /// </summary>
    /// <param name="startPos">起始点</param>
    /// <param name="endPos">结束点</param>
    /// <returns></returns>
    public static float PointDistance(Vector2 startPos,Vector2 endPos)
    {
        float disX = Mathf.Abs(startPos.x - endPos.x);
        float disY = Mathf.Abs(startPos.y - endPos.y);
        return Mathf.Sqrt(disX * disX + disY * disY);
    }

    public static float PointDistance1(Vector3 startPos,Vector3 endPos)
    {
        float disX = Mathf.Abs(startPos.x - endPos.x);
        float disZ = Mathf.Abs(startPos.z - endPos.z);
        return Mathf.Sqrt(disX * disX + disZ * disZ);
    }

    /// <summary>
    /// 计算两点之间的角度
    /// </summary>
    /// <param name="startPos">起始点</param>
    /// <param name="endPos">结束点</param>
    /// <returns></returns>
    public static float PointAngle(Vector2 startPos,Vector2 endPos)
    {
        float angle = 0;
        float disOne = Mathf.Abs(startPos.y - endPos.y);
        float disTwo = Mathf.Abs(startPos.x - endPos.x);
        angle = Mathf.Atan(disOne/disTwo) * 180 / Mathf.PI;
        if (startPos.y >= endPos.y)
        {
            angle = -angle;
        }
        if(startPos.x >= endPos.x)
        {
            angle = -180 - angle;
        }
        return angle;
    }

    /// <summary>
    /// 计算移动后的坐标
    /// </summary>
    public static Vector3 PointDistance(Vector3 startPos,float distance,float angle)
    {
        Vector3 endPos;
        float radians = AngleOrRadians(angle);  
        float x = Mathf.Cos(radians) * distance;
        float z = Mathf.Sin(radians) * distance;
        endPos = new Vector3(startPos.x+x,0,startPos.z-z);
        return endPos;
    }

    public static Vector3 PointDistance(float angle,float distance,Vector3 startPos)
    {
        Vector3 endPos;
        float radians = AngleOrRadians(angle);
        float x = Mathf.Cos(radians) * distance;
        float z = Mathf.Sin(radians) * distance;
        endPos = new Vector3(startPos.x + x, 0, startPos.z + z);
        return endPos;
    }

    private static float AngleOrRadians(float angle)
    {
        float radians = angle * Mathf.PI / 180;
        return radians;
    }

    public static bool MousePosChangeUiPos(RectTransform mapTrans,Vector2 mousePos, out Vector2 uiPos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(mapTrans, mousePos, null, out uiPos);
    }

    /// <summary>
    /// 2维ui大小和3维terrain大小的比值
    /// </summary>
    /// <param name="uiSize">ui大小</param>
    /// <param name="terrainSize">terrain大小</param>
    /// <returns></returns>
    public static Vector2 UiTerrainRadio(Vector2 uiSize,Vector3 terrainSize)
    {
        float x = uiSize.x / terrainSize.x;
        float y = uiSize.y / terrainSize.z;
        return new Vector2(x,y);
    }


    /// <summary>
    /// 3维terrain的大小和2维ui大小的比值
    /// </summary>
    /// <param name="terrainSize">terrain大小</param>
    /// <param name="uiSize">ui大小</param>
    /// <returns></returns>
    public static Vector2 UiTerrainRadio(Vector3 terrainSize,Vector2 uiSize)
    {
        float x = terrainSize.x / uiSize.x;
        float y = terrainSize.z / uiSize.y;
        return new Vector2(x,y);
    }
    
    /// <summary>
    /// 时间与表盘指针度数
    /// </summary>
    /// <returns></returns>
    public static float TimeAngle(float secondAngle,float time)
    {
        return secondAngle*time;
    }

}
