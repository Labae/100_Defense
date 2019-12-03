using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour, IDamagable
{
    private float mSpeed;
    private GameObject mModel;
    private MapManager mMap;
    private GameObject mEnemyEffect;

    public int Health { get; set; }

    public bool Initialize(MapManager map, string enemyKey, List<Vector3> path)
    {
        mMap = map;
        Enemy enemyData = Resources.Load("03.Datas/Game/EnemyData") as Enemy;
        if (!enemyData)
        {
            Debug.Log("Enemy data not load");
            return false;
        }

        int enemyIndex = -1;
        for (int i = 0; i < enemyData.dataArray.Length; i++)
        {
            if(enemyKey == enemyData.dataArray[i].Key)
            {
                enemyIndex = i;
                break;
            }
        }

        if(enemyIndex == -1)
        {
            Debug.Log("Failed find enemy index");
            return false;
        }

        EnemyData data = enemyData.dataArray[enemyIndex];

        transform.name = data.Name;
        mSpeed = data.Movespeed;
        Health = data.Health;

        mModel = CreateModel(data.Name, path[0]);
        if (!mModel)
        {
            Debug.Log("Enemy Model not load");
            return false;
        }

        mEnemyEffect = Resources.Load("01.Prefabs/Enemy/EnemyEffectImpact") as GameObject;
        if(!mEnemyEffect)
        {
            Debug.Log("EnemyEffectImpact not load");
            return false;
        }

        MeshRenderer meshRenderer = mModel.GetComponent<MeshRenderer>();
        Material mat = Resources.Load("02.Materials/02.Enemys/" + enemyData.dataArray[enemyIndex].Name) as Material;
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
        float cellHalfSizeY = mMap.GetCell(0,0).transform.localScale.y * 0.5f;
        model.transform.localPosition = new Vector3(0.0f, cellHalfSizeY + model.transform.localScale.y * 0.5f, 0.0f);

        return model;
    }

    private IEnumerator EnemyCoroutine(List<Vector3> path)
    {
        yield return StartCoroutine(ApperanceAnim());
        yield return StartCoroutine(FollowPath(path));
    }

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

        mMap.RemoveEnemy(this);

        yield return StartCoroutine(DestroyEnemy());
    }

    private Vector3 GetAngle(Vector3 direction)
    {
        if(direction.x > 0.0f)
        {
            return new Vector3(0.0f, 0.0f, 0.0f);
        }
        else if(direction.x < 0.0f)
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

    private IEnumerator DestroyEnemy()
    {
        Vector3 originScale = mModel.transform.localScale;
        float x = mModel.transform.localScale.x;
        float y = mModel.transform.localScale.y;
        float z = mModel.transform.localScale.z;
        float speed = 1.0f;

        while (mModel.transform.localScale != Vector3.zero)
        {
            x = Mathf.MoveTowards(x, 0, speed * Time.deltaTime);
            y = Mathf.MoveTowards(y, 0, speed * Time.deltaTime);
            z = Mathf.MoveTowards(z, 0, speed * Time.deltaTime);

            mModel.transform.localScale = new Vector3(x, y, z);
            yield return null;
        }

        mModel.transform.localScale = Vector3.zero;

        Destroy(this.gameObject);
    }

    public void Damage(int _damage)
    {
        Health -= _damage;
        if(Health <= 0)
        {
            mMap.RemoveEnemy(this);
            GameObject effect = Instantiate(mEnemyEffect, transform.position, Quaternion.identity) as GameObject;
            if(!effect)
            {
                Debug.Log("Failed Instantiate enemy effect");
                return;
            }

            Destroy(effect, 2.0f);
            Destroy(this.gameObject);
        }
    }
}
