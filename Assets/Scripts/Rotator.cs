using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotator : MonoBehaviour
{
    [SerializeField]private Transform rotator;
    [SerializeField] private bool counterclockwise = false;
    void Start()
    {
        var rotationAmount = counterclockwise ? -360f : 360f;
        rotator.DOLocalRotate(new Vector3(0f, rotationAmount, 0f), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);
    }
}
