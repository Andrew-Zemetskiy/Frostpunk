using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class Test : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _gridOrigin;
    
    private Grid _grid;

    private void Start()
    {
        _grid = new Grid(5, 10, 4f, _gridOrigin.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _layerMask))
            {
                var value = _grid.GetValue(raycastHit.point);
                _grid.SetValue(raycastHit.point, value + 1);
                transform.position = raycastHit.point;
            }
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