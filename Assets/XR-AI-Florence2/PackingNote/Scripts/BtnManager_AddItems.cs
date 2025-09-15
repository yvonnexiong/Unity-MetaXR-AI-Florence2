using TMPro;
using UnityEngine;

public class BtnManager_AddItems : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP_label;

    private string labelText;

    public void SetLabelText(string text)
    {
        labelText = text;
        TMP_label.text = labelText;
    }

    public string GetLabelText()
    {
        return labelText;
    }

}
