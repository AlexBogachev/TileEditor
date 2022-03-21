using System.Collections.Generic;

public class TilesCatalogue
{
    List<TileData> tileDatas;

    //На данный момент информация внесена "вручную" для демонстрации и проверки плиток различных текстур и форматов. 
    //В идеале, информация должна браться из какого-то стороннего источника (вероятнее всего в фрмате json), 
    // c возможной фильтрацией по коллекции/цвету/производителю и т.д.
    public TilesCatalogue()
    {
        tileDatas = new List<TileData>
        {
            new TileData("VT/A274/16000", "Textures/tile1", 0.15f, 0.072f),
            new TileData("6369 Пальмовый лес синий", "Textures/tile2", 0.2f, 0.2f)
        };
    }

    public List<TileData> GetCatalogue()
    {
        return tileDatas;
    }
}
