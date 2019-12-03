using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVManager : MonoBehaviour
{
    public string[,] LoadMap()
    {
        StreamReader strReader = new StreamReader(getPath("/Resources/03.Datas/Game/MapData.csv"));
        bool endOfFile = false;

        string[,] retval = new string[10,10];

        int x = 0;
        int y = 0;
        bool first = true;

        while(!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if(data_string == null)
            {
                endOfFile = true;
                break;
            }

            if(first)
            {
                first = false;
                continue;
            }

            var data_value = data_string.Split(',');
            retval[x, y] = data_value[1].ToString();

            x++;
            if(x >= 10)
            {
                y++;
                x = 0;
            }

            if(y >= 10)
            {
                endOfFile = true;
                break;
            }
        }

        return retval;
    }

    public void MapSave(string[,] mapData)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Key";
        rowDataTemp[1] = "TowerName";
        rowData.Add(rowDataTemp);

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                rowDataTemp = new string[2];
                int keyValue = x * 10 + y % 10;
                string key = keyValue.ToString();
                rowDataTemp[0] = key;
                rowDataTemp[1] = mapData[y, x];
                rowData.Add(rowDataTemp);
            }
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath("/Resources/03.Datas/Game/MapData.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    public PlayerInformation LoadPlayerInfo()
    {
        StreamReader strReader = new StreamReader(getPath("/Resources/03.Datas/Game/PlayerInformation.csv"));
        bool endOfFile = false;
        bool first = true;

        PlayerInformation playerInfo = new PlayerInformation();

        int x = 0;
        int y = 0;
        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endOfFile = true;
                break;
            }

            if (first)
            {
                first = false;
                continue;
            }

            var data_value = data_string.Split(',');

            playerInfo.Gold = int.Parse(data_value[1].ToString());

            x++;
            if (x >= 1)
            {
                y++;
                x = 0;
            }

            if(y >= 1)
            {
                endOfFile = true;
            }
        }

        return playerInfo;
    }

    public void SavePlayerInfo(PlayerInformation info)
    {
        List<string[]> rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Key";
        rowDataTemp[1] = "Value";
        rowData.Add(rowDataTemp);

        //for (int x = 0; x < 1; x++)
        //{
        //    for (int y = 0; y < 1; y++)
        //    {
        //        rowDataTemp = new string[2];
        //        int keyValue = x * 10 + y % 10;
        //        string key = keyValue.ToString();
        //        rowDataTemp[0] = key;
        //        rowDataTemp[1] = mapData[y, x];
        //        rowData.Add(rowDataTemp);
        //    }
        //}

        rowDataTemp = new string[2];
        string key = "Gold";
        rowDataTemp[0] = key;
        rowDataTemp[1] = info.Gold.ToString();
        rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath("/Resources/03.Datas/Game/PlayerInformation.csv");

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    private string getPath(string path)
    {
#if UNITY_EDITOR
        return Application.dataPath + path;
#elif UNITY_ANDROID
        return Application.persistentDataPath+path;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+path;
#else
        return Application.dataPath +"/"+"path;
#endif
    }
}
