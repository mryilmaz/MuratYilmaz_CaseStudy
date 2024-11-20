using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField]private Transform rotatingPlatform;
    public bool CounterClockwise = false;
    void Start()
    {
        var rotationAmount = CounterClockwise ? 360f : -360f;
        rotatingPlatform.DOLocalRotate(new Vector3(0f, 0f, rotationAmount), 15f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);
    }
}
