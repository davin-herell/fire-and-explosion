using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{
    public GameObject destroyedVersion;
    public GameObject explosionEffect;
    public float delay = 1f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    float countDown;
    bool startExplosion = false;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (startExplosion)
        {
            countDown -= Time.deltaTime;

            if (countDown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "ExplosionCatalyst")
        {
            startExplosion = true;
        };
    }

    void Explode()
    {
        // Show explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Instantiate the destroyed version of current object
        Instantiate(destroyedVersion, transform.position, transform.rotation);

        // Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }


        // Remove the exploded object
        Destroy(gameObject);
    }
}
