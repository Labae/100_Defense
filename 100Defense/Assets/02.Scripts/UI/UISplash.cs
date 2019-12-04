using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplash : MonoBehaviour
{
    public GameObject TextSet;
    public void SetTextColorWhite()
    {
        UILabel[] uILabels = TextSet.GetComponentsInChildren<UILabel>();
        for (int i = 0; i < uILabels.Length; i++)
        {
            uILabels[i].color = Color.white;
        }
    }
}
