using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(Tower))]
public class TowerEditor : BaseGoogleEditor<Tower>
{	    
    public override bool Load()
    {        
        Tower targetData = target as Tower;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<TowerData>(targetData.WorksheetName) ?? db.CreateTable<TowerData>(targetData.WorksheetName);
        
        List<TowerData> myDataList = new List<TowerData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            TowerData data = new TowerData();
            
            data = Cloner.DeepCopy<TowerData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
