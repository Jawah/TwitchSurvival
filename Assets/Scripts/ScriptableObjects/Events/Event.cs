using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : ScriptableObject
{
    public float m_EventLength;

    [TextArea]
    public string m_EventDescription;

    public List<string> m_PossibleAnswers = new List<string>();

    public virtual void Execute(List<List<string>> dividedList, CharacterManager characterV) { Debug.LogWarning("No override Execute function declared!"); }

    public virtual void Execute(List<List<string>> dividedList) { Debug.LogWarning("No override Execute function declared!"); }
}