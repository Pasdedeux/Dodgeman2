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
    //主摄
    public Camera mainCam;

    private Vector3 _camOffset;
    private bool _needCameraFollow;
    //转场时间
    private Coroutine _changeCoroutine;
    //Loading界面中央停留对象队列
    private List<LoadingObject> _showingList;
    private List<GameObject> _loadingList;

    private Vector3 _loadingStartPos, _loadingEndPos;
    private float _totalRotationByAngle = 0f, _trueRotationByAngle = 0f;

    private GameObject _mainLoadingContainer;
    private GameObject _mainPlayer;

    private WaitForSeconds _waitTime;
    private WaitForEndOfFrame _waitFrame;

    private void Awake()
    {
        _loadingList = new List<GameObject>();
        _showingList = new List<LoadingObject>();

        _waitTime = new WaitForSeconds( 0.5f );
        _waitFrame = new WaitForEndOfFrame();

        _camOffset = mainCam.transform.position;

        DontDestroyOnLoad( this );
    }



    public void StartFade( int targetLevel , Action callBack = null )
    {
        _changeCoroutine = StartCoroutine( IEStartFade( targetLevel , callBack ) );
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

            _mainLoadingContainer = GameObject.Find( "Loading" );
            //主菜单
            if( GameController.GameIsInit )
            {
                Debug.LogError( "直接进入主界面" );
                UIManager.Instance.Show( GlobalDefine.UINames.MainMenu );

                InitMain3DBar();
                _needCameraFollow = true;
            }
            //Loading
            else
            {
                Debug.LogError( "进入Loading" );
                //必要组件
                DataModel.Instance.InitData();
                SpawnManager.Instance.Install();
                PrefabLoader.Instance.InitUIPrefabs();
                UIManager.Instance.GetUIResource = PrefabLoader.Instance.GetPrefab;

                InitLoading3DBar();
                _needCameraFollow = true;
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

        if( !GameController.GameIsInit )
        {
            //完成Loading界面后，执行加载
            UILoading uiLoading = UIManager.Instance.Show( GlobalDefine.UINames.Loading ) as UILoading;
            uiLoading.ProcessCallFunc = ShowLoadingProgress;
            uiLoading.StartLoading();
        }
        else
            UIManager.Instance.Show( GlobalDefine.UINames.MainMenu );

        if( callBack != null )
            callBack.Invoke();
    }

    private void InitMain3DBar()
    {
        if( _showingList.Count == 0 )
            throw new Exception( "主场景展示物未登记展示物" );

        for( int i = 0; i < _showingList.Count; i++ )
        {
            if( _showingList[ i ].prefabName == GlobalDefine.PrefabNames.Floor )
            {
                var floor = PrefabLoader.Instance.GetPrefab( _showingList[ i ].prefabName );
                floor.transform.position = _showingList[i].position;
                floor.transform.SetParent( _mainLoadingContainer.transform );
            }
            else if( _showingList[ i ].prefabName == GlobalDefine.PrefabNames.Man )
            {
                var man = PrefabLoader.Instance.GetPrefab( _showingList[ i ].prefabName );
                man.transform.position = _showingList[ i ].position;
                man.transform.SetParent( _mainLoadingContainer.transform );
            }
            else if( _showingList[ i ].prefabName == GlobalDefine.PrefabNames.Terminal )
            {
                var term = PrefabLoader.Instance.GetPrefab( _showingList[ i ].prefabName );
                term.transform.position = _showingList[ i ].position;
                term.transform.SetParent( _mainLoadingContainer.transform );
            }
        }
    }

    private void InitLoading3DBar()
    {
        //Loading条地板
        int n = UnityEngine.Random.Range( 5 , 8 ), m = 0;
        n = 6;
        while( m <= n )
        {
            //加载必要预制件
            var floor = PrefabLoader.Instance.GetPrefab( GlobalDefine.PrefabNames.Floor );
            floor.transform.position = m * Vector3.right;
            floor.transform.SetParent( _mainLoadingContainer.transform );

            _loadingList.Add( floor );
            m++;
        }
        //中继点
        var enemy = PrefabLoader.Instance.GetPrefab( GlobalDefine.PrefabNames.Enemy );
        enemy.transform.position = n * Vector3.right + Vector3.up;

        _loadingList.Add( enemy );
        //Main主场景地板
        n += 5;
        while( m <= n )
        {
            ////加载必要预制件
            //var floor = PrefabLoader.Instance.GetPrefab( GlobalDefine.PrefabNames.Floor );
            //floor.transform.position = m * Vector3.right;
            //floor.transform.SetParent( _mainLoadingContainer.transform );

            _showingList.Add( new LoadingObject() { prefabName = GlobalDefine.PrefabNames.Floor , position = m * Vector3.right } );
            m++;
        }
        //终点
        //var terminal = PrefabLoader.Instance.GetPrefab( GlobalDefine.PrefabNames.Terminal );
        //terminal.transform.position = n * Vector3.right + Vector3.up;

        _showingList.Add( new LoadingObject() { prefabName = GlobalDefine.PrefabNames.Terminal , position = n * Vector3.right + Vector3.up } );

        //Loading玩家
        if( _mainPlayer != null )
        {
            SpawnManager.Instance.DespawnObject( _mainPlayer.transform );
            _mainPlayer = null;
        }
        _mainPlayer = PrefabLoader.Instance.GetPrefab( GlobalDefine.PrefabNames.Man );
        _mainPlayer.transform.position = Vector3.up;
        _mainPlayer.transform.SetParent( _mainLoadingContainer.transform );

        //计算得出每帧
        _loadingStartPos = _mainPlayer.transform.position;
        _loadingEndPos = _loadingList[ _loadingList.Count - 2 ].transform.position + Vector3.up;
        _totalRotationByAngle = Vector3.Distance( _loadingStartPos , _loadingEndPos ) * 90;

        _showingList.Add( new LoadingObject() { prefabName = GlobalDefine.PrefabNames.Man , position = ( n - 5 ) * Vector3.right + Vector3.up } );
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

    private void ShowLoadingProgress( float ratio )
    {
        if( ratio == 1 )
        {
            _needCameraFollow = false;
            GameController.GameIsInit = true;
            Debug.Log( ">>>>>>>>>>>>>>>>加载完成" );

            InitMain3DBar();
        }

        _trueRotationByAngle = ( _totalRotationByAngle * ratio ) % 180;
        _mainPlayer.transform.position = Vector3.Lerp( _loadingStartPos , _loadingEndPos , ratio );
        _mainPlayer.transform.rotation = Quaternion.Euler( 0 , 0 , -_trueRotationByAngle + Mathf.Floor( _totalRotationByAngle * ratio / 180 ) * 180 );
    }



    /// <summary>
    /// 用于Loading界面和主界面的摄像机跟随
    /// </summary>
    private void LateUpdate()
    {
        if( _needCameraFollow )
        {
            mainCam.transform.position = _camOffset + new Vector3( _mainPlayer.transform.position.x , 0 , 0 );
        }
    }



}


class LoadingObject
{
    public string prefabName;
    public Vector3 position; 
}
