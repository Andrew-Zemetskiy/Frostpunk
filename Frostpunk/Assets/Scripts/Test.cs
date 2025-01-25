using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class Test : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _layerMask;

    private Grid _grid;

    private void Start()
    {
        _grid = new Grid(5, 10, 4f);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // _grid.SetValue(UtilsClass.GetMouseWorldPositionWithZ(), 50);
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _layerMask))
            {
                _grid.SetValue(raycastHit.point, 50);
                transform.position = raycastHit.point;
            }
        }
    }
}