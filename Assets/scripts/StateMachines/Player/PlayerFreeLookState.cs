using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    // hashed strings stored as int for easier reading
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampeningTime = 0.1f;
    private bool shouldFade;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {

        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;

        // Just so that we're not partially through an animation
        stateMachine.animator.SetFloat(FreeLookSpeedHash, 0f);

        if (shouldFade)
        {
            stateMachine.animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeInFixedTimeAmt);
        }
        else
        {
            stateMachine.animator.Play(FreeLookBlendTreeHash);
        }

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        // Any time we refer to statemachine, we're referring to the player
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampeningTime, deltaTime);
            return;
        }

        stateMachine.animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampeningTime, deltaTime);
        FaceMoveDirection(movement, deltaTime);
    }

    private void FaceMoveDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation =
        Quaternion.Lerp(
            // current rotation
            stateMachine.transform.rotation,
            // looking rotation (what it should be)
            Quaternion.LookRotation(movement),
            // rate of turning
            deltaTime * stateMachine.RotationSmoothVal);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forwardCam = stateMachine.MainCamTransform.forward;
        Vector3 rightCam = stateMachine.MainCamTransform.right;

        forwardCam.y = 0f;
        rightCam.y = 0f;

        forwardCam.Normalize();
        rightCam.Normalize();

        return (forwardCam * stateMachine.InputReader.MovementValue.y) + (rightCam * stateMachine.InputReader.MovementValue.x);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }
}
