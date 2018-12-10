using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FirewoodEvent")]
public class FirewoodEvent : Event {

    public override void Instantiate()
    {
        GameManager.Instance.m_CountDownValue = m_EventLength;
        GameManager.Instance.m_QuestionText.text = "Add Wood to the Fire?";

        base.Instantiate();
    }

    public override void Execute(List<List<string>> dividedList)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            GameManager.Instance.m_FireWoodStrength = 3;
            GameManager.Instance.m_FirewoodValue--;
            GameManager.Instance.m_FirewoodText.text = GameManager.Instance.m_FirewoodValue.ToString() + "x";
        }
    }
}
