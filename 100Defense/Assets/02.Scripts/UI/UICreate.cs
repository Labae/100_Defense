using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreate : MonoBehaviour
{
    private static GameObject mButton;
    private static GameObject mBackground;
    private static GameObject mPanel;
    private static GameObject mLabel;
    public static bool Initialize()
    {
        mButton = Resources.Load("01.Prefabs/UI/Button") as GameObject;
        if (mButton == null)
        {
            Debug.Log("Failed Load Button Prefab");
            return false;
        }

        mBackground = Resources.Load("01.Prefabs/UI/Background") as GameObject;
        if (mBackground == null)
        {
            Debug.Log("Failed Load Background Prefab");
            return false;
        }

        mPanel = Resources.Load("01.Prefabs/UI/Panel") as GameObject;
        if (mPanel == null)
        {
            Debug.Log("Failed Load Panel Prefab");
            return false;
        }

        mLabel = Resources.Load("01.Prefabs/UI/Label") as GameObject;
        if (mLabel == null)
        {
            Debug.Log("Failed Load Label Prefab");
            return false;
        }

        return true;
    }

    public static GameObject CreateButton(
       Vector2 pos,
       Vector2 size,
       int depth,
       EventDelegate eventDelegate,
       Transform parent)
    {
        return CreateButton(pos, size, depth, eventDelegate, parent, FlexibleUIButton.ButtonType.Default, UIWidget.Pivot.Center, "Button");
    }

    public static GameObject CreateButton(
        Vector2 pos,
        Vector2 size,
        int depth,
        EventDelegate eventDelegate,
        Transform parent,
        FlexibleUIButton.ButtonType type,
        UIWidget.Pivot pivot,
        string name)
    {

        GameObject obj = NGUITools.AddChild(parent, mButton);
        obj.name = name;
        UISprite sprite = obj.GetComponent<UISprite>();
        UIButton btn = obj.GetComponent<UIButton>();
        FlexibleUIButton flexibleBtn = obj.GetComponent<FlexibleUIButton>();
        UISprite icon = obj.transform.GetChild(0).GetComponent<UISprite>();

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

    public static GameObject CreateBackground(
        Vector2 pos, Vector2 size, int depth, Transform parent)
    {
        return CreateBackground(pos, size, depth, parent, UIWidget.Pivot.Center, "Background");
    }

    public static GameObject CreateBackground(
        Vector2 pos, Vector2 size, int depth, Transform parent, UIWidget.Pivot pivot, string name)
    {
        GameObject obj = NGUITools.AddChild(parent, mBackground);
        obj.name = name;
        UISprite sprite = obj.GetComponent<UISprite>();
        sprite.width = (int)size.x;
        sprite.height = (int)size.y;
        sprite.depth = depth;
        sprite.pivot = pivot;
        obj.transform.localPosition = pos;

        return obj;
    }

    public static GameObject CreatePanel(Transform parnet, string name)
    {
        GameObject obj = NGUITools.AddChild(parnet, mPanel);
        obj.name = name;

        return obj;
    }

    public static GameObject CreateLabel(Vector2 pos, Vector2 maxSize, int depth, Transform parent, string text)
    {
        return CreateLabel(pos, maxSize, depth, parent, text, UIWidget.Pivot.Center, "Label");
    }

    public static GameObject CreateLabel(Vector2 pos, Vector2 maxSize, int depth, Transform parent, string text, UIWidget.Pivot pivot, string name)
    {
        GameObject obj = NGUITools.AddChild(parent, mLabel);
        obj.name = name;
        UILabel label = obj.GetComponent<UILabel>();
        label.overflowWidth = (int)maxSize.x;
        label.overflowHeight = (int)maxSize.y;
        label.depth = depth;
        label.pivot = pivot;
        label.text = text;

        UIWidget.Pivot parentPivot = UIWidget.Pivot.Center;
        if (parent.GetComponent<UIWidget>() != null)
        {
            parentPivot = parent.GetComponent<UIWidget>().pivot;
            obj.transform.localPosition = pos + GetBoxColliderOffset(parentPivot, maxSize);
        }
        else
        {
            obj.transform.localPosition = pos;
        }

        return obj;
    }
}
