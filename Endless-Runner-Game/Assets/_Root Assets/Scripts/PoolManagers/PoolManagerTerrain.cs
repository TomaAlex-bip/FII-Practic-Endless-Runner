using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerTerrain : PoolManagerBase
{
    public static PoolManagerTerrain Instance { get; private set; }


    [ContextMenu("Generate pool")]
    private void Awake()
    {
        base.Init();
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
