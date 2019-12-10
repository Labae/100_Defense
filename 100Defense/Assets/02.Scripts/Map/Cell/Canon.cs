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

    private const float mAngle = 30.0f;

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

    public void Loop(Transform target, float towerRotationY)
    {
        if (target == null)
        {
            return;
        }
        mAttackSpeedTimer -= Time.deltaTime;

        // TODO : 이거 이상함.
        Vector3 dir = target.position - transform.position;
        dir.y = 0.0f;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Debug.Log(transform.parent.parent.parent.name + "A" + angle);
        Debug.Log(transform.parent.parent.parent.name + towerRotationY);

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

        // 요기까지.

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
