using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        var desiredPosition = offset + target.transform.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
