using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour, IDamagable
{
    /// <summary>
    /// 적의 모델.
    /// </summary>
    private GameObject mModel;
    /// <summary>
    /// 맵 매니저 클래스
    /// </summary>
    private MapManager mMap;
    /// <summary>
    /// 적이 죽을 때 나오는 효과.
    /// </summary>
    private GameObject mEnemyEffect;

    /// <summary>
    /// 적의 이동 속도.
    /// </summary>
    private float mSpeed;
    /// <summary>
    /// 적의 체력.
    /// </summary>
    public int Health { get; set; }
    /// <summary>
    /// 적 처치시 나오는 돈.
    /// </summary>
    private int mPrice;

    #region Method
    /// <summary>
    /// 적 초기화 함수.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="enemyKey"></param>
    /// <param name="path"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Initialize(MapManager map, string enemyKey, List<Vector3> path, EnemyData data)
    {
        mMap = map;

        transform.name = data.Name;
        mSpeed = data.Movespeed;
        Health = data.Health;
        mPrice = data.Price;

        mModel = CreateModel(data.Name, path[0]);
        if (!mModel)
        {
            Debug.Log("Enemy Model not load");
            return false;
        }

        mEnemyEffect = Resources.Load("01.Prefabs/Enemy/EnemyEffectImpact") as GameObject;
        if (!mEnemyEffect)
        {
            Debug.Log("EnemyEffectImpact not load");
            return false;
        }

        MeshRenderer meshRenderer = mModel.GetComponent<MeshRenderer>();
        Material mat = Resources.Load("02.Materials/02.Enemys/" + data.Name) as Material;
        if (!mat)
        {
            Debug.Log("Enemy Material not load");
            return false;
        }
        meshRenderer.material = mat;

        mEnemyEffect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = mat;

        mMap.AddEnemy(this);
        StartCoroutine(EnemyCoroutine(path));

        return true;
    }

    /// <summary>
    /// 적 모델 생성 함수.
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="startPos"></param>
    /// <returns></returns>
    private GameObject CreateModel(string modelName, Vector3 startPos)
    {
        transform.position = startPos;

        GameObject modelData = Resources.Load("01.Prefabs/Enemy/" + modelName) as GameObject;
        if (!modelData)
        {
            return null;
        }

        GameObject model = Instantiate(modelData, Vector3.zero, Quaternion.identity);
        model.transform.SetParent(transform);
        float cellHalfSizeY = mMap.GetCell(0, 0).transform.localScale.y * 0.5f;
        model.transform.localPosition = new Vector3(0.0f, cellHalfSizeY + model.transform.localScale.y * 0.5f, 0.0f);

        return model;
    }

    /// <summary>
    /// 적 파괴 함수.
    /// </summary>
    public void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine(5.0f));
        GameObject effect = Instantiate(mEnemyEffect, transform.position, Quaternion.identity) as GameObject;
        if (!effect)
        {
            Debug.Log("Failed Instantiate enemy effect");
        }
        Destroy(effect, 2.0f);
    }
    #endregion

    #region Coroutine
    /// <summary>
    /// 적의 전체적인 코루틴.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator EnemyCoroutine(List<Vector3> path)
    {
        yield return StartCoroutine(ApperanceAnim());
        yield return StartCoroutine(FollowPath(path));
    }

    /// <summary>
    /// 적이 나타나는 애니메이션 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApperanceAnim()
    {
        Vector3 originScale = mModel.transform.localScale;
        mModel.transform.localScale = Vector3.zero;
        float x = 0;
        float y = 0;
        float z = 0;
        float speed = 1.0f;

        while (mModel.transform.localScale != originScale)
        {
            x = Mathf.MoveTowards(x, originScale.x, speed * Time.deltaTime);
            y = Mathf.MoveTowards(y, originScale.y, speed * Time.deltaTime);
            z = Mathf.MoveTowards(z, originScale.z, speed * Time.deltaTime);

            mModel.transform.localScale = new Vector3(x, y, z);
            yield return null;
        }

        mModel.transform.localScale = originScale;
    }

    /// <summary>
    /// 적이 path를 따라 가는 코루틴.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator FollowPath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            path[i] = new Vector3(path[i].x, transform.position.y, path[i].z);
        }

        Vector3 currentPos = path[0];
        Vector3 goalPos = path[path.Count - 1];
        Vector3 direction = Vector3.zero;

        transform.position = currentPos;

        float x = currentPos.x;
        float z = currentPos.z;
        int index = 0;
        Quaternion lookAt = Quaternion.identity;

        while (transform.position != goalPos)
        {
            if (transform.position == currentPos)
            {
                index++;
                if (index >= path.Count)
                {
                    yield break;
                }
                currentPos = path[index];

                direction = currentPos - path[index - 1];
                direction.Normalize();
                lookAt = Quaternion.Euler(GetAngle(direction));
            }

            transform.position = Vector3.MoveTowards(transform.position, currentPos, mSpeed * Time.deltaTime);
            transform.rotation = lookAt;

            yield return null;
        }

        yield return StartCoroutine(DestroyEnemyCoroutine());
        GameManager.Instance.GetPlayerInfo().Life--;
    }

    /// <summary>
    /// 적이 파괴되는 애니메이션 코루틴.
    /// </summary>
    /// <param name="_speed"></param>
    /// <returns></returns>
    private IEnumerator DestroyEnemyCoroutine(float _speed = 1.0f)
    {
        Vector3 originScale = mModel.transform.localScale;
        float x = mModel.transform.localScale.x;
        float y = mModel.transform.localScale.y;
        float z = mModel.transform.localScale.z;
        float speed = _speed;

        while (mModel.transform.localScale != Vector3.zero)
        {
            x = Mathf.MoveTowards(x, 0, speed * Time.deltaTime);
            y = Mathf.MoveTowards(y, 0, speed * Time.deltaTime);
            z = Mathf.MoveTowards(z, 0, speed * Time.deltaTime);

            mModel.transform.localScale = new Vector3(x, y, z);
            yield return null;
        }

        mModel.transform.localScale = Vector3.zero;

        mMap.RemoveEnemy(this);

        Destroy(this.gameObject);
    }
    #endregion

    #region Get
    /// <summary>
    /// 방향에 따라 각도를 받아오는 함수.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector3 GetAngle(Vector3 direction)
    {
        if (direction.x > 0.0f)
        {
            return new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if (direction.x < 0.0f)
        {
            return new Vector3(0.0f, 180.0f, 0.0f);
        }
        else if (direction.z > 0.0f)
        {
            return new Vector3(0.0f, 270.0f, 0.0f);
        }
        else if (direction.z < 0.0f)
        {
            return new Vector3(0.0f, 90.0f, 0.0f);
        }
        else
        {
            return Vector3.zero;
        }
    }
    #endregion

    #region IDamagable
    /// <summary>
    /// 데미지를 받을 때 실행되는 함수.
    /// </summary>
    /// <param name="_damage"></param>
    public void Damage(int _damage)
    {
        Health -= _damage;
        if (Health <= 0)
        {
            mMap.RemoveEnemy(this);
            GameObject effect = Instantiate(mEnemyEffect, transform.position, Quaternion.identity) as GameObject;
            if (!effect)
            {
                Debug.Log("Failed Instantiate enemy effect");
                return;
            }

            GameManager.Instance.GetPlayerInfo().Gold += mPrice;
            Destroy(effect, 2.0f);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
