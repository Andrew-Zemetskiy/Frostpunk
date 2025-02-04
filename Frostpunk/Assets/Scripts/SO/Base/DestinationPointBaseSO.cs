using UnityEngine;

[CreateAssetMenu(fileName = "DestinationPointBaseSO", menuName = "Scriptable Objects/Base/DestinationPointBaseSO")]
public class DestinationPointBaseSO : ScriptableObject
{
    [Header("Base")]
    public string structureName;
    public Transform prefab;
    public Sprite destinationBackgroundImage;
    public Sprite destinationPointImage;
    public Sprite miningResourceImage;
    public int baseHeatIndex;
    public int peopleCapacity;
    public float miningSpeedPerPerson;
    [Header("Size")] 
    public int width;
    public int height;
}
