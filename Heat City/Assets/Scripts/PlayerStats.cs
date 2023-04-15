using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth=200;
    private int actualHealth;
    public bool isInside;
    public int damagePerSecond = 1;
    public float damageDelay = 1f;
    private float nextDamageTime=0;

    void Start()
    {
        actualHealth = maxHealth;
    }

    async void Update()
    {
        if (!isInside)
        {
            if(Time.time >= nextDamageTime)
            {
                actualHealth -= damagePerSecond;

                nextDamageTime = Time.time + damageDelay;
            }

        }
    }

    public void SetInsideStatus(bool status)
    {
        this.isInside = status;
    }
}
