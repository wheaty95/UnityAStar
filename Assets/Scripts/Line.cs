using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Line
{
    public static LineRenderer DrawLine(Vector3 start, Vector3 end, Transform parent = null, bool useWorldSpace = true, Material material = null, LineAlignment lineAlignment = LineAlignment.TransformZ, params Vector3[] additionalPoints)
    {
        LineRenderer line = null;
        GameObject go = new GameObject("Line");

        if (parent != null)
        {
            go.transform.parent = parent;
        }
        line = go.AddComponent<LineRenderer>();

        List<Vector3> points = new List<Vector3>();
        points.Add(start);
        points.AddRange(additionalPoints);
        points.Add(end);

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
        line.alignment = lineAlignment;
        line.useWorldSpace = useWorldSpace;

        if (material)
        {
            line.material = material;
        }
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Line");
        return line;
    }
}
