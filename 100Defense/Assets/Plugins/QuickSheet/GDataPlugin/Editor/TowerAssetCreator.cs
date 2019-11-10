using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Tower")]
    public static void CreateTowerAssetFile()
    {
        Tower asset = CustomAssetUtility.CreateAsset<Tower>();
        asset.SheetName = "MySpreadSheet";
        asset.WorksheetName = "Tower";
        EditorUtility.SetDirty(asset);        
    }
    
}