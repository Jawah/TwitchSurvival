using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
	public List<Scenario> allScenarios = new List<Scenario>();

	public IEnumerator StartScenarioRoutine()
	{
		Scenario tempScenario = allScenarios[Random.Range(0, allScenarios.Count)];
		//yield return tempScenario.ExecuteScenario();
		yield return StartCoroutine(tempScenario.ExecuteScenario());
	}
}
