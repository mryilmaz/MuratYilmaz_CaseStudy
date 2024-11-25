using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShiningObstacle : MonoBehaviour
{
    [SerializeField]private ParticleSystem particleSystem1,particleSystem2;
    void Start()
    {
        particleSystem2.Stop();
        transform.DOLocalMoveX(-0.35f,5).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalRotate(new Vector3(0,360,0),3f,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleParticleSystems();
        }
    }

    private void ToggleParticleSystems()
    {
        if (particleSystem1.isPlaying)
        {
            particleSystem1.Stop(); 
            particleSystem2.Play();
        }
        else
        {
            particleSystem1.Play(); 
            particleSystem2.Stop(); 
        }
    }
}
