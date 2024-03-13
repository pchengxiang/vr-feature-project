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
    /// �`�N�A����ƥu�V�U�j�M�@�h
    /// </summary>
    /// <param name="path">�j�M���|</param>
    /// <param name="subname">�^�ǫ��w���ɦW�ɮ�(��:.json)</param>
    /// <returns>�^�Ǥ@�Ӧr��A���r���ۨ�ӭȡA�@�O�s���m(��Ƨ�)�A�G�O�ĤG�h�r��<�ɦW,�ɮפ��e></returns> 
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
