using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // on drestroyed

    public event Action<Target> OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
