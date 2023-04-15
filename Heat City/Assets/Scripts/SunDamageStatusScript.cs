using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDamageStatusScript : MonoBehaviour
{
    public bool isSunDoDamage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the house");
        }
    }
}
