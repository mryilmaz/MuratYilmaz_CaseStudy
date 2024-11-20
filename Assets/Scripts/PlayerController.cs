using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.4f; 
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private Animator animator; 
    public Joystick joystick; 
    private Rigidbody _rigidbody; 

    [Header("Movement Boundaries")]
    [SerializeField] private float minX = -0.5f; 
    [SerializeField] private float maxX = 0.5f;  
    [SerializeField] private float minZ = -0.7f;
    [SerializeField] private float maxZ = float.MaxValue;  

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = joystick.Horizontal;  
        float moveY = joystick.Vertical;    

        Vector3 move = new Vector3(moveX, 0, moveY);
        
        MovePlayer(move);
        if (move.magnitude > 0.1f) 
        {
            RotatePlayer(move);
        }

        UpdateAnimation(move);
    }

    void MovePlayer(Vector3 move)
    {
        _rigidbody.velocity = move * moveSpeed;
        
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    void RotatePlayer(Vector3 move)
    {
        Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    void UpdateAnimation(Vector3 move)
    {
        float speed = move.magnitude;
        animator.SetFloat("Speed", speed);
        if (speed < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}