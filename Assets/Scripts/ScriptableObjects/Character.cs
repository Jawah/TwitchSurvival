using UnityEngine;

[CreateAssetMenu]
public class Character : ResettableScriptableObject
{
    public string m_CharacterName;

    public float m_DailyMoraleLossFactor;
    public float m_DailyFullLossFactor;
    public float m_DailyWarmthLossFactor;

    public bool m_InUse = false;

    public override void Reset()
    {
        m_InUse = false;
    }
}
