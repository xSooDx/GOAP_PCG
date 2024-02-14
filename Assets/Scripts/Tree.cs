using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Tree : MonoBehaviour
{
    [SerializeField] GameObject woodPrefab;
    HealthComponent hc;

    private void Awake()
    {
        hc = GetComponent<HealthComponent>();
        hc.OnDeath = OnDeath;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
        Instantiate(woodPrefab, transform.position, transform.rotation);
    }
}
