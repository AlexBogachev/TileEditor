using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
///  Класс c полезными методами
/// </summary>
public static class UtilitiesScripts
{
    public static float GetSquare(List<Vector2> plane)
    {
        float firstSum = 0;
        float seconsSum = 0;
        for (int i = 0; i < plane.Count - 1; i++)
        {
            firstSum += plane[i].x * plane[i + 1].y;
            seconsSum += plane[i + 1].x * plane[i].y;
        }
        float firstProduct = plane[plane.Count - 1].x * plane[0].y;
        float secondProduct = plane[0].x * plane[plane.Count - 1].y;

        float square = 0.5f * Mathf.Abs(firstSum + firstProduct - seconsSum - secondProduct);

        return square;
    }

    public static Vector2 RotateAround(Vector2 point, Vector2 pivot, float angleDegree)
    {
        float angle = angleDegree * Mathf.PI / 180;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        float dx = point.x - pivot.x;
        float dy = point.y - pivot.y;
        float x = cos * dx - sin * dy + pivot.x;
        float y = sin * dx + cos * dy + pivot.x;

        Vector2 rotated = new Vector2(x, y);
        return rotated;
    }

    /// <summary>
    ///  Построение описанного прямоугольника вокруг заданного набора точек
    /// </summary>
    public static Rectangle GetCircumscribedRectangle(List<Vector2>contur, Vector2 rectCenter)
    {
        float minX = contur.Min(pos => pos.x);
        float maxX = contur.Max(pos => pos.x);
        float minY = contur.Min(pos => pos.y);
        float maxY = contur.Max(pos => pos.y);

        float width = maxX - minX;
        float height = maxY - minY;

        Rectangle rect = new Rectangle(rectCenter, width, height);

        return rect;
    }
}
