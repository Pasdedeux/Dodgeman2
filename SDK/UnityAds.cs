using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using UnityEngine.Advertisements;

public class UnityAds : Singleton<UnityAds> {

    //Unity Ads
    public delegate void UnityAdsRewardFullPlayed();
    public delegate void UnityAdsRewardSkipped();
    public delegate void UnityAdsRewardCancelled();


    string AndriodGameId = "";
    string IOSGameId = "";

    public UnityAdsRewardSkipped unityAdsRewardSkipped;
    public UnityAdsRewardCancelled unityAdsRewardFailed;
    public UnityAdsRewardFullPlayed unityAdsRewardFinished;

    public void Init()
    {
        if( Advertisement.isSupported )
        {
#if UNITY_IOS
            Advertisement.Initialize( GameManager.Instance.UnityIOS ,true );
#elif UNITY_ANDROID
            Advertisement.Initialize( GameManager.Instance.UnityAndriod , true );
#endif
        }
    }

    public bool RewardAdsIsReady()
    {
        return Advertisement.IsReady();
    }


    public void ShowRewardAds()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show( "rewardedVideo" , options );
    }

    void HandleShowResult( ShowResult result )
    {
        if( result == ShowResult.Finished )
        {
            Debug.Log( "Video completed - Offer a reward to the player" );
            if( unityAdsRewardFinished != null )
                unityAdsRewardFinished();
        }
        else if( result == ShowResult.Skipped )
        {
            Debug.LogWarning( "Video was skipped - Do NOT reward the player" );
            if( unityAdsRewardSkipped != null )
                unityAdsRewardSkipped();
        }
        else if( result == ShowResult.Failed )
        {
            Debug.LogError( "Video failed to show" );
            if( unityAdsRewardFailed != null )
                unityAdsRewardFailed();
        }
    }
}
