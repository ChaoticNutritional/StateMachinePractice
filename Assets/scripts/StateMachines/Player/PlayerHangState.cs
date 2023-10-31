using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangState : PlayerBaseState
{
    private Vector3 closestPoint;
    private Vector3 ledgeForward;
    public PlayerHangState(PlayerStateMachine stateMachine, Vector3 closestPoint, Vector3 ledgeForward) : base(stateMachine)
    {
        this.closestPoint = closestPoint;
        this.ledgeForward = ledgeForward;
    }

    // animation hash
    private readonly int HangAnimHash = Animator.StringToHash("Hanging Idle");

    public override void Enter()
    {
        // face forward
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.characterController.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.ledgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.characterController.enabled = true;

        // crossfade animation hash
        stateMachine.animator.CrossFadeInFixedTime(HangAnimHash, CrossFadeInFixedTimeAmt);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        // BONUS!!!!
        // Lets try giving the player to have a target while climbing
        // Lets give the option to allow the player to assassinate their target if they are hanging from a ledge
        // THIS IS FOR LATER 6/22/2022

        // if we press forward or jump, climb up


        // if we press back or down
        // switchstate to falling state
        if (stateMachine.InputReader.MovementValue.y > 0f)
        {
            stateMachine.SwitchState(new PlayerClimbupState(stateMachine));
        }
        else if (stateMachine.InputReader.MovementValue.y < 0f)
        {
            stateMachine.characterController.Move(Vector3.zero);
            stateMachine.forceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
    }
}

