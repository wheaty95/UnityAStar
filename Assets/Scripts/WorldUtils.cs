using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldUtils
{
    public static TextMesh Text(string text, Vector3 worldPos, Color colour, int fontSize, TextAlignment alignment, TextAnchor anchor)
    {
        GameObject go = new GameObject("World Text");
        TextMesh textMesh = go.AddComponent<TextMesh>();
        go.transform.position = worldPos;
        textMesh.text = text;
        textMesh.color = colour;
        textMesh.fontSize = fontSize;
        textMesh.alignment = alignment;
        textMesh.anchor = anchor;
        return textMesh;
    }
}
