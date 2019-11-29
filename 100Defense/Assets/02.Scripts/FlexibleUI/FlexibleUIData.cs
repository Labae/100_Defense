using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fixible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    public Object UIAtlas;
    public Material UIMaterial;

    public Sprite ButtonSprite;
    public Sprite IconExitSprite;
    public Sprite IconCheckSprite;
    public Sprite IconSettingSprite;

    public Sprite DefaultBackgroundSprite;
}
