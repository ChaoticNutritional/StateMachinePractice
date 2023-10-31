using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine){ }

    // hash our animation by name
    private readonly int DeathAnimHash = Animator.StringToHash("death");

    public override void Enter()
    {
        // crossfade to death animation
        stateMachine.animator.CrossFadeInFixedTime(DeathAnimHash, CrossFadeInFixedTimeAmt);
        stateMachine.weaponDamage.gameObject.SetActive(false);
        stateMachine.ragdoll.ToggleRagdoll(true);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
