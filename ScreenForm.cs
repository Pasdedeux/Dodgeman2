using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenForm : MonoBehaviour 
{

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    private void OnGUI( )
    {
        if ( GUI.Button( new Rect( 0 , 0 , 200 , 50 ) , "窗口1" ) )
        {
        }

        if ( GUI.Button( new Rect( 0 , 100 , 200 , 50 ) , "窗口2" ) )
        {
        }

        if ( GUI.Button( new Rect( 0 , 200 , 200 , 50 ) , "窗口3" ) )
        {
            
        }

        if ( GUI.Button( new Rect( 0 , 300 , 200 , 50 ) , "窗口4" ) )
        {
            
        }





        if ( GUI.Button( new Rect( 500 , 0 , 200 , 50 ) , "窗口1" ) )
        {
            UIManager.Instance.Close( GlobalDefine.UINames.MainMenu );
        }

        if ( GUI.Button( new Rect( 500 , 100 , 200 , 50 ) , "窗口2" ) )
        {
            
        }

        if ( GUI.Button( new Rect( 500 , 200 , 200 , 50 ) , "窗口3" ) )
        {
            
        }

        if ( GUI.Button( new Rect( 500 , 300 , 200 , 50 ) , "窗口4" ) )
        {
            
        }

    }
}
