using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Assets.Scripts;

public class UIMainMenu : BaseUI
{
    public Sprite sound_On, sound_Off;
    private Button _btnLevelsChoose, _btnSound, _btnStart;


    private void Awake()
    {
        CurrentUIType.uiType = UITypeEnum.Fixed;
        CurrentUIType.uiShowMode = UIShowModeEnum.Normal;

        InitElements();

    }

    private void InitElements()
    {
        _btnLevelsChoose = transform.Find( "Button_Levels" ).GetComponent<Button>();
        _btnSound = transform.Find( "Button_Sound" ).GetComponent<Button>();
        _btnStart = transform.Find( "Button_Start" ).GetComponent<Button>();
    }

    private void OnEnable()
    {
        _btnLevelsChoose.onClick.AddListener( () => { OnClickChooseLevel(); } );
        _btnSound.onClick.AddListener( () => { OnClickSound(); } );
        _btnStart.onClick.AddListener( () => { OnClickStart(); } );

        RefreshSoundBtn();
       
    }

    private void OnDisable()
    {
        _btnLevelsChoose.onClick.RemoveAllListeners();
        _btnSound.onClick.RemoveAllListeners();
        _btnStart.onClick.RemoveAllListeners();
    }

    private void OnClickStart()
    {
        UIManager.Instance.Close( GlobalDefine.UINames.MainMenu );

        --DataModel.Instance.CurLevel;
        PlayerController.Instance.FakeMoveOrder( Vector3.left );
    }

    private void OnClickSound()
    {
        DataModel.Instance.SoundOpen = !DataModel.Instance.SoundOpen;
        RefreshSoundBtn();
    }

    private void OnClickChooseLevel()
    {
        UIManager.Instance.Show( GlobalDefine.UINames.LevelChoose );
    }


    private void RefreshSoundBtn()
    {
        _btnSound.GetComponent<Image>().sprite = DataModel.Instance.SoundOpen ? sound_On : sound_Off;
    }
}
