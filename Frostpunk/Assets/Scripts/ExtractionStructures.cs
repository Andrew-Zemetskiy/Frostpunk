using UnityEngine;

public class ExtractionStructure : SelectableStructureBase
{
    public ResourceExtractionStructuresSO structureConfig;

    private float _resourceAmount;
    private float _miningSpeedPerPerson;
    private int _currentWorkersAmount;

    public float ResourceAmount
    {
        get => _resourceAmount;
        set => _resourceAmount = (_resourceAmount - value < 0) ? 0 : _resourceAmount - value;
    }

    public float MiningSpeedPerPerson
    {
        get => _miningSpeedPerPerson;
    }

    public int CurrentWorkersAmount
    {
        get => _currentWorkersAmount;
        set => _currentWorkersAmount = Mathf.Clamp(value, 0, structureConfig.peopleCapacity);
    }

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        _resourceAmount = Random.Range(structureConfig.minResourceAmount, structureConfig.maxResourceAmount);
        _miningSpeedPerPerson = structureConfig.miningSpeedPerPerson;
    }
}