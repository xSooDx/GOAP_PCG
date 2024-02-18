using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnDeathDelegate();
    public delegate void OnDamageDelegate(float currentHealth, float damage);

    public OnDeathDelegate OnDeath;
    public OnDamageDelegate OnDamage;

    [SerializeField] float startingHealth = 10;
    [SerializeField] float maxHealth = 10;
    [SerializeField] float currentHealth;
    public float CurrentHP { get { return currentHealth; } }
    public float MaxHP { get { return maxHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
    }

    public float TakeDamage (float damage)
    {
        currentHealth -= damage;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnDamage?.Invoke(currentHealth, damage);

        if (currentHealth <= 0) 
        {
            OnDeath?.Invoke();
        }

        return currentHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
