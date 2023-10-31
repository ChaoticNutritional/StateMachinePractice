using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private readonly int JumpAnimHash = Animator.StringToHash("jump up");
    private Vector3 playerMomentum;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.forceReceiver.Jump(stateMachine.JumpForce);

        playerMomentum = stateMachine.characterController.velocity;
        playerMomentum.y = 0f;

        stateMachine.ledgeDetector.OnLedgeDetected += HandleLedgeDetect;

        stateMachine.animator.CrossFadeInFixedTime(JumpAnimHash, CrossFadeInFixedTimeAmt);
    }

    private void HandleLedgeDetect(Vector3 MyLedgeDetectPos, Vector3 PointOnLedge)
    {
        
        stateMachine.SwitchState(new PlayerHangState(stateMachine, MyLedgeDetectPos, PointOnLedge));
    }

    public override void Exit()
    {
        stateMachine.ledgeDetector.OnLedgeDetected -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(playerMomentum, deltaTime);

        if(stateMachine.characterController.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }


}
