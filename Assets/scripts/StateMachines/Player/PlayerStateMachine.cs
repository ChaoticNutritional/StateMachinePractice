using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{

    [field: SerializeField] public CharacterController characterController { get; private set; }

    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSmoothVal { get; private set; }
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver forceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage weaponDamage { get; private set; }
    [field: SerializeField] public Health health { get; private set; }
    [field: SerializeField] public Ragdoll ragdoll { get; private set; }
    [field: SerializeField] public bool IsDead { get; protected set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }
    [field: SerializeField] public LedgeDetector ledgeDetector { get; private set; }

    //removing this
    //[field: SerializeField] public float DodgeCooldown { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }

    // This sets our first dodge value to the largest possible number, so it can be double negative added to Time.time
    // to evaluate to greater than the cooldown value;
    public float TimeOfLastDodge { get; private set; } = Mathf.NegativeInfinity;

    [SerializeField] public Transform MainCamTransform { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        MainCamTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
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
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        this.IsDead = true;
        SwitchState(new PlayerDeadState(this));
    }

    // For cooldowns only
    public void SetDodgeTime(float timeOfDodge)
    {
        TimeOfLastDodge = timeOfDodge;
    }
}
