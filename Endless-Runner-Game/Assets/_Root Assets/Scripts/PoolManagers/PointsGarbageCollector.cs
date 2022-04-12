using UnityEngine;

public class PointsGarbageCollector : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;

    private void Start()
    {
        player = PlayerMovement.Instance.transform;
        offset = transform.position - player.position;
    }

    private void Update()
    {
        transform.position = player.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpBoost") ||
            other.CompareTag("Invulnerability") ||
            other.CompareTag("PointsCollection"))
        {
            PoolManagerPoints.Instance.SendBackInPool(other.gameObject);
        }
    }
}
