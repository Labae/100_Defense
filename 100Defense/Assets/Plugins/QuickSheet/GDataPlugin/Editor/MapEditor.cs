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
[CustomEditor(typeof(Map))]
public class MapEditor : BaseGoogleEditor<Map>
{	    
    public override bool Load()
    {        
        Map targetData = target as Map;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<MapData>(targetData.WorksheetName) ?? db.CreateTable<MapData>(targetData.WorksheetName);
        
        List<MapData> myDataList = new List<MapData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            MapData data = new MapData();
            
            data = Cloner.DeepCopy<MapData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
