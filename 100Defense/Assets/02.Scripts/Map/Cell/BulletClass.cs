using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    /// <summary>
    /// 총알 이동 속도.
    /// </summary>
    private float mSpeed = 10.0f;
    /// <summary>
    /// 데미지.(공격력)
    /// </summary>
    private int mAttackDamage;
    /// <summary>
    /// 공격받을 타겟.
    /// </summary>
    private Transform mTargetTrs;
    /// <summary>
    /// 총알이 터지는 임팩트(효과).
    /// </summary>
    private GameObject mBulletImpact;
    /// <summary>
    /// 오브젝트 풀.
    /// </summary>
    private ObjectPool mObjectPool;

    #region Method
    /// <summary>
    /// 총알 초기화 함수.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <param name="impact"></param>
    /// <returns></returns>
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
    #endregion

    #region Coroutine
    /// <summary>
    /// 총알이 움직이는 코루틴.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
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
    #endregion

    #region Unity Event
    /// <summary>
    /// 총알이 대상과 충돌했을때 실행되는 함수.
    /// </summary>
    /// <param name="other"></param>
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
    #endregion

    #region Get
    /// <summary>
    /// 총알 폭팔 임팩트를 받아옴.
    /// </summary>
    /// <returns></returns>
    public GameObject GetBulletImpact()
    {
        return mBulletImpact;
    }
    #endregion
}
