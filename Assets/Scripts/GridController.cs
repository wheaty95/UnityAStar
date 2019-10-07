using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController<T>
{
    public enum Axis
    {
        Y,
        Z
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private int[,] gridArray;
    private Axis axis;

    private T[,] cells;

    public GridController(int width, int height, float cellSize, Vector3 origin, bool drawLines, Axis axis, Func<Vector3, T> GridPointAdded)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.axis = axis;

        gridArray = new int[width,height];
        cells = new T[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (GridPointAdded != null)
                {
                    cells[x, y] = GridPointAdded(GetWorldPos(x, y));
                }

                if (drawLines)
                {
                    Line.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), lineAlignment: LineAlignment.View);
                    Line.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), lineAlignment: LineAlignment.View);
                }
            }
        }

        if (drawLines)
        {
            Line.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), lineAlignment: LineAlignment.View);
            Line.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), lineAlignment: LineAlignment.View);
        }
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
        return x >= 0 && x < cells.GetLength(0) && y >= 0 && y < cells.GetLength(1);
    }

    public T GetGridPoint(Vector3 worldPos)
    {
        int x, y;
        GetGridXY(worldPos, out x, out y);
        return GetGridPoint(x, y);
    }

    public T GetGridPoint(int x, int y)
    {
        if (IsValidPos(x, y))
        {
            return cells[x, y];
        }
        return default;
    }

    public T GetGridPoint(Vector2Int point)
    {
        if (IsValidPos(point.x, point.y))
        {
            return cells[point.x, point.y];
        }
        return default;
    }

    public T[,] GetCells()
    {
        return cells;
    }

    public T[] GetNeighboringCells(Vector3 worldPos, bool diag = false)
    {
        int x, y;
        GetGridXY(worldPos, out x, out y);
        return GetNeighboringCells(x, y, diag);
    }

    public T[] GetNeighboringCells(int x, int y, bool diag = false)
    {
        List<T> neighbours = new List<T>();

        T north = GetGridPoint(x, y + 1);
        T south = GetGridPoint(x, y - 1);
        T east = GetGridPoint(x + 1, y);
        T west = GetGridPoint(x - 1, y);

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
            T northEast = GetGridPoint(x + 1, y + 1);
            T northWest = GetGridPoint(x - 1, y + 1);
            T southEast = GetGridPoint(x + 1, y - 1);
            T southWest = GetGridPoint(x - 1, y - 1);

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
