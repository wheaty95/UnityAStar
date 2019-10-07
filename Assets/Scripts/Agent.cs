using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    List<GridCell> path;
    GridController<GridCell> grid;
    public Vector2Int start;
    public Vector2Int end;
    public bool diag = false;

    public void Config(GridController<GridCell> grid)
    {
        this.grid = grid;
    }

    private void Update()
    {
        path = PathFinder.FindPath(grid, start, end, diag);
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            GridCell prev = null;
            foreach (GridCell cell in path)
            {
                if (prev != null)
                {
                    Gizmos.DrawLine(prev.transform.position, cell.transform.position);
                }
                else
                {
                    Gizmos.DrawLine(transform.position, cell.transform.position);
                }
                prev = cell;
            }
        }
    }
}