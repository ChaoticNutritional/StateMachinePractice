using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private readonly int BlockAnimHash = Animator.StringToHash("start block");
    private readonly int DeBlockAnimHash = Animator.StringToHash("de-block");

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeInFixedTimeAmt);
        // This blocks all damage while you're blocking which I think is boring
        stateMachine.health.SetInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.health.SetInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if(!stateMachine.InputReader.IsBlocking)
        {
            // THIS IS WHAT I THOUGHT WOULD BE GOOD, BUT LETS TRY TUTORIAL STUFF FIRST
            //ReturnToLocomotion();
            //return;
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        // 
    }
    // THE COOLER DANIEL
    /*{
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        FaceTarget();
        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeInFixedTimeAmt);
            ReturnToLocomotion();
        }
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
    }*/
}
