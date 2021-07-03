using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField]
    public GameObject destroyParticle;

    private Transform particleParent;

    private Queue<ParticleSystem> pool;

    private void Awake()
    {
        Instance = this;
        particleParent = new GameObject("ParticleParent").transform;
        pool = new Queue<ParticleSystem>();

        pool.Enqueue(GameObject.Instantiate(destroyParticle, particleParent).GetComponent<ParticleSystem>());
    }

    public void MakeParticle(Vector3 position)
    {
        ParticleSystem particle = pool.Peek();
        
        if (particle.isPlaying)
        {
            particle = GameObject.Instantiate(destroyParticle, particleParent).GetComponent<ParticleSystem>();
        }
        else
        {
            pool.Dequeue();
        }
        particle.transform.position = position;

        particle.Play();
        pool.Enqueue(particle);
    }
}
