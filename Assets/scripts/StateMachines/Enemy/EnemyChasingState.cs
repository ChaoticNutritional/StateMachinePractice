using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private const float AnimatorDampeningTime = 0.1f;
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //crossfade animation to the running one
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {
        // Debug.DrawRay(stateMachine.transform.position, stateMachine.Player.transform.position, Color.yellow);
        // if player is still in range
        // move along path
        // if player out of range

        if (!IsInChaseRange())
        {
            // switchstate back to idle state
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer();

        stateMachine.animator.SetFloat(SpeedHash, 1f, AnimatorDampeningTime, deltaTime);
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.nextPosition = stateMachine.transform.position;
    }

    private bool IsInAttackRange()
    {
        if (stateMachine.Player.GetComponent<PlayerStateMachine>().IsDead) { return false; }
        float playerDistSqr = MathF.Round((stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude, 3);

        return playerDistSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
