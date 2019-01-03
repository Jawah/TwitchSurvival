using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class Character : ResettableScriptableObject
{
    public string characterName;

    public Sprite characterSprite;

    public float dailyMoraleLossFactor;
    public float dailyFullLossFactor;
    public float dailyWarmthLossFactor;

    public float fullGainValue;
    public float warmthGainValue;

    public bool inUse;

    public override void Reset()
    {
        inUse = false;
    }
}
