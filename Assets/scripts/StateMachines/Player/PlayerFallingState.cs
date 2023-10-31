using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallAnimHash = Animator.StringToHash("jump down");

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private Vector3 playerMomentum;

    public override void Enter()
    {
        playerMomentum = stateMachine.characterController.velocity;
        playerMomentum.y = 0;

        stateMachine.ledgeDetector.OnLedgeDetected += HandleLedgeDetection;

        //Debug.Log("subscribed to ledge detection");

        stateMachine.animator.CrossFadeInFixedTime(FallAnimHash, CrossFadeInFixedTimeAmt);


    }

    private void HandleLedgeDetection(Vector3 closestPoint, Vector3 ledgeForward)
    {
        //Debug.Log("here");
        stateMachine.SwitchState(new PlayerHangState(stateMachine, closestPoint, ledgeForward));
    }

    public override void Exit()
    {
        //Debug.Log("unsub from ledge detection");
        stateMachine.ledgeDetector.OnLedgeDetected -= HandleLedgeDetection;
    }

    public override void Tick(float deltaTime)
    {
        Move(playerMomentum, deltaTime);
        if (stateMachine.characterController.isGrounded)
        {
            ReturnToLocomotion();
        }
        FaceTarget();
    }


}
