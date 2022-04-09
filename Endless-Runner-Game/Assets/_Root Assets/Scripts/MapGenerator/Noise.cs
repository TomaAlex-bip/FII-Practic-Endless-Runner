using UnityEngine;

public static class Noise
{
    public static float GenerateNoiseValue(NoiseSettings noiseSettings, int noiseIndex)
    {
        var prng = new System.Random(noiseSettings.seed);
        var offset = prng.Next(-10000, 10000);
        var sampleX = noiseIndex / noiseSettings.scale + offset;
        
        var noiseValue = (Mathf.PerlinNoise(sampleX, 0.0f) * 2 - 1) * noiseSettings.amplitude;
        noiseValue = Mathf.InverseLerp(-1, 1, noiseValue);

        return noiseValue;
    }
}