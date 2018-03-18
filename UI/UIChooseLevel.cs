using UnityEngine;
using UnityEngine.UI;

public class UIChooseLevel : BaseUI {

    public Sprite sound_On, sound_Off;
    private Button _btnHome, _btnSound;

    private void Awake()
    {
        CurrentUIType.uiType = UITypeEnum.PopUp;
        CurrentUIType.uiShowMode = UIShowModeEnum.Unique;
        CurrentUIType.uiTransparent = UITransparentEnum.NoPenetratingLow;

        InitElements();

    }

    private void InitElements()
    {
        _btnHome = transform.Find( "Button_Home" ).GetComponent<Button>();
        _btnSound = transform.Find( "Button_Sound" ).GetComponent<Button>();
    }

    private void OnEnable()
    {
        _btnHome.onClick.AddListener( () => { OnClickHome(); } );
        _btnSound.onClick.AddListener( () => { OnClickSound(); } );

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
    }


    private void RefreshSoundBtn()
    {
        _btnSound.GetComponent<Image>().sprite = DataModel.Instance.SoundOpen ? sound_On : sound_Off;
    }
}
