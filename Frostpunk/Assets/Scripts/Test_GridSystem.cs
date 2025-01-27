using UnityEngine;

public class Test_GridSystem : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _gridOrigin;

    [SerializeField] private HeatMapVisual _heatMapVisual;
    
    private Grid _grid;

    private void Start()
    {
        _grid = new Grid(5, 2, 4f, _gridOrigin.position);
        _heatMapVisual.SetGrid(_grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hitPoint = Utils.GetMouseWorldPosition3D(_cam, _layerMask);
            var value = _grid.GetValue(hitPoint);
            _grid.SetValue(hitPoint, value + 1);
            transform.position = hitPoint;
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _layerMask))
            {
                Debug.Log(_grid.GetValue(raycastHit.point));
                transform.position = raycastHit.point;
            }
        }
    }
}