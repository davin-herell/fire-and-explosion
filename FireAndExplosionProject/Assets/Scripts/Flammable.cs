using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{

    public GameObject burnedVersion;
    public GameObject flameEffect;
    public float burnDuration = 5f; // in seconds
    Material mat;
    GameObject instantiatedFlame;
    bool startBurning = false;
    bool startDissolving = false;
    bool hasBurned = false;
    bool hasDissolved = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (startBurning && !hasBurned)
        {
            Burn();
        }

        if (startDissolving && !hasDissolved)
        {
            Dissolve();
        }

        if (hasBurned && hasDissolved)
        {
            TurnToAsh();
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "FlameCatalyst")
        {
            startBurning = true;
            startDissolving = true;
        };
    }

    void setParticleDurationAndLifetime(GameObject obj, float duration, float lifetime)
    {
        if (obj)
        {
            foreach (Transform child in obj.GetComponentsInChildren<Transform>())
            {
                // Modify the effect's duration and lifetime
                ParticleSystem ps = obj.GetComponent<ParticleSystem>();

                if (ps)
                {
                    ps.Stop();

                    var main = ps.main;
                    main.duration = duration;
                    main.startLifetime = lifetime;

                    ps.Play();
                }
            }
        }
    }

    void Burn()
    {
        // Show flame effect
        instantiatedFlame = Instantiate(flameEffect, transform.position, transform.rotation) as GameObject;

        if (instantiatedFlame)
        {
            // Set the flame effect as the child of the current object
            instantiatedFlame.transform.parent = transform;

            setParticleDurationAndLifetime(instantiatedFlame, burnDuration, burnDuration - 2f);
        }

        hasBurned = true;
    }

    void Dissolve()
    {
        // Turn off shadow
        transform.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        mat.SetFloat("_DissolveAmount", mat.GetFloat("_DissolveAmount") + Time.deltaTime / burnDuration);

        if (mat.GetFloat("_DissolveAmount") >= 0.5f)
        {
            hasDissolved = true;
        }
    }

    void TurnToAsh()
    {
        // Instantiate the burned version of current object
        GameObject newObject = Instantiate(burnedVersion, transform.position, transform.rotation) as GameObject;

        // Set the flame effect as the child of the current object
        instantiatedFlame.transform.parent = newObject.transform;

        // Remove the burned object
        Destroy(gameObject);
    }
}
