using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TowerBuyPanel : MonoBehaviour
{
    [SerializeField] private Transform mTowerObjectParent;
    [SerializeField] private UILabel mTowerTitle;
    [SerializeField] private UILabel mTowerDescription;
    [SerializeField] private UIButton mBuyButton;

    private TowerData mTowerData;
    private GameObject mModel;

    public void SetData(TowerData towerData)
    {
        mTowerData = towerData;
        mModel = Resources.Load("01.Prefabs/UI/3D_Model/" + mTowerData.Modelname) as GameObject;
        if (mModel == null)
        {
            return;
        }
        GameObject uiSetTower = Instantiate(mModel, mTowerObjectParent);
        uiSetTower.transform.GetChild(0).localScale = uiSetTower.transform.GetChild(0).localScale * 250.0f;
        uiSetTower.GetComponent<UITowerRotation>().RotateTower();

        mTowerTitle.text = mTowerData.Modelname;

        StringBuilder sb = new StringBuilder();
        sb.Append("Type : ");
        sb.AppendLine(mTowerData.Type);
        sb.Append("Range : ");
        sb.AppendLine(mTowerData.Range.ToString());
        sb.Append("Damage : ");
        sb.AppendLine(mTowerData.Damage.ToString());
        sb.Append("AttackSpeed : ");
        sb.AppendLine(mTowerData.Attackspeed.ToString());
        sb.Append("Price : ");
        sb.AppendLine(mTowerData.Price.ToString());
        mTowerDescription.text = sb.ToString();
        mBuyButton.onClick.Add(new EventDelegate(BuyButton));
    }

    private void BuyButton()
    {
        if(GameManager.Instance.GetPlayerInfo().Gold < mTowerData.Price)
        {
            return;
        }

        GameManager.Instance.GetPlayerInfo().Gold -= mTowerData.Price;
        if (GameManager.Instance.GetPlayerInfo().ContainTowerData.ContainsKey(mTowerData))
        {
            GameManager.Instance.GetPlayerInfo().ContainTowerData[mTowerData]++;
        }
    }

    public void ExitButton()
    {
        Destroy(mTowerObjectParent.GetChild(0).gameObject);
        mTowerTitle.text = string.Empty;
        mTowerDescription.text = string.Empty;
        mBuyButton.onClick.Clear();
    }
}
