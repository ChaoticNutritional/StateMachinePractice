using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            // we are transitioning to an attack

            // then, get data for next state
            return nextInfo.normalizedTime;
        }

        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            // we are NOT transitioning but currently playing an attack
            return currentInfo.normalizedTime;
        }

        else
        {
            return 0f;
        }
    }
}
