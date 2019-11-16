using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private float mAttackSpeed;
    private float mAttackSpeedTimer;

    private GameObject mBulletPrefab;

    public void Initialize(float _speed)
    {
        mAttackSpeed = _speed;
        mAttackSpeedTimer = 0.0f;

        mBulletPrefab = Resources.Load("01.Prefabs/Bullet") as GameObject;
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
                GameObject bulletObj = Instantiate(mBulletPrefab, transform.position, Quaternion.identity);
                BulletClass bullet = bulletObj.AddComponent<BulletClass>();
                bullet.Initialize(target);
            }
        }
    }

    public void SetAttackTimerZero()
    {
        mAttackSpeedTimer = 0.0f;
    }
}
