using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Tree : MonoBehaviour
{
    [SerializeField] GameObject woodPrefab;
    [SerializeField] int minDrops = 2, maxDrops = 5;
    [SerializeField] float dropRadius = 1f;
    HealthComponent hc;
    

    private void Awake()
    {
        hc = GetComponent<HealthComponent>();
        hc.OnDeath = OnDeath;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
        int drops = Random.Range(minDrops, maxDrops);
        for (int i = 0; i < drops; i++)
        {
            Vector3 offset = Random.insideUnitSphere * dropRadius ;
            offset.y = transform.position.y;
            Instantiate(woodPrefab, transform.position + offset, transform.rotation);
        }
    }
}
