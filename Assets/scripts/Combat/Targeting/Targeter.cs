using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private List<Target> targets = new List<Target>();
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    public Target CurrentTarget { get; private set; }

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float distToClosest = Mathf.Infinity;

        foreach (Target target in targets)
        {
            // if target is on screen, viewpos will be between 0,0 and 1,1
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if (viewPos.x < 0 ||
            viewPos.x > 1 ||
            viewPos.y < 0 ||
            viewPos.y > 1)
            {
                continue;
            }

            Vector2 toScreenCenter = viewPos - new Vector2(0.5f, 0.5f);
            
            if (toScreenCenter.sqrMagnitude < distToClosest)
            {
                closestTarget = target;
                distToClosest = toScreenCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup?.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets?.Remove(target);
    }
}
