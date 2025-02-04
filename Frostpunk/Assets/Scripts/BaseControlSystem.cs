using System;
using System.Collections.Generic;
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

    [Header("Residents")] [SerializeField] private List<NavMeshAgent> _navMeshAgentList;
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
            Debug.Log("Clicked on UI, ignoring raycast.");
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

    public void SentResidentToObject(int residentId, int buildingId)
    {
        if (buildingId < 0) return;

        if (residentId < _navMeshAgentList.Count)
        {
            _navMeshAgentList[residentId].SetDestination(_buildingList[buildingId].TargetPoint.position);
        }
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