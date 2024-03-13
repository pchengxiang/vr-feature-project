using System.IO;
using UnityEditor;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject
    where T : ScriptableObject
{
    private static T s_Instance = null;
    public static string path = "Assets/Managers/";

    public static T instance
    {
        get
        {
            if (!s_Instance)
            {
                T[] objs = null;

#if UNITY_EDITOR
                // If we're running the game in the editor, the "Preloaded Assets" array will be ignored.
                // So get all the assets of type T using AssetDatabase.
                string[] objsGUID = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                int count = objsGUID.Length;
                objs = new T[count];
                for (int i = 0; i < count; i++)
                {
                    objs[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(objsGUID[i]));
                }
#else
                // Get all asset of type T from Resources or loaded assets.
                objs = Resources.FindObjectsOfTypeAll<T>();
#endif

                // If no asset of type T was found...
                if (objs.Length == 0)
                {
#if UNITY_EDITOR
                    ScriptableObject asset = CreateInstance<T>();
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string savePath = string.Format("{0}/{1}.asset", path, typeof(T).Name);
                    AssetDatabase.CreateAsset(asset, savePath);
                    objs = new T[1];
                    objs[0] =(T) asset;
#else
                    Debug.LogError("No asset of type \"" + typeof(T).Name + "\" has been found in loaded resources. Please create a new one and add it to the \"Preloaded Assets\" array in Edit > Project Settings > Player > Other Settings.");
#endif
                }

                // If more than one asset of type T was found...
                else if (objs.Length > 1)
                {
                    Debug.LogError("There's more than one asset of type \"" + typeof(T).Name + "\" loaded in this project. We expect it to have a Singleton behaviour. Please remove other assets of that type from this project.");
                }
                s_Instance = (objs.Length > 0) ? objs[0] : null;
            }

            return s_Instance;
        }
    }


}