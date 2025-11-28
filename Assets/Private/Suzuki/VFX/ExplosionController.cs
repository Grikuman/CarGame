using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject targetMachine;     // 消すマシン
    [SerializeField] private float hideDelay = 0.2f;       // マシンを消すまでの遅延（秒）

    private List<ParticleSystem> allParticles = new List<ParticleSystem>();

    void Start()
    {
        ParticleSystem selfParticle = GetComponent<ParticleSystem>();
        if (selfParticle != null)
            allParticles.Add(selfParticle);

        ParticleSystem[] childrenParticles = GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem particle in childrenParticles)
            if (!allParticles.Contains(particle))
                allParticles.Add(particle);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 爆発エフェクト再生
            foreach (ParticleSystem particle in allParticles)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.Play();
            }

            // 遅れてマシンを消す
            if (targetMachine != null)
                StartCoroutine(HideMachineWithDelay());
        }
    }

    IEnumerator HideMachineWithDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        targetMachine.SetActive(false);
        Debug.Log("マシンを非表示");
    }
}
