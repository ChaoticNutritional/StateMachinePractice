using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbupState : PlayerBaseState
{
    public PlayerClimbupState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private readonly int ClimbUpAnimHash = Animator.StringToHash("Climb Up");
    private Vector3 offset = new Vector3(0, 2.35f, .6f);

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(ClimbUpAnimHash, CrossFadeInFixedTimeAmt);
    }

    public override void Tick(float deltaTime)
    {
         
        if (GetNormalizedTime(stateMachine.animator, "Climbing") <= 1f)
        {
            return;
        }

        stateMachine.characterController.enabled = false;  
        stateMachine.transform.Translate(offset, Space.Self);
        stateMachine.characterController.enabled = true;
        Debug.Log("here");
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        Debug.Log("Switching out of climbing state");
        stateMachine.characterController.Move(Vector3.zero);
        stateMachine.forceReceiver.Reset();
    }

}
