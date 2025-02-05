using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResourcePointControlUI : UIHandlerBase, IInit
{
    [Header("MainObject")] [SerializeField]
    private GameObject _resourcePointUI;

    [Header("InnerObjects")] [Header("StaticObjects")] [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _objectPointImage;
    [SerializeField] private Image _miningResourceImage;

    [Header("DynamicObjects")] [SerializeField]
    private Slider _temperatureSlider;

    [SerializeField] private TextMeshProUGUI _workingTimeText;
    [SerializeField] private TextMeshProUGUI _resourceAmountText;
    [SerializeField] private TextMeshProUGUI _miningSpeedText;
    [SerializeField] private TextMeshProUGUI _peopleCapacityText;
    [SerializeField] private TextMeshProUGUI _availableWorkersText;

    [Header("Buttons")] 
    [SerializeField] private Button _noneAmountBtn;
    [SerializeField] private Button _maxAmountBtn;
    [SerializeField] private Button _minusOneAmountBtn;
    [SerializeField] private Button _plusOneAmountBtn;
    
    
    private ExtractionStructure _structureBase;
    
    private float _currentMiningSpeed;
    private int _currentPeopleAmount;
    private float _miningSpeedPerPerson;
    private int _peopleCapacity;

    public override void Init()
    {
        BaseControlSystem.Instance.OnObjectSelected += OnObjectSelected;
        BaseControlSystem.Instance.OnObjectDeselected += OnObjectDeselected;

        InitButtons();
    }

    private void OnDestroy()
    {
        BaseControlSystem.Instance.OnObjectSelected -= OnObjectSelected;
        BaseControlSystem.Instance.OnObjectDeselected -= OnObjectDeselected;
        
        _noneAmountBtn.onClick.RemoveAllListeners();
        _maxAmountBtn.onClick.RemoveAllListeners();
        _minusOneAmountBtn.onClick.RemoveAllListeners();
        _plusOneAmountBtn.onClick.RemoveAllListeners();
    }

    private void InitButtons()
    {
        _noneAmountBtn.onClick.AddListener(() => RemovePeople(_currentPeopleAmount));
        _maxAmountBtn.onClick.AddListener(() => AddPeople(_peopleCapacity - _currentPeopleAmount));
        _minusOneAmountBtn.onClick.AddListener(() => RemovePeople(1));
        _plusOneAmountBtn.onClick.AddListener(() => AddPeople(1));
    }
    
    private void OnObjectSelected(ExtractionStructure structure)
    {
        _structureBase = structure;
        UpdateDataStaticData();

        _resourcePointUI.SetActive(true);
    }

    private void OnObjectDeselected()
    {
        _resourcePointUI.SetActive(false);
    }

    private void UpdateDataStaticData()
    {
        var config = _structureBase.structureConfig;

        _nameText.text = config.structureName;
        _backgroundImage.sprite = config.destinationBackgroundImage;
        _objectPointImage.sprite = config.destinationPointImage;

        _miningResourceImage.sprite = config.miningResourceImage;

        //fields
        _peopleCapacity = config.peopleCapacity;
        _miningSpeedPerPerson = _structureBase.MiningSpeedPerPerson;
        
        UpdateDynamicData();
    }

    private void UpdateDynamicData()
    {
        _currentPeopleAmount = _structureBase.CurrentWorkersAmount;
        
        _temperatureSlider.value = 0.5f; //Set clamp for next data of temperature
        _workingTimeText.text = "08:00 18:00";
        _resourceAmountText.text = Mathf.CeilToInt(_structureBase.ResourceAmount).ToString();
        _miningSpeedText.text = MathF.Round(_miningSpeedPerPerson * _currentPeopleAmount, 1).ToString(CultureInfo.CurrentCulture) + " / h";
        _peopleCapacityText.text = $"{_currentPeopleAmount} / {_peopleCapacity}";
        _availableWorkersText.text = $"{BaseControlSystem.Instance.GetVacantsCount()} available";
    }

    private void AddPeople(int amount)
    {
        _structureBase.AddWorkers(amount);
        UpdateDynamicData();
    }

    private void RemovePeople(int amount)
    {
        _structureBase.RemoveWorkers(amount);
        UpdateDynamicData();
    }
}