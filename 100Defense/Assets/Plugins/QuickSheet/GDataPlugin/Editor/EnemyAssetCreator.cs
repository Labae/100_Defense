using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Enemy")]
    public static void CreateEnemyAssetFile()
    {
        Enemy asset = CustomAssetUtility.CreateAsset<Enemy>();
        asset.SheetName = "MySpreadSheet";
        asset.WorksheetName = "Enemy";
        EditorUtility.SetDirty(asset);        
    }
    
}