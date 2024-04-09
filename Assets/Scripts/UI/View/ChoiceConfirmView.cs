using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceConfirmView : ViewBase<ChoiceConfirmViewModel>
{
    //是
    private ButtonBase yesBtn;
    //否
    private ButtonBase noBtn;

    protected override void Awake()
    {
        base.Awake();
        yesBtn = transform.Find("Content/yesBtn").GetComponent<ButtonBase>();
        noBtn = transform.Find("Content/noBtn").GetComponent<ButtonBase>();
        yesBtn.RegistClick(OnClickNoBtn);
        noBtn.RegistClick(OnClickYesBtn);
    }
    private void OnClickNoBtn(GameObject obj)
    {
        UIMgr.GetInstance().CloseView(ViewType.ChoiceConfirmView);
    }
    private void OnClickYesBtn(GameObject obj)
    {
        UIMgr.GetInstance().CloseView(ViewType.ChoiceConfirmView);
        TaskMgr.GetInstance().ResportEndTask();
    }

}
