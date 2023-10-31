using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private const float AnimatorDampeningTime = 0.1f;
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        // Call move method in case movement needs to occur before check.
        // Move(deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        stateMachine.animator.SetFloat(SpeedHash, 0f, AnimatorDampeningTime, deltaTime);

    }
}
