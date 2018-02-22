using LTool;
using System;

public class DataModel:Singleton<DataModel>
{
    
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
    GoldPoint ,
    /// <summary>
    /// 怪物诞生点
    /// </summary>
    MonsterPoint,
    /// <summary>
    /// 道具诞生点
    /// </summary>
    PropPoint ,
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

