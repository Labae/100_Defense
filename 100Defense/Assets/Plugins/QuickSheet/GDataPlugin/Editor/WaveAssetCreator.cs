using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Wave")]
    public static void CreateWaveAssetFile()
    {
        Wave asset = CustomAssetUtility.CreateAsset<Wave>();
        asset.SheetName = "MySpreadSheet";
        asset.WorksheetName = "Wave";
        EditorUtility.SetDirty(asset);        
    }
    
}