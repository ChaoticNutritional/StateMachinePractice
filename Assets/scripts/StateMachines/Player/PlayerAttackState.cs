using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private Attack atk;
    private bool alreadyAppliedForce; // defaults to false

    private float prevFrameTime;
    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        atk = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.weaponDamage.SetAttack(atk.Damage, atk.Knockback);
        stateMachine.animator.CrossFade(atk.AnimationName, atk.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {

        // we need a move method
        // CREATED IN BASE STATE
        Move(deltaTime);
        // we need a facetarget method
        // USING EXISTING METHOD IN BASE STATE
        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.animator, "Attack");

        if (normalizedTime < 1f)
        {

            if (normalizedTime >= atk.ForceTime)
            {
                TryApplyForce();
            }

            // if IsAttacking is true
            if (stateMachine.InputReader.IsAttacking)
            {
                // try combo attack
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // return to freelook
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }

    public override void Exit()
    {

    }

    

    private void TryComboAttack(float normalizedTime)
    {
        // we're at the final attack
        if (atk.ComboStateIndex == -1) { return; }

        // we'r not far enough through the current attack to do a follow-up
        if (normalizedTime < atk.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new PlayerAttackState
            (
                stateMachine,
                atk.ComboStateIndex
            )
        );
    }

    private void TryApplyForce()
    {
        if (!alreadyAppliedForce)
        {
            stateMachine.forceReceiver.AddForce(stateMachine.transform.forward * atk.Force);
            alreadyAppliedForce = true;
        }
    }
}
