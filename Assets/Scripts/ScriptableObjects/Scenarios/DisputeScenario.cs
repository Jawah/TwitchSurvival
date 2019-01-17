using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/DisputeScenario")]
public class DisputeScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Das Radio empfängt ein Funk-Signal."
		);
		GameManager.Instance.CountDownValue = 10;
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Wir.. sind.. Militär.. Siedlung.. Kapinsky.. kommt..'"
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Kapinsky ist ein Ort in der Nähe. Und es klingt, als ob es das Militär geschafft hat, dort eine Siedlung aufzuschlagen."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Jedoch treiben sich in Kapinsky auch viele Banditen rum. Es könnte vielleicht auch eine Falle sein."
		);
		GameManager.Instance.CountDownValue = 10;
		yield return new WaitForSeconds(10);
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0])
		{
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Alle stimmen zu, aber niemand meldet sich freiwillig.\r\nEs soll gelost werden."
			);
			GameManager.Instance.CountDownValue = 10;
			yield return new WaitForSeconds(10);

			CharacterManager tempCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
			
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Und der Verlierer ist " + tempCharacter.characterName + ". \r\n'Falls ich in den nächsten 2 Stunden nicht zurück bin, rechnet nicht mit mir!'"
			);
			GameManager.Instance.CountDownValue = 10;
			yield return new WaitForSeconds(10);

			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				""
			);
			GameManager.Instance.CountDownValue = 5;
			yield return new WaitForSeconds(5);
			
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					tempCharacter.characterName + " kommt schnell mit guten Neuigkeiten zurück und zwar, dass in Kürze ein Trupp Special Forces kommt, welcher die Bewohner abholen soll."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Bis dahin sollen wir am Leben bleiben und abwarten!'"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Das weckt in den Bewohnnern einen ungeahnten Lebenswillen."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue += 2;
				}
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);	
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Es vergehen mehrere Stunden und die Rückkehr von " + tempCharacter.characterName + " damit unwahrscheinlicher und unwahrscheinlich.."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Wir sollten nicht auf " + tempCharacter.characterName + "'s Rückkehr hoffen.'"
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 2;
				}
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);	
			}
		}

		else
		{
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Es ist leider ein zu großes Risiko."
			);
			GameManager.Instance.CountDownValue = 10;
			yield return new WaitForSeconds(10);
			
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Trotz dieser Tatsache, hängt die Frage im Raum, was hätte werden können..."
			);
			GameManager.Instance.CountDownValue = 10;
			yield return new WaitForSeconds(10);
			
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				""
			);
			foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
			{
				character.MoraleValue -= 2;
			}
			GameManager.Instance.CountDownValue = 3;
			yield return new WaitForSeconds(3);	
		}
		
		yield return null;
	}
}
