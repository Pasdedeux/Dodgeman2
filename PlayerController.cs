using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;

/// <summary>
/// 临时用类，主要实现玩家控制
/// </summary>
public class PlayerController : SingletonMono<PlayerController>
{
    #region 控制参数
    //旋转总体时间
    private float ROTATE_TOTAL_TIME = 1f;

    #endregion



    private Transform _curPlayer;
    //旋转控制
    //旋转目标方向
    private Vector3 _curCmdDirection = Vector3.zero;
    //玩家角色已旋转时间
    private float _playerRotateTime = 0f;
    //玩家角色旋转速度
    private float _playerRotateSpeed = 0f;

	void Start () {
        _curPlayer = GameObject.Find( "Roles/Player/Man" ).transform;

    }
	
	void Update ()
    {
#if ( UNITY_EDITOR || UNITY_STANDALONE_WIN )
        //h = Input.GetAxisRaw( "Horizontal" );
        //v = Input.GetAxisRaw( "Vertical" );

        if ( Input.GetKeyUp( KeyCode.LeftArrow ) )
        {
            Debug.Log( "输出=>左" );
            _curCmdDirection = Vector3.right;
            ReCalculateRotateParam();
        }
        else if ( Input.GetKeyUp( KeyCode.UpArrow ) )
        {
            Debug.Log( "输出=>上" );
            _curCmdDirection = Vector3.back;
            ReCalculateRotateParam();
        }
        else if ( Input.GetKeyUp( KeyCode.RightArrow ) )
        {
            Debug.Log( "输出=>右" );
            _curCmdDirection = Vector3.left;
            ReCalculateRotateParam();
        }
        else if ( Input.GetKeyUp( KeyCode.DownArrow ) )
        {
            Debug.Log( "输出=>下" );
            _curCmdDirection = Vector3.forward;
            ReCalculateRotateParam();
        }

#elif UNITY_IPHONE || UNITY_ANDROID
         
#endif
    }


    /// <summary>
    /// 每次的方向操作，都需要重新计算旋转参数
    /// </summary>
    void ReCalculateRotateParam()
    {
        _playerRotateTime = 0f;
        //TODO 这里加入代码，将目标旋转角度转换为，制定时间内的速度
        //..
    }



    float VectorAngle( Vector2 from , Vector2 to )
    {
        float angle;

        Vector3 cross = Vector3.Cross( from , to );
        angle = Vector2.Angle( from , to );
        return cross.z > 0 ? -angle : angle;
    }


    private void FixedUpdate()
    {
        if ( _curCmdDirection != Vector3.zero )
        {
            
        }
    }
}
