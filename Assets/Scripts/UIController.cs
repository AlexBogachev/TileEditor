using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class DataUpdated : UnityEvent<UpdateData>
{
}

[Serializable]
public class TileChanged : UnityEvent<TileData>
{
}

public class UIController : MonoBehaviour
{
    [SerializeField] Dropdown tileNameDropdown;
    [SerializeField] InputField seamWidthField;
    [SerializeField] InputField offsetField;
    [SerializeField] InputField angleField;

    [HideInInspector] public DataUpdated dataUpdated = new DataUpdated();
    [HideInInspector] public TileChanged tileChanged = new TileChanged();

    TilesCatalogue catalogue;
    UpdateData activeData;

    public void Initialize(TilesCatalogue catalogue)
    {
        this.catalogue = catalogue;
        List<string> tilesNames = catalogue.GetCatalogue().Select(x => x.name).ToList();

        tileNameDropdown.ClearOptions();
        tileNameDropdown.AddOptions(tilesNames);

        dataUpdated.AddListener(ApplicationManager.Instance.UpdateTiles);

        activeData = new UpdateData(catalogue.GetCatalogue()[0].name, 0, 0, 0);
        dataUpdated.Invoke(activeData);

        ResetValues();

        seamWidthField.onValueChanged.AddListener(CheckSeamWidth);
        offsetField.onValueChanged.AddListener(CheckOffset);
        angleField.onValueChanged.AddListener(CheckAngle);

        tileNameDropdown.onValueChanged.AddListener(TileChanged);
    }

    /// <summary>
    ///  Меняется плитка 
    /// </summary>
    private void TileChanged(int value)
    {
        string tileName = tileNameDropdown.options[value].text;
        activeData.tileName = tileName;

        ResetValues();
        dataUpdated.Invoke(activeData);
    }

    // Проверяем и обновляем ширину шва + ограничиваем ширину шва 15мм (это и так много, но можно поставить и другое ограничение)
    private void CheckSeamWidth(string value)
    {
        int seamWidth;
        int.TryParse(value, out seamWidth);
        if (seamWidth < 0)
        {
            seamWidth = 0;
            seamWidthField.SetTextWithoutNotify("0");
        }
        else if (seamWidth > 15)
        {
            seamWidth = 15;
            seamWidthField.SetTextWithoutNotify("15");
        }
        activeData.seamWidth = seamWidth;
        dataUpdated.Invoke(activeData);
    }

    // Проверяем и обновляем смещение + ограничиваем смещение длиной плитки
    private void CheckOffset(string value)
    {
        int offset;
        int.TryParse(value, out offset);

        TileData tileData = catalogue.GetCatalogue().Find(x => x.name == activeData.tileName);

        if (offset < 0)
        {
            offset = 0;
            offsetField.SetTextWithoutNotify("0");
        }
        else if (offset / 1000.0f > tileData.widht)
        {
            offset = Mathf.RoundToInt(tileData.widht*1000);
            offsetField.SetTextWithoutNotify(offset.ToString());
        }
        activeData.offset = offset;
        dataUpdated.Invoke(activeData);
    }

    // Проверяем и обновляем угол + ограничение угла 90 градусов (можно и 180, если кто-то захчет посмотреть вверх ногами)
    private void CheckAngle(string value)
    {
        int angle;
        int.TryParse(value, out angle);
        if (angle < 0)
        {
            angle = 0;
            angleField.SetTextWithoutNotify("0");
        }
        else if (angle > 90)
        {
            angle = 90;
            angleField.SetTextWithoutNotify("90");
        }
        activeData.angle = angle;
        dataUpdated.Invoke(activeData);
    }

    private void ResetValues()
    {
        activeData.seamWidth = 0;
        activeData.offset = 0;
        activeData.angle = 0;

        seamWidthField.SetTextWithoutNotify("0");
        offsetField.SetTextWithoutNotify("0");
        angleField.SetTextWithoutNotify("0");

    }
}
