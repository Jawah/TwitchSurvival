using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/StormyScenario")]
public class StormyScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		yield return base.ExecuteScenario();
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		yield return new WaitForSeconds(2f);
		
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Draußen donnert und schüttet es."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Das Haus hat es schwer mit dem Wetter mitzuhalten. Das Holz knarrt und aus kleinen Rissen tropft es in's Gebäude."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Plötzlich bricht jedoch ein Balken aus dem Dach und schüttet mitten zwischen die provisorisch eingerichteten Betten."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Man könnte von außen auf das Dach klettern und das Loch wieder zunageln, aber das könnte gefährlich werden.\r\n\r\nEs so zu lassen, klingt aber auch nicht nach einer guten Idee."
		));
		
		//GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.interfaceHandler.DisableBigPanel());

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			CharacterManager tempCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
		
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				tempCharacter.characterName + " macht sich auch sofort mit Hammer, Nägeln und Holz auf den Weg."
			));

			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Ohne große Probleme schafft es " + tempCharacter.characterName + " das Loch zuzunageln. \r\nZum Glück wurde auch so schnell gehandelt, dass der Wasserschaden auch nur minimal ist."
				));
				
				
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Ohne große Probleme schafft es " + tempCharacter.characterName + " das Loch zuzunageln, rutscht jedoch mit dem letzten Schlag ab und rutscht das Dach herunter."
				));

				tempCharacter.healthState = CharacterManager.HealthState.Fracture;
				tempCharacter.StatusChanger();
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Das Bein ist gebrochen, aber das Dach ist zu. Der Wasserschaden wurde durch das schnelle Handeln auch minimal gehalten."
				));
			}
		}
		else
		{
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Provisorisch hingestellte Eimer sorgen für das Nötigste. Jedoch ist das Feuerholz völlig durchnässt und unbrauchbar geworden."
			));
			
			GameManager.Instance.FirewoodValue = 0;
			
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Den Bewohnern ist kalt und der Schlaf wird durch die nassen Matratzen auch weniger erholsam."
			));

			foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
			{
				character.WarmthValue -= 2f;
			}

			//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			//		""
			//	));
		}
        wasUsed = true;
	}
}
