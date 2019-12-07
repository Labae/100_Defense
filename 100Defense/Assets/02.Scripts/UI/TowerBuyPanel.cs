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
    private UIManager mUIManager;

    public void SetData(TowerData towerData, UIManager uiManager)
    {
        mTowerData = towerData;
        mUIManager = uiManager;

        GameObject model = Resources.Load("01.Prefabs/UI/3D_Model/" + mTowerData.Modelname) as GameObject;
        if (model == null)
        {
            return;
        }
        GameObject uiSetTower = Instantiate(model, mTowerObjectParent);
        uiSetTower.transform.GetChild(0).localScale = uiSetTower.transform.GetChild(0).localScale * 250.0f;
        uiSetTower.GetComponent<UITowerRotation>().RotateTower();

        mTowerTitle.text = mTowerData.Modelname;

        StringBuilder sb = new StringBuilder();
        sb.Append("Type : ");
        sb.AppendLine(StringTowerType(mTowerData.TOWERTYPE));
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

    public void BuyButton()
    {
        if (GameManager.Instance.GetPlayerInfo().Gold < mTowerData.Price)
        {
            return;
        }

        if(GameManager.Instance.GetMap().GetSelectedCell() == null)
        {
            return;
        }


        GameManager.Instance.GetMap().GetSelectedCell().BuildTower(mTowerData.TOWERKEY);
        GameManager.Instance.GetMap().SetSelectedCell(null);
        mUIManager.CloseTowerBuyPanel();
        ExitButton();
    }

    public void ExitButton()
    {
        Destroy(mTowerObjectParent.GetChild(0).gameObject);
        mTowerTitle.text = string.Empty;
        mTowerDescription.text = string.Empty;
        mBuyButton.onClick.Clear();
    }

    private string StringTowerType(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.Attack:
                return "Attack";
            case TowerType.Buff:
                return "Buff";
            default:
                return "NULL";
        }
    }
}
