using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerPoints : MonoBehaviour
{
    public static PoolManagerPoints Instance { get; private set; }
    
    public int SampleObstaclesLength { get => settings.sampleObjects.Count; }

    [SerializeField] private PoolManagerPointsSettings settings;
    [SerializeField] private Transform poolParent;

    private Dictionary<int, List<GameObject>> pooledObjects;
    

    [ContextMenu("Generate pool")]
    private void Awake()
    {
        InitializeSingleton();
        InitializePooledObjects();
    }
    
    public GameObject GetPooledObject(int sampleObjectIndex)
    {
        var list = pooledObjects[sampleObjectIndex];
        foreach (var obj in list)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        // create a new GameObject if requested and none is available, and put it in the objects pool
        var newGo = Instantiate(settings.sampleObjects[sampleObjectIndex], poolParent);
        pooledObjects[sampleObjectIndex].Add(newGo);
        return newGo;
    }

    public void SendBackInPool(GameObject targetGo)
    {
        if (!targetGo.activeInHierarchy) 
            return;
        
        targetGo.transform.parent = poolParent;
        targetGo.transform.position = Vector3.zero;
        targetGo.SetActive(false);
    }

    private void InitializePooledObjects()
    {
        pooledObjects = new Dictionary<int, List<GameObject>>();
        for (var sampleObjIndex = 0; sampleObjIndex < settings.sampleObjects.Count; ++sampleObjIndex)
        {
            for (var i = 0; i < settings.sizeOfPoolPerObject; ++i)
            {
                var go = Instantiate(settings.sampleObjects[sampleObjIndex].gameObject, poolParent);
                go.gameObject.SetActive(false);
                if (pooledObjects.ContainsKey(sampleObjIndex))
                {
                    pooledObjects[sampleObjIndex].Add(go);
                }
                else
                {
                    pooledObjects.Add(sampleObjIndex, new List<GameObject>(){ go });
                }
            }
        }
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
