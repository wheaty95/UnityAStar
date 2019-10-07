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
        m_grid = new GridController<GridCell>(m_gridWidth, m_gridHeight, m_cellSize,
            transform.position, true, GridController<GridCell>.Axis.Y, CellAdded);
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
        Vector3 centre = Vector3.zero;
        if (m_centreCell)
        {
            centre = new Vector3(m_cellSize, 0, m_cellSize) * 0.5f;
        }
        go.transform.position = worldPos + centre;
        GridCell cell = go.AddComponent<GridCell>();

        bool isWall = Random.Range(0, 10) < 1;
        cell.SetWall(isWall);
        go.SetActive(isWall);
        return cell;
    }
}