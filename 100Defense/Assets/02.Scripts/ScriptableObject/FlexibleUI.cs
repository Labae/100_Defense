using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleUI : MonoBehaviour
{
    protected FlexibleUIData skinData;

    public virtual void Awake()
    {
        skinData = Resources.Load("03.Datas/UI/DefaultSkin") as FlexibleUIData;
        if(skinData == null)
        {
            Debug.Log("Failed Load DefaultSkin");
            return;
        }
        OnSkinUI();
    }

    public virtual void Update()
    {
        if(skinData == null)
        {
            return;
        }

        if(Application.isEditor)
        {
            OnSkinUI();
        }
    }

    protected virtual void OnSkinUI()
    {

    }
}
