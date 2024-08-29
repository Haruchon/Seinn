using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public static class SaveSystem{

    public static string configDataPath = Application.persistentDataPath + "/configData.seinn";

    public static void SaveConfigData(MisionData data) 
    {
        string path = configDataPath;
        FileStream stream = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static MisionData LoadConfigData()
    {
        string path = configDataPath;
        MisionData data = new MisionData();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as MisionData;
            stream.Close();
        }
        return data;
    }

}
