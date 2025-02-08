using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHandler : UIHandlerBase, IInit
{
    public static BuildingHandler Instance;

    [SerializeField] private List<BuildingCost> _buildingsCost;

    private int selectedBuilding;
    
    public event Action<BuildingType> OnBuildingSelected; 
    
    private void Awake()
    {
        Instance = this;
    }

    public override void Init()
    {
        InGameTimeManager.Instance.OnTimeStep += CheckCostRequirement;
        GridBuildingSystem.Instance.OnObjectPlaced += PayForBuilding;
    }

    private void OnDisable()
    {
        InGameTimeManager.Instance.OnTimeStep -= CheckCostRequirement;
        GridBuildingSystem.Instance.OnObjectPlaced -= PayForBuilding;
    }

    public void SelectBuildingTypeByIndex(int index)
    {
        if (_buildingsCost.Count <= index) return;

        selectedBuilding = index;
        
        BuildingType[] values = (BuildingType[])Enum.GetValues(typeof(BuildingType));
        if (values.Length <= index) return;
        OnBuildingSelected?.Invoke(values[index]);
    }

    private void PayForBuilding()
    {
        ResourceHandler.Instance.CoalAmount -= _buildingsCost[selectedBuilding].coal;
        ResourceHandler.Instance.WoodAmount -= _buildingsCost[selectedBuilding].wood;
        ResourceHandler.Instance.SteelAmount -= _buildingsCost[selectedBuilding].steel;
    }
    
    private void CheckCostRequirement()
    {
        float coal = ResourceHandler.Instance.CoalAmount;
        float wood = ResourceHandler.Instance.WoodAmount;
        float steel = ResourceHandler.Instance.SteelAmount;
        
        foreach (var building in _buildingsCost)
        {
            if (building.coal > coal || building.wood > wood || building.steel > steel)
            {
                building.button.interactable = false;
                continue;
            }
            building.button.interactable = true;
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