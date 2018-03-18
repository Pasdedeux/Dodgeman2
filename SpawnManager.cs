using PathologicalGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Linq;
using LTool;

namespace Assets.Scripts.Managers
{
    class SpawnManager : Singleton<SpawnManager>
    {
        SpawnPool _pool;
        List<string> _prefabNameList;
        /// <summary>
        /// 初始化并获取对象池
        /// </summary>
        public void Install()
        {
            _prefabNameList = new List<string>();

            //初始化对象池
            _pool = GameObject.Find( "PoolManager" ).transform.GetComponent<SpawnPool>();
            if ( _pool == null )
                throw new Exception( "对象池初始化失败" );

            //建立对象池库及名单列表
            foreach ( var item in _pool.prefabList )
            {
                _prefabNameList.Add( item.name );
                AddPoolManager( _pool , item.transform , item .name == GlobalDefine.PrefabNames.Man ? 1 : 5);
            }
        }

        /// <summary>
        /// 从对象池中获取一个对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject SpwanObject( string name )
        {
            if ( !_prefabNameList.Contains( name ) )
                throw new Exception( "不存在预制件 " + name );

            return _pool.Spawn( name ).gameObject;
        }

        /// <summary>
        /// 回收如对象池
        /// </summary>
        /// <param name="item"></param>
        public void DespawnObject( Transform item )
        {
            _pool.Despawn( item , _pool.transform );
        }


        /// <summary>
        /// 增加池库
        /// </summary>
        /// <param name="_spawnPool"></param>
        /// <param name="_transform"></param>
        public static void AddPoolManager( SpawnPool _spawnPool , Transform _transform , int num = 5 )
        {
            PrefabPool refabPool = new PrefabPool( _transform );
            if ( !_spawnPool._perPrefabPoolOptions.Contains( refabPool ) )
            {
                refabPool = new PrefabPool( _transform );
                //默认初始化两个Prefab
                refabPool.preloadAmount = num;
                //开启限制
                refabPool.limitInstances = true;
                //关闭无限取Prefab
                refabPool.limitFIFO = true;
                //限制池子里最大的Prefab数量
                refabPool.limitAmount = 1000;
                //开启自动清理池子
                refabPool.cullDespawned = true;
                //最终保留
                refabPool.cullAbove = 10;
                //多久清理一次
                refabPool.cullDelay = 5;
                //每次清理几个
                refabPool.cullMaxPerPass = 5;
                //初始化内存池
                _spawnPool._perPrefabPoolOptions.Add( refabPool );
                _spawnPool.CreatePrefabPool( refabPool );
            }
        }
    }
}
