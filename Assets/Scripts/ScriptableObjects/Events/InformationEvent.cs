using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InformationEvent : Event
{
    [TextArea]
    public string m_DescriptionText;
    public float m_EventLength;
}
