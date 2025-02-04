using UnityEngine;

[CreateAssetMenu(fileName = "ResourceExtractionStructuresSO", menuName = "Scriptable Objects/ResourceExtractionStructuresSO")]
public class ResourceExtractionStructures : DestinationPointBaseSO
{
    [Header("Specific")]
    public ResourceType resourceType;
    
    public int minResourceAmount;
    public int maxResourceAmount;
    public float miningSpeedPerPerson;
}
