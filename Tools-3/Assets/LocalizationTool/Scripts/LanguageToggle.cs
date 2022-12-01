using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void LanguageToggleAction(string index);

public class LanguageToggle : MonoBehaviour
{

    public Toggle toggle;
    public Text text;
    public string languageIndex;
    event LanguageToggleAction onToggled;

    public void SetData(LanguageData data, bool isDefault, LanguageToggleAction action)
    {
        text.text = data.name;
        toggle.isOn = isDefault;
        languageIndex = data.index;
        onToggled = action;
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(bool on)
    {
        if (on)
        {
            onToggled(languageIndex);
        }
    }
}
