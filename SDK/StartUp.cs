using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class StartUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }


    private void OnGUI()
    {
        if(GUI.Button(new Rect(new Vector2(100,100), new Vector2(200,100)),"登陆"))
        {
            Social.localUser.Authenticate( ( bool success ) =>
            {
                Debug.Log( success ? "登陆成功" : "登录失败" );
            } );
        }

    }

}
