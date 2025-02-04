using UnityEngine;

[CreateAssetMenu(fileName = "ResourceExtractionStructuresSO", menuName = "Scriptable Objects/ResourceExtractionStructuresSO")]
public class ResourceExtractionStructuresSO : DestinationPointBaseSO
{
    [Header("Specific")]
    public ResourceType resourceType;
    
    public int minResourceAmount;
    public int maxResourceAmount;
}
