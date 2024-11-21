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

    // Platformla ilgili kontrol
    private RotatingPlatform currentPlatform; 

    // Kuvvetin büyüklüğü
    [SerializeField] private float platformForce = 5f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Hareket hesaplaması
        float moveX = joystick.Horizontal;  
        float moveY = joystick.Vertical;    
        Vector3 move = new Vector3(moveX, 0, moveY);
        
        // Hareket ve animasyon güncellemesi
        MovePlayer(move);
        if (move.magnitude > 0.1f) 
        {
            RotatePlayer(move);
        }

        UpdateAnimation(move);

        // Platformla etkileşimdeyken kuvvet uygula
        if (currentPlatform != null)
        {
            ApplyPlatformForce(currentPlatform);
        }
    }

    void MovePlayer(Vector3 move)
    {
        _rigidbody.velocity = move * moveSpeed;
        
        // Hareket sınırlandırma
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

    // RotatingPlatform ile etkileşime girdiğimizde
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle/RotatingPlatform"))
        {
            // RotatingPlatform bileşenini al
            RotatingPlatform platform = other.GetComponent<RotatingPlatform>();
            if (platform != null)
            {
                currentPlatform = platform; // Platformu geçici olarak sakla
            }
        }
    }

    // RotatingPlatform'dan sabit kuvvet uygulama
    void ApplyPlatformForce(RotatingPlatform platform)
    {
        // Platformun dönüş yönüne göre kuvvetin yönü
        float forceDirection = platform.CounterClockwise ? -1f : 1f;

        // Kuvveti X ekseninde uygula, platformun dönüş yönüne göre
        Vector3 force = new Vector3(forceDirection * platformForce, 0, 0);

        // Kuvveti oyuncuya uygula
        _rigidbody.AddForce(force, ForceMode.Force);
    }

    // OnTriggerExit ile platformdan çıktığında currentPlatform null yaparak
    // platformdan çıkıldığında etkisini kesmiş oluruz
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle/RotatingPlatform"))
        {
            currentPlatform = null; // Platformdan ayrıldığında currentPlatform'u sıfırla
        }
    }
}
