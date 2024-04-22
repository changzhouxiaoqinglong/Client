
using NWH.VehiclePhysics;
using UnityEngine;
using UnityEngine.InputSystem;
using static NWH.VehiclePhysics.Transmission;

/// <summary>
/// 车辆控制 输入管理类
/// </summary>
public class VehicleInputMgr : InputCtrBase
{
    private static VehicleInputMgr instance;

    public static VehicleInputMgr GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// 自动挡 手动挡
    /// </summary>
    public TransmissionType transmType;

    /// <summary>
    /// Type of input user input.
    /// Standard - standard keyboard, joystick or gamepad input mapped through the input manager.
    /// Mouse - uses mouse position on screen to control throttle/braking and steering.
    /// MouseSteer - uses LMB / RMB for throttle and braking and mouse for steering.
    /// </summary>
    public enum InputType { Standard, Mouse, MouseSteer }

    [Tooltip("Input type. " +
        "Standard - uses standard input manager for all the inputs. " +
        "Mouse - uses mouse position for steering and throttle. " +
        "MouseSteer - uses mouse position for steering, LMB and RMB for braking / throttle.")]
    public InputType inputType = InputType.Standard;


    public enum VerticalInputType { Standard, ZeroToOne, Composite }

    [Tooltip("Vertical input type." +
        "Standard - uses vertical axis in range of [-1, 1] where -1 is maximum braking and 1 maximum accleration." +
        "ZeroToOne - uses vertical axis in range of [0, 1], 0 being maximum braking and 1 maximum acceleration." +
        "Composite - uses separate axes, 'Accelerator' and 'Brake' to set the vertical axis value. Still uses a single vartical axis value [-1, 1] " +
        "throughout the system so applying full brakes and gas simultaneously is not possible.")]
    public VerticalInputType verticalInputType = VerticalInputType.Standard;

    /// <summary>
    /// 控制的车辆
    /// </summary>
    private VehicleController vehicleController = null;

    public VehicleController VehicleController
    {
        set
        {
            vehicleController = value;
        }
        get
        {
            return vehicleController;
        }
    }


    /// <summary>
    /// Tries to get the button value through input manager, if not falls back to hardcoded default value.
    /// </summary>
    private bool TryGetButtonDown(string buttonName, KeyCode altKey)
    {
        //try
        //{
        //    return Input.GetButtonDown(buttonName);
        //}
        //catch
        //{
        //Debug.LogWarning(buttonName + " input binding missing, falling back to default. Check Input section in manual for more info.");
        //    return Input.GetKeyDown(altKey);
        //}
        return false;
    }


    /// <summary>
    /// Tries to get the button value through input manager, if not falls back to hardcoded default value.
    /// </summary>
    private bool TryGetButton(string buttonName, KeyCode altKey)
    {
        //try
        //{
        //    return Input.GetButton(buttonName);
        //}
        //catch
        //{
        //    //Debug.LogWarning(buttonName + " input binding missing, falling back to default. Check Input section in manual for more info.");
        //    return Input.GetKey(altKey);
        //}
        return false;
    }

    private void Awake()
    {
        instance = this;
    }

    //挡位
    void Update()
    {
        
        if (vehicleController == null) return;

        //未启用
        if (!IsEnabled)
        {
          //  print("sds");
            //重置输入值
            vehicleController.input.Horizontal = 0;
            vehicleController.input.Vertical = 0;
            return;
        }

        try
        {
            if (vehicleController == null) return;

            // Manual shift
            if (TryGetButtonDown("ShiftUp", KeyCode.R))
                vehicleController.input.ShiftUp = true;

            if (TryGetButtonDown("ShiftDown", KeyCode.F))
                vehicleController.input.ShiftDown = true;

            vehicleController.input.Clutch = CustomInput.ClutchValue;
           // print(vehicleController.input.Clutch);
            if (vehicleController.transmission.transmissionType == Transmission.TransmissionType.Manual)//手动挡
            {
                try
                {
                    if (AppConfig.CAR_ID == CarIdConstant.ID_102)
                    {
                        //换挡
                        vehicleController.transmission.ShiftInto(CustomInput.ShiftLevel);
                       // print("102换挡");
                    }
                    else
                    {
                        //踩下离合
                        //if (vehicleController.transmission.clutchPedalPressedPercent >= 0.5f)
                        if (vehicleController.input.Clutch >= 0.5f)
                        {
                            //换挡
                            vehicleController.transmission.ShiftInto(CustomInput.ShiftLevel);
                        }
                    }

                    //#if DEBUG
                    //挂挡 键盘调试
                    if (Keyboard.current.digit1Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(1);
                    }
                    else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(2); 
                    }
                    else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(3); 
                    }
                    else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(4);
                    }
                    else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(5);
                    }
                    else if (Keyboard.current.digit0Key.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(0);
                    }
                    else if (Keyboard.current.backquoteKey.wasPressedThisFrame)
                    {
                        vehicleController.transmission.ShiftInto(-1);
                    }
//#endif
                }
                catch
                {
                    Debug.LogWarning("Some of the gear changing inputs might not be assigned in the input manager. Direct gear shifting " +
                        "will not work.");
                }
            }

            // Vertical axis
            //if (verticalInputType == VerticalInputType.Standard)
            //{
            //    //vertical = Input.GetAxisRaw("Vertical");
            //}
            //else if (verticalInputType == VerticalInputType.ZeroToOne)
            //{
            //    //vertical = (Mathf.Clamp01(Input.GetAxisRaw("Vertical")) - 0.5f) * 2f;
            //}
            //else if (verticalInputType == VerticalInputType.Composite)
            //{
            //    //float accelerator = Mathf.Clamp01(Input.GetAxisRaw("Accelerator"));
            //    //float brake = Mathf.Clamp01(Input.GetAxisRaw("Brake"));
            //    //vertical = accelerator - brake;
            //}
            vehicleController.input.Horizontal = CustomInput.DriveHorizontal;
           // print(CustomInput.DriveHorizontal);
            if (vehicleController.tracks.trackedVehicle)
            {
                if (CustomInput.DriveVertical <= 0.05f)
                {
                    vehicleController.input.Horizontal = 0;
                }
            }
            vehicleController.input.Vertical = CustomInput.DriveVertical;
            //vehicleController.input.Vertical = 111;
           // print(CustomInput.DriveVertical);
            // Engine start/stop发动机启动/熄火
            vehicleController.engine.SetRunInputValue(CustomInput.QiDongValue);
           // print(CustomInput.QiDongValue);   //bug 发动机没启动好像也能开
           // print(vehicleController.engine.IsRunning);
            //if(Input.GetKeyDown(KeyCode.Space))
            //    vehicleController.engine.SetRunInputValue(true);

            // Handbrake 手刹
            vehicleController.input.Handbrake = CustomInput.HandBrakeValue;

            //try
            //{
            //    vehicleController.input.Handbrake = Input.GetAxis("Handbrake");
            //}
            //catch
            //{
            //Debug.LogWarning("Handbrake axis not set up, falling back to default (Space).");
            //vehicleController.input.Handbrake = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            //    vehicleController.input.Handbrake = (Keyboard.current.spaceKey.ReadValue() > 0.5f) ? 1f : 0f;
            //}

            // Clutch 离合
            if (!vehicleController.transmission.automaticClutch)
            {
                //try
                //{
                //    vehicleController.input.Clutch = Input.GetAxis("Clutch");
                //}
                //catch
                //{
                //    Debug.LogError("Clutch is set to manual but the required axis 'Clutch' is not set. " +
                //        "Please set the axis inside input manager to use this feature.");
                //    vehicleController.transmission.automaticClutch = true;
                //}
            }

            // Lights
            vehicleController.input.leftBlinker = CustomInput.LeftBlinkerValue;
            if (vehicleController.input.leftBlinker) vehicleController.input.rightBlinker = false;

            vehicleController.input.rightBlinker = CustomInput.RightBlinkerValue;
            if (vehicleController.input.rightBlinker) vehicleController.input.leftBlinker = false;
            //if (TryGetButtonDown("LeftBlinker", KeyCode.Z))
            //{
            //    vehicleController.input.leftBlinker = !vehicleController.input.leftBlinker;
            //    if (vehicleController.input.leftBlinker) vehicleController.input.rightBlinker = false;
            //}
            //if (TryGetButtonDown("RightBlinker", KeyCode.X))
            //{
            //    vehicleController.input.rightBlinker = !vehicleController.input.rightBlinker;
            //    if (vehicleController.input.rightBlinker) vehicleController.input.leftBlinker = false;
            //}
            if (TryGetButtonDown("Lights", KeyCode.L)) vehicleController.input.lowBeamLights = !vehicleController.input.lowBeamLights;
            if (TryGetButtonDown("FullBeamLights", KeyCode.K)) vehicleController.input.fullBeamLights = !vehicleController.input.fullBeamLights;
            if (TryGetButtonDown("HazardLights", KeyCode.J))
            {
                vehicleController.input.hazardLights = !vehicleController.input.hazardLights;
                vehicleController.input.leftBlinker = false;
                vehicleController.input.rightBlinker = false;
            }

            // Horn 喇叭
            //vehicleController.input.horn = TryGetButton("Horn", KeyCode.H);
            vehicleController.input.horn = CustomInput.HornValue;

            // Raise trailer flag if trailer attach detach button pressed.
            if (TryGetButtonDown("TrailerAttachDetach", KeyCode.T))
                vehicleController.input.trailerAttachDetach = true;

            // Manual flip over
            if (vehicleController.flipOver.manual)
            {
                //try
                //{
                //    // Set manual flip over flag to true if vehicle is flipped over, otherwise ignore
                //    if (Input.GetButtonDown("FlipOver") && vehicleController.flipOver.flippedOver)
                //        vehicleController.input.flipOver = true;
                //}
                //catch
                //{
                //    Debug.LogError("Flip over is set to manual but 'FlipOver' input binding is not set. Either disable manual flip over or set 'FlipOver' binding.");
                //}
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("One or more of the required inputs has not been set. Check NWH Vehicle Physics README for more info or add the binding inside Unity input manager.");
            Debug.LogWarning(e);
        }
    }

    public override Transform GetTarget()
    {
        return vehicleController == null ? null : vehicleController.transform;
    }
}
