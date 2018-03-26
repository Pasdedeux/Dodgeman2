using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using System;
using GoogleMobileAds.Api;
using System.Text;
using System.Security.Cryptography;

public class AdmobAds : Singleton<AdmobAds> {

    /// <summary>
    /// 插页广告播放次数
    /// </summary>
    public int checkTimes = 0;
    public Action callBack;

    InterstitialAd _initAd;
    BannerView _bannerAd;

    public void Init()
    {
        checkTimes = 0;

#if UNITY_ANDROID
        string appId = GameManager.Instance.GoogleAppID_Andriod;
#elif UNITY_IPHONE
            string appId = GameManager.Instance.GoogleAppID_IOS;
#else
            string appId = "unexpected_platform";
#endif

        MobileAds.Initialize( appId );
        Debug.Log( "================>>>Succeed Init" );
        RequestBanner();
        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = GameManager.Instance.GoogleInterstitial_Andriod;
#elif UNITY_IPHONE
        string adUnitId = GameManager.Instance.GoogleInterstitial_IOS;
#elif UNITY_EDITOR
        string adUnitId = "unused";
#endif

        if( _initAd != null )
        {
            _initAd.Destroy();
            _initAd = null;
        }

        _initAd = new InterstitialAd( adUnitId );
        _initAd.OnAdClosed += AdCloseCallBack;
        _initAd.OnAdLoaded += AdLoadedCallBack;
        _initAd.OnAdFailedToLoad += AdFailedCallBack;
        _initAd.OnAdLeavingApplication += AdLeavingAppCallBack;

        _initAd.LoadAd( CreateAdRequest() );
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .Build();
    }

    public void PlayInterstitial()
    {
        if( _initAd.IsLoaded() )
        {
            Debug.Log( "Ad Show..." );
            _initAd.Show();
            //一旦加载是完成的，检查次数增加1
            checkTimes++;
        }
        else
        {
            Debug.Log( "Ad load failed..." );
            //如果是未加载成功时 直接 callback
            if( callBack != null )
                callBack();
        }
    }



    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = GameManager.Instance.GoogleBanner_Andriod;
#elif UNITY_IPHONE
        string adUnitId = GameManager.Instance.GoogleBanner_IOS;
#elif UNITY_EDITOR
        string adUnitId = "unused";
#endif

        _bannerAd = new BannerView( adUnitId , AdSize.SmartBanner , AdPosition.Bottom );
        _bannerAd.OnAdOpening += AdOpeningCallBack;
        _bannerAd.OnAdClosed += AdCloseCallBack;
        _bannerAd.OnAdLoaded += AdLoadedCallBack;
        _bannerAd.OnAdFailedToLoad += AdFailedCallBack;
        _bannerAd.OnAdLeavingApplication += AdLeavingAppCallBack;

        _bannerAd.LoadAd( CreateAdRequest() );
    }


    public void PlayBannerAD()
    {
        _bannerAd.Show();
    }

    private void AdOpeningCallBack( object sender , EventArgs e )
    {
        Debug.LogWarning( "AdOpened...." );
    }

    private void AdCloseCallBack( object sender , EventArgs e )
    {
        Debug.LogWarning( "Ad Closed...." );
        RequestInterstitial();
        if( callBack != null )
            callBack();
    }

    public void AdLoadedCallBack( object sender , EventArgs e )
    {
        Debug.LogWarning( "Ad Loading OK...." );
        if( _bannerAd != null )
            PlayBannerAD();
    }

    private void AdFailedCallBack( object sender , AdFailedToLoadEventArgs e )
    {
        Debug.LogError( "Ad Loading Failed...." );
        RequestInterstitial();
        if( callBack != null )
            callBack();
    }

    private void AdLeavingAppCallBack( object sender , EventArgs e )
    {
        Debug.LogWarning( "Ad Leved app...." );
    }

}

public class AdCommon
{

    private static string Md5Sum( string strToEncrypt )
    {

        UTF8Encoding ue = new UTF8Encoding();

        byte[] bytes = ue.GetBytes( strToEncrypt );

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash( bytes );

        string hashString = "";
        for( int i = 0; i < hashBytes.Length; i++ )
        {

            hashString += Convert.ToString( hashBytes[ i ] , 16 ).PadLeft( 2 , '0' );
        }

        return hashString.PadLeft( 32 , '0' );
    }

    public static string DeviceIdForAdmob
    {
        get
        {
#if UNITY_EDITOR
            return SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaObject secure = new AndroidJavaObject("android.provider.Settings$Secure");
            string deviceID = secure.CallStatic<string>("getString" , contentResolver, "android_id");
            return Md5Sum(deviceID).ToUpper();
#elif UNITY_IOS
            return Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier);
#else
            return SystemInfo.deviceUniqueIdentifier;
#endif
        }
    }
}
