using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpeedEmitter : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

    void Start() {
        ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
    }

    void Update() {
        emission.rateOverTime = WorldController.Instance.speed / 6.25f;
    }
}
