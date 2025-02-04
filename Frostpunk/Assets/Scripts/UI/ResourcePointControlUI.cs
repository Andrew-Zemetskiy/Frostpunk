using System;
using UnityEngine;

public class ResourcePointControlUI : UIHandlerBase, IInit
{
    [SerializeField] private GameObject _resourcePointUI;

    private ExtractionStructure _structureBase;
    
    public override void Init()
    {
        BaseControlSystem.Instance.OnObjectSelected += OnObjectSelected;
        BaseControlSystem.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDestroy()
    {
        BaseControlSystem.Instance.OnObjectSelected -= OnObjectSelected;
        BaseControlSystem.Instance.OnObjectDeselected -= OnObjectDeselected;
    }

    private void OnObjectSelected(ExtractionStructure structure)
    {
        structure.PrintHehe();
        _resourcePointUI.SetActive(true);
    }
    
    private void OnObjectDeselected()
    {
        _resourcePointUI.SetActive(false);
    }
}
