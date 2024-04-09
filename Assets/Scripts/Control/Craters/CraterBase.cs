
using UnityEngine;

/// <summary>
/// 弹坑
/// </summary>
public class CraterBase : UnityMono
{

    private const string CRATERICON= "Prefabs/Sprite/MaxMapIcon/CraterArea";

    /// <summary>
    /// 弹坑数据
    /// </summary>
    public CraterVarData VarData
    {
        get;set;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetRotation(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }

    public bool CraterDistanceCar(Vector3 carPos, float distance)
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        if((carPos.x < x + distance && carPos.x > x - distance) && (carPos.z < z + distance && carPos.z > z - distance))
        {
            return true;
        }
        return false;
    }

    public string GetImagePath()
    {
        return CRATERICON;
    }
}
