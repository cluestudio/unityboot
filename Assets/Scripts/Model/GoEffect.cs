using UnityEngine;
using System.Collections;

public class GoEffect : GoItem {
    public float lifeTime = 3;
    public float returnDelay = 2f;
    public bool emit { get; private set; }

    public virtual void Play() {
        StartParticle();
        StartCoroutine(DieAfter(lifeTime));
    }

    protected IEnumerator DieAfter(float wait) {
        yield return new WaitForSeconds(wait);
        StopParticle();
        yield return new WaitForSeconds(returnDelay);
        ClearParticle();

        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        float trailTime = 0;
        if (trail != null) {
            trailTime = trail.time;
            trail.time = 0;
        }
        yield return null;
        if (trail != null) {
            trail.time = trailTime;
        }

        yield return null;
        Service.goPooler.Return(this);
    }

    public override void OnGettingOutPool() { 
    }

    public override void OnGoingIntoPool() { 
        this.ClearParticle();
        StopAllCoroutines();
    }

    public void StartParticle() {
        emit = true;
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in particleSystems) {
            ps.time = 0;
            ps.Play();
        }
    }

    public void StopParticle() {
        emit = false;
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in particleSystems) {
            ps.Stop();
        }
    }

    public void ClearParticle() {
        emit = false;
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in particleSystems) {
            ps.Clear();
            ps.time = 0;
        }
    }
}
