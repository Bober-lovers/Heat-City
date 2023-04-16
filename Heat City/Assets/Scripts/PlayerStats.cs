using CodeMonkey.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth=200;
    public bool isInside;
    public float damagePerSecond = 1f;
    public float damageDelay = 2f;
    private float nextDamageTime=0;
    public HealthSystem HealthSystem;
    [SerializeField] private GameObject getHealthBarGameObject;

    void Start()
    {
        var component = GetComponent<HealthSystemComponent>();
        HealthSystem= component.GetHealthSystem();
        HealthSystem.SetHealthMax(maxHealth, true);
        getHealthBarGameObject.GetComponent<HealthBarUI>().SetHealthSystem(HealthSystem);
    }

    async void Update()
    {
        if (!isInside)
        {
            if(Time.time >= nextDamageTime)
            {
                HealthSystem.Damage(damagePerSecond);

                nextDamageTime = Time.time + damageDelay;
                Debug.Log(HealthSystem.GetHealth());
            }

        }
    }

    public void SetInsideStatus(bool status)
    {
        this.isInside = status;
    }
}
