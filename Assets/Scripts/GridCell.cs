using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell : MonoBehaviour
{
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    [HideInInspector] public int weight;
    [HideInInspector]  public GridCell ParentNode;
    private bool m_isWall = false;
    private float size;

    public void Config(PathFindingGrid grid, float size)
    {
        this.size = size;
        bool isWall = false;
        SetWall(isWall);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 1), -Vector3.up, out hit, Mathf.Infinity))
        {
            LayerMask layer = hit.transform.gameObject.layer;
            weight = grid.GetWeight(layer);
            Debug.Log(weight);
        }
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