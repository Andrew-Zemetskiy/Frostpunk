using System;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grid _grid;
    private Mesh _mesh;

    private void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
        UpdateHeatMapVisual();
    }

    private void UpdateHeatMapVisual()
    {
        Utils.CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv,
            out int[] triangles);
        for (int x = 0; x < _grid.GetWidth(); x++)
        {
            for (int z = 0; z < _grid.GetHeight(); z++)
            {
                int index = x * _grid.GetHeight() + z;
                Debug.Log(index);

                Vector3 quadSize = Vector3.one * _grid.GetCellSize();
                Utils.AddQuad(vertices, uv, triangles, index, _grid.GetWorldPosition(x, z) + quadSize * .5f, quadSize,
                    Vector2.zero);
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }
}