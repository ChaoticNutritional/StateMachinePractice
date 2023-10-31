using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetForward");
    private readonly int TargetingLRHash = Animator.StringToHash("TargetLR");

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnCancel;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeInFixedTimeAmt);
    }

    public override void Tick(float deltaTime)
    {
        // change to blocking when press block button
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockState(stateMachine));
        }

        // change to attack state when press attack 
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();

    }
    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            // backstep
            stateMachine.SwitchState(new PlayerDodgeState(stateMachine, new Vector2(0f, -1f)));
        }
        else
        {
            stateMachine.SwitchState(new PlayerDodgeState(stateMachine, stateMachine.InputReader.MovementValue));
        }
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        Vector3 normalizedMovementValue = stateMachine.InputReader.MovementValue.normalized;
        float AnimatorDampeningTime = 0.1f;
        stateMachine.animator.SetFloat(TargetingForwardHash, normalizedMovementValue.y, AnimatorDampeningTime, deltaTime);
        stateMachine.animator.SetFloat(TargetingLRHash, normalizedMovementValue.x, AnimatorDampeningTime, deltaTime);
    }
}
