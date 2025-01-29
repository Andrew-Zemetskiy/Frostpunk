using System.Xml.Linq;
using UnityEngine;

public class Test_GridSystem : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _gridOrigin;

    [SerializeField] private HeatMapVisual _heatMapVisual;

    private Grid<GridCellData> _grid;

    private void Start()
    {
        _grid = new Grid<GridCellData>(5, 2, 4f, _gridOrigin.position,
            (Grid<GridCellData> g, int x, int y) => new GridCellData(g, x, y));
        // _heatMapVisual.SetGrid(_grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hitPoint = Utils.GetMouseWorldPosition3D(_cam, _layerMask);
            var value = _grid.GetGridObject(hitPoint);
            GridCellData gridCellData = _grid.GetGridObject(hitPoint);
            if (gridCellData != null)
            {
                gridCellData.AddValue(5);
            }

            // _grid.SetValue(hitPoint, value + 1);
            transform.position = hitPoint;
        }

        if (Input.GetMouseButtonDown(1))
        {
            var hitPoint = Utils.GetMouseWorldPosition3D(_cam, _layerMask);
            var value = _grid.GetGridObject(hitPoint);
            Debug.Log(value);
            transform.position = hitPoint;
        }
    }
}

public class GridCellData
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<GridCellData> _grid;
    private int x;
    private int z;
    private int value;

    public GridCellData(Grid<GridCellData> grid, int x, int z)
    {
        _grid = grid;
        this.x = x;
        this.z = z;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        _grid.TriggerGridObjectChanged(x, z);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}