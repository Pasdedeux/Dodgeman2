using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;

public class UIMainMenu : BaseUI
{

    private void Awake()
    {
        CurrentUIType.isClearPopUp = false;
        CurrentUIType.uiShowMode = UIShowModeEnum.Normal;
        CurrentUIType.uiTransparent = UITransparentEnum.Penetrating;
        CurrentUIType.uiType = UITypeEnum.Normal;
    }
}
