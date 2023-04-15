using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    string newItemName;
    [SerializeField]
    PickUpItem itemController;
    [SerializeField]
    string needItem;

    [SerializeField]
    Vector3 newItemPosition;

    [SerializeField]
    bool dropItem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PickUpArea")
        {
            Debug.Log("interakcja mo¿liwa");
            itemController = other.transform.GetComponent<PickUpItem>();
        }
    }

    public void Interact()
    {
        if (itemController.item.name == needItem)
        {
            itemController.item.name = newItemName;
            if (dropItem)
            {
                itemController.DropItem(newItemPosition);
            }
        }
        //od³o¿enie przedmiotu lub zmiana jego nazwy
        
    }
}
