using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] int damage = 10;
    [SerializeField] bool explosive;
    [SerializeField] float explosionRadius;
    [SerializeField] float pushForce;
    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] LayerMask explosionMask;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocity;
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (explosive)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, explosionMask);
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.attachedRigidbody)
                    {
                        hit.attachedRigidbody.GetComponent<HealthComponent>()?.TakeDamage(damage);
                        hit.attachedRigidbody.AddExplosionForce(pushForce, transform.position, explosionRadius);
                    }
                }
            }
        }
        else
        {
            if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<HealthComponent>(out HealthComponent otherHealth))
            {
                other.attachedRigidbody.AddForce(transform.forward * pushForce);
                otherHealth.TakeDamage(damage);
            }
        }
        if(hitParticlePrefab)
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent otherHealth))
        {
            otherHealth.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        if(explosive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
