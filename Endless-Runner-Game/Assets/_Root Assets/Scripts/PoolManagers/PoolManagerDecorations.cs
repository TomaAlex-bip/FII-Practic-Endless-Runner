using UnityEngine;

public class PoolManagerDecorations : PoolManagerBase
{
    public static PoolManagerDecorations Instance { get; private set; }
    
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
