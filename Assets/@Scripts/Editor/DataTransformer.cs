using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Data;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<CreatureDataLoader, CreatureData>("Creature");
        ParseExcelDataToJson<EnvDataLoader, EnvData>("Env");

        Debug.Log("DataTransformer Completed");
    }

    #region Helpers

    static void ParseExcelDataToJson<Loader, LoaderData>(string filename) where Loader : new() where LoaderData : new()
    {
        var loader = new Loader();
        var field = loader.GetType().GetFields()[0];
        field.SetValue(loader, ParseExcelDataToList<LoaderData>(filename));

        var jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static List<LoaderData> ParseExcelDataToList<LoaderData>(string filename) where LoaderData : new()
    {
        var loaderDatas = new List<LoaderData>();

        var lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/ExcelData/{filename}Data.csv")
            .Split("\n");

        for (var l = 1; l < lines.Length; l++)
        {
            var row = lines[l].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            var loaderData = new LoaderData();

            var fields = typeof(LoaderData).GetFields();
            for (var f = 0; f < fields.Length; f++)
            {
                var field = loaderData.GetType().GetField(fields[f].Name);
                var type = field.FieldType;

                if (type.IsGenericType)
                {
                    var value = ConvertList(row[f], type);
                    field.SetValue(loaderData, value);
                }
                else
                {
                    var value = ConvertValue(row[f], field.FieldType);
                    field.SetValue(loaderData, value);
                }
            }

            loaderDatas.Add(loaderData);
        }

        return loaderDatas;
    }

    static object ConvertValue(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        var converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
    }

    static object ConvertList(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        // Reflection
        var valueType = type.GetGenericArguments()[0];
        var genericListType = typeof(List<>).MakeGenericType(valueType);
        var genericList = Activator.CreateInstance(genericListType) as IList;

        // Parse Excel
        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }

    #endregion

#endif
}
