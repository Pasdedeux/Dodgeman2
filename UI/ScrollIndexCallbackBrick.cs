using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;

public class ScrollIndexCallbackBrick : MonoBehaviour 
{
    private int _level;
    private Image _btnBg;
    private Text _txtLevelID;
    private Button _btnLevel;

    //未通关
    public Sprite spriteLockedLevel;
    public Color colorLockedText;

    //已通关
    public Sprite spriteUnlockedLevel;
    public Color colorUnlockedText;

    void ScrollCellIndex(int idx)
    {
        _level = Mathf.CeilToInt( idx * 0.5f ) + 1;
        _txtLevelID.text = _level.ToString();

        if( _level <= DataModel.Instance.CurrentMaxLevel )
        {
            _btnBg.sprite = spriteUnlockedLevel;
            _txtLevelID.color = colorUnlockedText;
            _txtLevelID.fontSize = 60;
        }
        else
        {
            _btnBg.sprite = spriteLockedLevel;
            _txtLevelID.color = colorLockedText;
            _txtLevelID.fontSize = 60;
        }

        gameObject.name = "Level " + idx.ToString();
    }


    private void Awake()
    {
        _btnBg = transform.Find( "Button_LevelBG" ).GetComponent<Image>();
        _txtLevelID = transform.Find( "Text_LevelID" ).GetComponent<Text>();
        _btnLevel = transform.Find( "Button_LevelBG" ).GetComponent<Button>();
    }


    private void OnEnable()
    {
        _btnLevel.onClick.AddListener( () => 
        {
            int level = _level;
            if( level <= DataModel.Instance.CurrentMaxLevel )
            {
                if( UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main" )
                {
                    DataModel.Instance.CurLevel = --level;
                    PlayerController.Instance.FakeMoveOrder( Vector3.left );
                }
                else
                    GameController.Instance.EnterLevel( level );

                UIManager.Instance.Close( GlobalDefine.UINames.LevelChoose );
            }
            else
            {
                Debug.LogError( "关卡未解锁" );
            }
        } );
    }


    private void OnDisable()
    {
        _btnLevel.onClick.RemoveAllListeners();
    }



    private void UpdateBtnStatus()
    {
        
    }
}
