using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingGrid : MonoBehaviour
{
    [SerializeField] [HideInInspector] GridController m_grid;

    [SerializeField] private int m_gridWidth;
    [SerializeField] private int m_gridHeight;
    [SerializeField] private float m_cellSize;
    [SerializeField] private bool m_centreCell;

    public void Create()
    {
        Clear();
        m_grid = gameObject.AddComponent<GridController>();
        m_grid.Config(m_gridWidth, m_gridHeight, m_cellSize, transform.position, true, GridController.Axis.Y, CellAdded);
        m_grid.DrawLines(transform);
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
        Gizmos.DrawWireCube(transform.position + (new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize) / 2), new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize));
    }

    private GridCell CellAdded(Vector3 worldPos)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.parent = transform;
        go.name = "Cell";
        Vector3 centre = m_centreCell ? new Vector3(m_cellSize, 0, m_cellSize) * 0.5f : Vector3.zero;
        go.transform.position = worldPos + centre;
        GridCell cell = go.AddComponent<GridCell>();
        cell.Config(m_cellSize);
        //go.transform.localScale = Vector3.one * m_cellSize;
        go.SetActive(true);
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Go");
        return cell;
    }
}
