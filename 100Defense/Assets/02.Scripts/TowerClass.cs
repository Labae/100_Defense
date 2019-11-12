using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    ID_TOWER01,
    ID_TOWER02,
};

public class TowerClass : MonoBehaviour
{
    private TowerType mTowerType;
    private Tower mTowerData;
    private Vector3 mOriginScale;
    private GameObject mModel;


    public bool Initialize(CellClass cell, string cellData)
    {
        transform.SetParent(cell.transform);
        transform.localPosition = Vector3.zero;

        mTowerData = Resources.Load("03.Datas/TowerData") as Tower;

        int towerIndex = -1;
        for (int i = 0; i < mTowerData.dataArray.Length; i++)
        {
            if (cellData == mTowerData.dataArray[i].Key)
            {
                mTowerType = (TowerType)i;
                towerIndex = i;
                break;
            }
        }

        if (towerIndex == -1)
        {
            Debug.Log("Failed TowerIndex Initilaize.");
            return false;
        }

        mModel = CreateModel(mTowerData.dataArray[towerIndex].Modelname);

        return true;
    }

    public bool Build(CellClass cell, TowerType type)
    {
        transform.SetParent(cell.transform);
        transform.localPosition = Vector3.zero;

        mTowerData = Resources.Load("03.Datas/TowerData") as Tower;

        int towerIndex = -1;
        mTowerType = type;
        towerIndex = (int)type;

        if (towerIndex == -1)
        {
            Debug.Log("Failed TowerIndex Initilaize.");
            return false;
        }

        mModel = CreateModel(mTowerData.dataArray[towerIndex].Modelname);
        StartCoroutine(ApperanceAnim());

        return true;
    }

    public void Destory()
    {
        StartCoroutine(DestoryCoroutine());
    }
    private GameObject CreateModel(string modelName)
    {
        GameObject modelData = Resources.Load("01.Prefabs/Tower/" + modelName) as GameObject;
        GameObject model = Instantiate(modelData, transform.position, Quaternion.identity);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = Vector3.zero;
        mOriginScale = model.transform.localScale;
        model.transform.localScale = Vector3.zero;

        return model;
    }

    public Tower GetTowerData()
    {
        return mTowerData;
    }

    public IEnumerator ApperanceAnim()
    {
        mModel.transform.localScale = Vector3.zero;
        float x = 0;
        float y = 0;
        float z = 0;
        float speed = 6.0f;
        float deltaSpeed;
        while (mModel.transform.localScale != mOriginScale)
        {
            deltaSpeed = speed * Time.deltaTime;
            x = Mathf.MoveTowards(x, mOriginScale.x, deltaSpeed);
            y = Mathf.MoveTowards(y, mOriginScale.y, deltaSpeed);
            z = Mathf.MoveTowards(z, mOriginScale.z, deltaSpeed);
            mModel.transform.localScale = new Vector3(x, y, z);

            yield return null;
        }

        mModel.transform.localScale = mOriginScale;
    }

    public IEnumerator DestoryCoroutine()
    {
        float x = mModel.transform.localScale.x;
        float y = mModel.transform.localScale.y;
        float z = mModel.transform.localScale.z;
        float speed = 6.0f;
        float deltaSpeed;
        while (mModel.transform.localScale != Vector3.zero)
        {
            deltaSpeed = speed * Time.deltaTime;
            x = Mathf.MoveTowards(x, 0, deltaSpeed);
            y = Mathf.MoveTowards(y, 0, deltaSpeed);
            z = Mathf.MoveTowards(z, 0, deltaSpeed);
            mModel.transform.localScale = new Vector3(x, y, z);

            yield return null;
        }

        Destroy(this.gameObject);

    }
}
