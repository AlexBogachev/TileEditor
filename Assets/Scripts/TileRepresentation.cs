using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Информация о геометрии плитки + разрезалась ли она
/// </summary>
public class TileRepresentation
{
    Rectangle originalTile;
    List<Vector2> slicedTile;

    public TileRepresentation(Rectangle originalTile, List<Vector2> slicedTile)
    {
        this.originalTile = originalTile;
        this.slicedTile = slicedTile;
    }

    public Rectangle GetOriginalTile()
    {
        return originalTile;
    }

    public List<Vector2> GetSlicedTile()
    {
        return slicedTile;
    }

    public string GetName()
    {
        return "NOT NAME";
    }
}
