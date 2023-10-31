using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("attack1");
    private const float AnimatorDampeningTime = 0.1f;
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        FacePlayer();
        
        stateMachine.weaponDamage.SetAttack(stateMachine.damageAmt, stateMachine.atkKnockback);

        stateMachine.animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.animator, "Attack") >= 1)
        {
            // Switch to chase state
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
    }
}
