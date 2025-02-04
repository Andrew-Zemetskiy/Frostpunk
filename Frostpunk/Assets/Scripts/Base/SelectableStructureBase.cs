using UnityEngine;

public abstract class SelectableStructureBase : MonoBehaviour
{
    [SerializeField] DestinationPointBaseSO structureConfig;
    
    public string StructureName => name;
    public Transform Prefab => prefab;
    public Sprite ObjectBackgroundImage => objectBackgroundImage;
    public int Width => width;
    public int Height => height;
    
    private string structureName;
    private Transform prefab;
    private Sprite objectBackgroundImage;
    private int width;
    private int height;

    private void Awake()
    {
        name = structureConfig.structureName;
        prefab = structureConfig.prefab;
        objectBackgroundImage = structureConfig.destinationBackgroundImage;
        width = structureConfig.width;
        height = structureConfig.height;
    }
}
