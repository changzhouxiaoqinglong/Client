using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmView : ArrangeViewBase<ChoiceConfirmViewModel>
{
    //是
    private ButtonBase yesBtn;
    protected override void Awake()
    {
        base.Awake();
        yesBtn = transform.Find("Content/YesBtn").GetComponent<ButtonBase>();
        yesBtn.RegistClick(OnClickYesBtn);
    }
    private void OnClickYesBtn(GameObject obj)
    {
        ArrangeUiMgr.GetInstance().CloseView(ArrangeViewType.ConfirmView);
        
    }


}
