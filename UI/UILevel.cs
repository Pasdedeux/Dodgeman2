using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.UI;
using System;
using Assets.Scripts;

public class UILevel : BaseUI {

    private Image _imageTipsBg;
    private Text _textTipsCount;
    private Button _btnChoose, _btnRestart, _btnTips;

    private void Awake()
    {
        CurrentUIType.uiType = UITypeEnum.Fixed;
        CurrentUIType.uiShowMode = UIShowModeEnum.Normal;

        InitElements();
    }

    private void InitElements()
    {
        _btnTips = transform.Find( "Button_Tips" ).GetComponent<Button>();
        _btnRestart = transform.Find( "Button_ReStart" ).GetComponent<Button>();
        _btnChoose = transform.Find( "Button_Choose" ).GetComponent<Button>();
        _imageTipsBg = _btnTips.transform.Find( "Image" ).GetComponent<Image>();
        _textTipsCount = transform.Find( "Text_TipsNum" ).GetComponent<Text>();
    }

    private void OnEnable()
    {
        _btnRestart.onClick.AddListener( () => { OnClickRestart(); } );
        _btnChoose.onClick.AddListener( () => { OnClickChoose(); } );
        _btnTips.onClick.AddListener( () => { OnClickTips(); } );

        RefreshTipsNum();
    }


    private void OnDisable()
    {
        _btnTips.onClick.RemoveAllListeners();
        _btnRestart.onClick.RemoveAllListeners();
        _btnChoose.onClick.RemoveAllListeners();
    }



    private void OnClickRestart()
    {
        UIManager.Instance.Close( GlobalDefine.UINames.Level );
        GameController.Instance.EnterLevel( DataModel.Instance.CurLevel );
    }

    private void OnClickChoose()
    {
        UIManager.Instance.Show( GlobalDefine.UINames.LevelChoose );
    }

    private void OnClickTips()
    {
        throw new NotImplementedException();
    }

    private void RefreshTipsNum()
    {
        _textTipsCount.text = DataModel.Instance.TipsNum.ToString();
    }

   
}
