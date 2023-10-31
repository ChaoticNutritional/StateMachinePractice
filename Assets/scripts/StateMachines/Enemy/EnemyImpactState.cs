using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactAnimHash = Animator.StringToHash("head impact");

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

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
        stateMachine.animator.CrossFadeInFixedTime(ImpactAnimHash, CrossFadeDuration);
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
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }
}
