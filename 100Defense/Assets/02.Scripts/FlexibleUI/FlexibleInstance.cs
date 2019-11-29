﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlexibleInstance : Editor
{
    [MenuItem("GameObject/Flexible UI/Button", priority =0)]
    public static void AddButton()
    {
        Create("Button");
    }

    [MenuItem("GameObject/Flexible UI/Background", priority =1)]
    public static void AddBackground()
    {
        Create("Background");
    }

    private static GameObject clickedObject;

    private static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>("01.Prefabs/UI/" + objectName));
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeObject as GameObject;
        if(clickedObject != null)
        {
            instance.transform.SetParent(clickedObject.transform, false);
        }

        return instance;
    }
}
