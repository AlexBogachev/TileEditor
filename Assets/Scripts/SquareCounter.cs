using UnityEngine;
using UnityEngine.UI;

public class SquareCounter : MonoBehaviour
{
    [SerializeField] Text squareValueText;

    public void Initialize(TileSlicer slicer)
    {
        slicer.squareUpdated.AddListener(UpdateSquareText);
    }

    private void UpdateSquareText(float square)
    {
        squareValueText.text = square.ToString("0.####");
    }
}
