using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private readonly int ImpactAnimHash = Animator.StringToHash("head impact");

    private float duration = 1.0f;

    public override void Enter()
    {
        // crossfade to impact animation
        // Grocery list:
        /*
            - crossfade time
            - animation hash
            - statemachine.animator.cross
        */
        stateMachine.animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeInFixedTimeAmt);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }
}
