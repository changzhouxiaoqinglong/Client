一、键盘操作
当w键按下时
//Old
        if (Input.GetKeyDown(KeyCode.W)) DoSomething();
//New
        if(Keyboard.current.wKey.wasPressedThisFrame) DoSomething();

当w键抬起时
//Old
        if (Input.GetKeyUp(KeyCode.W)) DoSomething();
//New  
        if (Keyboard.current.wKey.wasReleasedThisFrame) DoSomething();

当w键按着时
//Old
        if (Input.GetKey(KeyCode.W)) DoSomething();
//New
        if (Keyboard.current.wKey.ReadValue() > 0.5f) DoSomething();


二、鼠标操作
获取鼠标位置
//Old
        Vector3 mousePos = Input.mousePosition;
//New
        mousePos = Mouse.current.position.ReadValue();

获取鼠标滚轮
//Old
        float scroll = Input.GetAxis("Scroll Wheel");
//New
        scroll = Mouse.current.scroll.ReadValue().y;

获取鼠标右键抬起
//Old
        if (Input.GetMouseButtonUp(1)) DoSomething();
//New
        if (Mouse.current.rightButton.wasReleasedThisFrame) DoSomething();

获取鼠标中间按着
//Old
        if (Input.GetMouseButton(2)) DoSomething();
//New
        if (Mouse.current.middleButton.ReadValue() > 0.5f) DoSomething();

//两帧之间的偏移
//Old
    Input.GetAxis("Mouse X")
//New
    Debug.Log(Mouse.current.delta.ReadValue());
