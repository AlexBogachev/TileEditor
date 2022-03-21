using UnityEngine;

public enum MaterialType
{
    Panel,
    Tile
}

public class MaterialsContainer : MonoBehaviour
{
    [SerializeField] Material tileMaterial;
    [SerializeField] Material panelMaterial;

    public Material GetMaterial(MaterialType type)
    {
        switch (type)
        {
            case MaterialType.Panel:
                return panelMaterial;
            case MaterialType.Tile:
                return tileMaterial;
            default:
                return null;
                
        }
    }
}
