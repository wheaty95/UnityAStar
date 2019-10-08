using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell : MonoBehaviour
{
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    [HideInInspector]  public GridCell ParentNode;
    private bool m_isWall = false;
    private float size;

    public void Config(float size)
    {
        this.size = size;
        bool isWall = false;// Physics.CheckBox(transform.position, Vector3.one * size);
        SetWall(isWall);
    }

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
        gameObject.SetActive(wall);
    }
}