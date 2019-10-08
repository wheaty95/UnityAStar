using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private int m_gridWidth;
    [SerializeField] private int m_gridHeight;
    [SerializeField] private float m_cellSize;
    [SerializeField] private bool m_centreCell;
    [SerializeField] private Agent m_agent;
    [SerializeField] private Vector2Int m_startingCell;
    [SerializeField] private Vector2Int m_targetCell;

    GridController<GridCell> m_grid;

    void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        m_grid = new GridController<GridCell>(m_gridWidth, m_gridHeight, m_cellSize,
            transform.position, false, GridController<GridCell>.Axis.Y, CellAdded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize) / 2), new Vector3(m_gridWidth * m_cellSize, 1, m_gridHeight * m_cellSize));
    }

    private void Start()
    {
        Vector3 worldPos = m_grid.GetWorldPos(m_startingCell.x, m_startingCell.y);
        Vector3 centre = m_centreCell ? new Vector3(m_cellSize, 0, m_cellSize) * 0.5f : Vector3.zero;
        Agent agent = Instantiate(m_agent, worldPos + centre, Quaternion.identity, null);
        agent.Config(m_grid);
    }

    public GridController<GridCell> GetGrid()
    {
        return m_grid;
    }

    private GridCell CellAdded(Vector3 worldPos)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 centre = m_centreCell ? new Vector3(m_cellSize, 0, m_cellSize) * 0.5f : Vector3.zero;
        go.transform.position = worldPos + centre;
        GridCell cell = go.AddComponent<GridCell>();
        cell.Config(m_cellSize);
        go.transform.localScale = Vector3.one * m_cellSize;
        //go.SetActive(true);
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Go");
        return cell;
    }
}