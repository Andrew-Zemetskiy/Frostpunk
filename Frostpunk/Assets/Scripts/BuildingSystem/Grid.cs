using System;
using UnityEngine;

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
    private MeshRenderer[,] _textRenderers;

    private bool _isTextShowing;

    public Grid(int width, int height, float cellSize, Vector3 originPosition,
        Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, Transform textParent,
        bool showGridText = false)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        _isTextShowing = showGridText;

        _gridArray = new TGridObject[_width, _height];
        _valueTextArray = new TextMesh[_width, _height];
        _textRenderers = new MeshRenderer[_width, _height];

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
                    var text = Utils.CreateWorldText(_gridArray[x, z]?.ToString(), textParent,
                        GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 20,
                        Color.white, TextAnchor.MiddleCenter);

                    text.transform.eulerAngles = new Vector3(90f, 0f, 0f);
                    
                    _textRenderers[x,z] = text.GetComponent<MeshRenderer>();
                    if (!showGridText) _textRenderers[x,z].enabled = false; //hide text
                    _valueTextArray[x, z] = text;

                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += UpdateGridText;
        }
    }

    private void UpdateGridText(object sender, OnGridObjectChangedEventArgs eventArgs)
    {
        _valueTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
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
            _gridArray[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (0 <= x && 0 <= z && x < _width && z < _height)
        {
            return _gridArray[x, z];
        }

        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return GetGridObject(x, z);
    }

    public void TurnOnGridText(bool isTurnOn)
    {
        if (isTurnOn == !_isTextShowing)
        {
            foreach (var textRenderer in _textRenderers)
            {
                textRenderer.enabled = isTurnOn;
            }
            _isTextShowing = isTurnOn;
        }
    }

    public bool IsMousePositionInGrid(Vector2Int gridPosition)
    {
        if (gridPosition is { x: >= 0, y: >= 0 } && gridPosition.x < _width && gridPosition.y < _height)
        {
            return true;
        }

        return false;
    }
}