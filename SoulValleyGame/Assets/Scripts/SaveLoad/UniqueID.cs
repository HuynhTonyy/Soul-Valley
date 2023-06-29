using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string id= Guid.NewGuid().ToString();
    [SerializeField] 
    private static SerializableDictionary<string, GameObject> idDatabase 
        = new SerializableDictionary<string, GameObject>();

    public string ID => id;

    private void OnValidate()
    {
        if( idDatabase.ContainsKey(id)) Generate();
        else idDatabase.Add(id, this.gameObject);
    }
    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(id)) idDatabase.Remove(id);
    }

    private void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id, this.gameObject);
    }
}
