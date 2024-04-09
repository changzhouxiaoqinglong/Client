
using UnityEngine;
/// <summary>
/// 102车型
/// </summary>
public class Car102 : CarBase
{
    public Transform[] vCWheel;
    public Transform[] nWheel;

    protected override void Update()
    {
        if (vCWheel.Length == 0 || nWheel.Length == 0) return;
        for (int i = 0; i < vCWheel.Length; i++)
        {
            nWheel[i].position = vCWheel[i].position;
            nWheel[i].localEulerAngles = new Vector3(vCWheel[i].localEulerAngles.x, nWheel[i].localEulerAngles.y, nWheel[i].localEulerAngles.z);
        }
    }
}
