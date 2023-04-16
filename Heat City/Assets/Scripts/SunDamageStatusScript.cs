using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDamageStatusScript : MonoBehaviour
{
    public bool isSunDoDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.SetInsideStatus(!isSunDoDamage);
            }
        }
    }
}
