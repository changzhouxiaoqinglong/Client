
using NWH.VehiclePhysics;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 仪表
/// </summary>

public class Gauges : MonoBehaviour
{
    public VehicleController vehicleController;

    public AnalogGauge analogRpmGauge;
    public AnalogGauge analogSpeedGauge;
    public DigitalGauge digitalGearGauge;

    public DashLight leftBlinker;
    public DashLight rightBlinker;
    public DashLight lowBeam;
    public DashLight highBeam;
    public DashLight TCS;
    public DashLight ABS;
    public DashLight checkEngine;

    /// <summary>
    /// 新版修改
    /// </summary>
    public Text SpeedGauge;
    public Text RpmGauge;
    void Update()
    {
        vehicleController = VehicleInputMgr.GetInstance().VehicleController;
        if (vehicleController != null)
        {           
            if (analogRpmGauge != null) RpmGauge.text = vehicleController.engine.RPM.ToString();
            if (analogSpeedGauge != null) SpeedGauge.text = vehicleController.GetCurSpeed().ToString();


            if (analogRpmGauge != null) analogRpmGauge.Value = vehicleController.engine.RPM;
            if (analogSpeedGauge != null) analogSpeedGauge.Value = vehicleController.GetCurSpeed();
            if (digitalGearGauge != null) digitalGearGauge.stringValue = vehicleController.transmission.Gear.ToString();

            if (leftBlinker != null) leftBlinker.Active = vehicleController.effects.lights.leftBlinkers.On;
            if (rightBlinker != null) rightBlinker.Active = vehicleController.effects.lights.rightBlinkers.On;
            if (lowBeam != null) lowBeam.Active = vehicleController.effects.lights.headLights.On;
            if (highBeam != null) highBeam.Active = vehicleController.effects.lights.fullBeams.On;
            if (TCS != null) TCS.Active = vehicleController.drivingAssists.tcs.active;
            if (ABS != null) ABS.Active = vehicleController.drivingAssists.abs.active;
            if (checkEngine != null)
            {
                if (vehicleController.damage.DamagePercent > 0.5f)
                    checkEngine.Active = true;
                else
                    checkEngine.Active = false;
            }

            if (vehicleController.engine.Starting)
            {
                if (leftBlinker != null) leftBlinker.Active = true;
                if (rightBlinker != null) rightBlinker.Active = true;
                if (lowBeam != null) lowBeam.Active = true;
                if (highBeam != null) highBeam.Active = true;
                if (TCS != null) TCS.Active = true;
                if (ABS != null) ABS.Active = true;
                if (checkEngine != null) checkEngine.Active = true;
            }
        }
        else
        {
            if (analogRpmGauge != null) analogRpmGauge.Value = 0;
            if (analogSpeedGauge != null) analogSpeedGauge.Value = 0;
            if (digitalGearGauge != null) digitalGearGauge.stringValue = "";
            if (leftBlinker != null) leftBlinker.Active = false;
            if (rightBlinker != null) rightBlinker.Active = false;
            if (lowBeam != null) lowBeam.Active = false;
            if (highBeam != null) highBeam.Active = false;
            if (TCS != null) TCS.Active = false;
            if (ABS != null) ABS.Active = false;
            if (checkEngine != null) checkEngine.Active = false;
        }
    }
}
