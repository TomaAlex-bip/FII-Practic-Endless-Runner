using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance { get; private set; }

    [SerializeField] private ParticlesManagerData data;

    private void Awake()
    {
        InitializeSingleton();
    }

    public GameObject InstantiatePointsParticles(Vector3 position) =>
        Instantiate(data.pointParticles, position, Quaternion.identity);
    
    public GameObject InstantiateJumpBoostParticles(Vector3 position) =>
        Instantiate(data.jumpBoostParticles, position, Quaternion.identity);
    
    public GameObject InstantiateInvulnerabilityParticles(Vector3 position) =>
        Instantiate(data.invulnerabilityParticles, position, Quaternion.identity);
    
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
