using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOutComeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GameStateText, NotesCollected ,remainingtime;

    private void Awake()
    {
        PlayData playData = LevelsManager.instance.playData; 

        if (playData.GameWon)
        {
            GameStateText.text = "Game won!";
        }
        else
        {
            GameStateText.text = "Game lost";
        }
        
        remainingtime.text = 
            "Time survived: " + 
            Mathf.RoundToInt(playData.timeToCompletion).ToString();
        NotesCollected.text = 
            "Notes collected: " + 
            playData.NotesCollected.ToString();


        //Outcomes outcome = outcomes.Find(o => o.OutcomeRef == playData.outcome);

        //if (outcome != null)
        //{
        //    NotesCollected.text = "Congratulations, you're " + outcome.myName + "!";
        //    outcomeDescr.text = outcome.myDescription;
        //    outcomeImage.sprite = outcome.mySprite;
        //}

    }

}
