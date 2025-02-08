using System;
using UnityEngine;

public class BuildingHandler : UIHandlerBase, IInit
{
    public static BuildingHandler Instance;

    public event Action<BuildingType> OnBuildingSelected; 
    
    private void Awake()
    {
        Instance = this;
    }

    public override void Init()
    {
       
    }

    public void SelectBuildingTypeByIndex(int index)
    {
        BuildingType[] values = (BuildingType[])Enum.GetValues(typeof(BuildingType));
        if (values.Length <= index) return;
        OnBuildingSelected?.Invoke(values[index]);
    }
}
