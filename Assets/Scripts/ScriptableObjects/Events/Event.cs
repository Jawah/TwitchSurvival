using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : ScriptableObject
{
    public float m_EventLength;
    public string m_EventDescription;

    public List<string> m_PossibleAnswers = new List<string>();

    public abstract void Execute();
}
