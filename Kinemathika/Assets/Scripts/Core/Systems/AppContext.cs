using System;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;

public class AppContext : MonoBehaviour
{
    private static AppContext _instance;
    public static AppContext Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("AppContext");
                _instance = go.AddComponent<AppContext>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    private Dictionary<Type, object> _services = new();
    public void RegisterService<T>(T system)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
            throw new Exception($"System of type {type.Name} already registered.");
        _services[type] = system;
    }
    public T GetService<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out object system))
            return (T)system;
        throw new Exception($"System of type {type.Name} is NOT registered.");
    }
}
