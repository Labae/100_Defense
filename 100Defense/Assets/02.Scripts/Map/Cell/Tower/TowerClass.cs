using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClass : MonoBehaviour
{
    /// <summary>
    /// 타워의 원 크기.
    /// </summary>
    private Vector3 mOriginScale;
    /// <summary>
    /// 오브젝트 풀 클래스.
    /// </summary>
    private ObjectPool mObjectPool;
    /// <summary>
    /// 타워 정보.
    /// </summary>
    protected TowerData mTowerData;
    /// <summary>
    /// 타워 가격.
    /// </summary>
    private int mPrice;
    /// <summary>
    /// 타워 타입.
    /// </summary>
    private TowerType mTowerType;
    public TowerType TowerType
    {
        get
        {
            return mTowerType;
        }
    }

    #region Method
    /// <summary>
    /// 타워 초기화 함수.
    /// </summary>
    /// <param name="cellData"></param>
    /// <returns></returns>
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
        mTowerType = mTowerData.TOWERTYPE;

        return true;
    }

    /// <summary>
    /// 타워 루프 함수.
    /// </summary>
    /// <param name="enemies"></param>
    public virtual void Loop(List<EnemyClass> enemies)
    {
    }

    /// <summary>
    /// 타워 짓는 함수.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="cellData"></param>
    /// <returns></returns>
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
        mTowerType = mTowerData.TOWERTYPE;

        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), mTowerData.Towerkey);
        StopCoroutine(ApperanceAnim());
        StartCoroutine(ApperanceAnim());

        return true;
    }

    /// <summary>
    /// 타워 파괴 함수.
    /// </summary>
    /// <param name="cell"></param>
    public virtual void DestroyTower(CellClass cell)
    {
        cell.GetMap().SetMapData(cell.GetCellX(), cell.GetCellY(), null);
        cell.GetMap().RemoveTower(this);
        GameManager.Instance.GetPlayerInfo().Gold += Mathf.RoundToInt(mPrice * 0.5f);
        StartCoroutine(DestoryCoroutine());
    }

    #region Change Tower Data Method
    public virtual void Upgrade(float newAttackRange, int newAttackDamage, float newAttackSpeed)
    {

    }

    public virtual void DownGrade(float newAttackRange, int newAttackDamage, float newAttackSpeed)
    {

    }

    public virtual CellClass[] GetBuffArea()
    {
        return null;
    }
    #endregion

    #endregion

    #region Coroutine
    /// <summary>
    /// 타워 등장 애니메이션.
    /// </summary>
    /// <returns></returns>
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

    #endregion

    #region Get
    public int GetPrice()
    {
        return mPrice;
    }
    #endregion

}
