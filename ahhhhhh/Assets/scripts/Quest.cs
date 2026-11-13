using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;


    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + System.Guid.NewGuid().ToString();
        }
    }

 
}
[System.Serializable]

public class QuestObjective
{
    public string objectiveID;
    public string description;
    public ObjectiveType type;
    public int requiredAmount;
    public int currentAmount;

    public bool IsCompleted() => currentAmount >= requiredAmount;
}

public enum ObjectiveType { CollectItem, Explore }

[System.Serializable]

public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();
        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool IsCompleted() => objectives.TrueForAll(o => o.IsCompleted());

    public string QuestID => quest.questID;
}
public class QuestManager : MonoBehaviour
{
    public List<QuestProgress> activeQuests = new List<QuestProgress>();
    public void StartQuest(Quest quest)
    {
        if (activeQuests.Exists(q => q.QuestID == quest.questID))
        {
            Debug.LogWarning($"Quest {quest.questName} is already active.");
            return;
        }
        QuestProgress newQuestProgress = new QuestProgress(quest);
        activeQuests.Add(newQuestProgress);
        Debug.Log($"Started quest: {quest.questName}");
    }
    public void UpdateObjective(string questID, string objectiveID, int amount)
    {
        QuestProgress questProgress = activeQuests.Find(q => q.QuestID == questID);
        if (questProgress == null)
        {
            Debug.LogWarning($"Quest with ID {questID} not found.");
            return;
        }
        QuestObjective objective = questProgress.objectives.Find(o => o.objectiveID == objectiveID);
        if (objective == null)
        {
            Debug.LogWarning($"Objective with ID {objectiveID} not found in quest {questID}.");
            return;
        }
        objective.currentAmount += amount;
        if (objective.currentAmount > objective.requiredAmount)
        {
            objective.currentAmount = objective.requiredAmount;
        }
        Debug.Log($"Updated objective: {objective.description}, Current Amount: {objective.currentAmount}/{objective.requiredAmount}");
        if (questProgress.IsCompleted())
        {
            Debug.Log($"Quest {questProgress.quest.questName} completed!");
            // Handle quest completion logic here
        }
    }
}