using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line3dControl : MonoBehaviour
{
    public static Line3dControl Instance;

    private GameObject lineModel;

    private List<Vector3> listPoint = new List<Vector3>();

    private const float lineDistance = 40f;

    private void Awake()
    {
        Instance = this;
        lineModel = transform.GetChild(0).gameObject;
    }


    /// <summary>
    /// 创建线
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="radio"></param>
    public void CreateLine(Transform[] lines,Vector2 radio)
    {
        Debug.Log("TerrainY" + (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.curTerrain.transform.position.y);
        float posY = (SceneMgr.GetInstance().curScene as Train3DSceneCtrBase).terrainChangeMgr.curTerrain.transform.position.y + 380;
        for (int i = 1; i < lines.Length; i++)
        {
            RectTransform lineRect = lines[i].GetComponent<RectTransform>();
            GameObject obj = Instantiate(lineModel,transform);
            Vector2 lineSize = lineRect.sizeDelta;
            Vector2 linePos = lineRect.anchoredPosition;
            Vector3 lineAngle = new Vector3(0, -lineRect.localEulerAngles.z, 0);
            Vector3 lineScale = new Vector3(lineSize.x / 10 * radio.x, lineRect.localScale.y, lineRect.localScale.z);
            obj.transform.localPosition = new Vector3(radio.x * linePos.x - 5, posY, radio.y * linePos.y);
            obj.transform.localEulerAngles = lineAngle;
            obj.transform.localScale = lineScale;
            if (i == 1)
                CreateFristPath(new Vector3(obj.transform.localPosition.x,obj.transform.localPosition.y,obj.transform.localPosition.z-10), obj.transform.localScale.x * 10, lineDistance, obj.transform.localEulerAngles.y);
            else
                CreateAreaPath(obj.transform.localPosition, obj.transform.localScale.x * 10, lineDistance, obj.transform.localEulerAngles.y);
        }    
    }


    /// <summary>
    /// 销毁线
    /// </summary>
    public void DestroyLine()
    {
        for(int i = 1; i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        listPoint.Clear();
    }


    /// <summary>
    /// 创建路径上的点
    /// </summary>
    /// <param name="startPos">开始的位置</param>
    /// <param name="size">线的长度</param>
    /// <param name="distance">偏航距离</param>
    /// <param name="angle">偏航角度</param>
    private void CreateAreaPath(Vector3 startPos,float size,float distance, float angle)
    {
        listPoint.Add(MathsMgr.PointDistance(startPos, distance, angle - 90));
        listPoint.Add(MathsMgr.PointDistance(startPos, distance, angle + 90));
        Vector3 endPos = MathsMgr.PointDistance(startPos, size, angle);
        listPoint.Add(MathsMgr.PointDistance(endPos, distance, angle - 90));
        listPoint.Add(MathsMgr.PointDistance(endPos, distance, angle + 90));
    }

    private void CreateFristPath(Vector3 startPos,float size,float distance,float angle)
    {
        listPoint.Add(MathsMgr.PointDistance(startPos, distance, angle - 90));
        listPoint.Add(MathsMgr.PointDistance(startPos, distance, angle + 90));
        Vector3 endPos = MathsMgr.PointDistance(startPos, size, angle);
        listPoint.Add(MathsMgr.PointDistance(endPos, distance, angle - 90));
        listPoint.Add(MathsMgr.PointDistance(endPos, distance, angle + 90));
    }

    /// <summary>
    /// 获取路径数组
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetListPoint()
    {
        return listPoint;
    }
}
