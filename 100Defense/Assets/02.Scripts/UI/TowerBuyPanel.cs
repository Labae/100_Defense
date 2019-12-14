using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TowerBuyPanel : MonoBehaviour
{
    /// <summary>
    /// 타워 모델이 나올 부모
    /// </summary>
    [SerializeField] private Transform mTowerObjectParent = null;
    /// <summary>
    /// 타워 제목
    /// </summary>
    [SerializeField] private UILabel mTowerTitle = null;
    /// <summary>
    /// 타워에 대한 설명
    /// </summary>
    [SerializeField] private UILabel mTowerDescription = null;
    /// <summary>
    /// 구매 버튼
    /// </summary>
    [SerializeField] private UIButton mBuyButton = null;

    /// <summary>
    /// 타워 정보
    /// </summary>
    private TowerData mTowerData;
    private UIManager mUIManager;

    /// <summary>
    /// Tower Object UI에 대한 오브젝트 풀.
    /// </summary>
    private readonly Dictionary<string, GameObject> mUISetTowerObjectDictionary = new Dictionary<string, GameObject>();

    #region Method
    public void SetData(TowerData towerData, GameObject model, UIManager uiManager)
    {
        mTowerData = towerData;
        mUIManager = uiManager;

        if (!mUISetTowerObjectDictionary.ContainsKey(mTowerData.Towerkey))
        {
            GameObject uiSetTower = Instantiate(model, mTowerObjectParent);
            UITowerRotation towrRotation = uiSetTower.GetComponent<UITowerRotation>();
            towrRotation.RotateTower();

            mUISetTowerObjectDictionary.Add(mTowerData.Towerkey, uiSetTower);
        }
        else
        {
            GameObject towerObj = mUISetTowerObjectDictionary[mTowerData.Towerkey];
            towerObj.SetActive(true);
            towerObj.GetComponent<UITowerRotation>().RotateTower();
        }

        mTowerTitle.text = mTowerData.Modelname;

        StringBuilder sb = new StringBuilder();
        sb.Append("Type : ");
        sb.AppendLine(GetStringTowerType(mTowerData.TOWERTYPE));
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
    #endregion

    #region Button Event

    public void BuyButton()
    {
        if (GameManager.Instance.GetPlayerInfo().Gold < mTowerData.Price)
        {
            // TODO : 가격 부족 알려주는 Image.
            return;
        }

        if(GameManager.Instance.GetMapManager().GetSelectedCell() == null)
        {
            // TODO : Cell 선택 알려주는 Image.
            return;
        }

        GameManager.Instance.GetMapManager().GetSelectedCell().BuildTower(mTowerData.Towerkey);
        GameManager.Instance.GetMapManager().SetSelectedCell(null);
        mUIManager.CloseTowerBuyPanel();
        ExitButton();
    }

    public void ExitButton()
    {
        if(mUISetTowerObjectDictionary.ContainsKey(mTowerData.Towerkey))
        {
            GameObject towerObj = mUISetTowerObjectDictionary[mTowerData.Towerkey];
            towerObj.GetComponent<UITowerRotation>().StopRotateTower();
            towerObj.SetActive(false);
        }
        mTowerTitle.text = string.Empty;
        mTowerDescription.text = string.Empty;
        mBuyButton.onClick.Clear();
        mUIManager.CloseTowerBuyPanel();
    }
    #endregion

    #region Get
    private string GetStringTowerType(TowerType towerType)
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
    #endregion
}
