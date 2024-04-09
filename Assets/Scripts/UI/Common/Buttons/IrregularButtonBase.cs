using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrregularButtonBase : ButtonBase
{

    protected override void Awake()
    {
        base.Awake();
        Image image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.1f;
    }

}
