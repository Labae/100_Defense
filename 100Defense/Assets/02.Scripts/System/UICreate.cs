using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreate : MonoBehaviour
{
    public static GameObject CreateButton(
        Vector2 pos,
        Vector2 size,
        int depth,
        EventDelegate eventDelegate,
        Transform parent,
        FlexibleUIButton.ButtonType type = FlexibleUIButton.ButtonType.Default,
        UIWidget.Pivot pivot = UIWidget.Pivot.Center,
        string name = "button")
    {
        GameObject obj = Instantiate(Resources.Load("01.Prefabs/UI/Button") as GameObject);
        if (obj == null)
        {
            Debug.Log("Failed Load Button Prefab");
            return null;
        }

        obj.name = name;
        UISprite sprite = obj.GetComponent<UISprite>();
        UIButton btn = obj.GetComponent<UIButton>();
        FlexibleUIButton flexibleBtn = obj.GetComponent<FlexibleUIButton>();
        UISprite icon = obj.transform.GetChild(0).GetComponent<UISprite>();

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        sprite.width = (int)size.x;
        sprite.height = (int)size.y;
        sprite.depth = depth;
        sprite.pivot = pivot;
        obj.transform.localPosition = pos;

        sprite.GetComponent<BoxCollider2D>().offset = GetBoxColliderOffset(pivot, sprite.localSize);

        icon.width = sprite.width;
        icon.height = sprite.height;
        icon.depth = depth;
        icon.pivot = pivot;
        icon.transform.localPosition = Vector3.zero;

        if (eventDelegate != null)
        {
            btn.onClick.Add(eventDelegate);
        }

       // btn.disabledColor 

        flexibleBtn.buttonType = type;

        return obj;
    }

    private static Vector2 GetBoxColliderOffset(UIWidget.Pivot pivot, Vector2 size)
    {
        Vector2 offset;
        switch (pivot)
        {
            case UIWidget.Pivot.TopLeft:
                offset = new Vector2(size.x, -size.y);
                break;
            case UIWidget.Pivot.Top:
                offset = new Vector2(0, -size.y);
                break;
            case UIWidget.Pivot.TopRight:
                offset = new Vector2(-size.x, -size.y);
                break;
            case UIWidget.Pivot.Left:
                offset = new Vector2(size.x, 0);
                break;
            case UIWidget.Pivot.Center:
                offset = new Vector2(0, 0);
                break;
            case UIWidget.Pivot.Right:
                offset = new Vector2(-size.x, 0);
                break;
            case UIWidget.Pivot.BottomLeft:
                offset = new Vector2(size.x, size.y);
                break;
            case UIWidget.Pivot.Bottom:
                offset = new Vector2(0, size.y);
                break;
            case UIWidget.Pivot.BottomRight:
                offset = new Vector2(-size.x, size.y);
                break;
            default:
                offset = Vector2.zero;
                break;
        }

        return offset *= 0.5f;
    }
}
