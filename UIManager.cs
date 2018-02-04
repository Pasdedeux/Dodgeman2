using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;


/// <summary>
/// 1、完成第二关的关卡跳转，到主菜单
/// 2、加入滚动形象
/// 3、细化场景
/// </summary>



public class UIManager : SingletonMono<UIManager>
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

        SceneManager.LoadScene( targetLevel );

        yield return _waitTime;

        PlayerController.Instance.CurPlayer = GameObject.Find( "Roles/Player/Man" ).transform;

        fadeBG.raycastTarget = false;
        fadeBG.CrossFadeAlpha( 0 , 0.5f , true );

    }
}
