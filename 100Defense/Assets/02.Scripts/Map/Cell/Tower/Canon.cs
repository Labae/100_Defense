using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    /// <summary>
    /// 공격 속도.
    /// </summary>
    private float mAttackSpeed;
    /// <summary>
    /// 공격 속도에 따른 타이머. (이 타이머가 0이 되면 값이 mAttackSpeed값으로 변경)
    /// </summary>
    private float mAttackSpeedTimer;
    /// <summary>
    /// 공격력.
    /// </summary>
    private int mAttackDamage;
    /// <summary>
    /// 공격 범위.
    /// </summary>
    private float mAttackRange;

    /// <summary>
    /// 오브젝트 풀 클래스.
    /// </summary>
    private ObjectPool mObjectPool;
    /// <summary>
    /// 초기화 되었는지를 표시.
    /// </summary>
    private bool mIsInitialize;

    /// <summary>
    /// 최대 공격 가능 각도.
    /// </summary>
    private const float mViewAngle = 30.0f;

    /// <summary>
    /// 타워의 y 각도.
    /// </summary>
    private float mTowerYRotation;

    public float ViewAngle
    {
        get
        {
            return mViewAngle;
        }
    }

    public float AttackRange
    {
        get
        {
            return mAttackRange;
        }
    }

    #region Method
    /// <summary>
    /// 캐논 초기화 함수.
    /// </summary>
    /// <param name="towerData"></param>
    /// <returns></returns>
    public bool Initialize(TowerData towerData)
    {
        mAttackSpeed = towerData.Attackspeed;
        mAttackSpeedTimer = 0.0f;
        mAttackDamage = towerData.Damage;
        mAttackRange = towerData.Range;

        mObjectPool = GameManager.Instance.GetObjectPool();
        if(!mObjectPool)
        {
            Debug.Log("Failed Get mObjectPool");
            return false;
        }

        mIsInitialize = true;
        return mIsInitialize;
    }

    /// <summary>
    /// 캐논 루프 함수.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="towerRotationY"></param>
    public void Loop(Transform target, float towerRotationY)
    {
        mAttackSpeedTimer -= Time.deltaTime;
        mTowerYRotation = towerRotationY;
        if (target == null)
        {
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        // y좌표는 차이나면 안되므로 0으로 설정해야함.
        dir.y = 0.0f;
        if(Vector3.Angle(transform.forward, dir) < ViewAngle * 0.5f)
        {
            if (mAttackSpeedTimer <= 0)
            {
                mAttackSpeedTimer = mAttackSpeed;
                mObjectPool.SpawnBulletFromPool(target, transform.position, mAttackDamage);
            }
        }
    }

    /// <summary>
    /// 공격력 업그레이드
    /// </summary>
    /// <param name="newAttackDamage"></param>
    public void UpgradeAttackDamage(int newAttackDamage)
    {
        mAttackDamage = mAttackDamage + newAttackDamage;
    }

    /// <summary>
    /// 공격 속도 업그레이드.
    /// </summary>
    /// <param name="newAttackSpeed"></param>
    public void UpgradeAttackSpeed(float newAttackSpeed)
    {
        mAttackSpeed = mAttackSpeed + newAttackSpeed;
    }

    /// <summary>
    /// 공격력 다운그레이드.
    /// </summary>
    /// <param name="newAttackDamage"></param>
    public void DowngradeAttackDamage(int newAttackDamage)
    {
        mAttackDamage = mAttackDamage - newAttackDamage;
    }

    /// <summary>
    /// 공격속도 다운그레이드.
    /// </summary>
    /// <param name="newAttackSpeed"></param>
    public void DowngradeAttackSpeed(float newAttackSpeed)
    {
        mAttackSpeed = mAttackSpeed - newAttackSpeed;
    }

    /// <summary>
    /// 공격 범위 업그레이드.
    /// </summary>
    /// <param name="newAttackRange"></param>
    public void UpgradeAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange + newAttackRange;
    }

    /// <summary>
    /// 공격력 업그레이드.
    /// </summary>
    /// <param name="newAttackRange"></param>
    public void DowngradeAttackRange(float newAttackRange)
    {
        mAttackRange = mAttackRange - newAttackRange;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += mTowerYRotation;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    #endregion

    #region Get Set
    /// <summary>
    /// 공격 속도 타이머를 0으로 설정.
    /// </summary>
    public void SetAttackTimerZero()
    {
        mAttackSpeedTimer = 0.0f;
    }

    /// <summary>
    /// 초기화 되었는지를 가져옴.
    /// </summary>
    /// <returns></returns>
    public bool GetIsInitialize()
    {
        return mIsInitialize;
    }
    #endregion
}
