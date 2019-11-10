using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Map")]
    public static void CreateMapAssetFile()
    {
        Map asset = CustomAssetUtility.CreateAsset<Map>();
        asset.SheetName = "MySpreadSheet";
        asset.WorksheetName = "Map";
        EditorUtility.SetDirty(asset);        
    }
    
}