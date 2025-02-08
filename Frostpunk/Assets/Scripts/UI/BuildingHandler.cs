using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : UIHandlerBase, IInit
{
    public static BuildingHandler Instance;

    [SerializeField] private List<BuildingCost> _buildingsCost;
    
    public event Action<BuildingType> OnBuildingSelected; 
    
    private void Awake()
    {
        Instance = this;
    }

    public override void Init()
    {
        InGameTimeManager.Instance.OnTimeStep += CheckCostRequirement;
    }

    public void SelectBuildingTypeByIndex(int index)
    {
        if (_buildingsCost.Count <= index) return;

        BuildingType[] values = (BuildingType[])Enum.GetValues(typeof(BuildingType));
        if (values.Length <= index) return;
        OnBuildingSelected?.Invoke(values[index]);
    }

    private void CheckCostRequirement()
    {
        float coal, wood, steel;
        coal = ResourceHandler.Instance.CoalAmount;
        wood = ResourceHandler.Instance.WoodAmount;
        steel = ResourceHandler.Instance.SteelAmount;

        bool isFit;
        foreach (var building in _buildingsCost)
        {
            isFit = true;
            if (building.coal > coal || building.wood > wood || building.steel > steel)
            {
                isFit = false;
            }
            
            building.button.interactable = isFit;
        }
    }
    
}

[System.Serializable]
public struct BuildingCost
{
    public BuildingType type;
    public Button button;
    public int coal;
    public int wood;
    public int steel;
}