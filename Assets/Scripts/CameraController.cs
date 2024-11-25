using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;       
    [SerializeField] private Vector3 offset;        
    [SerializeField] private float smoothSpeed = 0.125f; 

    private Vector3 velocity = Vector3.zero; 
    private Vector3 smoothPosition;
    private bool followingPlayer = true;

    private void Awake()
    {
        GameManager.instance.onLevelFinished += GoEndingPosition;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    void LateUpdate()
    {
        if (!followingPlayer)return;
        transform.position = smoothPosition;
    }

    private void GoEndingPosition()
    {
        followingPlayer = false;
        //transform.DOMove(new Vector3(0, 0, 0), 1); test
    }
}