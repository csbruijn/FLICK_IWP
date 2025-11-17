using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOutComeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GameStateText, outcomeName, outcomeDescr,remainingtime;
    [SerializeField] private Image outcomeImage;

    [SerializeField] private List<Outcomes> outcomes = new List<Outcomes>();

    private void Awake()
    {
        PlayData playData = LevelsManager.instance.playData; 

        if (playData.GameWon)
        {
            GameStateText.text = "Game won!";
            remainingtime.text = "Time remaining: " + Mathf.RoundToInt(playData.timeToCompletion).ToString();
        }
        else
        {
            GameStateText.text = "Game lost";
            remainingtime.text = ""; 
        }

        Outcomes outcome = outcomes.Find(o => o.OutcomeRef == playData.outcome);

        if (outcome != null)
        {
            outcomeName.text = "Congratulations, you're " + outcome.myName + "!";
            outcomeDescr.text = outcome.myDescription;
            outcomeImage.sprite = outcome.mySprite;
        }

    }

}
