using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvailableQuest : MonoBehaviour
{
    public QuestNodePanel panel;
    public QuestData questData;

    [SerializeField]
    TextMeshProUGUI questTitle;

    public void UpdateSelf()
    {
        questTitle.text = questData.questTitle;
    }

    public void ShowQuestStartUI()
    {
        panel.SetData(questData.rootNode);
        panel.Show();
    }
}
