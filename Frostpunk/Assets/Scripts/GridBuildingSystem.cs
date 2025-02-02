using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance;

    public event Action OnObjectPlaced;
    public event Action OnSelectedChanged;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _originPosition;
    [SerializeField] private Camera cameraForBuildings;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectSOList;
    [SerializeField] private Transform textHandler;
    [SerializeField] private bool showGridText;

    private PlacedObjectTypeSO placedObjectTypeSO;
    private Grid<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    private void Awake()
    {
        Instance = this;

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new Grid<GridObject>(10, 5, 4f, _originPosition.position,
            (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), textHandler, showGridText);

        placedObjectTypeSO = null;
        if (cameraForBuildings == null)
            cameraForBuildings = Camera.main;
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public override string ToString()
        {
            return x + "," + z + "\n" + (placedObject == default ? "" : "<PO>");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null)
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition3D(cameraForBuildings, _layerMask);
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            if (!grid.IsMousePositionInGrid(placedObjectOrigin)) return; //if outside the grid

            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

            bool canBuild = true;
            foreach (var gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) +
                                                    new Vector3(rotationOffset.x, 0, rotationOffset.y) *
                                                    grid.GetCellSize();
                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir,
                    placedObjectTypeSO);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                OnObjectPlaced?.Invoke();
            }
            else
            {
                //Cannot build here!
                Debug.Log("Cannot build here!");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition3D(cameraForBuildings, _layerMask);
            GridObject gridObject = grid.GetGridObject(mousePosition);
            if (gridObject != null)
            {
                //Valid grid pos
                PlacedObject placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null)
                {
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            Debug.Log(dir);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            placedObjectTypeSO = placedObjectSOList[0];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            placedObjectTypeSO = placedObjectSOList[1];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            placedObjectTypeSO = placedObjectSOList[2];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DeselectObjectType();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            showGridText = !showGridText;
            TurnOnText(showGridText);
        }
    }

    private void DeselectObjectType()
    {
        placedObjectTypeSO = null;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke();
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Utils.GetMouseWorldPosition3D(cameraForBuildings, _layerMask);
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) +
                                                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO != null)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }
    
    public void TurnOnText(bool value) =>  grid.TurnOnGridText(value);
}