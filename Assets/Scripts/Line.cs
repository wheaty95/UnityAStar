using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Line
{
    public static LineRenderer DrawLine(Vector3 start, Vector3 end, GameObject parent = null, bool useWorldSpace = true, Material material = null, LineAlignment lineAlignment = LineAlignment.TransformZ, params Vector3[] additionalPoints)
    {
        LineRenderer line = null;

        if (parent == null)
        {
            parent = new GameObject("Line");

            line = parent.GetComponent<LineRenderer>();
            if (line == null)
            {
                line = parent.AddComponent<LineRenderer>();
            }
        }
        else
        {
            line = parent.AddComponent<LineRenderer>();
        }

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

        return line;
    }
}
