using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour, IObserver
{
    /// <summary>
    /// Life 이미지를 자식으로 하는 Grid
    /// </summary>
    [SerializeField] private UIGrid LifeGrid = null;
    /// <summary>
    /// Life GameObject.
    /// </summary>
    private GameObject mLife;

    #region Unity Event
    private void Start()
    {
        LifeGrid.repositionNow = true;
        mLife = Resources.Load("01.Prefabs/UI/Life") as GameObject;
        if (mLife == null)
        {
            Debug.Log("Life obj is null");
            return;
        }

        int life = GameManager.Instance.GetPlayerInfo().Life;
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(mLife, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.name = "Life" + i.ToString();

            if(i > life - 1)
            {
                obj.SetActive(false);
            }
        }
    }
    #endregion

    #region IObserver
    public void OnNotify(IObservable ob)
    {
        PlayerInformation info = ob as PlayerInformation;

        if (info != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < info.Life; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            GetComponent<UIGrid>().Reposition();
        }
    }
    #endregion
}
