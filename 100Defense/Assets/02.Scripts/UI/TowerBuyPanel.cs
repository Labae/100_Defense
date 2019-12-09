using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TowerBuyPanel : MonoBehaviour
{
    [SerializeField] private Transform mTowerObjectParent = null;
    [SerializeField] private UILabel mTowerTitle = null;
    [SerializeField] private UILabel mTowerDescription = null;
    [SerializeField] private UIButton mBuyButton = null;

    private TowerData mTowerData;
    private UIManager mUIManager;

    public void SetData(TowerData towerData, GameObject model, UIManager uiManager)
    {
        mTowerData = towerData;
        mUIManager = uiManager;

        GameObject uiSetTower = Instantiate(model, mTowerObjectParent);
        UITowerRotation towrRotation = uiSetTower.GetComponent<UITowerRotation>();
        towrRotation.RotateTower();

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
            // TODO : 가격 부족 알려주는 Image.
            return;
        }

        if(GameManager.Instance.GetMap().GetSelectedCell() == null)
        {
            // TODO : Cell 선택 알려주는 Image.
            return;
        }

        GameManager.Instance.GetMap().GetSelectedCell().BuildTower(mTowerData.Towerkey);
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
