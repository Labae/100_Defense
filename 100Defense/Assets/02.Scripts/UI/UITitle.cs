using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITitle : MonoBehaviour
{
    /// <summary>
    /// 뒷 터치 방지 패널.
    /// </summary>
    [SerializeField] private UIPanel mTouchGuard;
    /// <summary>
    /// 게임 시작 버튼.
    /// </summary>
    [SerializeField] private UIButton mStartButton;
    /// <summary>
    /// 제작자 패널.
    /// </summary>
    [SerializeField] private GameObject mCreditPanel;

    #region Unity Event
    private void Start()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Title);
        mStartButton.onClick.Add(new EventDelegate(() => SceneMove.Instance.MoveGameScene()));
    }
    #endregion

    #region Button Event
    /// <summary>
    /// 제작자 패널 여는 버튼.
    /// </summary>
    public void OpenCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(0.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = mCreditPanel.GetComponent<UIPanel>().depth;
    }

    /// <summary>
    /// 제작자 패널을 닫는 버튼.
    /// </summary>
    public void CloseCredit()
    {
        mCreditPanel.transform.DOLocalMoveX(1000.0f, 0.2f).SetEase(Ease.Linear);
        mTouchGuard.depth = 0;
    }
    #endregion
}
