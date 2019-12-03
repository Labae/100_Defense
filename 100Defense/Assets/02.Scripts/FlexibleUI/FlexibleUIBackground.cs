using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(BoxCollider2D))]
public class FlexibleUIBackground : FlexibleUI
{
    private UISprite mSprite;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        mSprite = GetComponent<UISprite>();
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();

        mSprite.atlas = skinData.UIAtlas as INGUIAtlas;
        if (mSprite.atlas == null)
        {
            Debug.Log("Button Sprite Atlas is null");
            return;
        }

        collider2D.isTrigger = true;

        mSprite.type = UIBasicSprite.Type.Sliced;
        mSprite.SetSprite(skinData.DefaultBackgroundSprite);
        mSprite.autoResizeBoxCollider = true;
    }
}
