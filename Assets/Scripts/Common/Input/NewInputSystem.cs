using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 新的输入系统
/// </summary>
public class NewInputSystem : MonoSingleTon<NewInputSystem>
{
    private void Update()
    {
//#if DEBUG
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CustomInput.QiDongValue = !CustomInput.QiDongValue;
        }
//#endif
    }

    /// <summary>
    /// WASD
    /// </summary>
    public void WASD(InputAction.CallbackContext context)
    {      
        var v = context.ReadValue<Vector2>();
        //print(v.y);
        //if (context.started) { Debug.Log(string.Format("方向 Started " + v.x.ToString() + " " + v.y.ToString())); }
        //if (context.performed) { Debug.Log(string.Format("performed " + v.x.ToString() + " " + v.y.ToString())); }
        //if (context.canceled) { Debug.Log(string.Format("方向 canceled " + v.x.ToString() + " " + v.y.ToString())); }
        CustomInput.Horizontal = v.x;
        CustomInput.Vertical = v.y;
//#if DEBUG
        CustomInput.DriveHorizontal = v.x;
        CustomInput.DriveVertical = v.y;
//#endif
    }


    /// <summary>
    /// 启动 
    /// </summary>
    public void Qidong(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("启动 Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("启动 Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("启动 Canceled:{0}", v)); }

        if (v > 0.5f)
        {
            CustomInput.QiDongValue = true;
        }
        else
        {
            CustomInput.QiDongValue = false;
        }
    }

    /// <summary>
    /// 离合
    /// </summary>
    public void Clutch(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("离合 Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("离合 Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("离合 Canceled:{0}", v)); }
        CustomInput.ClutchValue = v;
    }

    /// <summary>
    /// 一档
    /// </summary>
    public void Dang1(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }
        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = 1;
        }
    }

    /// <summary>
    /// 二档
    /// </summary>
    public void Dang2(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }

        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = 2;
        }
    }

    /// <summary>
    /// 三档
    /// </summary>
    public void Dang3(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }

        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = 3;
        }
    }

    /// <summary>
    /// 四档
    /// </summary>
    public void Dang4(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }

        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = 4;
        }
    }

    /// <summary>
    /// 五档
    /// </summary>
    public void Dang5(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }

        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = 5;
        }
    }

    /// <summary>
    /// 倒档
    /// </summary>
    public void DangReverse(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format(" Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format(" Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format(" Canceled:{0}", v)); }

        if (context.canceled)
        {
            CustomInput.ShiftLevel = 0;
        }
        else
        {
            CustomInput.ShiftLevel = -1;
        }
    }

    /// <summary>
    /// 油门 
    /// </summary>
    public void YouMeng(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("油门 Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("油门 Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("油门 Canceled:{0}", v)); }

        CustomInput.DriveVertical = v;
    }

    /// <summary>
    /// 刹车
    /// </summary>
    public void ShaChe(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("刹车 Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("刹车 Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("刹车 Canceled:{0}", v)); }

        CustomInput.DriveVertical = -v;
    }

    /// <summary>
    /// 方向
    /// </summary>
    public void FangXiang(InputAction.CallbackContext context)
    {
        print("FangXiang");
        var v = context.ReadValue<Vector2>();
        //if (context.started) { Debug.Log(string.Format("方向 Started " + v.x.ToString() + " " + v.y.ToString())); }
        //if (context.performed) { Debug.Log(string.Format("方向 performed " + v.x.ToString() + " " + v.y.ToString())); }
        //if (context.canceled) { Debug.Log(string.Format("方向 canceled " + v.x.ToString() + " " + v.y.ToString())); }
        CustomInput.DriveHorizontal = v.x;
    }

    /// <summary>
    /// 手刹
    /// </summary>
    public void HandBrake(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("Canceled:{0}", v)); }
        Debug.Log(" NewInputSystem 手刹");
        CustomInput.HandBrakeValue = v;
    }

    /// <summary>
    /// 左转向灯
    /// </summary>
    public void LeftBlinker(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("Canceled:{0}", v)); }

        if (v > 0.5f)
        {
            CustomInput.LeftBlinkerValue = true;
        }
        else
        {
            CustomInput.LeftBlinkerValue = false;
        }
    }

    /// <summary>
    /// 右转向灯
    /// </summary>
    public void RightBlinker(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("Canceled:{0}", v)); }

        if (v > 0.5f)
        {
            CustomInput.RightBlinkerValue = true;
        }
        else
        {
            CustomInput.RightBlinkerValue = false;
        }
    }

    /// <summary>
    /// 喇叭
    /// </summary>
    public void Horn(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        //if (context.started) { Debug.Log(string.Format("Started:{0}", v)); }
        //if (context.performed) { Debug.Log(string.Format("Performed:{0}", v)); }
        //if (context.canceled) { Debug.Log(string.Format("Canceled:{0}", v)); }
        if (context.performed)
        {
            CustomInput.HornValue = true;
        }
        else
        {
            CustomInput.HornValue = false;
        }
    }
}
