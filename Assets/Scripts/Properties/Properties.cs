using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that mimics the behavior of [Java properties](https://docs.oracle.com/en/java/javase/17/docs/api//java.base/java/util/Properties.html).
/// </summary>
public class Properties
{

    private static Dictionary<String, String> list;
    private static String filename;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        new Properties(".\\Assets\\Scripts\\Properties\\Moorhuhn.properties");
    }

    public Properties(String file)
    {
        reload(file);
    }

    public static String get(String field, String defValue)
    {
        return (get(field) == null) ? (defValue) : (get(field));
    }
    public static String get(String field)
    {
        return (list.ContainsKey(field)) ? (list[field]) : (null);
    }

    public static void set(String field, System.Object value)
    {
        if (!list.ContainsKey(field))
            list.Add(field, value.ToString());
        else
            list[field] = value.ToString();
    }

    public void Save()
    {
        Save(filename);
    }

    public void Save(String filename)
    {
        Properties.filename = filename;

        if (!System.IO.File.Exists(filename))
            System.IO.File.Create(filename);

        System.IO.StreamWriter file = new System.IO.StreamWriter(filename);

        foreach (String prop in list.Keys.ToArray())
            if (!String.IsNullOrWhiteSpace(list[prop]))
                file.WriteLine(prop + "=" + list[prop]);

        file.Close();
    }

    public static void reload()
    {
        reload(filename);
    }

    public static void reload(String filename)
    {
        Properties.filename = filename;
        list = new Dictionary<String, String>();

        if (System.IO.File.Exists(filename))
            loadFromFile(filename);
        else
            System.IO.File.Create(filename);
    }

    private static void loadFromFile(String file)
    {
        foreach (String line in System.IO.File.ReadAllLines(file))
        {
            if ((!String.IsNullOrEmpty(line)) &&
                (!line.StartsWith(";")) &&
                (!line.StartsWith("#")) &&
                (!line.StartsWith("'")) &&
                (line.Contains('=')))
            {
                int index = line.IndexOf('=');
                String key = line.Substring(0, index).Trim();
                String value = line.Substring(index + 1).Trim();

                if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                    (value.StartsWith("'") && value.EndsWith("'")))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                try
                {
                    //ignore dublicates
                    list.Add(key, value);
                }
                catch { }
            }
        }
    }


}
