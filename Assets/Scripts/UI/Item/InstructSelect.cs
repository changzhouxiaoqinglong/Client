
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 车长指令下发选择项
/// </summary>
public class InstructSelect : MonoBehaviour
{
    /// <summary>
    /// 席位号
    /// </summary>
    public int seatId;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public bool IsBeSelect()
    {
        return toggle.isOn;
    }
}
