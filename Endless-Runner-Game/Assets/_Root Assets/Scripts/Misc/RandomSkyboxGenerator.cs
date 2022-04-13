using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSkyboxGenerator : MonoBehaviour
{
    [SerializeField] private RandomSkyboxGeneratorData data;

    private void Awake()
    {
        var randomSkybox = data.skyboxes[Random.Range(0, data.skyboxes.Count)];
        RenderSettings.skybox = randomSkybox;
    }
}
