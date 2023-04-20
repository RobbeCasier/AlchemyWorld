using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field:Header("Music")]
    [field:SerializeField] public List<KeyValuePair<string, EventReference>> Music { get; private set; }

    [field:Header("Ambient")]
    [field:SerializeField] public List<KeyValuePair<string, EventReference>>  Ambient { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}