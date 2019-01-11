﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/MedPackEvent")]
public class MedPackEvent : Event {
    
    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = eventLength;
        GameManager.Instance.interfaceHandler.questionText.text = "Should " + GameManager.Instance.characterHandler.currentCharacter.characterName + " get a MedPack?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > 0 || dividedList[1].Count > 0)
        {
            if (dividedList[0].Count > dividedList[1].Count)
            {
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[0][0];
                characterV.healthState = CharacterManager.HealthState.Default;
                characterV.StatusChanger();
                GameManager.Instance.MedPackValue--;
                GameManager.Instance.interfaceHandler.medPackText.text = GameManager.Instance.MedPackValue.ToString() + "x";
            }
            else
            {
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[1][0];
            }   
        }
        else
        {
            GameManager.Instance.pollHandler.chosenAnswer = "nothing";
        }
    }
}
