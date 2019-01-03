using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class Item : ResettableScriptableObject
{
    public string itemName;
    public string itemDescription;
    public string itemShortDescription;

    public float moraleFactorChangeValue;
    public float fullFactorChangeValue;
    public float warmthFactorChangeValue;

    public Sprite icon;

    public bool inUse;

    public override void Reset()
    {
        inUse = false;
    }
}