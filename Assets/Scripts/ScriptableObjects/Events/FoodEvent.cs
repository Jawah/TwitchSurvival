﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FoodEvent")]
public class FoodEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = eventLength;
        GameManager.Instance.interfaceHandler.questionText.text = "Should " + GameManager.Instance.characterHandler.currentCharacter.characterName + " get something to eat?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
            {
                character.AddFull();
            }
            GameManager.Instance.FoodValue--;
            GameManager.Instance.interfaceHandler.foodText.text = GameManager.Instance.FoodValue.ToString() + "x";
        }
    }
}
