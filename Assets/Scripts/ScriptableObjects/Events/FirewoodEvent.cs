using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FirewoodEvent")]
public class FirewoodEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.CountDownValue = m_EventLength;
        GameManager.Instance.m_InterfaceHandler.m_QuestionText.text = "Add Wood to the Fire?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            GameManager.Instance.FirewoodStrengthValue = 3;
            GameManager.Instance.FirewoodValue--;
            GameManager.Instance.m_InterfaceHandler.m_FirewoodText.text = GameManager.Instance.FirewoodValue.ToString() + "x";
            GameManager.Instance.SetNewTemperature();
        }
    }
}
