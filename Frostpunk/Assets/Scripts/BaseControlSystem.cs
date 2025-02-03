using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class BaseControlSystem : MonoBehaviour
{
    public static BaseControlSystem Instance;
    
    [Header("Residents")]
    [SerializeField] private List<NavMeshAgent> _navMeshAgentList;
    [Header("Buildings")]
    [SerializeField] private List<Building> _buildingList;
    
    private InputSystem _inputSystem;
    private Camera _camera;
    private float _baseSpeed;

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
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.TryGetComponent<Building>(out Building building))
            {
                Debug.Log(building.BuildingName);
            }
            
            // _navMeshAgent.destination = hit.point;
        }
    }

    public void SentResidentToObject(int residentId, int buildingId)
    {
        if (buildingId < 0) return;

        if (residentId < _navMeshAgentList.Count)
        {
            _navMeshAgentList[residentId].SetDestination(_buildingList[buildingId].TargetPoint.position);
        }
    }
}