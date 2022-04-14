using UnityEngine;

public class PoolManagerObstacles : PoolManagerBase
{
    public static PoolManagerObstacles Instance { get; private set; }

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
