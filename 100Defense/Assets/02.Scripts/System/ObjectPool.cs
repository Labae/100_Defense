using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    };

    #region Dictionary
    /// <summary>
    /// 맵에 설치될 타워의 오브젝트 풀.
    /// </summary>
    public Dictionary<string, Queue<GameObject>> TowerObjectPoolDictionary;
    /// <summary>
    /// 오브젝트들이 Active가 false일때 저장될 부모 Transform.
    /// </summary>
    private Dictionary<string, Transform> mTowerObjectPoolParentChildDictionary;
    /// <summary>
    /// Tower Data Dictionary
    /// </summary>
    public Dictionary<string, TowerData> TowerDataDictionary;
    #endregion

    private Tower mTowerData;

    #region Method
    public bool Initialize(Vector2 mapSize)
    {
        TowerObjectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        TowerDataDictionary = new Dictionary<string, TowerData>();
        mTowerObjectPoolParentChildDictionary = new Dictionary<string, Transform>();

        GameObject objectPoolParnet = new GameObject("Object Pool Parent");
        Transform objectPoolParentTrs = objectPoolParnet.transform;

        mTowerData = GetTowerData();
        
        int towerTypeLength = mTowerData.dataArray.Length;
        int poolSize = (int)mapSize.x * (int)mapSize.y - ((int)mapSize.x + (int)mapSize.y - 1);

        for (int i = 0; i < towerTypeLength; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            Pool pool = new Pool();
            pool.key = mTowerData.dataArray[i].Towerkey;
            pool.prefab = Resources.Load("01.Prefabs/Tower/" + mTowerData.dataArray[i].Modelname) as GameObject;
            pool.size = poolSize;

            GameObject objectPoolParnetChild = new GameObject("Object Pool Parent" + i.ToString());
            objectPoolParnetChild.transform.SetParent(objectPoolParentTrs);
            objectPoolParnetChild.transform.position = Vector3.zero;
            objectPoolParnetChild.transform.rotation = Quaternion.identity;
            objectPoolParnetChild.transform.localScale = Vector3.one;

            for (int index = 0; index < pool.size; index++)
            {
                GameObject obj = Instantiate(pool.prefab, objectPoolParnetChild.transform);
                obj.SetActive(false);
                obj.transform.localScale = Vector3.zero;
                objectPool.Enqueue(obj);
            }

            TowerObjectPoolDictionary.Add(pool.key, objectPool);
            mTowerObjectPoolParentChildDictionary.Add(pool.key, objectPoolParnetChild.transform);
            TowerDataDictionary.Add(mTowerData.dataArray[i].Towerkey, mTowerData.dataArray[i]);
        }

        return true;
    }

    /// <summary>
    /// return gameobject doesn't active.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public GameObject SpawnTowerFromPool(string key, Transform parent)
    {
        if(!TowerObjectPoolDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in TowerObjectPoolDictionary");
            return null;
        }

        GameObject objectToSpawn = TowerObjectPoolDictionary[key].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(parent);

        return objectToSpawn;
    }

    public void HideTower(string key, GameObject tower)
    {
        if (!TowerObjectPoolDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in TowerObjectPoolDictionary");
            return;
        }

        if (!mTowerObjectPoolParentChildDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in mTowerObjectPoolParentChildDictionary");
            return;
        }

        TowerObjectPoolDictionary[key].Enqueue(tower);
        tower.SetActive(false);
        tower.transform.GetChild(0).localScale = Vector3.zero;
        tower.transform.SetParent(mTowerObjectPoolParentChildDictionary[key]);
    }
    #endregion

    #region Get
    public Tower GetTowerData()
    {
        if(mTowerData != null)
        {
            return mTowerData;
        }

        mTowerData = Resources.Load("03.Datas/Game/TowerData") as Tower;
        if (!mTowerData)
        {
            Debug.Log("Tower data not load");
            return null;
        }

        return mTowerData;
    }
    #endregion
}
