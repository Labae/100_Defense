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
    private const float mAngle = 30.0f;

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
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        dir.y = 0.0f;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        if(towerRotationY >= 180.0f)
        {
            angle += 180.0f;
            towerRotationY -= 180.0f;
        }

        float finalAngle = (towerRotationY >= angle) ? towerRotationY - angle : angle - towerRotationY;

        if (finalAngle > mAngle)
        {
            Debug.DrawRay(transform.position, transform.forward * 10.0f, Color.red);
            return;
        }

        if (mAttackSpeedTimer <= 0)
        {
            mAttackSpeedTimer = mAttackSpeed;
            mObjectPool.SpawnBulletFromPool(target, transform.position, mAttackDamage);
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
