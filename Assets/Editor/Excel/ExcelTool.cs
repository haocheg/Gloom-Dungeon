using Excel;
using LitJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

/*
*Title:
*Description:
*
*/

public class ExcelTool
{
    /// <summary>
    /// 变量名所在行数
    /// </summary>
    public static int VariableNameRow = 1;

    /// <summary>
    /// 变量类型所在行数
    /// </summary>
    public static int VariableTypeRow = 2;

    /// <summary>
    /// 主键所在行数
    /// </summary>
    public static int KeyRow = 3;

    /// <summary>
    /// excel文件存放路径
    /// </summary>
    public static string EXCEL_PATH = Application.dataPath + "/Res/Excel/";

    /// <summary>
    /// 数据结构类存储路径
    /// </summary>
    public static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";

    /// <summary>
    /// 数据容器类存储路径
    /// </summary>
    public static string CONTAINER_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";

    /// <summary>
    /// 数据内容起始行数
    /// </summary>
    public static int DataBeginColumn = 4;

    #region 编辑器工具菜单

    [MenuItem("ExcelToData/ExcelToJson")]
    private static void GenerateExcelJsonInfo()
    {
        GenerateExcelInfo();
    }

    private static void GenerateExcelInfo()
    {
        DirectoryInfo dinfo = Directory.CreateDirectory(EXCEL_PATH);
        FileInfo[] files = dinfo.GetFiles();

        DataTableCollection tablecollection;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;

            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelreader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tablecollection = excelreader.AsDataSet().Tables;
                fs.Close();
            }

            foreach (DataTable table in tablecollection)
            {
                GenerateJsonDataClass(table);
                GenerateJsonContainerclass(table);
                GenerateJsonData(table);
            }
        }
    }
    #endregion

    #region 生成表的数据结构类

    private static void GenerateJsonDataClass(DataTable table)
    {
        DataRow rowName = GetVariableNameRow(table);
        DataRow rowType = GetVariableTypeRow(table);

        if (!Directory.Exists(DATA_CLASS_PATH))
            Directory.CreateDirectory(DATA_CLASS_PATH);

        string s = "[System.Serializable]\npublic class " + table.TableName + "\n{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            s += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }
        s += "}";

        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", s);

        AssetDatabase.Refresh();
    }
    #endregion

    #region 生成表的数据容器类

    private static void GenerateJsonContainerclass(DataTable table)
    {
        int keyIndex = GetKeyColumn(table);
        DataRow rowType = GetVariableTypeRow(table);

        if (!Directory.Exists(CONTAINER_CLASS_PATH))
            Directory.CreateDirectory(CONTAINER_CLASS_PATH);

        string s = "using System.Collections.Generic;\n";
        s += "[System.Serializable]\npublic class " + table.TableName + "Container\n{\n";
        s += "    ";
        s += "public Dictionary<" + rowType[keyIndex].ToString() + ", " +
             table.TableName + "> dataDic = new Dictionary<" +
             rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";
        s += "}";

        File.WriteAllText(CONTAINER_CLASS_PATH + table.TableName + "Container.cs", s);
        AssetDatabase.Refresh();
    }
    #endregion

    /// <summary>
    /// 生成Json数据文件
    /// </summary>
    /// <param name="table"></param>
    private static void GenerateJsonData(DataTable table)
    {
        if (!Directory.Exists(JsonMgr.DATA_JSON_PATH))
            Directory.CreateDirectory(JsonMgr.DATA_JSON_PATH);

        string path = JsonMgr.DATA_JSON_PATH + table.TableName + ".json";
        DataRow row;
        DataRow rowName = GetVariableNameRow(table);
        DataRow rowType = GetVariableTypeRow(table);
        SheetData sheetData = new SheetData();
        sheetData.keyType = rowType[GetKeyColumn(table)].ToString();
        for (int i = DataBeginColumn; i < table.Rows.Count; i++)
        {
            Dictionary<string, object> rowDic = new Dictionary<string, object>();
            row = table.Rows[i];
            for (int j = 0; j < table.Columns.Count; j++)
            {
                rowDic[rowName[j].ToString()] = row[j];
            }
            sheetData.dataDic.Add(row[0].ToString(), rowDic);
        }
        File.WriteAllText(path, JsonMapper.ToJson(sheetData));

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 得到变量名所在行数据
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[VariableNameRow];
    }

    /// <summary>
    /// 得到变量类型所在行数据
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[VariableTypeRow];
    }

    /// <summary>
    /// 得到主键在哪列
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static int GetKeyColumn(DataTable table)
    {
        DataRow row = table.Rows[KeyRow];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
                return i;
        }
        return 0;
    }
}


