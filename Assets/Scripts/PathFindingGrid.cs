using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingGrid : MonoBehaviour
{
    [System.Serializable]
    public class Weights
    {
        [SerializeField] public LayerMask layer;
        [SerializeField] public int weight;
        [SerializeField] public bool walkable;
    }

    [SerializeField] [HideInInspector] GridController m_grid;

    [SerializeField] private int m_gridWidth;
    [SerializeField] private int m_gridHeight;
    [SerializeField] private float m_cellSize;
    [SerializeField] private bool m_centreCell;
    [SerializeField] private List<Weights> m_weights;

    public bool IsInLayerMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public int GetWeight(LayerMask mask)
    {
        foreach (Weights layerMask in m_weights)
        {
            if (IsInLayerMask(layerMask.layer, mask.value))
            {
                return layerMask.weight;
            }
        }
        return 0;
    }

    public bool GetIsWalkable(LayerMask mask)
    {
        foreach (Weights layerMask in m_weights)
        {
            if (IsInLayerMask(layerMask.layer, mask.value))
            {
                return layerMask.walkable;
            }
        }
        return true;
    }

    public void Create()
    {
        Clear();
        m_grid = gameObject.AddComponent<GridController>();
        m_grid.Config(m_gridWidth, m_gridHeight, m_cellSize, transform.position, true, GridController.Axis.Y, CellAdded);
    }

    public GridController GetGrid()
    {
        return m_grid;
    }

    public void Clear()
    {
        GridController grid = GetComponent<GridController>();
        if (grid)
        {
            DestroyImmediate(grid);
        }

        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public Vector2Int GetGridXY(Vector3 position)
    {
        m_grid.GetGridXY(position, out int x, out int y);
        return new Vector2Int(x, y);
    }

    public Vector2Int GetGridSize()
    {
        return new Vector2Int(m_gridWidth, m_gridHeight);
    }

    public Vector3 GetGridXY(Vector2Int gridPos)
    {
        return m_grid.GetWorldPos(gridPos.x, gridPos.y);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position + (new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize) / 2), new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize));
    }

    private GridCell CellAdded(Vector3 worldPos)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.parent = transform;
        go.name = "Cell";
        Vector3 centre = m_centreCell ? new Vector3(m_cellSize, 0, m_cellSize) * 0.5f : Vector3.zero;
        go.transform.position = worldPos + centre;
        GridCell cell = go.AddComponent<GridCell>();
        cell.Config(this,m_cellSize);
        //go.transform.localScale = Vector3.one * m_cellSize;
        go.SetActive(false);
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Go");
        return cell;
    }
}
