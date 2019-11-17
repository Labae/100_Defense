using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    private float mSpeed = 3.0f;
    private int mAttackDamage;
    private Transform mTargetTrs;
    public void Initialize(Transform target, int damage)
    {
        mTargetTrs = target.GetChild(0);
        mAttackDamage = damage;

        StartCoroutine(MoveCoroutine(mTargetTrs));
    }

    private IEnumerator MoveCoroutine(Transform target)
    {
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, mSpeed * Time.deltaTime);
            transform.LookAt(target);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IDamagable damagable = other.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(mAttackDamage);
                Destroy(this.gameObject);
            }
        }
    }
}
