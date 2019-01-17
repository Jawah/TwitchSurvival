using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/StormyScenario")]
public class StormyScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Draußen donnert und schüttet es."
		);
		GameManager.Instance.CountDownValue = 5;
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return new WaitForSeconds(5);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Das Haus hat es schwer mit dem Wetter mitzuhalten. Das Holz knarrt und aus kleinen Rissen tropft es in's Gebäude."
		);
		GameManager.Instance.CountDownValue = 9;
		yield return new WaitForSeconds(9);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Plötzlich bricht jedoch ein Balken aus dem Dach und schüttet mitten zwischen die provisorisch eingerichteten Betten."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Man könnte von außen auf das Dach klettern und das Loch wieder zunageln, aber das könnte gefährlich werden.\r\n\r\nEs so zu lassen, klingt aber auch nicht nach einer guten Idee."
		);
		GameManager.Instance.CountDownValue = 14;
		yield return new WaitForSeconds(14);
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			CharacterManager tempCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
		
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				tempCharacter.characterName + " meldet sich freiwillig und macht sich auch sofort mit Hammer und Holz auf den Weg."
			);
			GameManager.Instance.CountDownValue = 14;
			yield return new WaitForSeconds(14);

			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Ohne große Probleme schafft es " + tempCharacter.characterName + " das Loch zuzunageln. \r\nZum Glück wurde auch so schnell gehandelt, dass der Wasserschaden auch nur minimal ist."
				);
				GameManager.Instance.CountDownValue = 13;
				yield return new WaitForSeconds(13);
				
				
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Ohne große Probleme schafft es " + tempCharacter.characterName + " das Loch zuzunageln, rutscht jedoch mit dem letzten Schlag ab, rutscht das Dach herunter."
				);
				GameManager.Instance.CountDownValue = 12;
				yield return new WaitForSeconds(12);

				tempCharacter.healthState = CharacterManager.HealthState.Fracture;
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Das Bein ist gebrochen, aber das Dach ist zu. Der Wasserschaden wurde durch das schnelle Handeln auch minimal gehalten."
				);
				GameManager.Instance.CountDownValue = 12;
				yield return new WaitForSeconds(12);
			}
		}
		else
		{
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Provisorisch hingestellte Eimer sorgen für das Nötigste. Jedoch ist das Feuerholz völlig durchnässt und unbrauchbar geworden."
			);
			GameManager.Instance.FirewoodValue = 0;
			GameManager.Instance.CountDownValue = 12;
			yield return new WaitForSeconds(12);
			
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Den Bewohnern ist kalt und der Schlaf wird durch die nassen Matratzen auch weniger erholsam."
			);

			foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
			{
				character.WarmthValue -= 2f;
			}
			
			GameManager.Instance.CountDownValue = 12;
			yield return new WaitForSeconds(12);
		}
	}
}
