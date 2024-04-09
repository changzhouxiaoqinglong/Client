using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCar02B : VirtualCarBase
{
    protected override void InitVirtualDevices()
    {
        base.InitVirtualDevices();
        //车载侦毒器
        virtualDevices.Add(new VirtualCarDrugPoison02B());
    }

}
