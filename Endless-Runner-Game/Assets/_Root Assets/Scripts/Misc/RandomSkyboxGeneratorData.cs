using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkyboxGeneratorSettings", menuName = "ScriptableObjects/SkyboxGenerator", order = 1)]
public class RandomSkyboxGeneratorData : ScriptableObject
{
    public List<Material> skyboxes;
}
