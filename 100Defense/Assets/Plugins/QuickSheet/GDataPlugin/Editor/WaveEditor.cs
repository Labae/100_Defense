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
[CustomEditor(typeof(Wave))]
public class WaveEditor : BaseGoogleEditor<Wave>
{	    
    public override bool Load()
    {        
        Wave targetData = target as Wave;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<WaveData>(targetData.WorksheetName) ?? db.CreateTable<WaveData>(targetData.WorksheetName);
        
        List<WaveData> myDataList = new List<WaveData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            WaveData data = new WaveData();
            
            data = Cloner.DeepCopy<WaveData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
