using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonFileService
{
    public static async Task<T> LoadAsync<T>(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found in: {path}");
            return default;
        }

        string json = await File.ReadAllTextAsync(path);
        return JsonUtility.FromJson<T>(json);
    }

    public static async Task SaveAsync<T>(string filename, T data)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        string json = JsonUtility.ToJson(data, true);

        using var writer = new StreamWriter(path, false);
        await writer.WriteAsync(json);
    }

    public static bool FileExists(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        return File.Exists(path);
    }

    public static void Delete(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(path)) File.Delete(path);
    }
}