using UnityEngine;

[CreateAssetMenu(fileName = "DestinationPointBaseSO", menuName = "Scriptable Objects/Base/DestinationPointBaseSO")]
public class DestinationPointBaseSO : ScriptableObject
{
    [Header("Base")]
    public string structureName;
    public Sprite destinationPointImage;
    public Sprite miningResourceImage;
    public int baseHeatIndex;
    public int peopleCapacity;
}
