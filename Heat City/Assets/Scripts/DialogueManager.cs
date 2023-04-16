using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Procedural;

public enum LANG { POL, ENG }

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue box")]
    //DialogueBox
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI textField;
    public RectTransform dialogueBox;
    public RectTransform showDialoguePos;
    public RectTransform hideDialoguePos;
    public Image backgroundBox;
    public Image backgroundBoxNPC;
    public bool hidden = true;

    [Header("Quest box")]
    //QuestBox
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public RectTransform questBox;
    public RectTransform showQuestPos;
    public RectTransform hideQuestPos;

    public bool hiddenQuest = true;

    [Space]
    public LANG language = LANG.POL;

    //Potem na private to zamienic
    public bool dialogueStarted = false;
    public int currentDialogueID = 0;
    
    Dialogue[] npcDialogues = null;
    NPC refNPC;
    Quest startedQuest;
    InteractionNPC playerScript;

    Coroutine dialogueBoxCor;
    Coroutine questBoxCor;

    //Singleton
    private void Awake()
    {
        playerScript = FindObjectOfType<InteractionNPC>();
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        dialogueBox.position = new Vector3(dialogueBox.position.x, hideDialoguePos.position.y, dialogueBox.position.z);
        questBox.position = new Vector3(questBox.position.x, hideQuestPos.position.y, questBox.position.z);

        hiddenQuest = true;
        hidden = true;
    }


    public int startDialogue(NPC interNPC)
    {
        Debug.Log("Talking with " + interNPC.name);
        if(interNPC == null)
        {
            Debug.LogError("Cannot start talking with null NPC!");
            return 1;
        }
        refNPC = interNPC;
        dialogueStarted = true;
        currentDialogueID = 0;

        if (interNPC.questNumber < interNPC.quest.Length)
        {
            switch (interNPC.quest[interNPC.questNumber].questStatus)
            {
                case QuestStatus.Starting:
                    //To zabezpieczenie przed tym aby gracz robil pare questow, nie mam czasu i checi zaby zrobic zarzadzanie wieloma questami xd
                    if (!playerScript.doingQuest)
                        npcDialogues = interNPC.quest[interNPC.questNumber].startQuestDialogues;
                    else
                        npcDialogues = new Dialogue[] { new Dialogue { textPL = "Wróæ do mnie jak skoñczysz swoje aktualne zadanie!", 
                            textENG = "Come back to me when you finish your current quest!", playerTalking = false } };
                    break;
                case QuestStatus.Going:
                    npcDialogues = interNPC.quest[interNPC.questNumber].whileQuestDialogues;
                    break;
                case QuestStatus.ItemObtained:
                    npcDialogues = interNPC.quest[interNPC.questNumber].endQuestDialogues;
                    break;
            }

        }
        else
        {
            npcDialogues = interNPC.normalDialogues;
        }


        if (currentDialogueID >= npcDialogues.Length)
        {
            endDialogue();
            return 1;
        }

        updateDialogueText();

        dialogueBoxCor = null;
        dialogueBoxCor = StartCoroutine(showDialogueWindow());

        return 0;
    }

    public int nextDialogue()
    {
        currentDialogueID++;
        if(currentDialogueID >= npcDialogues.Length)
        {
            endDialogue();
            return 1;
        }
        updateDialogueText();

        return 0;
    }

    public void startQuest(Quest quest)
    {
        startedQuest = quest;
        updateQuestText();

        playerScript.startQuest(quest.item.transform);

        questBox.position = showQuestPos.position;
        questBoxCor = null;
        questBoxCor = StartCoroutine(showQuestWindow());
    }

    public void updateQuestText()
    {
        if (language == LANG.POL)
        {
            questName.text = startedQuest.namePL;
            if(startedQuest.questStatus == QuestStatus.ItemObtained)
                questDescription.text = startedQuest.descriptionAfterItemPL;
            else
                questDescription.text = startedQuest.descriptionPL;
        }
        else
        {
            questName.text = startedQuest.nameENG;
            if (startedQuest.questStatus == QuestStatus.ItemObtained)
                questDescription.text = startedQuest.descriptionAfterItemENG;
            else
                questDescription.text = startedQuest.descriptionENG;
        }
    }

    public void endQuest()
    {
        questBox.position = hideQuestPos.position;
        playerScript.endQuest();

        questBoxCor = null;
        questBoxCor = StartCoroutine(hideQuestWindow());
    }

    void updateDialogueText()
    {
        if (!npcDialogues[currentDialogueID].playerTalking)
        {
            nameField.text = refNPC.npcName;
            nameField.color = refNPC.npcDialogueColor;
            nameField.alignment = TextAlignmentOptions.Right;
            backgroundBox.color = new Color(255, 255, 255, 0);
            backgroundBoxNPC.color = new Color(255, 255, 255, 255);
        }
        else
        {
            nameField.text = playerScript.playerName;
            nameField.color = playerScript.dialogueColor;
            nameField.alignment = TextAlignmentOptions.Left;
            backgroundBox.color = new Color(255, 255, 255, 255);
            backgroundBoxNPC.color = new Color(255, 255, 255, 0);
        }


        if (language == LANG.POL)
        {
            textField.text = npcDialogues[currentDialogueID].textPL;
        }
        else
        {
            textField.text = npcDialogues[currentDialogueID].textENG;
        }
    }

    public void endDialogue()
    {
        Debug.Log("Dialogue ended");

        if (refNPC.questNumber < refNPC.quest.Length)
        {
            switch (refNPC.quest[refNPC.questNumber].questStatus)
            {
                case QuestStatus.Starting:
                    if (!playerScript.doingQuest)
                        refNPC.startQuest(); break;
                case QuestStatus.ItemObtained: refNPC.endQuest(); break;

            }
        }

        dialogueBoxCor = null;
        dialogueBoxCor = StartCoroutine(hideDialogueWindow());
    }


    //To mozna jako animacje zrobic ale mi sei nie chce sr
    IEnumerator showDialogueWindow()
    {
        if (!hidden)
            yield break;

        dialogueBox.position = new Vector3(dialogueBox.position.x, showDialoguePos.position.y, dialogueBox.position.z);

        Debug.Log("Showing dialogueBox");

        hidden = false;
        yield return null;
    }

    IEnumerator hideDialogueWindow()
    {
        if (hidden)
            yield break;

        dialogueBox.position = new Vector3(dialogueBox.position.x, hideDialoguePos.position.y, dialogueBox.position.z);

        Debug.Log("Hiding dialogueBox");

        hidden = true;
        yield return null;
    }

    IEnumerator showQuestWindow()
    {
        if (!hiddenQuest)
            yield break;

        questBox.position = new Vector3(questBox.position.x, showQuestPos.position.y, questBox.position.z);

        Debug.Log("Showing quest window");

        hiddenQuest = false;
        yield return null;
    }

    IEnumerator hideQuestWindow()
    {
        if (hiddenQuest)
            yield break;

        questBox.position = new Vector3(questBox.position.x, hideQuestPos.position.y, questBox.position.z);

        Debug.Log("Hiding quest window");

        hiddenQuest = true;
        yield return null;
    }
}
