using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Dialogue
{
    public string textPL;
    public string textENG;
    public bool playerTalking = false;
}
public enum QuestStatus { Starting, Going, ItemObtained, Ended };

[System.Serializable]
public class Quest
{
    public string namePL;
    public string nameENG;

    public string descriptionPL;
    public string descriptionENG;
    public string descriptionAfterItemPL;
    public string descriptionAfterItemENG;

    [HideInInspector]
    public QuestStatus questStatus = QuestStatus.Starting;
    public GameObject item;

    public Dialogue[] startQuestDialogues;
    public Dialogue[] whileQuestDialogues;
    public Dialogue[] endQuestDialogues;
}

public class NPC : MonoBehaviour
{
    public string npcName = "xd";
    public Color npcDialogueColor = Color.white;

    public int questNumber = 0;
    public Quest[] quest;

    public Dialogue[] normalDialogues;

    private void Awake()
    {
        for (int i = 0; i < quest.Length; i++)
        {
            quest[i].item.SetActive(false);
        }
    }

    public void startQuest()
    {
        quest[questNumber].questStatus = QuestStatus.Going;
        DialogueManager.instance.startQuest(quest[questNumber]);
        quest[questNumber].item.SetActive(true);
    }

    public void itemObtained()
    {
        quest[questNumber].questStatus = QuestStatus.ItemObtained;
    }

    public void endQuest()
    {
        quest[questNumber].questStatus = QuestStatus.Ended;
        DialogueManager.instance.endQuest();
        questNumber++;

        if (questNumber==4)
        {
            SceneManager.LoadScene("finish");
        }
    }
}
