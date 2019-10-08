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
    public float speed = 2;
    private int cellIndex = 0;
    private bool finished = false;

    public void Config(GridController<GridCell> grid)
    {
        this.grid = grid;
        path = PathFinder.FindPath(grid, start, end, diag);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, path[cellIndex].transform.position);
        if (distance < 0.1f)
        {
            cellIndex++;
            if (cellIndex == path.Count)
            {
                int x, y;
                grid.GetGridXY(transform.position, out x, out y);
                path = PathFinder.FindPath(grid, new Vector2Int(x,y), new Vector2Int(Random.Range(0,9), Random.Range(0, 9)), diag);
                cellIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, path[cellIndex].transform.position, speed * Time.deltaTime); 
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.black;
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