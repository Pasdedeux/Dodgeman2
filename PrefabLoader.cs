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
        //TODO 临时加载预制件列表
        _prefabList.Add( GlobalDefine.UINames.Loading , Resources.Load<GameObject>( "UI/Panel_Loading" ) );
        _prefabList.Add( GlobalDefine.UINames.MainMenu , Resources.Load<GameObject>( "UI/Panel_MainMenu" ) );
        _prefabList.Add( GlobalDefine.UINames.Level , Resources.Load<GameObject>( "UI/Panel_Level" ) );
        _prefabList.Add( GlobalDefine.UINames.LevelChoose , Resources.Load<GameObject>( "UI/Panel_LevelChoose" ) );
        _prefabList.Add( GlobalDefine.UINames.TipsConfirm , Resources.Load<GameObject>( "UI/Panel_Tips" ) );
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
