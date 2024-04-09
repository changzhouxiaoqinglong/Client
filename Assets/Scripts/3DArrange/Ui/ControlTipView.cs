using NWH.VehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlTipView : ArrangeViewBase<ChoiceConfirmViewModel>
{
    //是
    private ButtonBase yesBtn;
    private Slider slider;
    private CameraMove cameraObj;

    protected override void Awake()
    {
        base.Awake();
        yesBtn = transform.Find("Content/YesBtn").GetComponent<ButtonBase>();
        slider = transform.Find("Content/desc/Slider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        if (GameObject.Find("Arrange/Camera").GetComponent<CameraMove>() != null)
        {
            slider.gameObject.SetActive(true);
            cameraObj = GameObject.Find("Arrange/Camera").GetComponent<CameraMove>();
            slider.value = cameraObj.moveSpeed;
            slider.onValueChanged.AddListener(ChangeCameraSpeed);
        }
        yesBtn.RegistClick(OnClickYesBtn);
    }
    private void ChangeCameraSpeed(float value)
    {
        if (value == 0)
        {
            value = 0.1f;
        }
        cameraObj.moveSpeed = value;
    }
    private void OnClickYesBtn(GameObject obj)
    {
        ArrangeUiMgr.GetInstance().CloseView(ArrangeViewType.ControlTipView);
    }


}
