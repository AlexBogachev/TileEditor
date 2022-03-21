using ClipperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SquareUpdated: UnityEvent<float>
{
}

/// <summary>
///  Класс разрезает плитку по заданному шаблону панели
/// </summary>
public class TileSlicer : MonoBehaviour
{
    TileStacker stacker;
    Rectangle panel;

    Clipper clipper;
    List<List<IntPoint>> solution;

    List<IntPoint> panelPoints;
    List<IntPoint> rectanglePoints;

    List<TileRepresentation> tileRepresentations;
    public SquareUpdated squareUpdated = new SquareUpdated();

    //Т.к. clipper использует int числа для рассчета, в документации рекомендуется использовать масштабирование для работы с float числами
    float scaleFactor = 1000000000.0f;

    public TileSlicer(TileStacker stacker, Rectangle panel)
    {
        this.stacker = stacker;
        this.panel = panel;

        clipper = new Clipper();
        solution = new List<List<IntPoint>>();

        panelPoints = new List<IntPoint>();
        rectanglePoints = new List<IntPoint>();

        tileRepresentations = new List<TileRepresentation>();
    }

    public void SliceTiles()
    {
        float square = 0.0f;
        panelPoints.Clear();
        tileRepresentations.Clear();

        panelPoints = Vector2ArrayToListIntPoint(panel.GetVertices().ToList());
        foreach(Rectangle rectangle in stacker.GetTiles())
        {
            rectanglePoints.Clear();
            rectanglePoints = Vector2ArrayToListIntPoint(rectangle.GetVertices().ToList());

            clipper.AddPath(rectanglePoints, PolyType.ptSubject, true);
            clipper.AddPath(panelPoints, PolyType.ptClip, true);
            clipper.Execute(ClipType.ctIntersection, solution);

            if(solution.Count != 0)
            {
                foreach (List<IntPoint> intPoints in solution)
                {
                    List<Vector2> convertedSolution = ListIntPointToListVector2(intPoints);
                    float convertedSolutionSquare = UtilitiesScripts.GetSquare(convertedSolution);
                    float squareDifference = rectangle.GetSquare() - convertedSolutionSquare;
                    //Площадь решения отличается от площади плитки - создаем представление плитки со списком точек решения (результат обрезания)
                    if (squareDifference > 1.0f / scaleFactor)
                    {
                        tileRepresentations.Add(new TileRepresentation(rectangle, convertedSolution));
                        square += convertedSolutionSquare;
                    }
                    //Площадь решения не отличается от площади плитки - плитка не разрезалась (нулевой список точек решения)
                    else
                    {
                        tileRepresentations.Add(new TileRepresentation(rectangle, new List<Vector2>()));
                        square += rectangle.GetSquare();
                    }
                }
            }
            clipper.Clear();
            solution.Clear();
        }
        squareUpdated.Invoke(square);
    }

    public List<TileRepresentation> GetTilesRepresentations()
    {
        return tileRepresentations;
    }

    private List<Vector2> ListIntPointToListVector2(List<IntPoint> p)
    {
        List<Vector2> v2List = new List<Vector2>();

        for (int i = 0; i < p.Count; i++)
        {
            Vector2 pos = new Vector2(p[i].X / scaleFactor, p[i].Y / scaleFactor);
            v2List.Add(pos);
        }
        return v2List;
    }

    private List<IntPoint> Vector2ArrayToListIntPoint(List<Vector2> shape)
    {
        List<IntPoint> list = new List<IntPoint>();
        foreach (Vector3 v in shape)
        {
            Vector2 pos = new Vector2(v.x * scaleFactor, v.y * scaleFactor);
            list.Add(new IntPoint((long)pos.x, (long)pos.y));
        }
        return list;
    }
}
