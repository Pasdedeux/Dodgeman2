using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;

public class PrefabLoader : Singleton<PrefabLoader>
{
    private Dictionary<string , GameObject> _prefabList;

    public void Init()
    {
        _prefabList = new Dictionary<string , GameObject>();
        //TODO 临时
        //加载预制件列表
        _prefabList.Add( "Camera_Light" , Resources.Load<GameObject>( "Camera_Light" ) );
        _prefabList.Add( "Terminus" , Resources.Load<GameObject>( "Terminus" ) );
        _prefabList.Add( "Enemy" , Resources.Load<GameObject>( "Enemy" ) );
        _prefabList.Add( "Tower" , Resources.Load<GameObject>( "Tower" ) );
        _prefabList.Add( "Floor" , Resources.Load<GameObject>( "Floor" ) );
        _prefabList.Add( "Man" , Resources.Load<GameObject>( "Man" ) );
        _prefabList.Add( "funs" , Resources.Load<GameObject>( "funs" ) );
    }

    public GameObject GetPrefab( string prefabName )
    {
        if( _prefabList == null )
            Init();

        if( !_prefabList.ContainsKey( prefabName ) )
            //TODO 先尝试寻找加载，否则为null
            return null;

        return GameObject.Instantiate( _prefabList[ prefabName ] );
    }
}
