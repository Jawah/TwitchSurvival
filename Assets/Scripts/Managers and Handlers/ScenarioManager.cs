using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
	public List<Scenario> allScenarios = new List<Scenario>();

	public TypingText scenarioTextTyper;
	
	public IEnumerator StartScenarioRoutine()
	{
		Scenario tempScenario = allScenarios[Random.Range(0,allScenarios.Count)];
		//Scenario tempScenario = allScenarios[1];
		yield return StartCoroutine(tempScenario.ExecuteScenario());
	}
}
