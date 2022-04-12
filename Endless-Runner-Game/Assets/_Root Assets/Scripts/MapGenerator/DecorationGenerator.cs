using UnityEngine;

[System.Serializable]
public class DecorationGenerator
{
    public static GameObject GenerateRandomDecoration()
    {
        var randomIndex = Random.Range(0, PoolManagerDecorations.Instance.SampleObstaclesLength);
        var go = PoolManagerDecorations.Instance.GetPooledObject(randomIndex);
        return go;
    }
}
