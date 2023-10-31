using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myself;

    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int damage;
    private float Knockback;

    public void SetAttack(int damage, float Knockback)
    {
        this.damage = damage;
        this.Knockback = Knockback;
    }

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == myself) { return; }

        if(alreadyCollidedWith.Contains(other)) { return; }
        
        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.dealDamage(damage);
        }

        if(other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            // apply force
            Vector3 direction = (other.transform.position - myself.transform.position).normalized;
            forceReceiver.AddForce(direction * Knockback);
        }

    }
}
