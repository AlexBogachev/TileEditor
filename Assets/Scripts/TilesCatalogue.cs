using System.Collections.Generic;

public class TilesCatalogue
{
    List<TileData> tileDatas;

    //�� ������ ������ ���������� ������� "�������" ��� ������������ � �������� ������ ��������� ������� � ��������. 
    //� ������, ���������� ������ ������� �� ������-�� ���������� ��������� (��������� ����� � ������ json), 
    // c ��������� ����������� �� ���������/�����/������������� � �.�.
    public TilesCatalogue()
    {
        tileDatas = new List<TileData>
        {
            new TileData("VT/A274/16000", "Textures/tile1", 0.15f, 0.072f),
            new TileData("6369 ��������� ��� �����", "Textures/tile2", 0.2f, 0.2f)
        };
    }

    public List<TileData> GetCatalogue()
    {
        return tileDatas;
    }
}
