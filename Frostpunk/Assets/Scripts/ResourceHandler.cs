using System;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public event Action OnResourceChanged;
    
    public static ResourceHandler Instance;

    private float _coalAmount = 20;
    private float _woodAmount = 5;
    private float _steelAmount = 0;
    
    public float CoalAmount
    {
        get { return _coalAmount; }
        set { _coalAmount = value; }
    }

    public float WoodAmount
    {
        get { return _woodAmount; }
        set { _woodAmount = value; }
    }

    public float SteelAmount
    {
        get { return _steelAmount; }
        set { _steelAmount = value; }
    }
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddResource(ResourceType resourceType, float amount)
    {
        switch (resourceType)
        {
            case ResourceType.Coal:
                _coalAmount += amount;
                break;
            case ResourceType.Wood:
                _woodAmount += amount;
                break;
            case ResourceType.Steel:
                _steelAmount += amount;
                break;
        }
        OnResourceChanged?.Invoke();
    }
}
