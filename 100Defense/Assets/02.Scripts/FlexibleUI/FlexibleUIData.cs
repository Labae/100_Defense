using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fixible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    public Object UIAtlas;
    public Material UIMaterial;

    public Color DefaultButtonColor;
    public Sprite ButtonSprite;
    public Color ExitButtonColor;
    public Sprite IconExitSprite;
    public Color CheckButtonColor;
    public Sprite IconCheckSprite;
}
