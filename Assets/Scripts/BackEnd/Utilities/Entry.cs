using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entry<TKey, TValue> : ScriptableObject
{
    public TKey key;
    public TValue value;

    public TValue GetValue(TKey targetKey)
    {
        return EqualityComparer<TKey>.Default.Equals(key, targetKey) ? value : default;
    }
}