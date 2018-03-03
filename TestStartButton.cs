using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartButton : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.Instance.StartFade( 1 );
    }
}
