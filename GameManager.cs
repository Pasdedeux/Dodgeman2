using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;

public class GameManager : SingletonMono<GameManager> {

    [Header( "UnityAds" )]
    public string UnityAndriod = "1687538";
    public string UnityIOS = "1687539";

    [Header( "Admob_Andriod" )]
    public string GoogleAppID_Andriod = "ca-app-pub-3940256099942544~3347511713";
    public string GoogleBanner_Andriod = "ca-app-pub-3940256099942544/6300978111";
    public string GoogleInterstitial_Andriod = "ca-app-pub-3940256099942544/1033173712";
    [Header( "Admob_IOS" )]
    public string GoogleAppID_IOS = "ca-app-pub-3940256099942544~1458002511";
    public string GoogleBanner_IOS = "ca-app-pub-3940256099942544/2934735716";
    public string GoogleInterstitial_IOS = "ca-app-pub-3940256099942544/4411468910";

    [Header( "Unity广告弹出间隔" )]
    public int unityAds_Interval = 4;

    [Header( "Unity广告奖励次数" )]
    public int unityAds_Tips = 9;

    [Header( "Tips 初始化次数" )]
    public int tipsInitNum = 9;

}
