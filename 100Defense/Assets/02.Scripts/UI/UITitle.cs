using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITitle : MonoBehaviour
{
    [SerializeField] private UIPanel mTouchGuard;
    [SerializeField] private UIButton mStartButton;
    [SerializeField] private GameObject mCreditPanel;

    private void Start()
    {
        mStartButton.onClick.Add(new EventDelegate(() => SceneMove.Instance.MoveGameScene()));
    }

    public void OpenCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(0.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = mCreditPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(1000.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = 0;
    }
}
