using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;
    [SerializeField] private NavMeshAgent agent;

    private float verticalVelocity;

    private Vector3 impact;
    private Vector3 dampingVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;


    private void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }

        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // ref keyword allows us to modofy certain variable types outside of scope, or to be modified by other methods
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude <= 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
        
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            // if we are the enemy
            agent.enabled = false;
        }
    }

    public void Jump(float jumpforce)
    {
        verticalVelocity += jumpforce;
    }
    
    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

}
