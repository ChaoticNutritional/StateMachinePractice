using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected const float CrossFadeDuration = 0.1f;
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
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
    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.GetComponent<PlayerStateMachine>().IsDead) { return false; }

        float playerDistSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistSqr <= stateMachine.PlayerChaseRadius * stateMachine.PlayerChaseRadius;
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        // get direction vector pointing from our statemachine (this enemy) to the player statemachine (the player)
        Vector3 faceDirection = stateMachine.Player.transform.position - stateMachine.transform.position;

        faceDirection.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
    }
}
