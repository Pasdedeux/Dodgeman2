﻿using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseLevel : BaseUI {

    public Sprite sound_On, sound_Off;
    private Button _btnHome, _btnSound;
    private LoopVerticalScrollRect _verticalScrollRect;

    private void Awake()
    {
        CurrentUIType.uiType = UITypeEnum.PopUp;
        CurrentUIType.uiShowMode = UIShowModeEnum.Unique;
        CurrentUIType.uiTransparent = UITransparentEnum.Penetrating;

        InitElements();

    }

    private void InitElements()
    {
        _btnHome = transform.Find( "Button_Home" ).GetComponent<Button>();
        _btnSound = transform.Find( "Button_Sound" ).GetComponent<Button>();
        _verticalScrollRect = transform.Find( "Loop Vertical Scroll Rect" ).GetComponent<LoopVerticalScrollRect>();
        _verticalScrollRect.EndDragCallBack = EndDragAction;
    }

    private void OnEnable()
    {
        _btnHome.onClick.AddListener( () => { OnClickHome(); } );
        _btnSound.onClick.AddListener( () => { OnClickSound(); } );
        _verticalScrollRect.totalCount = DataModel.Instance.CurrentMaxLevel * 2+1;

        RefreshSoundBtn();
    }

    private void OnDisable()
    {
        _btnHome.onClick.RemoveAllListeners();
        _btnSound.onClick.RemoveAllListeners();
    }

    
    private void OnClickSound()
    {
        DataModel.Instance.SoundOpen = !DataModel.Instance.SoundOpen;
        RefreshSoundBtn();
    }

    private void OnClickHome()
    {
        UIManager.Instance.Close( GlobalDefine.UINames.LevelChoose );
        if( UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Main" )
        {
            GameController.Instance.EnterMainMenu();
        }
    }


    private void RefreshSoundBtn()
    {
        _btnSound.GetComponent<Image>().sprite = DataModel.Instance.SoundOpen ? sound_On : sound_Off;
    }


    private void EndDragAction()
    {
        //var content = _verticalScrollRect.content;
        //var first = content.GetChild( 1 );
        //int levelIndex;
        //if( first.name.Contains( "Level" ) )
        //    levelIndex = int.Parse( first.name.Split( ' ' )[ 1 ] );
        //else
        //    levelIndex = int.Parse( content.GetChild( 2 ).name.Split( ' ' )[ 1 ] );

        //_verticalScrollRect.SrollToCell( levelIndex );
    }
}
