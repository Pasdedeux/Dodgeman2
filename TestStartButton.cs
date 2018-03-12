using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartButton : MonoBehaviour {

    public void StartGame()
    {
        UIManager.Instance.Close( GlobalDefine.UINames.MainMenu );
        Assets.Scripts.GameController.Instance.EnterLevel( 1 );
    }
}
