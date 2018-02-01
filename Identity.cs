using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identity : MonoBehaviour
{
    /// <summary>
    /// 关联的预制件
    /// </summary>
    public List<GameObject> linkPrefab;

    /// <summary>
    /// 角色类型
    /// </summary>
    public IRole role;

    /// <summary>
    /// 物体身份类型
    /// </summary>
    public ObjectType type;

    /// <summary>
    /// 障碍物类型
    /// </summary>
    public ObstaclesType obstacleType;

    /// <summary>
    /// 点位类型
    /// </summary>
    public PointsType pointsType;

    /// <summary>
    /// 道具类型
    /// </summary>
    public PropType propType;

   /// <summary>
   /// 角色类型
   /// </summary>
    public RolesType rolesType;

    public int ID,Score;


    void Start( )
    {
        //根据type信息添加组件
        TypeFilter();
    }

    private void TypeFilter( )
    {
        switch ( type )
        {
            case ObjectType.None:

                break;
            case ObjectType.Obstacles:
                
                break;
            case ObjectType.Points:

                break;
            case ObjectType.Roles:

                break;
            case ObjectType.Prop:
                
                break;
        }
    }
}
