using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
using System.ComponentModel;
using System.Text.RegularExpressions;

public static class FileLoader
{


    public static string LoadFile(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        else
        {
            throw new FileNotFoundException();
        }
    }

    public static Dictionary<string, string> LoadModule(string path, string[] fileList)
    {
        var result = new Dictionary<string, string>();
        foreach (var file in fileList)
        {
            var filePath = path + "/" + file;
            result.Add(file, LoadFile(filePath));
        }
        return result;
    }
    /// <summary>
    /// 注意，此函數只向下搜尋一層
    /// </summary>
    /// <param name="path">搜尋路徑</param>
    /// <param name="subname">回傳指定副檔名檔案(例:.json)</param>
    /// <returns>回傳一個字典，此字典放著兩個值，一是存放位置(資料夾)，二是第二層字典<檔名,檔案內容></returns> 
    public static Dictionary<string,Dictionary<string,string>> LoadModules(string path,string subname)
    {
        var searchSubNameMatch = new Regex("\\" + subname + "$");
        var result = new Dictionary<string, Dictionary<string,string>>();
        foreach (var dir in Directory.GetDirectories(path))
        {
            var inner = new Dictionary<string, string>();
            
            foreach (var file in Directory.GetFiles(dir))
            {
                if (searchSubNameMatch.IsMatch(file))
                {
                    var fileNamePosition = file.LastIndexOf("\\") + 1;
                    inner.Add(file.Substring(fileNamePosition,file.Length - fileNamePosition - subname.Length), LoadFile(file));
                }
            }
            result.Add(dir, inner);
        }
        return result;
    }
}
