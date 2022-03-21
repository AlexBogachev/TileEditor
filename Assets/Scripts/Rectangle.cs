using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Геометрическое представление прямоугольника (плитка, панель)
/// </summary>
public class Rectangle
{
    Vector2 rectCenter;
    Vector2 lowerLeftVertex;

    float width;
    float height;

    Vector2[] vertices;

    public Rectangle(Vector2 rectCenter, float width, float height)
    {
        this.rectCenter = rectCenter;
        this.width = width;
        this.height = height;

        lowerLeftVertex = rectCenter + (new Vector2(-width / 2.0f, 0.0f) + new Vector2(0.0f, -height / 2.0f));

        vertices = new Vector2[]
        {
            lowerLeftVertex,
            lowerLeftVertex + new Vector2(0.0f,height),
            lowerLeftVertex + (new Vector2(0.0f,height) + new Vector2(width,0.0f)),
            lowerLeftVertex + new Vector2(width,0.0f)
        };
    }

    public void RotateAroundCenter(float angle)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 rotated = UtilitiesScripts.RotateAround(vertices[i],rectCenter, angle);
            vertices[i] = rotated;
        }
    }

    public float GetSquare()
    {
        return width * height;
    }

    public Vector2[] GetVertices()
    {
        return vertices;
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }

    public Vector2 GetRectCenter()
    {
        return rectCenter;
    }

    public Vector2 GetMinCoordinatePosition()
    {
        return lowerLeftVertex;
    }

    public Vector2 GetMaxCoordinatePosition()
    {
        return lowerLeftVertex + (new Vector2(0.0f, height) + new Vector2(width, 0.0f));
    }
}
