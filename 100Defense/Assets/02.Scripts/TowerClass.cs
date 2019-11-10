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

    public bool Initialize(CellClass cell, string cellData)
    {
        transform.SetParent(cell.transform);
        transform.localPosition = Vector3.zero;

        mTowerData = Resources.Load("03.Datas/TowerData") as Tower;

        int towerIndex = -1;
        for(int i = 0; i < mTowerData.dataArray.Length;  i++)
        {
            if(cellData == mTowerData.dataArray[i].Key)
            {
                mTowerType = (TowerType)i;
                towerIndex = i;
                break;
            }
        }

        if(towerIndex == -1)
        {
            Debug.Log("Failed TowerIndex Initilaize.");
            return false;
        }

        GameObject model = CreateModel(mTowerData.dataArray[towerIndex].Modelname);

        return true;
    }

    private GameObject CreateModel(string modelName)
    {
        GameObject modelData = Resources.Load("01.Prefabs/Tower/" + modelName) as GameObject;
        GameObject model = Instantiate(modelData, transform.position, Quaternion.identity);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = Vector3.zero;

        return model;
    }
}
