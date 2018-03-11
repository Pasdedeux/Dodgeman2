using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTool;
using Assets.Scripts.Managers;

public class PrefabLoader : Singleton<PrefabLoader>
{
    private Dictionary<string , GameObject> _prefabList;

    public void InitUIPrefabs()
    {
        _prefabList = new Dictionary<string , GameObject>();
        //TODO 临时
        //加载预制件列表
        _prefabList.Add( GlobalDefine.UINames.MainMenu , Resources.Load<GameObject>( "UI/Panel_MainMenu" ) );
        _prefabList.Add( GlobalDefine.UINames.Loading , Resources.Load<GameObject>( "UI/Panel_Loading" ) );
    }

    public GameObject GetPrefab( string prefabName )
    {
        //命名规则
        if( prefabName.Contains("UI") )
        {
            if( !_prefabList.ContainsKey( prefabName ) )
                //TODO 先尝试寻找加载，否则为null
                return null;
            return GameObject.Instantiate( _prefabList[ prefabName ] );
        }
        else
        {
            return SpawnManager.Instance.SpwanObject( prefabName );
        }
    }
}
