using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private float mAttackSpeed;
    private float mAttackSpeedTimer;
    private int mAttackDamage;

    private GameObject mBulletPrefab;

    public void Initialize(float _speed, int _damage)
    {
        mAttackSpeed = _speed;
        mAttackSpeedTimer = 0.0f;
        mAttackDamage = _damage;

        mBulletPrefab = Resources.Load("01.Prefabs/Bullet/Bullet") as GameObject;
        if (!mBulletPrefab)
        {
            Debug.Log("Failed load bulletObj");
            return;
        }

    }

    public void Loop(Transform target)
    {
        if (target != null)
        {
            mAttackSpeedTimer -= Time.deltaTime * 5.0f;
            if (mAttackSpeedTimer <= 0)
            {
                mAttackSpeedTimer = mAttackSpeed;
                if(!CreateBullet(target))
                {
                    return;
                }
            }
        }
    }

    private bool CreateBullet(Transform target)
    {
        GameObject bulletObj = Instantiate(mBulletPrefab, transform.position, Quaternion.identity);
        BulletClass bullet = bulletObj.AddComponent<BulletClass>();
        if(!bullet.Initialize(target, mAttackDamage))
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
}
