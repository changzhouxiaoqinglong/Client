using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldControl : MonoBehaviour
{
    private InputField input;

    private void Start()
    {
        input = GetComponent<InputField>();
    }

    public void OnEndEdit()
    {
        if (input.text == "") return;
        bool isDigitOrletter = Regex.IsMatch(input.text, @"^[a-zA-Z0-9]+$");
        if (!isDigitOrletter)
        {
            UIMgr.GetInstance().ShowToast("输入的不是数字和字母");
            input.text = "";
            input.ActivateInputField();
        }
    }

   
}
