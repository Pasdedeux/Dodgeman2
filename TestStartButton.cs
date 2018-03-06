using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartButton : MonoBehaviour {

    public void StartGame()
    {
        SceneLoadManager.Instance.StartFade( 1 );
    }
}
