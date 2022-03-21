using System;

/// <summary>
///  �������������� ������
/// </summary>
[Serializable]
public class TileData
{
    public string name;
    public string pathToTexture; //�� ������ ������, ������ ���� �� ����� Resources, �� ��� ����� ���� ��� �������� ������ ��� ��������� ���� (� ����� ����� ������ � �������� ����� �������� ���� � ���������);
    public float widht;
    public float height;

    //��� ����� ���� ����� ������������� ����� ���, ������� ����� ����������,
    //��������, ���������/����/������������� � �.�.
    public TileData(string name, string pathToTexture, float widht, float height)
    {
        this.name = name;
        this.pathToTexture = pathToTexture;
        this.widht = widht;
        this.height = height;
    }
}
