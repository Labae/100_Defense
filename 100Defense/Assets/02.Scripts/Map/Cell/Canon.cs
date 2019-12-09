using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private float mAttackSpeed;
    private float mAttackSpeedTimer;
    private int mAttackDamage;

    private GameObject mBulletPrefab;

    private bool mIsInitialize;

    private const float mAngle = 30.0f;

    public bool Initialize(TowerData towerData)
    {
        if (mIsInitialize)
        {
            return mIsInitialize;
        }

        mAttackSpeed = towerData.Attackspeed;
        mAttackSpeedTimer = 0.0f;
        mAttackDamage = towerData.Damage;

        mBulletPrefab = Resources.Load("01.Prefabs/Bullet/Bullet") as GameObject;
        if (!mBulletPrefab)
        {
            Debug.Log("Failed load bulletObj");
            return false;
        }

        mIsInitialize = true;
        return mIsInitialize;
    }

    public void Loop(Transform target)
    {
        mAttackSpeedTimer -= Time.deltaTime;

        Vector3 dir = target.position - transform.position;
        dir.y = 0.0f;

        Vector3 arcTan = dir - transform.forward;

        float angle = Mathf.Atan2(arcTan.z, arcTan.x) * Mathf.Rad2Deg;
        bool canShoot = ((angle <= mAngle && angle >= 0.0f) || (angle <= mAngle - 180.0f && angle < 0.0f)) ? true : false;

        if(!canShoot)
        {
            return;
        }

        if (mAttackSpeedTimer <= 0)
        {
            mAttackSpeedTimer = mAttackSpeed;
            if (!CreateBullet(target))
            {
                return;
            }
        }

    }

    private bool CreateBullet(Transform target)
    {
        GameObject bulletObj = Instantiate(mBulletPrefab, transform.position, Quaternion.identity);
        BulletClass bullet = bulletObj.AddComponent<BulletClass>();
        if (!bullet.Initialize(target, mAttackDamage))
        {
            Debug.Log("Failed Initialize bulletClass");
            return false;
        }
        return true;
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
