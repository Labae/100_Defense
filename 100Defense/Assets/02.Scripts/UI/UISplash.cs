using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplash : MonoBehaviour
{
    [SerializeField] private GameObject TextSet;
    public void SetTextColorWhite()
    {
        SplashText[] splashTexts = TextSet.GetComponentsInChildren<SplashText>();
        for (int i = 0; i < splashTexts.Length; i++)
        {
            splashTexts[i].SetTextWhite();
        }
    }
}
