using Assets.Scripts.BackEnd.Enems;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores objects (like Sprites or Prefabs) by a specific type key.
/// </summary>
/// <typeparam name="TType">The key type (e.g. enum).</typeparam>
/// <typeparam name="TObject">The object to store (e.g. Sprite, GameObject).</typeparam>
public class TypedObjectCache<TType, TObject> where TType : notnull
{
    /// <summary>Internal dictionary that maps keys to objects.</summary>
    private readonly Dictionary<TType, TObject> _dictionary = new();

    /// <summary>Stores or replaces an object with a key.</summary>
    public void SetObject(TType type, TObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"Trying to assign a null object to key: {type}");
            return;
        }

        _dictionary[type] = obj;
    }

    /// <summary>Returns the object for a given key, or null if it’s not set.</summary>
    public TObject GetObject(TType type)
    {
        if (!_dictionary.TryGetValue(type, out var obj) || obj == null)
        {
            Debug.LogWarning($"Object not found for key: {type}");
        }
        return obj;
    }

    /// <summary>Returns true if an object exists for the given key.</summary>
    public bool HasObject(TType type) => _dictionary.ContainsKey(type);

    /// <summary>Tries to get an object without throwing or logging.</summary>
    public bool TryGetObject(TType type, out TObject obj) => _dictionary.TryGetValue(type, out obj);

    public void Clear() => _dictionary.Clear();
    public void Remove(TType type) => _dictionary.Remove(type);
    public IReadOnlyDictionary<TType, TObject> GetAll() => _dictionary;

}