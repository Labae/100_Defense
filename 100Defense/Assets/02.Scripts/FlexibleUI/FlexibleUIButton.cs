using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(BoxCollider2D))]
public class FlexibleUIButton : FlexibleUI
{
    public enum ButtonType
    {
        Default,
        Exit,
        Check,
        Setting
    };

    private UISprite mSprite;
    private UIButton mButton;
    private BoxCollider2D mBoxCollider;
    private UISprite iconSprite;

    public ButtonType buttonType;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        mSprite = GetComponent<UISprite>();
        mButton = GetComponent<UIButton>();
        mBoxCollider = GetComponent<BoxCollider2D>();

        mSprite.atlas = skinData.UIAtlas as INGUIAtlas;
        if (mSprite.atlas == null)
        {
            Debug.Log("Button Sprite Atlas is null");
            return;
        }

        mBoxCollider.isTrigger = true;
        mSprite.autoResizeBoxCollider = true;
        mBoxCollider.size = new Vector2(mSprite.width, mSprite.height);

        mSprite.type = UIBasicSprite.Type.Sliced;

        iconSprite = transform.Find("Icon").GetComponent<UISprite>();
        if (iconSprite == null)
        {
            Debug.Log("NULL iconSprite");
            return;
        }

        if (buttonType == ButtonType.Default)
        {
            iconSprite.atlas = null;
        }
        else
        {
            iconSprite.atlas = skinData.UIAtlas as INGUIAtlas;
            switch (buttonType)
            {
                case ButtonType.Check:
                    iconSprite.SetSprite(skinData.IconCheckSprite);
                    break;
                case ButtonType.Exit:
                    iconSprite.SetSprite(skinData.IconExitSprite);
                    break;
                case ButtonType.Setting:
                    iconSprite.SetSprite(skinData.IconSettingSprite);
                    break;
            }
        }

        mButton.tweenTarget = null;

        iconSprite.gameObject.SetActive(false);
        iconSprite.gameObject.SetActive(true);
    }
}
