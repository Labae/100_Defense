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
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : BaseGoogleEditor<Enemy>
{	    
    public override bool Load()
    {        
        Enemy targetData = target as Enemy;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<EnemyData>(targetData.WorksheetName) ?? db.CreateTable<EnemyData>(targetData.WorksheetName);
        
        List<EnemyData> myDataList = new List<EnemyData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            EnemyData data = new EnemyData();
            
            data = Cloner.DeepCopy<EnemyData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
