using System.Collections.Generic;
using UnityEngine;

public class PoolManagerBase : MonoBehaviour
{

    [SerializeField] private List<GameObject> sampleObjects;

    [SerializeField] private int sizeOfPoolPerObject;

    [SerializeField] private Transform poolParent;

    private Dictionary<int, List<GameObject>> pooledObjects;

    
    protected void Init()
    {
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
        return null;
    }

    public void SendBackInPool(GameObject targetGo)
    {
        if (!targetGo.activeInHierarchy) 
            return;
        
        targetGo.transform.parent = poolParent;
        targetGo.transform.position = Vector3.zero;
        targetGo.SetActive(false);
    }

    public int GetNumberOfSampleObjects() => sampleObjects.Count;

    private void InitializePooledObjects()
    {
        pooledObjects = new Dictionary<int, List<GameObject>>();
        for (var sampleObjIndex = 0; sampleObjIndex < sampleObjects.Count; ++sampleObjIndex)
        {
            for (var i = 0; i < sizeOfPoolPerObject; ++i)
            {
                var go = Instantiate(sampleObjects[sampleObjIndex], poolParent);
                go.SetActive(false);
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
}
