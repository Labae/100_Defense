using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    private float mSpeed = 10.0f;
    private int mAttackDamage;
    private Transform mTargetTrs;
    private GameObject mBulletImpact;
    private ObjectPool mObjectPool;

    public bool Initialize(Transform target, int damage, GameObject impact)
    {
        if(target == null)
        {
            return false;
        }
        mTargetTrs = target.GetChild(0);
        if (mTargetTrs == null)
        {
            return false;
        }
        mAttackDamage = damage;


        mBulletImpact = impact;
        if (!mBulletImpact)
        {
            Debug.Log("Failed load bulletEffect");
            return false;
        }

        mObjectPool = GameManager.Instance.GetObjectPool();
        if (!mObjectPool)
        {
            Debug.Log("Failed Get ObjectPool");
            return false;
        }
        StartCoroutine(MoveCoroutine(mTargetTrs));

        return true;
    }

    private IEnumerator MoveCoroutine(Transform target)
    {
        while (target != null && transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, mSpeed * Time.deltaTime);
            transform.LookAt(target);

            yield return null;
        }

        if(target == null)
        {
            mObjectPool.HideBullet(gameObject);
            yield break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamagable damagable = other.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(mAttackDamage);
                mBulletImpact.transform.position = other.transform.position;
                mBulletImpact.SetActive(true);
                ParticleSystem ps = mBulletImpact.GetComponent<ParticleSystem>();
                ps.Play();
                mObjectPool.HideBullet(gameObject);
            }
        }
    }

    public GameObject GetBulletImpact()
    {
        return mBulletImpact;
    }
}
