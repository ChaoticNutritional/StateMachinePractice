using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{

    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public float PlayerChaseRadius { get; private set; }
    [field: SerializeField] public CharacterController characterController { get; private set; }
    [field: SerializeField] public ForceReceiver forceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage weaponDamage { get; private set; }
    [field: SerializeField] public int damageAmt { get; private set; }
    [field: SerializeField] public float atkKnockback { get; private set; }
    [field: SerializeField] public Health health { get; private set; }
    [field: SerializeField] public Target target { get; private set; }
    [field: SerializeField] public Ragdoll ragdoll { get; private set; }


    public GameObject Player { get; private set; }

    private void Start()
    {
        // we don't want our navmesh doing the work for us
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        // Let's just drag it into the inspector later?
        Player = GameObject.FindGameObjectWithTag("Player");

        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChaseRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    private void OnEnable()
    {
        health.OnTakeDamage += HandleTakeDamage;
        health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= HandleTakeDamage;
        health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }
}
