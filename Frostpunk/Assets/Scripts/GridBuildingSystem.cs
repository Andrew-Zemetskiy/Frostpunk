using System;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private Transform testBuilding;
    [SerializeField] private LayerMask _layerMask;

    private Grid<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new Grid<GridObject>(5, 2, 4f, Vector3.zero,
            (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return x + "," + z;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXZ(Utils.GetMouseWorldPosition3D(Camera.main, _layerMask), out int x, out int z);
            Instantiate(testBuilding, grid.GetWorldPosition(x, z), Quaternion.identity);
        }
    }
}