using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/MerchantScenario")]
public class MerchantScenario : Scenario {

	public override IEnumerator ExecuteScenario()
	{
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Es klopft an die Tür."
		);
		GameManager.Instance.CountDownValue = 5;
		GameManager.Instance.audioManager.PlayEffect(audioClips[0]);
		yield return new WaitForSeconds(5);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Du gehst zum Guckloch und siehst eine merkwürdig geformte, riesige Gestalt."
		);
		GameManager.Instance.CountDownValue = 7;
		yield return new WaitForSeconds(7);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Hi, Hi, Hi.. ich bin Halbert der legendäre Händler. Ich komme von weit her, oh yeah.'"
		);
		GameManager.Instance.CountDownValue = 8;
		yield return new WaitForSeconds(8);

		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Meine Reise treibt mich über's kaputte Land und dabei reich ich Leuten meine Hand!'"
		);
		GameManager.Instance.CountDownValue = 8;
		yield return new WaitForSeconds(8);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Ich kaufe und verkaufe Ware, aber nur für das Bare!'"
		);
		GameManager.Instance.CountDownValue = 6;
		yield return new WaitForSeconds(6);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Während der Händler weiterspricht, merkst du, dass die riesige Gestalt eigentlich ein kleiner Mann mit einem riesigen Rucksack ist."
		);
		GameManager.Instance.CountDownValue = 12;
		yield return new WaitForSeconds(12);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Und trotz seiner komischen Art, zeigt ein Blick auf seine Kleidung und auf sein Gesicht, dass seine Reise wohl keine leichte war."
		);
		GameManager.Instance.CountDownValue = 11;
		yield return new WaitForSeconds(11);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"'Naja, ich kommem mal zum Punkt. Kann ich von dir etwas Proviant erwirtschaften?'"
		);
		GameManager.Instance.CountDownValue = 8;
		yield return new WaitForSeconds(8);
		
		GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
			"Da Geld jedoch kein valides Konzept mehr ist, möchte er tauschen. \r\n\r\n" + GameManager.Instance.FoodValue/2 + " Rationen von deinem Proviant gegen eine Stange Zigaretten?"
		);
		GameManager.Instance.CountDownValue = 13;
		yield return new WaitForSeconds(13);
		
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);

		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.pollHandler.DoPoll(scenarioEvents[0]));
		yield return GameManager.Instance.CoroutineCaller(GameManager.Instance.AfterQuestion());

		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);

		if (GameManager.Instance.pollHandler.chosenAnswer == scenarioEvents[0].possibleAnswers[0]) //yes
		{
			int rnd = Random.Range(0, 10);

			if (rnd <= 4)
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Der Tausch ist erfolgreich und das Haus erhält Zigaretten und gibt etwas von seinen Rationen ab."
				);
				GameManager.Instance.CountDownValue = 8;
				yield return new WaitForSeconds(8);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
				GameManager.Instance.itemHandler.InstantiateNewItem("Cigarettes");
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);	
			}
			else
			{
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Der Tausch ist erfolgreich und das Haus erhält Zigaretten."
				);
				GameManager.Instance.CountDownValue = 6;
				yield return new WaitForSeconds(6);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				GameManager.Instance.FoodValue -= GameManager.Instance.FoodValue / 2;
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Nachdem du jedoch die erste Schachtel öffnest, fällt dir auf, dass diese mit alten Salzstangen gefüllt ist.. und alle anderen auch!"
				);
				GameManager.Instance.CountDownValue = 11;
				yield return new WaitForSeconds(11);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"Bevor du dich dem Händler zuwenden kannst, läuft der schon schneller weg als du ihn jemals einholen könntest."
				);
				GameManager.Instance.CountDownValue = 10;
				yield return new WaitForSeconds(10);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					"'Hihihihi, du kriegst mich nie, du Trottel!'"
				);
				GameManager.Instance.CountDownValue = 6;
				yield return new WaitForSeconds(6);
				
				GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
					""
				);
				
				foreach (CharacterManager character in GameManager.Instance.characterHandler.activeCharacters)
				{
					character.MoraleValue -= 0.5f;
				}
				
				GameManager.Instance.CountDownValue = 3;
				yield return new WaitForSeconds(3);
			}
		}
		else
		{
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Mit den Worten 'Alles klar, du Sackgesicht' dreht sich der Händler um und geht weg..."
			);
			GameManager.Instance.CountDownValue = 7;
			yield return new WaitForSeconds(7);
			
			GameManager.Instance.scenarioManager.scenarioTextTyper.Type(
				"Achja, und er zeigt dir noch den Mittelfinger."
			);
			GameManager.Instance.CountDownValue = 4;
			yield return new WaitForSeconds(4);
		}
	}
}
