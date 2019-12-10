using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClass : MonoBehaviour
{
    private Vector3 mOriginScale;
    private Canon mCanon;
    private ObjectPool mObjectPool;
    private TowerData mTowerData;
    private float mAttackRange;

    private float mRotateSpeed = 1.0f;
    private int mPrice;

    public bool Initialize(string cellData)
    {
        transform.localPosition = Vector3.zero;
        mOriginScale = Vector3.one * 2.0f;

        mObjectPool = GameManager.Instance.GetObjectPool();
        if (mObjectPool == null)
        {
            Debug.Log("Failed Get Object Pool");
            return false;
        }

        mTowerData = null;
        if (!mObjectPool.TowerDataDictionary.ContainsKey(cellData))
        {
            Debug.Log("Failed Find TowerData in TowerDataDictionary");
            return false;
        }

        mTowerData = mObjectPool.TowerDataDictionary[cellData];

        mCanon = GetComponentInChildren<Canon>();
        if (!mCanon)
        {
            Debug.Log("Failed Get Canon Component");
            return false;
        }

        if(!mCanon.IsInitialize())
        {
            if (!mCanon.Initialize(mTowerData))
            {
                Debug.Log("Failed Initialize Canon");
                return false;
            }
        }

        mAttackRange = mTowerData.Range;
        mPrice = mTowerData.Price;

        return true;
    }

    public void Loop(List<EnemyClass> enemies)
    {
        Transform target = null;
        target = GetWithinRange(enemies);
        if (target == null)
        {
            return;
        }

        Rotate(enemies, target);
        mCanon.Loop(target);
    }

    private Transform GetWithinRange(List<EnemyClass> enemies)
    {
        Transform nearestEnemy = null;
        float shortestDist = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (dist <= shortestDist)
            {
                shortestDist = dist;
                nearestEnemy = enemies[i].transform;
                break;
            }
        }

        if (shortestDist <= mAttackRange)
        {
            return nearestEnemy;
        }
        else
        {
            return null;
        }
    }

    private void Rotate(List<EnemyClass> enemies, Transform target)
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * mRotateSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
    }

    public bool Build(CellClass cell, string cellData)
    {
        transform.localPosition = Vector3.zero;
        mOriginScale = Vector3.one * 2.0f;

        mObjectPool= GameManager.Instance.GetObjectPool();
        if (mObjectPool == null)
        {
            Debug.Log("Failed Get Object Pool");
            return false;
        }

        mTowerData = null;
        if (!mObjectPool.TowerDataDictionary.ContainsKey(cellData))
        {
            Debug.Log("Failed Find TowerData in TowerDataDictionary");
            return false;
        }

        mTowerData = mObjectPool.TowerDataDictionary[cellData];

        mCanon = GetComponentInChildren<Canon>();
        if (!mCanon)
        {
            Debug.Log("Failed Get Canon Component");
            return false;
        }

        if (!mCanon.Initialize(mTowerData))
        {
            Debug.Log("Failed Initialize Canon");
            return false;
        }

        mAttackRange = mTowerData.Range;
        mPrice = mTowerData.Price;

        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), mTowerData.Towerkey);
        StopCoroutine(ApperanceAnim());
        StartCoroutine(ApperanceAnim());

        return true;
    }

    public void DestroyTower(CellClass cell)
    {
        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), null);
        cell.GetMap().RemoveTower(this);
        StartCoroutine(DestoryCoroutine());
    }

    public void Destroyimmediately(CellClass cell)
    {
        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), null);
        cell.GetMap().RemoveTower(this);
        mObjectPool.HideTower(mTowerData.Towerkey, gameObject);
    }

    public IEnumerator ApperanceAnim()
    {
        transform.localScale = Vector3.zero;
        float x = 0;
        float y = 0;
        float z = 0;
        float speed = 5.0f;
        float deltaSpeed;
        while (transform.localScale != mOriginScale)
        {
            deltaSpeed = speed * Time.deltaTime;
            x = Mathf.MoveTowards(x, mOriginScale.x, deltaSpeed);
            y = Mathf.MoveTowards(y, mOriginScale.y, deltaSpeed);
            z = Mathf.MoveTowards(z, mOriginScale.z, deltaSpeed);
            transform.localScale = new Vector3(x, y, z);

            yield return null;
        }

        transform.localScale = mOriginScale;
    }

    public IEnumerator DestoryCoroutine()
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        float speed = 6.0f;
        float deltaSpeed;
        while (transform.localScale != Vector3.zero)
        {
            deltaSpeed = speed * Time.deltaTime;
            x = Mathf.MoveTowards(x, 0, deltaSpeed);
            y = Mathf.MoveTowards(y, 0, deltaSpeed);
            z = Mathf.MoveTowards(z, 0, deltaSpeed);
            transform.localScale = new Vector3(x, y, z);

            yield return null;
        }

        mObjectPool.HideTower(mTowerData.Towerkey, gameObject);
    }

    public int GetPrice()
    {
        return mPrice;
    }
}
