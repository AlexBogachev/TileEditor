
/// <summary>
///  ����� ����� ���������� � ���������� ��������� (������������ ������, ������ ���, �������� � ����)
/// </summary>
public class UpdateData
{
    public string tileName;
    public int seamWidth;
    public int offset;
    public int angle;

    public UpdateData(string tileName, int seamWidth, int offset, int angle)
    {
        this.tileName = tileName;
        this.seamWidth = seamWidth;
        this.offset = offset;
        this.angle = angle;
    }
}
