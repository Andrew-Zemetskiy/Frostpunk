using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExtractionStructure : SelectableStructureBase
{
    private Dictionary<int, NavMeshAgent> _workersDictionary = new Dictionary<int, NavMeshAgent>();
    
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
        private set => _currentWorkersAmount = value;
        //Mathf.Clamp(value, 0, structureConfig.peopleCapacity);
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