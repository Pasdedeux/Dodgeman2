using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using System;
using UnityEngine.UI;

public class UILoading : BaseUI
{
    public Text processText;
    public Action<float> ProcessCallFunc;

    private WaitForSeconds _waitFrame = new WaitForSeconds(0.016f);

    private void Awake()
    {
        CurrentUIType.isClearPopUp = false;
        CurrentUIType.uiShowMode = UIShowModeEnum.Normal;
        CurrentUIType.uiTransparent = UITransparentEnum.Penetrating;
        CurrentUIType.uiType = UITypeEnum.Normal;
    }

    public void StartLoading()
    {
        StartCoroutine( IELoading() );
    }

    private IEnumerator IELoading()
    {
        yield return _waitFrame;
        //显示加载进度
        int displayProgress = 0;

        int toProgress = 50;
        DataModel.Instance.InitData();
        //按帧增加
        while( displayProgress < toProgress )
        {
            ++displayProgress;
            ProcessCallFunc( displayProgress * 0.01f );
            processText.text = displayProgress.ToString();
            yield return _waitFrame;
        }

        toProgress = 80;
        while( displayProgress < toProgress )
        {
            //同样按帧增加
            ++displayProgress;
            ProcessCallFunc( displayProgress * 0.01f );
            processText.text = displayProgress.ToString();
            yield return _waitFrame;
        }

        toProgress = 90;
        //UnityAds广告初始化
        UnityAds.Instance.Init();
        //Admob广告初始化
        AdmobAds.Instance.Init();
        yield return _waitFrame;
        while( displayProgress < toProgress )
        {
            //同样按帧增加
            ++displayProgress;
            ProcessCallFunc( displayProgress * 0.01f );
            processText.text = displayProgress.ToString();
            yield return _waitFrame;
        }

        toProgress = 100;
        while( displayProgress < toProgress )
        {
            //同样按帧增加
            ++displayProgress;
            ProcessCallFunc( displayProgress * 0.01f );
            processText.text = displayProgress.ToString();
            yield return _waitFrame;
        }
    }
}
