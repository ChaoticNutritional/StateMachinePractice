using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    protected float CrossFadeInFixedTimeAmt = 0.1f;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.characterController.Move((motion + stateMachine.forceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget()
    {
        // check if we have a target
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        // get the vector3 pointing from our player to our target (other.position - player.position)
        Vector3 faceDirection = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;

        faceDirection.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
    }

    protected void ReturnToLocomotion()
    {
        if(stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else{
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
