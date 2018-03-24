using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIChooseLevel : BaseUI
{

    public static int totalCount, currentChooseIndex = 2;//关卡值被指为双数
    public static Transform currentSelectItem;

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
        _verticalScrollRect.BeginDragCallBack = BeginDragAction;
    }



    private void OnEnable()
    {
        _btnHome.onClick.AddListener( () => { OnClickHome(); } );
        _btnSound.onClick.AddListener( () => { OnClickSound(); } );
        _verticalScrollRect.totalCount = ( DataModel.Instance.CurrentMaxLevel + 1 ) * 2 + 1;

        totalCount = _verticalScrollRect.totalCount;

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
        var content = _verticalScrollRect.content;
        var last = content.GetChild( content.childCount - 1 );
        int levelIndex;
        if( last.name.Contains( "Level" ) )
        {
            levelIndex = int.Parse( last.name.Split( '_' )[ 2 ] );
            currentSelectItem = content.GetChild( content.childCount - 3 ).GetChild( 0 );
        }
        else
        {
            levelIndex = int.Parse( content.GetChild( content.childCount - 2 ).name.Split( '_' )[ 2 ] );
            currentSelectItem = content.GetChild( content.childCount - 4 ).GetChild( 0 );
        }
        //当前应该放大的元素标号，用于初始化
        currentChooseIndex = int.Parse( currentSelectItem.parent.name.Split( '_' )[ 2 ] );

        _verticalScrollRect.SrollToCell( levelIndex - 4 );
        _verticalScrollRect.ScrollOverCallBack = () =>
        {
            currentSelectItem.DOScale( Vector3.one * 1.2f , 0.2f );
        };

    }


    private void BeginDragAction()
    {
        currentSelectItem.DOScale( Vector3.one , 0.2f );
    }
}
