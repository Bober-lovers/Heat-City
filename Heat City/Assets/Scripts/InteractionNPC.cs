using CubePeople;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractionNPC : MonoBehaviour
{
    public string playerName = "Eustachy";
    public Color dialogueColor = Color.white;

    public bool doingQuest = false;
    bool canStartTalk = false;
    NPC nearNPC = null;

    public float angle = 0;
    bool talking = false;
    bool questStarted = false;
    bool searchingForItem = false;
    public Transform arrowObject;
    Transform itemToObtain;

    EightDirectionMovement movementScript;
    AnimationController animationScript;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<EightDirectionMovement>();
        animationScript = GetComponent<AnimationController>();

        arrowObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!talking && canStartTalk && Input.GetKeyDown(KeyCode.Q))
        {
            //Jezeli uda sie odaplic dialog to zaczynamy rozmowe :)
            if (DialogueManager.instance.startDialogue(nearNPC) == 0)
                startPlayerTalking();
        }

        if (talking)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(DialogueManager.instance.nextDialogue() == 1)
                    endPlayerTalking();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                DialogueManager.instance.endDialogue();
                endPlayerTalking();
            }
        }

        if (searchingForItem)
        {
            Vector3 dirToObtainObj = itemToObtain.position - transform.position;
            dirToObtainObj.y = 0;


            arrowObject.position = transform.position + dirToObtainObj.normalized * 0.5f;
            arrowObject.LookAt(itemToObtain.position);
        }
    }


    //This function is supposed to disable all input from player
    //Na chama jak na razie po prostu wylaczam skrypt poruszania sie
    void startPlayerTalking()
    {
        talking = true;
        movementScript.enabled = false;
        animationScript.enabled = false;
    }

    void endPlayerTalking()
    {
        talking = false;
        movementScript.enabled = true;
        animationScript.enabled = true;
    }

    public void startQuest(Transform item)
    {
        questStarted = true;
        searchingForItem = true;
        itemToObtain = item;
        doingQuest = true;
        arrowObject.gameObject.SetActive(true);
    }

    public void endQuest()
    {
        questStarted = false;
        doingQuest = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            canStartTalk = true;
            nearNPC = other.GetComponentInParent<NPC>();
        }

        if (other.CompareTag("Item"))
        {
            searchingForItem = false;
            itemToObtain = null;
            arrowObject.gameObject.SetActive(false);
            other.GetComponentInParent<QuestItem>().pickedUp();

            DialogueManager.instance.updateQuestText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            canStartTalk = false;
            nearNPC = null;
        }
    }
}
