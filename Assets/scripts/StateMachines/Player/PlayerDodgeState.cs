using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeLR");

    private float remainingDodgeTime;
    private Vector3 dodgingDirectionInput;

    private const float CrossFadeDuration = 0.1f;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);
        stateMachine.animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        Debug.Log("Remaining dodge time: " + remainingDodgeTime);

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0f)
        {
            remainingDodgeTime = 0f;
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.health.SetInvulnerable(false);
    }
}
