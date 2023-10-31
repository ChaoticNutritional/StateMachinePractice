using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isInvulnerable;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void dealDamage(int damageAmt)
    {
        if(isInvulnerable) { return; }
        
        currentHealth = Mathf.Max(currentHealth - damageAmt, 0);
        
        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
            return;
        }

        OnTakeDamage?.Invoke();
        Debug.Log(currentHealth);
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }
}
