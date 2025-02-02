using System;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;
    private TextMesh[,] _valueTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int ,TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[_width, _height];
        _valueTextArray = new TextMesh[_width, _height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                _gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        bool showGrid = true;
        if (showGrid)
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < _gridArray.GetLength(1); z++)
                {
                    var text = UtilsClass.CreateWorldText(_gridArray[x, z]?.ToString(), null,
                        GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 20,
                        Color.white, TextAnchor.MiddleCenter);

                    text.transform.eulerAngles = new Vector3(90f, 0f, 0f);
                    _valueTextArray[x, z] = text;

                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public int GetWidth() => _width;

    public int GetHeight() => _height;

    public float GetCellSize() => _cellSize;

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * _cellSize + _originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (0 <= x && 0 <= z && x < _width && z < _height)
        {
            _gridArray[x, z] = value; //Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE)
            TriggerGridObjectChanged(x, z);
            _valueTextArray[x, z].text = _gridArray[x, z]?.ToString();
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        UpdateGridText(x, z);
    }

    private void UpdateGridText(int x, int z)
    {
        _valueTextArray[x, z].text = _gridArray[x, z]?.ToString();
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (0 <= x && 0 <= z && x < _width && z < _height)
        {
            return _gridArray[x, z];
        }
        else
        {
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }
}