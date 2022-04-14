using UnityEngine;

public class PoolManagerPoints : PoolManagerBase
{
    public static PoolManagerPoints Instance { get; private set; }
    
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
