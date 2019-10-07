using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int gCost;
    public int hCost;
    public GridCell ParentNode;
    private bool m_isWall = false;

    public int FCost()
    {
        return gCost + hCost;
    }

    public bool IsWall()
    {
        return m_isWall;
    }

    public void SetWall(bool wall)
    {
        m_isWall = wall;
    }
}