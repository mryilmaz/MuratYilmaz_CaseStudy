using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;       
    [SerializeField] private Vector3 offset;        
    [SerializeField] private float smoothSpeed = 0.125f; 

    private Vector3 velocity = Vector3.zero; 
    private Vector3 smoothPosition; 

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    void LateUpdate()
    {
        //transform.position = new Vector3(transform.position.x,smoothPosition.y, smoothPosition.z);
        transform.position = smoothPosition;
    }
}