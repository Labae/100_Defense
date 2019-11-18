using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    private float mSpeed = 10.0f;
    private int mAttackDamage;
    private Transform mTargetTrs;
    private GameObject mBulletEffectObj;

    public bool Initialize(Transform target, int damage)
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


        mBulletEffectObj = Resources.Load("01.Prefabs/Bullet/BulletEffectImapct") as GameObject;
        if (!mBulletEffectObj)
        {
            Debug.Log("Failed load bulletEffect");
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
            Destroy(this.gameObject);
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
                GameObject obj = Instantiate(mBulletEffectObj, transform.position, Quaternion.identity) as GameObject;
                if (!obj)
                {
                    Debug.Log("Failed Instantiate bulletEffect");
                    return;
                }

                Destroy(obj, 2.0f);
                damagable.Damage(mAttackDamage);
                Destroy(this.gameObject);
            }
        }
    }
}
