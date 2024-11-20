using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HalfDonut : MonoBehaviour
{
    [SerializeField] private Transform leftStick, rightStick;
    private Sequence _moveTween;

    private void Start()
    {
        MoveSticks();
    }

    private void MoveSticks()
    {
        _moveTween = DOTween.Sequence();
        _moveTween.Append(leftStick.DOLocalMoveX(-0.15f, 2f).SetEase(Ease.OutSine));
        _moveTween.Join(rightStick.DOLocalMoveX(-0.15f, 2f).SetEase(Ease.OutSine));
        _moveTween.SetDelay(1);
        _moveTween.SetLoops(-1, LoopType.Yoyo);
    }
}
