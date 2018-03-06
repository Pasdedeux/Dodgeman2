using LTool;
using System;
using UnityEngine;
using System.Collections.Generic;

public class DataModel : Singleton<DataModel>
{
    public int CurLevel { get; set; }
}


public class LevelClass
{
    public int levelID = 0;
    public List<LevelCell> leveCellList = new List<LevelCell>();

    public LevelClass() { }

    public LevelClass( int id )
    {
        levelID = id;
        leveCellList = new List<LevelCell>();
    }
}



public class LevelCell
{
    public string name;
    public string prefabName;
    public double px, py, pz;
    public double rx, ry, rz;
    public double sx, sy, sz;
    public bool noIdentity = false;

    public ObjectType type = ObjectType.None;
    public RolesType roleType = RolesType.None;
    public PropType propType = PropType.None;
    public PointsType pointType = PointsType.None;
    public ObstaclesType obstacleType = ObstaclesType.None;

    public List<LevelCell> leveCellList = new List<LevelCell>();

    public LevelCell() { }

    public LevelCell( Transform target )
    {
        name = target.name;
        prefabName = name.Split( " (".ToCharArray() )[ 0 ];
        px = target.position.x;
        py = target.position.y;
        pz = target.position.z;
        rx = target.rotation.eulerAngles.x;
        ry = target.rotation.eulerAngles.y;
        rz = target.rotation.eulerAngles.z;
        sx = target.localScale.x;
        sy = target.localScale.y;
        sz = target.localScale.z;

        Identity ident = target.GetComponent<Identity>();
        this.noIdentity = ident == null ? true : false;
        if( !this.noIdentity )
        {
            type = ident.type;
            roleType = ident.rolesType;
            propType = ident.propType;
            pointType = ident.pointsType;
            obstacleType = ident.obstacleType;
        }
    }


    public void CopyTo( LevelCell lc )
    {
        lc.name = this.name;
        lc.prefabName = this.prefabName;
        lc.px = this.px;
        lc.py = this.py;
        lc.pz = this.pz;
        lc.rx = this.rx;
        lc.ry = this.ry;
        lc.rz = this.rz;
        lc.sx = this.sx;
        lc.sy = this.sy;
        lc.sz = this.sz;
        lc.noIdentity = this.noIdentity;

        lc.type = this.type;
        lc.roleType = this.roleType;
        lc.propType = this.propType;
        lc.pointType = this.pointType;
        lc.obstacleType = this.obstacleType;
    }
}

[Flags]
/// <summary>
/// 角色身上状态
/// </summary>
public enum RoleStatus
{
    /// <summary>
    /// 无状态
    /// </summary>
    None = 1 << 0,
}

/// <summary>
/// 类型-技能
/// </summary>
public enum SkillType
{
    None,
}

/// <summary>
/// 编辑器-物体总体类型
/// </summary>
public enum ObjectType
{
    None,
    /// <summary>
    /// 不可通行类
    /// </summary>
    Obstacles,
    /// <summary>
    /// 点位类
    /// </summary>
    Points,
    /// <summary>
    /// 角色类
    /// </summary>
    Roles,
    /// <summary>
    /// 道具类
    /// </summary>
    Prop,
}

/// <summary>
/// 类型-障碍物
/// </summary>
public enum ObstaclesType
{
    None,
    /// <summary>
    /// 地面
    /// </summary>
    Ground,
    /// <summary>
    /// 障碍物
    /// </summary>
    Obstacle,
    /// <summary>
    /// 空气墙
    /// </summary>
    AirObstacle,
    /// <summary>
    /// 装饰物
    /// </summary>
    Decoration
}

/// <summary>
/// 点位类型
/// </summary>
public enum PointsType
{
    None,
    /// <summary>
    /// 玩家出生点
    /// </summary>
    PlayerPoint,
    /// <summary>
    /// 金币点
    /// </summary>
    GoldPoint,
    /// <summary>
    /// 怪物诞生点
    /// </summary>
    MonsterPoint,
    /// <summary>
    /// 道具诞生点
    /// </summary>
    PropPoint,
    /// <summary>
    /// 传送点出口
    /// </summary>
    TransferPointExit,
}

/// <summary>
/// 道具类型
/// </summary>
public enum PropType
{
    None,
    /// <summary>
    /// 金币道具
    /// </summary>
    Gold,
}

/// <summary>
/// 角色类型
/// </summary>
public enum RolesType
{
    /// <summary>
    /// 未指定
    /// </summary>
    None,
    /// <summary>
    /// 玩家
    /// </summary>
    Player,
    /// <summary>
    /// 怪物
    /// </summary>
    Monster,
}

