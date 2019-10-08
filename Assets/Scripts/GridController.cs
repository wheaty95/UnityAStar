using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridController : MonoBehaviour
{
    public enum Axis
    {
        Y,
        Z
    }

    [SerializeField] [HideInInspector] private int width;
    [SerializeField] [HideInInspector] private int height;
    [SerializeField] [HideInInspector] private float cellSize;
    [SerializeField] [HideInInspector] private Vector3 origin;
    [SerializeField] [HideInInspector] private int[,] gridArray;
    [SerializeField] [HideInInspector] private Axis axis;

    [SerializeField] public CellList cells;

    [System.Serializable]
    public class RowContainer
    {
        public List<GridCell> ContainedList = new List<GridCell>();
    }

    [System.Serializable]
    public class CellList
    {
        public List<RowContainer> row = new List<RowContainer>();
    }

    private List<LineRenderer> lines;

    public void Config(int width, int height, float cellSize, Vector3 origin, bool drawLines, Axis axis, Func<Vector3, GridCell> GridPointAdded)
    {
        if (lines != null)
        {
            foreach (LineRenderer line in lines)
            {
                if (line != null)
                {
                    GameObject.Destroy(line.gameObject);
                }
            }
        }
        lines = new List<LineRenderer>();

        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.axis = axis;

        gridArray = new int[width,height];
        cells = new CellList();

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            cells.row.Add(new RowContainer());
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (GridPointAdded != null)
                {
                    cells.row[x].ContainedList.Add(GridPointAdded(GetWorldPos(x, y)));
                }
            }
        }
    }

    public void DrawLines(Transform parent)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                lines.Add(Line.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), parent, lineAlignment: LineAlignment.View));
                lines.Add(Line.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), parent, lineAlignment: LineAlignment.View));
            }
        }

        lines.Add(Line.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), parent, lineAlignment: LineAlignment.View));
        lines.Add(Line.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), parent, lineAlignment: LineAlignment.View));
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        switch (axis)
        {
            case Axis.Y:
                return origin + new Vector3(x, 0, y) * cellSize;
            case Axis.Z:
                return origin + new Vector3(x, y) * cellSize;
        }
        return origin + new Vector3(x, y) * cellSize;
    }

    public void GetGridXY(Vector3 worldPos, out int x, out int y)
    {
        switch (axis)
        {
            case Axis.Y:
                x = Mathf.FloorToInt(worldPos.x / cellSize);
                y = Mathf.FloorToInt(worldPos.z / cellSize);
                return;
            case Axis.Z:
                x = Mathf.FloorToInt(worldPos.x / cellSize);
                y = Mathf.FloorToInt(worldPos.y / cellSize);
                return;
        }
        x = Mathf.FloorToInt(worldPos.x / cellSize);
        y = Mathf.FloorToInt(worldPos.y / cellSize);
    }  

    public Vector3 GetCentreOffset()
    {
        Vector3 centre = new Vector3();

        switch (axis)
        {
            case Axis.Y:
                centre = new Vector3(cellSize, cellSize, 0) * 0.5f;
                break;
            case Axis.Z:
                centre = new Vector3(cellSize, 0, cellSize) * 0.5f;
                break;
        }

        return centre;
    }

    public bool IsValidPos(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public GridCell GetGridPoint(Vector3 worldPos)
    {
        int x, y;
        GetGridXY(worldPos, out x, out y);
        return GetGridPoint(x, y);
    }

    public GridCell GetGridPoint(int x, int y)
    {
        if (IsValidPos(x, y))
        {
            return cells.row[x].ContainedList[y];
        }
        return default;
    }

    public GridCell GetGridPoint(Vector2Int point)
    {
        if (IsValidPos(point.x, point.y))
        {
            return cells.row[point.x].ContainedList[point.y];
        }
        return default;
    }

    public GridCell[,] GetCells()
    {
        return null;
    }

    public GridCell[] GetNeighboringCells(Vector3 worldPos, bool diag = false)
    {
        int x, y;
        GetGridXY(worldPos, out x, out y);
        return GetNeighboringCells(x, y, diag);
    }

    public GridCell[] GetNeighboringCells(int x, int y, bool diag = false)
    {
        List<GridCell> neighbours = new List<GridCell>();

        GridCell north = GetGridPoint(x, y + 1);
        GridCell south = GetGridPoint(x, y - 1);
        GridCell east = GetGridPoint(x + 1, y);
        GridCell west = GetGridPoint(x - 1, y);

        if (north != null)
        {
            neighbours.Add(north);
        }

        if (south != null)
        {
            neighbours.Add(south);
        }

        if (east != null)
        {
            neighbours.Add(east);
        }

        if (west != null)
        {
            neighbours.Add(west);
        }

        if (diag)
        {
            GridCell northEast = GetGridPoint(x + 1, y + 1);
            GridCell northWest = GetGridPoint(x - 1, y + 1);
            GridCell southEast = GetGridPoint(x + 1, y - 1);
            GridCell southWest = GetGridPoint(x - 1, y - 1);

            if (northEast != null)
            {
                neighbours.Add(northEast);
            }

            if (northWest != null)
            {
                neighbours.Add(northWest);
            }

            if (southEast != null)
            {
                neighbours.Add(southEast);
            }

            if (southWest != null)
            {
                neighbours.Add(southWest);
            }
        }
        return neighbours.ToArray();
    }
}
