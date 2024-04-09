
using UnityEngine;

public static class VectorExtension
{
    public static CustVect3 ToCustVect3(this Vector3 vect3)
    {
        return new CustVect3(vect3.x, vect3.y, vect3.z);
    }

    public static CustVect2 ToCustVect2(this Vector2 vect2)
    {
        return new CustVect2(vect2.x, vect2.y);
    }
}
