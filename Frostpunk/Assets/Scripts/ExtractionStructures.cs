using System;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ExtractionStructure : SelectableStructureBase
{
    public event Action<ResourceType, float> OnResourceExtraction;
    
    private Dictionary<int, NavMeshAgent> _workersDictionary = new Dictionary<int, NavMeshAgent>();
    
    public ResourceExtractionStructuresSO structureConfig;
    
    private float _resourceAmount;
    private float _miningSpeedPerPerson;
    private int _currentWorkersAmount;
    
    private float _hourToTickRatio = 1f / 10f;

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
        private set => _currentWorkersAmount = value;
        //Mathf.Clamp(value, 0, structureConfig.peopleCapacity);
    }

    public bool IsHaveResources { get; private set; } = true;
    
    private void Awake()
    {
        InitData();
    }

    private void Start()
    {
        InGameTimeManager.Instance.OnTimeStep += OnTimeStep;
    }

    private void OnTimeStep()
    {
        if (_resourceAmount > 0)
        {
            var miningSpeed = CurrentWorkersAmount * MiningSpeedPerPerson;
            var miningPerTick = miningSpeed * _hourToTickRatio;
            
            if (_resourceAmount - miningPerTick <= 0)
            {
                miningPerTick = _resourceAmount;
                IsHaveResources = false;
            }
            _resourceAmount -= miningPerTick;
            // OnResourceExtraction?.Invoke(structureConfig.resourceType, miningPerTick);
            ResourceHandler.Instance.AddResource(structureConfig.resourceType, miningPerTick);
        }
    }
    
    private void InitData()
    {
        _resourceAmount = Random.Range(structureConfig.minResourceAmount, structureConfig.maxResourceAmount);
        _miningSpeedPerPerson = structureConfig.miningSpeedPerPerson;
    }

    public void AddWorkers(int amount)
    {
        if (CurrentWorkersAmount + amount <= structureConfig.peopleCapacity)
        {
            BaseControlSystem.Instance.AppointWorkersToMining(ref _workersDictionary, amount, out int appointedWorkers, transform);
            CurrentWorkersAmount += appointedWorkers;
        }
    }

    public void RemoveWorkers(int amount)
    {
        if (CurrentWorkersAmount - amount >= 0)
        {
            BaseControlSystem.Instance.RemoveWorkersFromProduction(ref _workersDictionary, amount, out int removedWorkers);
            CurrentWorkersAmount -= removedWorkers;
        }
    }
}