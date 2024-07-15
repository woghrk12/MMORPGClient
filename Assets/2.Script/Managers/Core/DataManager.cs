using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<K, V>
{
    public Dictionary<K, V> MakeDictionary();
}

public class DataManager
{
    #region Methods

    public void Init()
    {
        
    }

    private T LoadJson<T, K, V>(string path) where T : ILoader<K, V>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        return JsonUtility.FromJson<T>(textAsset.text);
    }

    #endregion Methods
}
