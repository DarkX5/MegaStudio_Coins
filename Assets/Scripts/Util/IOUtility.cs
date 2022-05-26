using System;
using System.IO;
using UnityEngine;

public static class IOUtility
{
    private static string saveDirName = "_Saves";
    public static bool SaveToDisk(string saveName, string savedValue)
    {
        try
        {
            // cache file path
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, saveDirName, saveName); 

            CheckDirExistence(System.IO.Path.Combine(Application.persistentDataPath, saveDirName));

            // save data (WriteAllText also creates the file)
            File.WriteAllText(filePath, savedValue);

            // // save data - buggy
            // using (File.CreateText(filePath))
            // {
            //     using (TextWriter writer = new StreamWriter(filePath, false))
            //     {

            //         writer.WriteLine(savedValue);
            //         writer.Close();
            //     }
            // }
        }
        catch (Exception ex)
        {
            // for debugging
            var aux = ex.Message;
            return false;
        }

        return true;
    }

    public static string LoadFromDisk(string saveName)
    {
        string result = string.Empty;
        try
        {
            // cache file path
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, saveDirName, saveName);

            CheckDirExistence(System.IO.Path.Combine(Application.persistentDataPath, saveDirName));

            // check file exists
            if (!File.Exists(filePath))
            {
                // create file if it does not exist
                File.Create(filePath);
            }

            // return file contents
            return File.ReadAllText(filePath);

            // load data - buggy
            // using (StreamReader reader = File.OpenText(filePath))
            // {
            //     result = reader.ReadLine();
            //     reader.Close();
            // }
            // return result; 
        }
        catch (Exception ex)
        {
            var aux = ex.Message;
            return string.Empty;
        }
    }

    private static void CheckDirExistence(string dirPath)
    {
        // check directory exists
        if (!Directory.Exists(dirPath))
        {
            // create dir if it does not exist
            Directory.CreateDirectory(dirPath);
        }
    }
}
