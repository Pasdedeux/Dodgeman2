using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;


public enum TouchDirection
{
    Unkown,
    Left,
    Right,
    Up,
    Down
}

public class TouchInput : SingletonMono<TouchInput> 
{
    private Vector2 touchBeginPos;
    private Vector2 touchEndPos;

  
    public TouchDirection GetTouchMoveDirection( )
    {
        if ( Input.touchCount > 0 )
        {
            TouchDirection dir = TouchDirection.Unkown;
            if ( Input.touches[ 0 ].phase != TouchPhase.Canceled )
            {
                if( Camera.main.ScreenToViewportPoint( Input.touches[ 0 ].position ).x > 0.8f && Camera.main.ScreenToViewportPoint( Input.touches[ 0 ].position ).y < 0.2f ) return dir;

                switch ( Input.touches[ 0 ].phase )
                {
                    case TouchPhase.Began:
                        touchBeginPos = Input.touches[ 0 ].position;
                        break;
                    case TouchPhase.Ended:
                        touchEndPos = Input.touches[ 0 ].position;

                        float offSetX = touchEndPos.x - touchBeginPos.x;
                        float offSetY = touchEndPos.y - touchBeginPos.y;
                        float angle = Mathf.Atan2( offSetY , offSetX ) * Mathf.Rad2Deg;

                        angle += 28f;

                        if ( angle > 180 ) angle = 360 - angle;
                        else if ( angle < -180 ) angle += 360;

                        if ( angle <= 45f && angle > -45f )
                        {
                            ////Debug.Log( "右" );
                            dir = TouchDirection.Right;
                        }
                        else if ( angle > 45f && angle <= 135f )
                        {
                            ////Debug.Log( "上" );
                            dir = TouchDirection.Up;
                        }
                        else if ( ( angle > 135f && angle < 180f ) || ( angle > -180f && angle <= -135f ) )
                        {
                            ////Debug.Log( "左" );
                            dir = TouchDirection.Left;
                        }
                        else if ( angle > -135f && angle <= -45f )
                        {
                            ////Debug.Log( "下" );
                            dir = TouchDirection.Down;
                        }

                        //if ( Mathf.Abs( touchBeginPos.x - touchEndPos.x ) > Mathf.Abs( touchBeginPos.y - touchEndPos.y ) )
                        //{
                        //    if ( touchBeginPos.x > touchEndPos.x )
                        //    {
                        //        dir = TouchDirection.Left;
                        //    }
                        //    else
                        //    {
                        //        dir = TouchDirection.Right;
                        //    }
                        //}
                        //else
                        //{
                        //    if ( touchBeginPos.y > touchEndPos.y )
                        //    {
                        //        dir = TouchDirection.Down;
                        //    }
                        //    else
                        //    {
                        //        dir = TouchDirection.Up;
                        //    }
                        //}
                        break;
                }
            }
            return dir;
        }
        else
        {
            return TouchDirection.Unkown;
        }


       
    }
}
