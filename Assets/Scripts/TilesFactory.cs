using System.Collections.Generic;
using UnityEngine;

public class TilesFactory
{
    List<GameObject> tiles;
    Transform parent;

    public TilesFactory()
    {
        tiles = new List<GameObject>();

    }

    public void BuildTilesRepresentations(TileSlicer slicer, MeshBuilder builder, Transform parent)
    {
        tiles.Clear();
        foreach (TileRepresentation r in slicer.GetTilesRepresentations())
        {
            tiles.Add(builder.BuildMesh(r, parent, false));
        }
    }

    public List<GameObject> GetTiles()
    {
        return tiles;
    }
}
