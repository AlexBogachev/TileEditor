using System;

/// <summary>
///  Характеристики плитки
/// </summary>
[Serializable]
public class TileData
{
    public string name;
    public string pathToTexture; //На данный момент, указан путь до папки Resources, но это может быть или интернет ресурс или локальный файл (в общем любой ресурс с которого можно получить файл с текстурой);
    public float widht;
    public float height;

    //Тут может быть много зарактеристик кроме тех, которые здесь определены,
    //например, коллекция/цвет/производитель и т.д.
    public TileData(string name, string pathToTexture, float widht, float height)
    {
        this.name = name;
        this.pathToTexture = pathToTexture;
        this.widht = widht;
        this.height = height;
    }
}
