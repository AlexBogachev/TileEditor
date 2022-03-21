using UnityEngine;
using Jobberwocky.GeometryAlgorithms.Source.Algorithms.Triangulation2D;
using Jobberwocky.GeometryAlgorithms.Source.Parameters;
using Jobberwocky.GeometryAlgorithms.Source.Core;

public class MeshBuilder : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;

    TileRepresentation tileRepresentation;
    bool isTileSliced;

    TileData activeData;

    public GameObject BuildMesh(TileRepresentation tileRepresentation, Transform parent, bool useDelunay)
    {
        this.tileRepresentation = tileRepresentation;
        isTileSliced = tileRepresentation.GetSlicedTile().Count == 0 ? false : true;

        GameObject obj = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(parent);

        Triangulation2DParameters tr2DParam = new Triangulation2DParameters();
        Triangulation2DWrapper triangulation2DWrapper = new Triangulation2DWrapper();

        Vector2[] vertices;
        if(!isTileSliced)
        {
            vertices = tileRepresentation.GetOriginalTile().GetVertices();
        }
        else
        {
            vertices = tileRepresentation.GetSlicedTile().ToArray();
        }

        tr2DParam.Points = ConvertVector2ToVector3(vertices);
        tr2DParam.Boundary = ConvertVector2ToVector3(vertices);
        tr2DParam.Delaunay = useDelunay;
        tr2DParam.Side = Side.Back; 

        Geometry g = triangulation2DWrapper.Triangulate2D(tr2DParam);

        MeshFilter mf = obj.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.Clear(false);
        mf.mesh = mesh;
        mesh.vertices = ConvertVerticesToVector3(g.Vertices);
        mesh.triangles = g.Indices;
        mesh.uv = ConvertVerticesUV(g.Vertices);
        mesh.RecalculateNormals();
        mesh.Optimize();

        SetTexture(obj);

        return obj;
    }

    public void SetTexture(GameObject tileObj)
    {
        if (activeData != null)
        {
            MeshRenderer meshRenderer = tileObj.GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = (Texture2D)Resources.Load(activeData.pathToTexture);
        }
        
    }

    public void SetActiveTileData(TileData newData)
    {
        activeData = newData;
    }

    /// <summary>
    ///  В зависимости от того, была ли разрезана плитка - устанавливаем координаты UV-развертки
    /// </summary>
    private Vector2[] ConvertVerticesUV(Vertex[] vertices)
    {
        Vector2[] uvs;

        if (!isTileSliced)
        {
            uvs = new Vector2[]
            {
            new Vector2(0.0f,0.0f),
            new Vector2(0.0f,1.0f),
            new Vector2(1.0f,1.0f),
            new Vector2(1.0f,0.0f),
            new Vector2(0.0f,0.0f),
            new Vector2(0.0f,1.0f),
            new Vector2(1.0f,1.0f),
            new Vector2(1.0f,0.0f)
            };
        }
        else
        {
            uvs = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                uvs[i] = ConverVertexToUV(vertices[i]);
            }
        }
        return uvs;
    }

    private Vector3[] ConvertVector2ToVector3(Vector2[] v2)
    {
        Vector3[] positions = new Vector3[v2.Length];
        for (int i = 0; i < v2.Length; i++)
        {
            positions[i] = new Vector3(v2[i].x, v2[i].y, 0.0f);
        }
        return positions;
    }

    private Vector3[] ConvertVerticesToVector3(Vertex[] vert)
    {
        Vector3[] positions = new Vector3[vert.Length];
        for (int i = 0; i < vert.Length; i++)
        {
            positions[i] = vert[i].Position;
        }
        return positions;
    }

    /// <summary>
    ///  преобразование координат вершин в координаты UV-развертки
    /// </summary>
    private Vector2 ConverVertexToUV(Vertex vertex)
    {
        Rectangle originalRectangle = tileRepresentation.GetOriginalTile();

        float UV_x = (vertex.Position.x - originalRectangle.GetMinCoordinatePosition().x) / (originalRectangle.GetMaxCoordinatePosition().x - originalRectangle.GetMinCoordinatePosition().x);
        float UV_y = (vertex.Position.y - originalRectangle.GetMinCoordinatePosition().y) / (originalRectangle.GetMaxCoordinatePosition().y - originalRectangle.GetMinCoordinatePosition().y);

        Vector2 UV = new Vector2(UV_x, UV_y);

        return UV;
    }
}
