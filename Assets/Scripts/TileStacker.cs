using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///   ласс рассчитывает количество плитки, необходимой дл€ заполнени€ панели и размещает эту плитку
/// </summary>
public class TileStacker
{
    Rectangle panel;
    TileData tileData;
    float seamWidth;
    float offset;
    float angle;

    Rectangle circumscribedRectangle;

    int columns;
    int rows;

    List<Rectangle> tiles;

    public TileStacker()
    {
        tiles = new List<Rectangle>();
    }

    public void StackTiles(Rectangle panel, TileData tileData, float seamWidth, float offset, float angle)
    {
        this.panel = panel;
        this.tileData = tileData;
        this.seamWidth = seamWidth;
        this.offset = offset;
        this.angle = angle;

        tiles.Clear();

        CalculateCircumscribedRectangle();

        CalculateColumns();
        CalculateRows();

        FillPanelWithTiles();
    }

    public List<Rectangle> GetTiles()
    {
        return tiles;
    }

    /// <summary>
    ///  –ассчитываем описанный пр€моугольник, внутри которого будут размещены плитки (до обрезки)
    /// </summary>
    private void CalculateCircumscribedRectangle()
    {
        if (angle != 0)
        {
            //„тобы не поворачивать плитки - повернем панель, по которой будет производитьс€ обрезка
            panel.RotateAroundCenter(angle);
            circumscribedRectangle = UtilitiesScripts.GetCircumscribedRectangle(panel.GetVertices().ToList(), panel.GetRectCenter());
        }
        else
        {
            circumscribedRectangle = panel;
        }
    }

    private void CalculateColumns()
    {
        float fractional = circumscribedRectangle.GetWidth() / (tileData.widht + seamWidth);
        columns = Mathf.CeilToInt(fractional);
    }

    private void CalculateRows()
    {
        float fractional = circumscribedRectangle.GetHeight() / (tileData.height + seamWidth);
        rows = Mathf.CeilToInt(fractional);
    }

    /// <summary>
    ///  «аполн€ем площадь плитками
    /// </summary>
    private void FillPanelWithTiles()
    {
        Vector2 tilePosition;

        for (int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                // —ама€ перва€ плитка
                if (i == 0 && j == 0)
                {
                    tilePosition = circumscribedRectangle.GetMinCoordinatePosition() + (new Vector2(tileData.widht / 2, 0.0f) + new Vector2(0.0f, tileData.height / 2));
                }
                //ѕерва€ плитка в каждом последующем столбце
                else if (j == 0)
                {
                    tilePosition = tiles[tiles.Count - rows].GetRectCenter() + new Vector2(tileData.widht + seamWidth, 0.0f);
                }
                else
                {
                    //ѕлитки подверженные смещению
                    if((j+1)%2 == 0)
                    {
                        tilePosition = tiles[tiles.Count - 1].GetRectCenter() + (new Vector2(0.0f, tileData.height + seamWidth) + new Vector2(-offset, 0.0f));
                    }
                    //ѕлитки не подверженные смещению
                    else
                    {
                        tilePosition = tiles[tiles.Count - 2].GetRectCenter() + (new Vector2(0.0f, (tileData.height + seamWidth))*2);
                    }
                    
                }
                tiles.Add(new Rectangle(tilePosition, tileData.widht, tileData.height));
            }
        }

        AddAdditionalTiles();
    }


    /// <summary>
    ///  ƒобавл€ютс€ дополнительные плитки, чтобы избежать пустых мест при большом смещении (+1 вправо к каждой четной плитке в последнем столбце)
    /// </summary>
    public void AddAdditionalTiles()
    {
        for(int i = tiles.Count - (rows-1); i<tiles.Count; i += 2)
        {
            Vector2 tilePosition = tiles[i].GetRectCenter() + new Vector2(tileData.widht + seamWidth, 0.0f);
            tiles.Add(new Rectangle(tilePosition, tileData.widht, tileData.height));
        }
    }

}
