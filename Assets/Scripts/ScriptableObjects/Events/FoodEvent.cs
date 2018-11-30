using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/FoodEvent")]
public class FoodEvent : Event {

	public override void Execute(List<List<string>> dividedList, CharacterManager characterV)
    {
        if (dividedList[0].Count > dividedList[1].Count)
        {
            characterV.AddFull();
            GameManager.Instance.m_FoodValue--;
            GameManager.Instance.m_FoodText.text = GameManager.Instance.m_FoodValue.ToString() + "x";
        }
    }
}
