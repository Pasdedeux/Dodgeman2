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
    private const float SPEED_CHANGE_TIME = 0.2f;
    //加减速所用距离
    private const float SPEED_CHANGE_LENGTH = 0.5f;
    //怪物检测层
    private const string LAYERNAME_MONSTER = "Target";
    //下坠路线的默认移动速度
    private const float PLAYER_MOVE_SPEED_FALL = 5F;
    //下坠路线的默认旋转速度
    private const float PLAYER_ROLL_SPEED_FALL = 380f;

    #endregion

    
    private Transform _curPlayer;
    public Transform CurPlayer
    {
        get { return _curPlayer; }
        set
        {
            _curPlayer = value;
            _playerRigidBody = _curPlayer.GetComponentInChildren<Rigidbody>();
            _playerColliderBox = _curPlayer.GetComponentInChildren<BoxCollider>();
        }
    }
    //玩家刚体+碰撞盒
    private Rigidbody _playerRigidBody;
    private BoxCollider _playerColliderBox;

    //加速时间片段长度
    private float _fixedTimeDelta;
    //转向目标方向
    private Vector3 _curCmdDirection = Vector3.zero;
    //转动轴
    private Vector3 _rotateAxis = Vector3.zero;
    //玩家角色已旋转时间
    private float _curTurnTime = 0f;
    //碰撞检测
    private RaycastHit[] _rayHitArr;

    //玩家橘色移动目标
    private Transform _targetObject;
    //玩家角色移动目标点
    private Vector3 _targetPosistion;
    //是否正在移动中
    private bool _isMoving = false;
    //玩家角色当前移动速度
    private float _playerMoveSpeed = 0f;
    private float _playerMoveSpeedFall = 0f;
    //玩家加速度
    private float _speedUpRatio;
    //玩家已加速时间
    private float _curSpeedUpTime;


    //玩家滚动速度
    private float _playerRollSpeed;
    private float _playerRollSpeedFall;
    //玩家滚动时间
    private float _playerRollTime;
    //玩家已滚动时间
    private float _curRollTime;
    //玩家需要滚动的角度
    private float _rollDegree;

    //
    private static Vector3 DEST_POINT = new Vector3();
    private static Vector2 PLAYE_POS = new Vector2();
    private static Vector2 DEST_POS = new Vector2();

    private bool _isSelfKill = false;

    private void Awake()
    {
        _rayHitArr = new RaycastHit[ 4 ];
        _fixedTimeDelta = Time.fixedDeltaTime;

        CalculateControlParam();
    }

    

	void Update ()
    {
        if ( !_isMoving )
        {
#if ( UNITY_EDITOR || UNITY_STANDALONE_WIN )
            if ( Input.GetKeyUp( KeyCode.LeftArrow ) )
            {
                _curCmdDirection = Vector3.right;
            }
            else if ( Input.GetKeyUp( KeyCode.UpArrow ) )
            {
                _curCmdDirection = Vector3.back;
            }
            else if ( Input.GetKeyUp( KeyCode.RightArrow ) )
            {
                _curCmdDirection = Vector3.left;
            }
            else if ( Input.GetKeyUp( KeyCode.DownArrow ) )
            {
                _curCmdDirection = Vector3.forward;
            }

#elif UNITY_IPHONE || UNITY_ANDROID
            switch( TouchInput.Instance.GetTouchMoveDirection() )
            {
                case TouchDirection.Up:
                    _curCmdDirection = Vector3.right;
                    break;
                case TouchDirection.Down:
                    _curCmdDirection = Vector3.left;
                    break;
                case TouchDirection.Left:
                    _curCmdDirection = Vector3.forward;
                    break;
                case TouchDirection.Right:
                    _curCmdDirection = Vector3.back;
                    break;
            }
#endif

            if( !_isMoving && _curCmdDirection != Vector3.zero )
            {
                if ( CanPassDetection( _curPlayer.position , _curCmdDirection ) )
                {
                    StartControlParam();
                }
                else
                {
                    //_curCmdDirection = Vector3.zero;
                    StartControlParam( true );
                }
            }
        }
    }


    //float ___speedDownTime = 0f;
    private void FixedUpdate()
    {
        if ( _curCmdDirection != Vector3.zero )
        {
            //=========================正常路线
            if( !_isSelfKill )
            {
                PLAYE_POS.x = _curPlayer.position.x;
                PLAYE_POS.y = _curPlayer.position.z;

                DEST_POS.x = _targetPosistion.x;
                DEST_POS.y = _targetPosistion.z;

                //--移动
                if( Vector2.Distance( PLAYE_POS , DEST_POS ) < SPEED_CHANGE_LENGTH )
                {
                    _curSpeedUpTime -= _fixedTimeDelta;
                    _playerMoveSpeed -= _speedUpRatio * _fixedTimeDelta;

                    ////TODO
                    //___speedDownTime += _fixedTimeDelta;

                    //计算float精度有问题，判断并不准确，用这种方式处理
                    if( _curSpeedUpTime < _fixedTimeDelta )
                    {
                        //TODO
                        //Debug.Log( "减速耗时 " + ___speedDownTime );
                        Debug.Log( "到达指定位置 " );
                        //___speedDownTime = 0f;

                        _curSpeedUpTime = 0;
                        _playerMoveSpeed = 0;

                        DEST_POINT.x = _targetPosistion.x;
                        DEST_POINT.z = _targetPosistion.z;
                        DEST_POINT.y = _curPlayer.position.y;

                        _curPlayer.position = DEST_POINT;

                        var id = _targetObject.GetComponent<Identity>();
                        if( id != null )
                        {
                            if( id.type == ObjectType.Points )
                            {
                                UIManager.Instance.StartFade( id.id );
                            }
                        }

                        StopControlParam();
                    }
                }
                else
                {
                    //计算float精度有问题，判断并不准确，用这种方式处理
                    if( _curSpeedUpTime < SPEED_CHANGE_TIME )
                    {
                        _curSpeedUpTime += _fixedTimeDelta;
                        _playerMoveSpeed += _speedUpRatio * _fixedTimeDelta;

                        if( _curSpeedUpTime > SPEED_CHANGE_TIME - _fixedTimeDelta )
                        {
                            Debug.Log( "加速耗时: " + _curSpeedUpTime );
                            _curSpeedUpTime = SPEED_CHANGE_TIME;
                            //_playerMoveSpeedFall = _playerMoveSpeed;
                        }
                    }
                }

                DEST_POINT = _targetPosistion;
                _curPlayer.position = Vector3.MoveTowards( _curPlayer.position , DEST_POINT , _playerMoveSpeed * _fixedTimeDelta );

                //--滚动
                if( _curRollTime < _playerRollTime )
                {
                    _curRollTime += _fixedTimeDelta;
                    _curPlayer.Rotate( _rotateAxis , _playerRollSpeed * _fixedTimeDelta , Space.World );
                }
            }

            //=========================坠落路线
            else
            {
                _curPlayer.Translate( -1 * _curCmdDirection * _playerMoveSpeedFall * _fixedTimeDelta , Space.World );
                _curPlayer.Rotate( _rotateAxis , _playerRollSpeedFall * _fixedTimeDelta , Space.World );
            }
        }
    }





    /// <summary>
    /// 根据配置计算参数
    /// </summary>
    private void CalculateControlParam()
    {
        _speedUpRatio = SPEED_CHANGE_LENGTH * 2f / ( SPEED_CHANGE_TIME * SPEED_CHANGE_TIME );
        _playerRollSpeedFall = PLAYER_ROLL_SPEED_FALL;
        _playerMoveSpeedFall = PLAYER_MOVE_SPEED_FALL;
    }



    /// <summary>
    /// 运动开始前准备更改参数
    /// </summary>
    void StartControlParam( bool selfKill = false )
    {
        _isSelfKill = selfKill;
        //滚动轴
        _rotateAxis = Quaternion.AngleAxis( -90 , Vector3.up ) * _curCmdDirection;

        if( !selfKill )
        {
            var distance = Vector3.Distance( _curPlayer.position , _targetPosistion );
            //重制参数视为开始运动
            _isMoving = true;
            //旋转参数
            _curTurnTime = 0f;

            //移动参数
            _curSpeedUpTime = 0f;
            _playerMoveSpeed = 0f;

            //滚动参数
            _curRollTime = 0f;
            _rollDegree = 90 * distance;

            _playerRollTime = 2 * SPEED_CHANGE_TIME + ( distance - 2 * SPEED_CHANGE_LENGTH ) / ( _speedUpRatio * SPEED_CHANGE_TIME );

            Debug.Log( "滚动预估时间 " + _playerRollTime );

            _playerRollSpeed = _rollDegree / _playerRollTime;
            _playerRollSpeedFall = _playerRollSpeed;
        }
        else
        {
            _isMoving = true;
            _playerColliderBox.enabled = true;
            _playerRigidBody.useGravity = true;
        }
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
        Debug.DrawRay( oriPoint , direction * -100 , Color.red , 1f );

        if ( Physics.RaycastNonAlloc( oriPoint , -1 * direction , _rayHitArr , 100 , 1 << LayerMask.NameToLayer( LAYERNAME_MONSTER ) ) > 0 )
        {
            var hitinfo = _rayHitArr[ 0 ].transform;
            _targetObject = hitinfo;
            _targetPosistion = _targetObject.position + new Vector3( 0 , 0.5f , 0 );
            return true;
        }
        return false;
    }





}
