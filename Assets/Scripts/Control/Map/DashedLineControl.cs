using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLineControl : MonoBehaviour
{
    public static DashedLineControl Instance;

    private GameObject dashedLineModel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dashedLineModel = transform.GetChild(0).gameObject;
    }

    public Vector3 CreateDashedLine(Vector2 uiRadio,Vector3 startPos,Vector3 endPos,float angle)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 tempPos = startPos;
        float tempDistance = 10 / uiRadio.x;
        while (true)
        {
            if (MathsMgr.PointDistance1(tempPos, endPos) <= tempDistance)
                break;
            GameObject gameObj = Instantiate(dashedLineModel, transform);
            gameObj.SetActive(true);
            gameObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiRadio.x * tempPos.x, uiRadio.y * tempPos.z);
            gameObj.transform.localEulerAngles = new Vector3(0, 0, angle);
            tempPos = MathsMgr.PointDistance(angle, tempDistance * 2, tempPos);
        }
        return tempPos;

    }


}
