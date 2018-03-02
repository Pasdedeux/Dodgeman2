using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.UI;
using System;


/// <summary>
/// 2、加入滚动形象
/// </summary>
public class SceneManager : SingletonMono<SceneManager>
{
    //转场背景时间
    public Image fadeBG;
    //转场时间
    private WaitForSeconds _waitTime = new WaitForSeconds( 0.5f );

    private void Awake()
    {
        fadeBG.CrossFadeAlpha( 0 , 0.01f , true );
        fadeBG.raycastTarget = false;

        DontDestroyOnLoad( this );
    }

    public void StartFade( int targetLevel, Action callBack = null )
    {
        StartCoroutine( IEStartFade( targetLevel, callBack ) );
    }

    IEnumerator IEStartFade( int targetLevel , Action callBack )
    {
        fadeBG.raycastTarget = true;
        fadeBG.CrossFadeAlpha( 1 , 0.5f , true );
        yield return _waitTime;

        UnityEngine.SceneManagement.SceneManager.LoadScene( targetLevel );

        yield return _waitTime;

        fadeBG.raycastTarget = false;
        fadeBG.CrossFadeAlpha( 0 , 0.5f , true );

        //查找目标点
        var player = GameObject.Find( "Roles/Player/Man" );
        if( player != null )
            PlayerController.Instance.CurPlayer = player.transform;

        //TODO LoadingScene
        if( targetLevel == 0 )
        {
            Destroy( GameObject.Find( "Managers" ) );
        }
    }
}
