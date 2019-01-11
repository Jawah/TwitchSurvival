using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatText : ScriptableObject
{
	[TextArea]
	[SerializeField] string text;

	[SerializeField] int playTime;
	
	
	public void Execute()
	{
		GameManager.Instance.CoroutineCaller(ExecutionCoroutine());
	}

	public IEnumerator ExecutionCoroutine()
	{
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(true);
		GameManager.Instance.interfaceHandler.storyText.text = text;
		yield return new WaitForSeconds(playTime);
		GameManager.Instance.interfaceHandler.storyPanel.SetActive(false);
	}
}
