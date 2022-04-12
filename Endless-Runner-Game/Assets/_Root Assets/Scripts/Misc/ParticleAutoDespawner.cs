using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDespawner : MonoBehaviour
{

    [SerializeField] private float timer = 1f;


    private void Awake()
    {
        StartCoroutine(SelfDestroy());
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
