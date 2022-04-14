using UnityEngine;

[CreateAssetMenu(fileName = "ParticlesManagerData", menuName = "ScriptableObjects/ParticlesManager", order = 1)]
public class ParticlesManagerData : ScriptableObject
{
    public GameObject pointParticles;
    public GameObject jumpBoostParticles;
    public GameObject invulnerabilityParticles;
}
