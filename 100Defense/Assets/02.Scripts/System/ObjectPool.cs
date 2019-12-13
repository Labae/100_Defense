using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public class TowerPool
    {
        private readonly string key;
        private readonly GameObject prefab;
        private readonly int size;

        public TowerPool(string _key, GameObject _prefab, int _size)
        {
            key = _key;
            prefab = _prefab;
            size = _size;
        }

        public string GetKey()
        {
            return key;
        }

        public GameObject GetPrefab()
        {
            return prefab;
        }

        public int GetSize()
        {
            return size;
        }
    };

    #region Dictionary
    /// <summary>
    /// 맵에 설치될 타워의 오브젝트 풀.
    /// </summary>
    private Dictionary<string, Queue<GameObject>> mTowerObjectPoolDictionary;
    /// <summary>
    /// 오브젝트들이 Active가 false일때 저장될 부모 Transform.
    /// </summary>
    private Dictionary<string, Transform> mTowerObjectPoolParentChildDictionary;
    /// <summary>
    /// Tower Data Dictionary
    /// </summary>
    private Dictionary<string, TowerData> mTowerDataDictionary;
    private Dictionary<BuffType, Queue<GameObject>> mTowerEffectDictionary;
    /// <summary>
    /// bullet object pool.
    /// </summary>
    public Queue<GameObject> BulletObjectPoolQueue;
    private Queue<GameObject> mBulletImactPoolQueue;
    private List<Transform> mTowerEffectParents;
    #endregion

    private Tower mTowerData;

    private int mBulletAndImpactPoolLength = 250;
    private Transform mBulletPoolParent;
    private Transform mBulletImpactPoolParent;
    private WaitForSeconds mWFSImpactHide;

    public Dictionary<string, TowerData> TowerDataDictionary { get => mTowerDataDictionary; }

    #region Method
    public bool Initialize(Vector2 mapSize)
    {
        mTowerObjectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
        mTowerDataDictionary = new Dictionary<string, TowerData>();
        mTowerObjectPoolParentChildDictionary = new Dictionary<string, Transform>();
        BulletObjectPoolQueue = new Queue<GameObject>();
        mBulletImactPoolQueue = new Queue<GameObject>();
        mTowerEffectDictionary = new Dictionary<BuffType, Queue<GameObject>>();
        mTowerEffectParents = new List<Transform>();
        mWFSImpactHide = new WaitForSeconds(2.0f);

        GameObject objectPoolParnet = new GameObject("Object Pool Parent");
        Transform objectPoolParentTrs = objectPoolParnet.transform;

        mTowerData = GetTowerData();

        int towerTypeLength = mTowerData.dataArray.Length;
        int poolSize = (int)mapSize.x * (int)mapSize.y - ((int)mapSize.x + (int)mapSize.y - 1);

        for (int i = 0; i < towerTypeLength; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            TowerPool pool = new TowerPool(mTowerData.dataArray[i].Towerkey,
                Resources.Load("01.Prefabs/Tower/" + mTowerData.dataArray[i].Modelname) as GameObject,
                poolSize);

            GameObject objectPoolParnetChild = new GameObject("Object Pool Parent" + i.ToString());
            objectPoolParnetChild.transform.SetParent(objectPoolParentTrs);
            objectPoolParnetChild.transform.position = Vector3.zero;
            objectPoolParnetChild.transform.rotation = Quaternion.identity;
            objectPoolParnetChild.transform.localScale = Vector3.one;

            for (int index = 0; index < pool.GetSize(); index++)
            {
                GameObject obj = Instantiate(pool.GetPrefab(), objectPoolParnetChild.transform);
                obj.SetActive(false);
                obj.transform.localScale = Vector3.zero;
                objectPool.Enqueue(obj);
            }

            mTowerObjectPoolDictionary.Add(pool.GetKey(), objectPool);
            mTowerObjectPoolParentChildDictionary.Add(pool.GetKey(), objectPoolParnetChild.transform);
            TowerDataDictionary.Add(mTowerData.dataArray[i].Towerkey, mTowerData.dataArray[i]);
        }

        GameObject bulletPrefab = Resources.Load("01.Prefabs/Bullet/Bullet") as GameObject;
        GameObject bulletImapct = Resources.Load("01.Prefabs/Bullet/BulletImpact") as GameObject;
        GameObject bulletPoolParent = new GameObject("Bullet Pool Parent");
        GameObject bulletImpactPoolParent = new GameObject("BulletImpact Pool Parent");

        mBulletPoolParent = bulletPoolParent.transform;
        mBulletImpactPoolParent = bulletImpactPoolParent.transform;
        mBulletPoolParent.SetParent(objectPoolParentTrs);
        mBulletImpactPoolParent.SetParent(objectPoolParentTrs);

        for (int i = 0; i < mBulletAndImpactPoolLength; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, mBulletPoolParent);
            bullet.AddComponent<BulletClass>();
            bullet.SetActive(false);
            BulletObjectPoolQueue.Enqueue(bullet);

            GameObject imapct = Instantiate(bulletImapct, mBulletImpactPoolParent);
            imapct.SetActive(false);
            mBulletImactPoolQueue.Enqueue(imapct);
        }

        int buffTypeCount = (int)BuffType.End - 1;

        GameObject towerEffectParent = new GameObject("TowerEffect Pool Parent");
        towerEffectParent.transform.SetParent(objectPoolParentTrs);
        towerEffectParent.transform.position = Vector3.zero;
        towerEffectParent.transform.rotation = Quaternion.identity;
        towerEffectParent.transform.localScale = Vector3.one;

        GameObject attackEffect = Resources.Load("01.Prefabs/Tower/Buff/AttackEffect") as GameObject;
        GameObject rangeEffect = Resources.Load("01.Prefabs/Tower/Buff/RangeEffect") as GameObject;
        GameObject speedEffect = Resources.Load("01.Prefabs/Tower/Buff/SpeedEffect") as GameObject;

        List<GameObject> towerEffectPrefabs = new List<GameObject>();
        towerEffectPrefabs.Add(attackEffect);
        towerEffectPrefabs.Add(rangeEffect);
        towerEffectPrefabs.Add(speedEffect);
        for (int index = 0; index < buffTypeCount; index++)
        {
            GameObject parent = new GameObject("Buff" + index.ToString());
            parent.transform.SetParent(towerEffectParent.transform);
            mTowerEffectParents.Add(parent.transform);
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(towerEffectPrefabs[index], parent.transform);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            BuffType key = (BuffType)index + 1;

            mTowerEffectDictionary.Add(key, objectQueue);
        }


        return true;
    }

    /// <summary>
    /// return object pool gameobject
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public GameObject SpawnTowerFromPool(string key, Transform parent)
    {
        if (!mTowerObjectPoolDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in TowerObjectPoolDictionary");
            return null;
        }

        GameObject objectToSpawn = mTowerObjectPoolDictionary[key].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(parent);

        return objectToSpawn;
    }

    public void HideTower(string key, GameObject tower)
    {
        if (!mTowerObjectPoolDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in TowerObjectPoolDictionary");
            return;
        }

        if (!mTowerObjectPoolParentChildDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in mTowerObjectPoolParentChildDictionary");
            return;
        }

        mTowerObjectPoolDictionary[key].Enqueue(tower);
        tower.SetActive(false);
        tower.transform.GetChild(0).localScale = Vector3.zero;
        tower.transform.SetParent(mTowerObjectPoolParentChildDictionary[key]);
        TowerClass towerClass = tower.GetComponent<TowerClass>();
        if (towerClass != null)
        {
            Destroy(towerClass);
        }
    }

    public void SpawnBulletFromPool(Transform target, Vector3 position, int damage)
    {
        if (BulletObjectPoolQueue.Count < 0 || mBulletImactPoolQueue.Count < 0)
        {
            Debug.Log("BulletObjectPoolQueue or mBulletImactPoolQueue is less than 0");
            return;
        }

        GameObject objectToSpawn = BulletObjectPoolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.SetActive(true);
        objectToSpawn.GetComponent<BulletClass>().Initialize(target, damage, mBulletImactPoolQueue.Dequeue());
    }

    public void HideBullet(GameObject bullet)
    {
        BulletObjectPoolQueue.Enqueue(bullet);
        bullet.SetActive(false);
        bullet.transform.SetParent(mBulletPoolParent);
        bullet.transform.localPosition = Vector3.zero;

        GameObject bulletImpact = bullet.GetComponent<BulletClass>().GetBulletImpact();
        mBulletImactPoolQueue.Enqueue(bulletImpact);
        bulletImpact.transform.SetParent(mBulletImpactPoolParent);
        StartCoroutine(BulletImpactHideCoroutine(bulletImpact));
    }

    public GameObject SpawnTowerEffect(BuffType key, Transform parent)
    {
        if (!mTowerEffectDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in mTowerBuffDictionary");
        }

        GameObject objectToSpawn = mTowerEffectDictionary[key].Dequeue();
        if (objectToSpawn == null)
        {
            Debug.Log("Failed to spawn tower buff effect");
            return null;
        }
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(parent);
        objectToSpawn.transform.localPosition = Vector3.zero;
        objectToSpawn.transform.localScale = Vector3.one;

        ParticleSystem ps = objectToSpawn.GetComponent<ParticleSystem>();
        ps.Play();

        return objectToSpawn;
    }

    public void HideTowerEffect(BuffType key, GameObject tower)
    {
        if (!mTowerEffectDictionary.ContainsKey(key))
        {
            Debug.Log("Failed Find key in mTowerBuffDictionary");
            return;
        }

        mTowerEffectDictionary[key].Enqueue(tower);
        tower.SetActive(false);
        int index = (int)key - 1;
        tower.transform.SetParent(mTowerEffectParents[index]);
    }
    #endregion

    #region Coroutine
    private IEnumerator BulletImpactHideCoroutine(GameObject impact)
    {
        yield return mWFSImpactHide;
        impact.SetActive(false);
        impact.transform.localPosition = Vector3.zero;
    }
    #endregion

    #region Get
    public Tower GetTowerData()
    {
        if (mTowerData != null)
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
