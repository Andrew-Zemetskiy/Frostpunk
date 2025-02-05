using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BaseControlSystem : MonoBehaviour
{
    public static BaseControlSystem Instance;

    public event Action<ExtractionStructure> OnObjectSelected;
    public event Action OnObjectDeselected;

    [Header("Residents")] 
    [SerializeField] private List<NavMeshAgent> _navMeshAgentList; //on object pool will be deleted 

    [SerializeField] private Transform _freeZone;
    private Dictionary<int, NavMeshAgent> _allResidentsDictionary = new Dictionary<int, NavMeshAgent>();
    private Dictionary<int, NavMeshAgent> _vacantResidentsDictionary = new Dictionary<int, NavMeshAgent>();
    private int _vacantResidentIndex = 0;
    
    
    [Header("Buildings")] [SerializeField] private List<Building> _buildingList;

    [Header("ExtractionPointsConfigs")] [SerializeField]
    private List<ResourceExtractionStructuresSO> _extractionStructuresConfigsList;

    private InputSystem _inputSystem;
    private Camera _camera;
    private float _baseSpeed;

    //For UI
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;
    private bool isResourceUIActive = false;

    private void Awake()
    {
        Instance = this;
        _inputSystem = new InputSystem();
    }

    private void Start()
    {
        _camera = Camera.main;
        FillResidentsDictionary();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Move.performed += OnMouseClick;
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Disable();
        _inputSystem.Player.Move.performed -= OnMouseClick;
    }

    private void OnMouseClick(InputAction.CallbackContext obj)
    {
        // Is click on UI
        if (IsPointerOverUI())
        {
            // Debug.Log("Clicked on UI, ignoring raycast.");
            return;
        }

        //Select building
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out SelectableStructureBase structure))
            {
                object inheritor = ObjectClarification(structure);
                if (inheritor != null)
                {
                    if (isResourceUIActive == false)
                    {
                        SetEnableToGO(true, (ExtractionStructure)inheritor);
                    }
                }
            }
            else if (isResourceUIActive == true)
            {
                SetEnableToGO(false);
            }
        }
    }

    private object ObjectClarification(SelectableStructureBase structure)
    {
        if (structure is ExtractionStructure extractionStructure)
        {
            return extractionStructure;
        }
        return null;
    }

    private void FillResidentsDictionary()
    {
        foreach (var resident in _navMeshAgentList)
        {
            _allResidentsDictionary.Add(_vacantResidentIndex, resident);
            _vacantResidentsDictionary.Add(_vacantResidentIndex, resident);
            _vacantResidentIndex += 1;
        }
        Debug.Log(_allResidentsDictionary.Count);
    }
    
    public int GetRisedentsCount() => _allResidentsDictionary.Count;
    
    public int GetVacantsCount() => _vacantResidentsDictionary.Count;

    public void AppointWorkersToMining(ref Dictionary<int, NavMeshAgent> workers, int amount, out int appointedWorkers, Transform target)
    {
        appointedWorkers = 0;
        if (amount == 0 || _vacantResidentsDictionary.Count == 0) return;
        
        Debug.Log($"AppointWorkerToMining. Before. vacantDictionary: {_vacantResidentsDictionary.Count}; workersDictionary: {workers.Count} ");
        while (appointedWorkers < amount && _vacantResidentsDictionary.Count > 0)
        {
            var firstVacantResident = _vacantResidentsDictionary.First();
            workers.Add(firstVacantResident.Key, firstVacantResident.Value);
            _vacantResidentsDictionary.Remove(firstVacantResident.Key);
            
            SentResidentToObject(firstVacantResident.Value, target);
            appointedWorkers += 1;
        }
        Debug.Log($"AppointWorkerToMining. After. vacantDictionary: {_vacantResidentsDictionary.Count}; workersDictionary: {workers.Count} \n");
    }

    public void RemoveWorkersFromProduction(ref Dictionary<int, NavMeshAgent> workersDictionary, int amount, out int removedWorkers)
    {
        removedWorkers = 0;
        if (amount == 0 || workersDictionary.Count == 0) return;
        
        Debug.Log($"RemoveWorkerFromMining. Before. vacantDictionary: {_vacantResidentsDictionary.Count}; workersDictionary: {workersDictionary.Count} ");
        while (removedWorkers < amount && workersDictionary.Count > 0)
        {
            var firstWorker = workersDictionary.First();
            _vacantResidentsDictionary.Add(firstWorker.Key, firstWorker.Value);
            workersDictionary.Remove(firstWorker.Key);

            SentResidentToObject(firstWorker.Value, _freeZone);
            removedWorkers += 1;
        }
        Debug.Log($"RemoveWorkerFromMining. After. vacantDictionary: {_vacantResidentsDictionary.Count}; workersDictionary: {workersDictionary.Count} \n");
    }
    
    private void SentResidentToObject(NavMeshAgent resident, Transform targetObject)
    {
        resident.SetDestination(targetObject.position);
    }

    //UI
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(_eventSystem)
        {
            position = Mouse.current.position.value
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventData, results);
        return results.Count > 0;
    }

    private void SetEnableToGO(bool isEnable, ExtractionStructure structure = default)
    {
        if (isEnable)
        {
            OnObjectSelected?.Invoke(structure);
        }
        else
        {
            OnObjectDeselected?.Invoke();
        }

        isResourceUIActive = isEnable;
    }
}