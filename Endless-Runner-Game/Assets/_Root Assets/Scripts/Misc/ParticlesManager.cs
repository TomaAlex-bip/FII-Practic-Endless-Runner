using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance { get; private set; }

    [SerializeField] private GameObject pointParticles;
    [SerializeField] private GameObject jumpBoostParticles;
    [SerializeField] private GameObject invulnerabilityParticles;

    private void Awake()
    {
        InitializeSingleton();
    }

    public GameObject InstantiatePointsParticles(Vector3 position) =>
        Instantiate(pointParticles, position, Quaternion.identity);
    
    public GameObject InstantiateJumpBoostParticles(Vector3 position) =>
        Instantiate(jumpBoostParticles, position, Quaternion.identity);
    
    public GameObject InstantiateInvulnerabilityParticles(Vector3 position) =>
        Instantiate(invulnerabilityParticles, position, Quaternion.identity);
    
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
