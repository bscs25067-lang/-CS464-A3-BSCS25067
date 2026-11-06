using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothTime = 0.125f;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position based on the target and offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly transition from current position to desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothTime);
    }
}