using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;

public class UIChooseLevel : BaseUI {

    private void Awake()
    {
        CurrentUIType.isClearPopUp = false;
        CurrentUIType.uiShowMode = UIShowModeEnum.Unique;
        CurrentUIType.uiTransparent = UITransparentEnum.NoPenetratingLow;
        CurrentUIType.uiType = UITypeEnum.Normal;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
