﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/DisputeScenario")]
public class DisputeScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		yield return base.ExecuteScenario();
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		yield return new WaitForSeconds(2f);
		
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Ihr empfangt ein Funk-Signal."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Wir.. sind.. Militär.. Siedlung.. Kapinsky.. kommt..'"
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Kapinsky ist ein Ort in der Nähe. Und es klingt, als ob es das Militär geschafft hat, dort eine Siedlung aufzuschlagen."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Jedoch treiben sich in Kapinsky auch viele Banditen rum. Es könnte vielleicht auch eine Falle sein."
		));
		
		//GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.interfaceHandler.DisableBigPanel());

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0])
		{
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Jemand soll hingehen..."
			));

			CharacterManager tempCharacter = GameManager.Instance.characterHandler.RandomActiveCharacter();
			
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Und der 'Unglückliche' ist " + tempCharacter.characterName + ". \r\n\r\n'Falls ich in den nächsten 2 Stunden nicht zurück bin, rechnet nicht mit mir!'"
			));
			
			//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			//	""
			//));
			
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					tempCharacter.characterName + " kommt schnell mit guten Neuigkeiten zurück und zwar, dass in Kürze ein Trupp Special Forces kommen soll, welcher die Bewohner abholt."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Bis dahin sollen wir am Leben bleiben und abwarten!'"
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Das weckt in den Bewohnnern einen ungeahnten Lebenswillen."
				));
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue += 3f;
				}
				
				//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				//	""
				//));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Es vergehen mehrere Stunden und die Rückkehr von " + tempCharacter.characterName + " wird damit unwahrscheinlicher und unwahrscheinlicher..."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Wir sollten nicht auf " + tempCharacter.characterName + "'s Rückkehr hoffen.'"
				));
				
				GameManager.Instance.characterHandler.KillCharacter(tempCharacter.characterName);
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 2;
				}
				
				//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				//	""
				//));
			}
		}

		else
		{
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Es ist leider ein zu großes Risiko."
			));
			
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Trotz dieser Tatsache hängt die Frage im Raum, was hätte werden können..."
			));
			
			foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
			{
				character.MoraleValue -= 1.5f;
			}
			
			//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			//	""
			//));
		}
        wasUsed = true;
		yield return null;
	}
}
