﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class TableOfPoints : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{ kills = 7, name = "Dani", powerups = 2, time = 1},
            new HighscoreEntry{ kills = 13, name = "Alex", powerups = 3, time = 0},
            new HighscoreEntry{ kills = 9, name = "Kegen", powerups = 4, time = 3},
            new HighscoreEntry{ kills = 11, name = "Phillip", powerups = 5, time = 2},
        };

        //Sort entry list by Score
        // Players are now sorted bei highet to Lowest points
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].kills > highscoreEntryList[i].kills)
                {
                    // Swap
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("PosText").GetComponent<Text>().text = rankString;

        int kills = highscoreEntry.kills; //int score = Random.Range(0, 100);
        entryTransform.Find("WonPointsText").GetComponent<Text>().text = kills.ToString();

        string name = highscoreEntry.name; //string name = "Daniyal";
        entryTransform.Find("PlayerText").GetComponent<Text>().text = name;

        int powerups = highscoreEntry.powerups;
        entryTransform.Find("ActivePowerupsText").GetComponent<Text>().text = powerups.ToString();
        
        int deaths = highscoreEntry.time;
        entryTransform.Find("TimeText").GetComponent<Text>().text = deaths.ToString();

        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Highlight first Rank
        if (rank == 1)
        {
            entryTransform.Find("PosText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("PlayerText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("WonPointsText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("ActivePowerupsText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("TimeText").GetComponent<Text>().color = Color.green;
        }

        // Highlight last Rank
        if (rank == 4)
        {
            entryTransform.Find("PosText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("PlayerText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("WonPointsText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("ActivePowerupsText").GetComponent<Text>().color = Color.red;
            entryTransform.Find("TimeText").GetComponent<Text>().color = Color.red;

            entryTransform.Find("Skull").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFFFFF");

            entryTransform.Find("Trophy").gameObject.SetActive(false);
        } 
        else
        {
            entryTransform.Find("Skull").gameObject.SetActive(false);
        }

        // Set Trophy for 1ST, 2ND and 3RD Rank
        switch (rank)
        {
            case 1:
                entryTransform.Find("Trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
                break;
            case 2:
                entryTransform.Find("Trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
                break;
            case 3:
                entryTransform.Find("Trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
                break;
            default:
                entryTransform.Find("Trophy").gameObject.SetActive(false);
                break;
        }

        transformList.Add(entryTransform);
    }

    private class HighscoreEntry
    {
        public int kills;
        public string name;
        public int powerups;
        public int time;
    }

}
