using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public NPC connectedToNPCQuest;

    public void pickedUp()
    {
        connectedToNPCQuest.itemObtained();
        Destroy(gameObject);
    }
}
