using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private KeyCode pickUpKey;

    private bool holdItem;
    [SerializeField]
    private Transform hands;
    private Transform toPickUp;
    public Transform item;

    [SerializeField]
    Interaction interaction;

    //gracz naciska przycisk do podnoszenia przedmiotu
    private void Update()
    {
        if (Input.GetKeyDown(pickUpKey))
        {
            //gra sprawdza czy gracz nie trzyma przedmiotu
            if(holdItem)
            {
                //sprawdza czy nie jest w miejscu z interakcj¹
                if(interaction != null)
                {
                    //interakcja
                    interaction.Interact();
                }
                else
                {
                    //jeœli trzyma przedmiot to puszcza trzymany
                    DropItem(item.position);
                }
            }
            //jeœli nie trzyma
            else
            {
                //gra sprawdza czy przed graczem jest przedmiot do podniesienia
                if (toPickUp != null)
                {
                    item = toPickUp;
                    item.SetParent(hands);
                    Vector3 position = item.localPosition;
                    Debug.Log(position);
                    position.z = 0.7f;
                    item.GetComponent<Collider>().enabled = false;
                    Collider[] collider = item.GetComponentsInChildren<Collider>();
                    foreach(Collider collider1 in collider)
                    {
                        collider1.enabled = false;
                    }
                    item.localPosition = position;
                    holdItem = true;
                    //tutaj zmieñmy jeszcze animacjê na chodzenie z itemem ale to nie teraz, teraz to mi siê nie chce
                }
            }
        }
    }

    public void DropItem(Vector3 position)
    {
        item.SetParent(null);
        item.position = position;
        item.GetComponent<Collider>().enabled = true;
        Collider[] collider = item.GetComponentsInChildren<Collider>();
        foreach (Collider collider1 in collider)
        {
            collider1.enabled = true;
        }
        item = null;
        holdItem = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickable")
        {
            Debug.Log(other);
            toPickUp = other.transform;
        }
        if(other.GetComponent<Interaction>()!=null)
        {
            interaction = other.GetComponent<Interaction>();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.transform == toPickUp)
        {
            Debug.Log("ju¿ nie");
            toPickUp = null;
        }
        if(other.GetComponent<Interaction>() != null)
        {
            interaction = null;
        }
    }
}
