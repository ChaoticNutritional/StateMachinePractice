using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;
    [field: SerializeField] public bool animOver { get; private set; }

    public void EnableWeapon()
    {
        // set to enabled on swing
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        // disable after swing
        weaponLogic.SetActive(false);
    }

    public void IsOver()
    {
        animOver = true;
    }

    public void HasStarted()
    {
        animOver = false;
    }
}
