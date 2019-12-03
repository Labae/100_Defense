using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITitle : MonoBehaviour
{
    public UIPanel TouchGuard;
    public GameObject CreditPanel;

    private void Start()
    {
    }

    public void OpenCredit()
    {
        CreditPanel.transform.DOLocalMoveX(0.0f, 0.2f).SetEase(Ease.Linear);
        TouchGuard.depth = CreditPanel.GetComponent<UIPanel>().depth;
    }

    public void CloseCredit()
    {
        CreditPanel.transform.DOLocalMoveX(1000.0f, 0.2f).SetEase(Ease.Linear);
        TouchGuard.depth = 0;
    }
}
