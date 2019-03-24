using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/MerchantScenario")]
public class MerchantScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		yield return base.ExecuteScenario();
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		yield return new WaitForSeconds(2f);
		
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Es klopft an der Tür."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Du gehst zum Guckloch und siehst eine merkwürdig geformte, riesige Gestalt."
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Hi, Hi, Hi.. ich bin Halbert der legendäre Händler. Ich komme von weit her, oh yeah.'"
		));

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Meine Reise treibt mich übers kaputte Land und dabei reich ich Leuten meine Hand!'"
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Ich kaufe und verkaufe Ware, aber nur für das Bare!'"
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Während der Händler weiterspricht, merkst du, dass die riesige Gestalt eigentlich ein kleiner Mann mit einem riesigen Rucksack ist."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Und trotz seiner komischen Art, zeigt ein Blick auf seine Kleidung und auf sein Gesicht, dass seine Reise wohl keine leichte war."
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"'Naja, ich komme mal zum Punkt. Kann ich von dir etwas Proviant erwirtschaften?'"
		));
		
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
			"Da Geld jedoch kein valides Konzept mehr ist, möchte er tauschen. \r\n\r\n" + GameManager.Instance.FoodValue/2 + " Rationen von deinem Proviant gegen eine Stange Zigaretten?"
		));
		
		//GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.interfaceHandler.DisableBigPanel());

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Der Tausch ist erfolgreich und das Haus erhält Zigaretten und gibt etwas von seinen Rationen ab."
				));
				
				//GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				//	""
				//);
				GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
				GameManager.Instance.itemHandler.InstantiateNewItem("Zigaretten");

				//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				//		""
				//	));
			}
			else
			{
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Der Tausch ist erfolgreich und das Haus erhält Zigaretten."
				));
				
				GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
				//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				//	""
				//));
				
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Nachdem du jedoch die erste Schachtel öffnest, fällt dir auf, dass diese mit alten Salzstangen gefüllt ist.. und alle anderen auch!"
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"Bevor du dich dem Händler zuwenden kannst, läuft der schon schneller weg als du ihn jemals einholen könntest."
				));
				
				yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
					"'Hihihihi, du kriegst mich nie, du Trottel!'"
				));
				
				//yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				//	""
				//));
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 0.5f;
				}

				//yield return GameManager.Instance.CoroutineCaller(
				//	GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(""
				//	));
			}
		}
		else
		{
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Mit den Worten 'Alles klar, du Sackgesicht' dreht sich der Händler um und geht weg..."
			));
			
			yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.scenarioManager.scenarioTextTyper.TypeRoutine(
				"Achja, und er zeigt dir noch den Mittelfinger."
			));
		}

        wasUsed = true;
	}
}
