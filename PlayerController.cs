using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using DG.Tweening;
using System;

/// <summary>
/// 临时用类，主要实现玩家控制
/// </summary>
public class PlayerController : SingletonMono<PlayerController>
{
    #region 固定配置参数
    //转向时间
    private const float ROTATE_TOTAL_TIME = 1f;
    //加减速时间
    private const float SPEED_CHANGE_TIME = 1f;
    //加减速所用距离
    private const float SPEED_CHANGE_LENGTH = 1f;
    //怪物检测层
    private const string LAYERNAME_MONSTER = "Target";
    #endregion



    private Transform _curPlayer;
    //加速时间片段长度
    private float _fixedTimeDelta;
    //旋转目标方向
    private Vector3 _curCmdDirection = Vector3.zero;
    //玩家角色已旋转时间
    private float _curTurnTime = 0f;
    //碰撞检测
    private RaycastHit[] _rayHitArr;

    //玩家角色移动目标点
    private Vector3 _targetPosistion;
    //是否正在移动中
    private bool _isMoving = false;
    //玩家角色当前移动速度
    private float _playerMoveSpeed = 0f;
    //玩家加速度
    private float _speedUpRatio;
    //玩家已加速时间
    private float _curSpeedUpTime;

    private void Awake()
    {
        _rayHitArr = new RaycastHit[ 4 ];
        _fixedTimeDelta = Time.fixedDeltaTime;

        CalculateControlParam();
    }

    

    void Start ()
    {
        _curPlayer = GameObject.Find( "Roles/Player/Man" ).transform;

    }
	
	void Update ()
    {
        if ( /*!_isMoving*/true )
        {
#if ( UNITY_EDITOR || UNITY_STANDALONE_WIN )
            if ( Input.GetKeyUp( KeyCode.LeftArrow ) )
            {
                //Debug.Log( "输出=>左" );
                _curCmdDirection = Vector3.right;
            }
            else if ( Input.GetKeyUp( KeyCode.UpArrow ) )
            {
                //Debug.Log( "输出=>上" );
                _curCmdDirection = Vector3.back;
            }
            else if ( Input.GetKeyUp( KeyCode.RightArrow ) )
            {
                //Debug.Log( "输出=>右" );
                _curCmdDirection = Vector3.left;
            }
            else if ( Input.GetKeyUp( KeyCode.DownArrow ) )
            {
                //Debug.Log( "输出=>下" );
                _curCmdDirection = Vector3.forward;

            }

#elif UNITY_IPHONE || UNITY_ANDROID
         
#endif

            if ( !_isMoving && _curCmdDirection != Vector3.zero )
            {
                if ( CanPassDetection( _curPlayer.position , _curCmdDirection ) )
                {
                    StartControlParam();
                }
                else
                {
                    _curCmdDirection = Vector3.zero;
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if ( _curCmdDirection != Vector3.zero )
        {
            //转向
            if( _curTurnTime< ROTATE_TOTAL_TIME )
            {
                _curTurnTime += _fixedTimeDelta;
                _curPlayer.rotation = Quaternion.Slerp( _curPlayer.rotation , Quaternion.LookRotation( _curCmdDirection ) , _curTurnTime );
            }
            //移动
            else
            {
                //TODO 下面的加速/减速出现重叠情况
                //
                if ( _curSpeedUpTime < SPEED_CHANGE_TIME )
                {
                    Debug.Log( "开始加速" );
                    _curSpeedUpTime += _fixedTimeDelta;
                    _playerMoveSpeed += _speedUpRatio * _fixedTimeDelta;
                }

                if ( Vector3.Distance( _curPlayer.position, _targetPosistion )<= SPEED_CHANGE_LENGTH )
                {
                    Debug.Log( "开始减速" );
                    _curSpeedUpTime -= _fixedTimeDelta;
                    _playerMoveSpeed -= _speedUpRatio * _fixedTimeDelta;

                    if ( _curSpeedUpTime <=0 )
                    {
                        _curSpeedUpTime = 0;
                        _playerMoveSpeed = 0;
                        _curPlayer.position = _targetPosistion;
                        Debug.Log( "到达指定位置" );
                        StopControlParam();
                    }
                }

                //移动
                _curPlayer.position = Vector3.MoveTowards( _curPlayer.position , _targetPosistion , _playerMoveSpeed * _fixedTimeDelta );
            }
        }
    }




    /// <summary>
    /// 根据配置计算参数
    /// </summary>
    private void CalculateControlParam()
    {
        _speedUpRatio = SPEED_CHANGE_LENGTH * 2f / ( SPEED_CHANGE_TIME * SPEED_CHANGE_TIME );

    }



    /// <summary>
    /// 运动开始前准备更改参数
    /// </summary>
    void StartControlParam()
    {
        //重制参数视为开始运动
        _isMoving = true;
        //旋转参数
        _curTurnTime = 0f;

        //移动参数
        _curSpeedUpTime = 0f;
        _playerMoveSpeed = 0f;

        //滚动参数
    }




    /// <summary>
    /// 运动完成后初始化控制参数
    /// </summary>
    void StopControlParam()
    {
        _curCmdDirection = Vector3.zero;
        _isMoving = false;
    }




    private bool CanPassDetection( Vector3 oriPoint , Vector3 direction )
    {
        //TODO 射线根据当前摄像机角度做了修正
        //..
        Debug.DrawRay( oriPoint , direction * -100 , Color.red , 1f );
        if ( Physics.RaycastNonAlloc( oriPoint , -1 * direction , _rayHitArr , 100 , 1 << LayerMask.NameToLayer( LAYERNAME_MONSTER ) ) > 0 )
        {
            var hitinfo = _rayHitArr[ 0 ].transform;
            _targetPosistion = hitinfo.position;
            return true;
        }
        return false;
    }



}
