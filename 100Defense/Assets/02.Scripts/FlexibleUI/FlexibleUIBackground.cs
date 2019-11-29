using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
public class FlexibleUIBackground : FlexibleUI
{
    private UISprite mSprite;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        mSprite = GetComponent<UISprite>();

        mSprite.atlas = skinData.UIAtlas as INGUIAtlas;
        if (mSprite.atlas == null)
        {
            Debug.Log("Button Sprite Atlas is null");
            return;
        }

        mSprite.type = UIBasicSprite.Type.Sliced;
        mSprite.SetSprite(skinData.DefaultBackgroundSprite);
    }
}
