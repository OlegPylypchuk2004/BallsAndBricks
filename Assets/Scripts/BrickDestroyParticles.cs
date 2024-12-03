using System;
using System.Collections;
using UnityEngine;

public class BrickDestroyParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public event Action<BrickDestroyParticles> ParticlesPlayed;

    private void OnEnable()
    {
        _particleSystem.Play();

        StartCoroutine(StopParticles());
    }

    private IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(_particleSystem.main.startLifetime.constantMax);

        _particleSystem.Stop();

        ParticlesPlayed?.Invoke(this);
    }
}