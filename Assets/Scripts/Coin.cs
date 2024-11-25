using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent coinCollect;
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(90,360,0),2,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinCollect.Invoke();
            transform.SetParent(other.transform);
            transform.DOLocalJump(Vector3.zero, 0.2f, 1, 1);
            transform.DOScale(Vector3.zero, 1);
        }
    }
}
