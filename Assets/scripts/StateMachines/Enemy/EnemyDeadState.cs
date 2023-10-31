using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    // hash our animation by name
    private readonly int DeathAnimHash = Animator.StringToHash("death");

    public override void Enter()
    {
        // crossfade to death animation
        stateMachine.ragdoll.ToggleRagdoll(true);
        stateMachine.weaponDamage.gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.GetComponent<Target>());
        stateMachine.animator.CrossFadeInFixedTime(DeathAnimHash, CrossFadeDuration);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        
    }
}
