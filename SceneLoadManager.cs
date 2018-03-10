using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.UI;
using System;
using System.IO;
using LitJson;
using Assets.Scripts;
using Assets.Scripts.Managers;

/// <summary>
/// 2、加入滚动形象
/// </summary>
public class SceneLoadManager : SingletonMono<SceneLoadManager>
{
    private Dictionary<int , LevelClass> _levelInfoDict = new Dictionary<int , LevelClass>();
    //转场背景时间
    public Image fadeBG;
    public Camera mainCam;
    //转场时间
    private Coroutine _changeCoroutine;
    //Loading界面中央停留对象队列
    private List<GameObject> _showingList;

    private WaitForSeconds _waitTime;
    private WaitForEndOfFrame _waitFrame;

    private void Awake()
    {
        _showingList = new List<GameObject>();
        _waitTime = new WaitForSeconds( 0.5f );
        _waitFrame = new WaitForEndOfFrame();
        DontDestroyOnLoad( this );
    }

    

    public void StartFade( int targetLevel, Action callBack = null )
    {
        _changeCoroutine = StartCoroutine( IEStartFade( targetLevel, callBack ) );
    }

    IEnumerator IEStartFade( int targetLevel , Action callBack )
    {
        fadeBG.raycastTarget = true;
        fadeBG.CrossFadeAlpha( 1 , 0.5f , true );
        yield return _waitTime;


        AsyncOperation sceneprocess;
        //场景内容加载
        if( targetLevel == 0 )
        {
            yield return sceneprocess = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( 0 );

            //主菜单
            if( GameController.Instance.GameIsInit )
            {
                Debug.LogError( "直接进入主界面" );
                UIManager.Instance.Show( GlobalDefine.UINames.MainMenu );
            }
            //Loading
            else
            {
                Debug.LogError( "进入Loading" );
                //加载配置文件和数据初始化
                DataModel.Instance.InitData();
                SpawnManager.Instance.Install();
                PrefabLoader.Instance.InitUIPrefabs();

                //加载必要预制件
                //..


                //组建初始化
                UIManager.Instance.GetUIResource = PrefabLoader.Instance.GetPrefab;

                GameController.Instance.GameIsInit = true;
            }
        }
        else
        {
            //跳到关卡
            LevelClass levelClass;
            yield return sceneprocess = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( 1 );
            if( _levelInfoDict.ContainsKey( targetLevel ) )
                levelClass = _levelInfoDict[ targetLevel ];
            else
            {
                string filepath = Application.dataPath + string.Format( @"/StreamingAssets/Level_{0}.txt" , targetLevel );
                FileInfo fileInfo = new FileInfo( filepath );
                if( !File.Exists( filepath ) )
                {
                    Debug.LogError( "关卡 " + targetLevel + " 不存在" );
                    Debug.Log( "<color=green>所有关卡已通关</color>" );
                    _changeCoroutine = StartCoroutine( IEStartFade( 0 , callBack ) );
                    yield break;
                }
                using( var fs = fileInfo.OpenText() )
                    levelClass = JsonMapper.ToObject<LevelClass>( fs.ReadToEnd() );
            }
            GameObject scene = GameObject.Find( "Scene" );
            if( scene != null )
                DestroyImmediate( scene );

            var rootChildList = levelClass.leveCellList;
            for( int k = 0; k < rootChildList.Count; k++ )
                DetectAllChildrenLoad( null , rootChildList[ k ] );
        }

        yield return _waitTime;

        fadeBG.CrossFadeAlpha( 0 , 0.5f , true );

        yield return _waitTime;

        fadeBG.raycastTarget = false;

        if( callBack!=null )
            callBack.Invoke();
    }


    void DetectAllChildrenLoad( Transform parent , LevelCell leveCell )
    {
        LevelCell levelCell = leveCell;
        GameObject go;

        if( !levelCell.noIdentity )
        {
            go = PrefabLoader.Instance.GetPrefab( levelCell.prefabName );
            Identity iden = go.GetComponent<Identity>() == null ? go.AddComponent<Identity>() : go.GetComponent<Identity>();
            iden.type = levelCell.type;
            iden.rolesType = levelCell.roleType;
            iden.propType = levelCell.propType;
            iden.pointsType = levelCell.pointType;
            iden.obstacleType = levelCell.obstacleType;
        }
        else
            go = new GameObject();
        go.name = levelCell.name;
        go.transform.position = new Vector3( ( float )levelCell.px , ( float )levelCell.py , ( float )levelCell.pz );
        go.transform.rotation = Quaternion.Euler( new Vector3( ( float )levelCell.rx , ( float )levelCell.ry , ( float )levelCell.rz ) );
        go.transform.localScale = new Vector3( ( float )levelCell.sx , ( float )levelCell.sy , ( float )levelCell.sz );

        if( parent != null )
            go.transform.parent = parent;

        if( levelCell.leveCellList.Count > 0 )
        {
            for( int w = 0; w < levelCell.leveCellList.Count; w++ )
            {
                DetectAllChildrenLoad( go.transform , levelCell.leveCellList[ w ] );
            }
        }
    }
}
