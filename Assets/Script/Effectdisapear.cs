using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectdisapear : MonoBehaviour
{
    [Tooltip("Particle systems to destroy upon collision.")]
    public ParticleSystem[] ProjectileDestroyParticleSystemsOnCollision;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(ball,transform.position,transform.rotation);
            // destroy particle systems after a slight delay
            if (ProjectileDestroyParticleSystemsOnCollision != null)
            {
                foreach (ParticleSystem p in ProjectileDestroyParticleSystemsOnCollision)
                {
                    GameObject.Destroy(p, 0.1f);
                }
            }
        }
    }
}
