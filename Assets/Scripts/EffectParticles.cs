using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    void Start()
    {
        particles.Play();
        Destroy(gameObject, particles.main.duration);
    }
}
