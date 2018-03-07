using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartButton : MonoBehaviour {

    public void StartGame()
    {
        Assets.Scripts.GameController.Instance.EnterLevel( 1 );
    }
}
