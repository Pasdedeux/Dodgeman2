using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        public string prefabName;
        public int poolSize = 5;

        public virtual GameObject GetObject( int index )
        {
            var go = SpawnManager.Instance.SpwanObject( index%2 ==0 ? GlobalDefine.UINames.Child_LevelBrick : GlobalDefine.UINames.Child_LevelLine );
            //TODO 这里需要加入对是否是当前可以进行的最后一关的判定
            //..

            go.transform.localScale = Vector3.one;
            Debug.Log( go.transform.localScale );
            return go;
        }

        public virtual void ReturnObject(Transform go)
        {
            go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            SpawnManager.Instance.DespawnObject( go );
        }
    }
}
