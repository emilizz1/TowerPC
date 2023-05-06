using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoSingleton<GameSettings>
{
    public bool demo;
    public bool steam = true;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        SavedData.Load();
    }
}
