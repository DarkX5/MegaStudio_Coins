using System;
using System.IO;

public static class IOUtility
{
    private static string savePath = "/_Saves";
    public static bool SaveToDisk(string saveName, string savedValue)
    {
        string fullPath = $"{savePath}/{saveName}";
        try
        {
            if (!File.Exists(fullPath)) {
                File.Create($"{savePath}/{saveName}");
            }
            File.WriteAllText($"{savePath}/{saveName}", savedValue);
        } catch (Exception ex) {
            var aux = ex.Message;
            return false;
        }
        
        return true;
    }
    public static string LoadFromDisk(string saveName)
    {
        string fullPath = $"{savePath}/{saveName}";
        try
        {
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath);
            }
            return File.ReadAllText(fullPath);
        }
        catch (Exception ex)
        {
            var aux = ex.Message;
            return string.Empty;
        }
    }
}
