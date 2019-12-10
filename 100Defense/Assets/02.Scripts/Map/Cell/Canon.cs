using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private float mAttackSpeed;
    private float mAttackSpeedTimer;
    private int mAttackDamage;

    private ObjectPool mObjectPool;

    private bool mIsInitialize;

    private const float mAngle = 10.0f;

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

    public void Loop(Transform target)
    {
        mAttackSpeedTimer -= Time.deltaTime;

        //Vector3 dir = target.position - transform.position;
        //dir.y = 0.0f;

        //float angle = Quaternion.FromToRotation(transform.forward, dir).eulerAngles.y;
        //if(angle >= 180.0f)
        //{
        //    angle -= 180.0f;
        //    angle = 180.0f - angle;
        //}
        //if(angle > mAngle)
        //{
        //    return;
        //}

        if (mAttackSpeedTimer <= 0)
        {
            mAttackSpeedTimer = mAttackSpeed;
            mObjectPool.SpawnBulletFromPool(target, transform.position, mAttackDamage);
        }

    }

    public void SetAttackTimerZero()
    {
        mAttackSpeedTimer = 0.0f;
    }

    public bool IsInitialize()
    {
        return mIsInitialize;
    }
}
