using System.Collections.Generic;
using QPhysics;
using LitJson;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelFactory
{
    private static List<LevelCreator> levels;
    private static List<string> id;

    private static void InitLevels() 
    {
        TextAsset[] bindata = Resources.LoadAll<TextAsset>("QFrameLevels/");

        levels = new List<LevelCreator>();
        id = new List<string>();
        foreach (TextAsset a in bindata)
        {
            LevelCreator lc = new LevelCreator(a.text);
            levels.Add(lc);
            id.Add(lc.id);
        }
    }

    public static int IndexOf(string levelID)
    {
        if(levels == null) InitLevels();
        for (int i = 0; i < levels.Count; i++)
        {
            if (id[i].Equals(levelID)) return i;
        }
        return -1;
    }

	public static Level createLevel(string levelID)
	{
        if (levels == null) InitLevels();
        return levels[IndexOf(levelID)].GetLevel();
	}

    public static List<LevelCreator> GetAllLevels()
    {
        if(levels == null) InitLevels();
        return levels;
    }

    public static string[] GetNames()
    {
        if(levels == null) InitLevels();
        string[] names = new string[levels.Count];
        for (int i = 0; i < levels.Count; i++ )
        {
            names[i] = levels[i].name;
        }
        return names;
    }

    public static string[] GetIDs()
    {
        if(levels == null) InitLevels();
        string[] names = new string[levels.Count];
        for (int i = 0; i < levels.Count; i++)
        {
            names[i] = levels[i].id;
        }
        return names;
    }
}


