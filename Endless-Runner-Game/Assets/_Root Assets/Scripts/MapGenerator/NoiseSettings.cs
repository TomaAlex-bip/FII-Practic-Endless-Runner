using UnityEngine;

[CreateAssetMenu(fileName = "NoiseSettings", menuName = "ScriptableObjects/NoiseSettingsSO", order = 1)]
public class NoiseSettings : ScriptableObject
{
    [Range(0.01f, 50f)]
    public float scale = 10f;
    public float amplitude = 1f;

    public bool randomSeed = true;
    public int seed;
    
}