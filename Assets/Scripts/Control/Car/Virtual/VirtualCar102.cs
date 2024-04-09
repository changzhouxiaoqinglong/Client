
public class VirtualCar102 : VirtualCarBase
{
    protected override void InitVirtualDevices()
    {
        base.InitVirtualDevices();
        //车载辐射仪
        virtualDevices.Add(new VirtualCarRadiom102());
    }
}
