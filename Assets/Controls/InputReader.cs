using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;

    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public Vector2 MovementValue { get; private set; }

    private Controls controls;

    private void Start()
    {
        // At start, create new controls object 🔫
        controls = new Controls();

        // point that new controls object to this script to see its methods
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    // JUMP 
    public void OnJump(InputAction.CallbackContext context)
    {
        // InputAction.CallBackContext tells us how the jump button was just used
        if (!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    // DODGE 
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        IsAttacking = context.performed;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }
}
