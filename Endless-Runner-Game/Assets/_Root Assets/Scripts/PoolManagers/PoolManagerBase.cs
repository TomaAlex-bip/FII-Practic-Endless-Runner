using System.Collections.Generic;
using UnityEngine;

public class PoolManagerBase : MonoBehaviour
{

    [SerializeField] private List<GameObject> sampleObjects;

    [SerializeField] private int sizeOfPool;

    [SerializeField] private Transform poolParent;

    private List<GameObject> pooledObjects;

    
    protected void Init()
    {
        InitializePooledObjects();
    }
    
    public GameObject GetPooledObject()
    {
        var randomIndex = Random.Range(0, sizeOfPool);

        var allActive = true;
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                allActive = false;
                break;
            }
        }

        if (allActive)
        {
            return null;
        }
        
        while (true)
        {
            if (!pooledObjects[randomIndex].activeInHierarchy)
            {
                pooledObjects[randomIndex].SetActive(true);
                return pooledObjects[randomIndex];
            }

            randomIndex = Random.Range(0, sizeOfPool);
        }
    }

    public void SendBackInPool(GameObject targetGo)
    {
        if (targetGo.activeInHierarchy)
        {
            targetGo.transform.parent = poolParent;
            targetGo.transform.position = Vector3.zero;
            targetGo.SetActive(false);
        }
    }

    public List<GameObject> GetSampleObjects()
    {
        return sampleObjects;
    }

    private void InitializePooledObjects()
    {
        pooledObjects = new List<GameObject>();
        for (var i = 0; i < sizeOfPool; ++i)
        {
            var randomIndex = Random.Range(0, sampleObjects.Count);
            var go = Instantiate(sampleObjects[randomIndex], poolParent);
            go.SetActive(false);
            pooledObjects.Add(go);
        }
    }
}


public enum PoolObjectType
{
    Obstacle, Terrain, Cloud
}

