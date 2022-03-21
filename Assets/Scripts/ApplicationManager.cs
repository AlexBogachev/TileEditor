using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager Instance;

    TilesCatalogue tilesCatalogue;
    Rectangle tilePanel;

    TileStacker stacker;
    TileSlicer slicer;

    [SerializeField] Transform tilesParent;
    MeshBuilder meshBuilder;
    TilesFactory factory;

    UIController uIController;

    float square;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        tilesCatalogue = new TilesCatalogue();
        meshBuilder = FindObjectOfType<MeshBuilder>();
        BuildPanel();

        stacker = new TileStacker();
        slicer = new TileSlicer(stacker, tilePanel);
        factory = new TilesFactory();

        FindObjectOfType<SquareCounter>().Initialize(slicer);
        slicer.squareUpdated.AddListener(UpdateSquareValue);
    }

    private void Start()
    {
        uIController = FindObjectOfType<UIController>();
        uIController.Initialize(tilesCatalogue);
    }

    public void UpdateTiles(UpdateData data)
    {
        ResetTilesParentTransform();
        CalculateTiles(data);
        BuildTiles(data);
    }

    // В реальной программе, по хорошему, нужно либо масштабировать полотно, либо регулировать настройки камеры в зависимости от размера 
    // полотна (размеры которого также можно устанавливать - по хорошему, тогда нужно делать отдельный класс для полотна, сейчас это не очень актуально, но если расширять возможности, то будет необходимо), чтобы
    // полотно полностью помещалось на экран. + создавать отдельный класс для управления камерой (увеличить/уменьшить + двигаться вдоль плотна). 
    private void BuildPanel()
    {
        tilePanel = new Rectangle(new Vector2(0.0f, 0.0f), 1.1f, 0.9f);
        GameObject panel = meshBuilder.BuildMesh(new TileRepresentation(tilePanel, new List<Vector2>()), null, false);
        panel.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        panel.GetComponent<MeshRenderer>().material = FindObjectOfType<MaterialsContainer>().GetMaterial(MaterialType.Panel);
    }

    private void ResetTilesParentTransform()
    {
        tilesParent.rotation = Quaternion.Euler(Vector3.zero);
        foreach (GameObject tile in factory.GetTiles())
        {
            Destroy(tile);
        }
    }

    private void CalculateTiles(UpdateData data)
    {
        TileData tileData = tilesCatalogue.GetCatalogue().Find(x => x.name == data.tileName);
        meshBuilder.SetActiveTileData(tileData);

        stacker.StackTiles(tilePanel, tileData, data.seamWidth / 1000.0f, data.offset / 1000.0f, data.angle);
        slicer.SliceTiles();
    }

    private void BuildTiles(UpdateData data)
    {
        factory.BuildTilesRepresentations(slicer, meshBuilder, tilesParent);
        tilePanel.RotateAroundCenter(-data.angle);
        tilesParent.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -data.angle));
    }

    private void UpdateSquareValue(float newSquare)
    {
        square = newSquare;
    }
}
