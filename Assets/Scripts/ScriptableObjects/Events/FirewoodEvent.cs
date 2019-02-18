using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FirewoodEvent")]
public class FirewoodEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.interfaceHandler.questionText.text = "Holz in's Feuer werfen?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList)
    {
        if (dividedList[0].Count > 0 || dividedList[1].Count > 0)
        {
            if (dividedList[0].Count > dividedList[1].Count)
            {
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[0][0];
                GameManager.Instance.FirewoodStrengthValue = 3;
                GameManager.Instance.FirewoodValue--;
                GameManager.Instance.interfaceHandler.firewoodText.text = GameManager.Instance.FirewoodValue.ToString() + "x";
                GameManager.Instance.SetNewTemperature();
            }
            else
            {
                GameManager.Instance.pollHandler.chosenAnswer = dividedList[1][0];
            }
        }
        else
        {
            GameManager.Instance.pollHandler.chosenAnswer = "Keine Antworten";
        }
    }
}
