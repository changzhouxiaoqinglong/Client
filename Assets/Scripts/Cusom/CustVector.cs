using UnityEngine;

/// <summary>
/// 自定义vector3
/// </summary>
[System.Serializable]
public class CustVect3
{
    public double x;
    public double y;
    public double z;

    public CustVect3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return string.Format($"({x},{y},{z})");
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector3 vect3Obj)
        {
            return vect3Obj.x == x && vect3Obj.y == y && vect3Obj.z == z;
        }
        if (obj is CustVect3 custVect3)
        {
            return custVect3.x == x && custVect3.y == y && custVect3.z == z;
        }
        return false;
    }
}


/// <summary>
/// 自定义vector2
/// </summary>
[System.Serializable]
public class CustVect2
{
    public double x;
    public double y;

    public CustVect2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format($"({x},{y})");
    }
}

public static class CustVectorExtension
{
    public static Vector3 ToVector3(this CustVect3 custVect3)
    {
        return new Vector3((float)custVect3.x, (float)custVect3.y, (float)custVect3.z);
    }

    public static Quaternion ToQuaternion(this CustVect3 custVect3)
    {
        return Quaternion.Euler((float)custVect3.x, (float)custVect3.y, (float)custVect3.z);
    }

    public static Vector2 ToVector2(this CustVect3 custVect3)
    {
        return new Vector2((float)custVect3.x, (float)custVect3.y);
    }

    public static Vector2 ToVector2(this CustVect2 custVect2)
    {
        return new Vector2((float)custVect2.x, (float)custVect2.y);
    }
}

