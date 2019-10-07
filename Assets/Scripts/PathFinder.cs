using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static List<GridCell> FindPath(GridController<GridCell> grid, Vector2Int start, Vector2Int end, bool diag = false)
    {
        List<GridCell> Path = null;
        GridCell StartNode = grid.GetGridPoint(start);
        GridCell TargetNode = grid.GetGridPoint(end);

        if (TargetNode == null || StartNode == null)
        {
            return null;
        }

        List<GridCell> OpenList = new List<GridCell>();
        HashSet<GridCell> ClosedList = new HashSet<GridCell>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            GridCell CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost() < CurrentNode.FCost() || OpenList[i].FCost() == CurrentNode.FCost() && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
              Path = GetFinalPath(StartNode, TargetNode);
            }

            foreach (GridCell NeighborNode in grid.GetNeighboringCells(CurrentNode.transform.position, diag))
            {
                if (NeighborNode.IsWall() || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }

        return Path;
    }

    private static List<GridCell> GetFinalPath(GridCell a_StartingNode, GridCell a_EndNode)
    {
        List<GridCell> FinalPath = new List<GridCell>();
        GridCell CurrentNode = a_EndNode;

        while (CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        FinalPath.Reverse();

        return FinalPath;
    }

    private static int GetManhattenDistance(GridCell a_nodeA, GridCell a_nodeB)
    {
        float ix = Mathf.Abs(a_nodeA.transform.position.x - a_nodeB.transform.position.x);
        float iy = Mathf.Abs(a_nodeA.transform.position.z - a_nodeB.transform.position.z);

        return Mathf.RoundToInt(ix + iy);
    }
}