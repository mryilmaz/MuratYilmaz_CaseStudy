using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Opponent : MonoBehaviour
{
    [SerializeField] private Animator animator; 
    [SerializeField] private Transform target,paintWall;
    private NavMeshAgent agent;
    private Vector3 startPoint;
    private Rigidbody _rigidbody;
    private FinishZone finishZone;
    private RankingManager rankingManager;
    private CapsuleCollider capsuleCollider;
    private bool isFinished;
    [SerializeField] private float platformForce = 0.02f;
    private void Start()
    {
        startPoint = transform.localPosition;
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        agent.destination = target.position;
        agent.speed=Random.Range(0.1f,0.17f);
        finishZone=FindObjectOfType<FinishZone>();
        rankingManager = FindObjectOfType<RankingManager>();
        rankingManager.competitors.Add(transform);
        animator.SetBool("isWalking",true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OpponentFinish"))
        {
            agent.enabled = false;
            _rigidbody.isKinematic = true;
            capsuleCollider.enabled = false;
            TakeASpot();
        }
        
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle/RotatorBody")||other.gameObject.CompareTag("Obstacle/Shining"))
        {
            MoveToStart();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            _rigidbody.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _rigidbody.isKinematic = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle/RotatingPlatform"))
        {
            ApplySideForce(other.GetComponent<RotatingPlatform>());
        }
    }
    private void ApplySideForce(RotatingPlatform platform)
    {
        var forceDirection = platform.CounterClockwise ? -1f : 1f;
        Vector3 sideForce = new Vector3(platformForce*forceDirection, 0f, 0f);
        
        agent.velocity += sideForce * Time.deltaTime;
        var rotationAmount = 10f; 

        
        Quaternion targetRotation = Quaternion.Euler(0f, rotationAmount * -forceDirection, 0f);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * targetRotation, 0.1f);
    }

    private void TakeASpot()
    {
        if (finishZone.avaliableSlots.Count > 0)
        {
            var targetPos=finishZone.avaliableSlots[0].position;
            finishZone.avaliableSlots.RemoveAt(0);
            transform.DOLookAt(targetPos, 0.2f);
            transform.DOMove(targetPos, 2f).SetEase(Ease.Linear).OnComplete(()=>
            {
                animator.SetBool("isWalking", false);
            });
        }
    }
    

    private void MoveToStart()
    {
        transform.localPosition = startPoint;
    }
}
