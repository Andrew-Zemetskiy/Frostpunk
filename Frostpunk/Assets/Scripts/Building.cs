using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private string _buildingName;
    [SerializeField] private Transform _targetPoint;

    public string BuildingName
    {
        get => _buildingName;
        private set => _buildingName = value;
    }

    public Transform TargetPoint
    {
        get => _targetPoint;
        private set => _targetPoint = value;
    }
}