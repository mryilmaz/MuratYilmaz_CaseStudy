using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
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
    
    private RotatingPlatform currentPlatform; 
    
    [SerializeField] private float platformForce = 5f;
    
    private bool isFinishing = false;  
    [SerializeField] private Transform finishPoint, startPoint;  

    private void Start()
    {
        gameManager = GameManager.instance;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isFinishing)
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

            if (currentPlatform != null)
            {
                ApplyPlatformForce(currentPlatform);
            }
        }
    }

    private void MovePlayer(Vector3 move)
    {
        _rigidbody.velocity = move * moveSpeed;
        
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    private void RotatePlayer(Vector3 move)
    {
        Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimation(Vector3 move)
    {
        float speed = move.magnitude;
        animator.SetFloat("Speed", speed);
        if (speed < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    
    private void ApplyPlatformForce(RotatingPlatform platform)
    {
        
        float forceDirection = platform.CounterClockwise ? -1f : 1f;

        
        Vector3 force = new Vector3(forceDirection * platformForce, 0, 0);

        
        _rigidbody.AddForce(force, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone/Finish"))
        {
            other.GetComponent<Collider>().enabled = false;
            _rigidbody.isKinematic = true;
            gameManager.OnLevelFinished();
            isFinishing = true;
            MoveToFinish();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle/RotatingPlatform"))
        {
            
            RotatingPlatform platform = other.GetComponent<RotatingPlatform>();
            if (platform != null)
            {
                currentPlatform = platform; 
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle/RotatingPlatform"))
        {
            currentPlatform = null; 
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle/RotatorBody"))
        {
            gameManager.OnPlayerFail();
            MoveToStart();
        }
    }

    private void MoveToFinish()
    {
        transform.DOMove(finishPoint.position, 2f).SetEase(Ease.Linear)  
            .OnComplete(() => FinishAtTarget());
        transform.DOLookAt(finishPoint.position, 0.2f);
    }

    private void FinishAtTarget()
    {
        UpdateAnimation(Vector3.zero);
        transform.DORotateQuaternion(finishPoint.rotation, 0.5f);  
    }

    private void MoveToStart()
    {
        transform.position = startPoint.position;
    }
    
}
