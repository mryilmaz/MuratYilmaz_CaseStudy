using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField] private ParticleSystem finishParticles;
    public List<Transform> avaliableSlots;
    void Start()
    {
        GameManager.instance.onLevelFinished += PopParticles;
    }

    private void PopParticles()
    {
        finishParticles.Play();
    }
}
