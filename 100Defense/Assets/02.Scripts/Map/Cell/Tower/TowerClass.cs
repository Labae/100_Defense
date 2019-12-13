using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClass : MonoBehaviour
{
    private Vector3 mOriginScale;
    private ObjectPool mObjectPool;
    protected TowerData mTowerData;

    private int mPrice;

    public virtual bool Initialize(string cellData)
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

       
        mPrice = mTowerData.Price;

        return true;
    }

    public virtual void Loop(List<EnemyClass> enemies)
    {

    }

    public virtual bool Build(CellClass cell, string cellData)
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

        mPrice = mTowerData.Price;

        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), mTowerData.Towerkey);
        StopCoroutine(ApperanceAnim());
        StartCoroutine(ApperanceAnim());

        return true;
    }

    public virtual void DestroyTower(CellClass cell)
    {
        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), null);
        cell.GetMap().RemoveTower(this);
        GameManager.Instance.GetPlayerInfo().Gold += Mathf.RoundToInt(mPrice * 0.5f);
        StartCoroutine(DestoryCoroutine());
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
    public virtual void UpdateAttackRange(float newAttackRange)
    {
    }
    public virtual void DowngradeAttackRange(float newAttackRange)
    {
    }
    public virtual void UpdateAttackDamage(int newAttackDamage)
    {
    }
    public virtual void DowngradeAttackDamage(int newAttackDamage)
    {
    }
    public virtual void UpdateAttackSpeed(float newAttackSpeed)
    {
    }
    public virtual void DowngradeAttackSpeed(float newAttackSpeed)
    {
    }
}
